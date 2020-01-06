using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
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
            var Users = users.ToList();
            ArrayList Roles = new ArrayList();
            foreach (var user in Users)
            {
                foreach (var role in user.Roles)
                {
                    var userRole = db.Roles.Find(role.RoleId).Name; // The actual names of the roles
                    Roles.Add(userRole);
                }
            }

            ViewBag.Roles = Roles;
            ViewBag.Users = users;

            ViewBag.UserRole = "admin";

            if (User.IsInRole("user") || User.IsInRole("moderator") || User.IsInRole("admin"))
            {
                ViewBag.IsLogedIn = true;
            }
            else
            {
                ViewBag.IsLogedIn = false;
            }

            var curruser = db.Users.Find(User.Identity.GetUserId());
            ViewBag.CurrentUserGroups = curruser.Groups;

            return View();
        }

        [HttpGet]
        public ActionResult Show(string id)
        {
            var user = db.Users.Find(id);
            var roleId = user.Roles.First().RoleId;

            var role = db.Roles.Find(roleId);

            if (user == null)
            {
                TempData["message"] = "Can't find user";
                return View("ErrNoEnt");
            }

            ViewBag.Role = role.Name;

            ViewBag.UserRole = "admin";

            if (User.IsInRole("user") || User.IsInRole("moderator") || User.IsInRole("admin"))
            {
                ViewBag.IsLogedIn = true;
            }
            else
            {
                ViewBag.IsLogedIn = false;
            }

            var curruser = db.Users.Find(User.Identity.GetUserId());
            ViewBag.CurrentUserGroups = curruser.Groups;

            return View(user);
        }

        [HttpPut]
        public ActionResult MakeModerator (string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            userManager.AddToRole(id, "moderator");
            userManager.RemoveFromRole(id, "user");

            return RedirectToAction("Show", new { id });
        }

        [HttpPut]
        public ActionResult RevokeModerator (string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            userManager.AddToRole(id, "user");
            userManager.RemoveFromRole(id, "moderator");

            return RedirectToAction("Show", new { id });
        }

        /*[HttpPut]
        public ActionResult RevokeAdmin(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            userManager.AddToRole(id, "user");
            userManager.RemoveFromRole(id, "admin");

            return RedirectToAction("Show", new { id });
        }*/

        [HttpPut]
        public ActionResult MakeAdmin(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            userManager.AddToRole(id, "admin");
            if (userManager.GetRoles(id).Contains("user"))
            {
                userManager.RemoveFromRole(id, "user");
            }
            if (userManager.GetRoles(id).Contains("moderator"))
            {
                userManager.RemoveFromRole(id, "moderator");
            }

            return RedirectToAction("Show", new { id });
        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var user = db.Users.Find(id);

            userManager.Delete(user);

            return RedirectToAction("Index");
        }
    }
}