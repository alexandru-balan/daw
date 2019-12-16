using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YahooGroups.Models;

namespace YahooGroups.Controllers
{

    [Authorize(Roles = "admin")]
    public class IdentityController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            var users = from user in db.Users orderby user.Id select user;

            ViewBag.Users = users;

            return View();
        }

        [HttpGet]
        public ActionResult Show(int userId)
        {
            var user = db.Users.Find(userId);

            if (user == null)
            {
                TempData["message"] = "Can't find user";
                return View("ErrNoEnt");
            }

            return View(user);
        }

        [HttpPut]
        public ActionResult MakeModerator (int userId)
        {
            var user = db.Users.Find(userId);
            var UserRole = from role in db.Roles where role.Name == "user" select role; // Get user role
            var ModeratorRole = from role in db.Roles where role.Name == "moderator" select role; // Get moderator role

            var UserInUserRole = UserRole.First().Users.Where((x) => // Get a record from UserRole where this record has the 'user' role and the 'UserId' == user.Id
            {
                if (x.UserId == user.Id)
                {
                    return true;
                }
                return false;
            }).First();

            // Make that record have the 'moderator' role now
            if (TryUpdateModel(UserInUserRole))
            {
                UserInUserRole.RoleId = ModeratorRole.First().Id;
                db.SaveChanges();
                return RedirectToAction("Show", new { user.Id });
            }
            else
            {
                TempData["message"] = "Can't make this user a mod";
                return RedirectToAction("Show", new { user.Id });
            }
        }

        [HttpPut]
        public ActionResult RevokeModerator (int userId)
        {
            var user = db.Users.Find(userId);
            var UserRole = from role in db.Roles where role.Name == "user" select role; // Get user role
            var ModeratorRole = from role in db.Roles where role.Name == "moderator" select role; // Get moderator role

            var UserInModRole = ModeratorRole.First().Users.Where((u) =>
            {
                if (u.UserId == user.Id)
                {
                    return true;
                }
                return false;
            });

            if (UserInModRole.Count() == 0)
            {
                return View("RevokeErr");
            }

            if (TryUpdateModel(UserInModRole.First()))
            {
                UserInModRole.First().RoleId = UserRole.First().Id;
                db.SaveChanges();
                return RedirectToAction("Show", new { userId });
            }
            else
            {
                return View("RevokeErr");
            }
        }

        [HttpPut]
        public ActionResult MakeAdmin(int userId)
        {
            var user = db.Users.Find(userId);
            var UserRole = from role in db.Roles where role.Name == "user" select role; // Get user role
            var ModeratorRole = from role in db.Roles where role.Name == "moderator" select role; // Get moderator role
            var AdminRole = from role in db.Roles where role.Name == "admin" select role; // Get admin role

            
            var UserInUserRole = UserRole.First().Users.Where((x, b) => // Get a record from UserRole where this record has the 'user' role and the 'UserId' == user.Id
            {
                if (x.UserId == user.Id)
                {
                    return true;
                }
                return false;
            });

            if (UserInUserRole.Count() != 0)
            {
                if (TryUpdateModel(UserInUserRole))
                {
                    UserInUserRole.First().RoleId = AdminRole.First().Id;
                    db.SaveChanges();
                    return RedirectToAction("Show", new { user.Id });
                }
                else
                {
                    TempData["message"] = "Can't make this user a mod";
                    return RedirectToAction("Show", new { user.Id });
                }
            }
            else
            {
                // This means that this user is not an 'user' so he must be a 'moderator'

                var UserInModRole = UserRole.First().Users.Where((x) => // Get a record from UserRole where this record has the 'user' role and the 'UserId' == user.Id
                {
                    if (x.UserId == user.Id)
                    {
                        return true;
                    }
                    return false;
                }).First();

                if (TryUpdateModel(UserInModRole))
                {
                    UserInModRole.RoleId = AdminRole.First().Id;
                    db.SaveChanges();
                    return RedirectToAction("Show", new { user.Id });
                }
                else
                {
                    TempData["message"] = "Can't make this user a mod";
                    return RedirectToAction("Show", new { user.Id });
                }
            }
            
        }

        [HttpDelete]
        public ActionResult Delete(int userId)
        {
            // Check if the given user is an admin. You shouldn't be able to delete an admin account
            var user = db.Users.Find(userId);
            var AdminRole = from role in db.Roles where role.Name == "admin" select role;

           
            var UserInAdminRole = user.Roles.Where((x) =>
            {
                if (x.RoleId == AdminRole.First().Id)
                {
                    return true;
                }

                return false;
            });

            if (UserInAdminRole.Count() == 0)
            {
                // This user is not an admin so we delete him
                db.Users.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                // This user is an admin and can't be removed
                return View("DeleteAdmin");
            }
        }
    }
}