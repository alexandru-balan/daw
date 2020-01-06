using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using YahooGroups.Models;
namespace YahooGroups.Controllers
{
    public class GroupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index(int? id)
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            
            var groups = from gr in db.Groups select gr;
            
            ViewBag.Groups = groups.ToList();

            bool IsLogedIn = false;

            if (User.IsInRole("user") || User.IsInRole("moderator") || User.IsInRole("admin"))
            {
                IsLogedIn = true;
            }

            ViewBag.IsLogedIn = IsLogedIn;

            if (User.IsInRole("admin"))
            {
                ViewBag.UserRole = "admin";
            }

            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.CurrentUserGroups = user.Groups;

            return View();
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            GroupModels group = db.Groups.Find(id);
            var moderator = db.Users.Find(group.moderatorId);
            var currentId = User.Identity.GetUserId();
            var user = db.Users.Find(currentId);
            bool hasJoined = false;

            if (!User.IsInRole("admin") && !User.IsInRole("moderator") && !User.IsInRole("user"))
            {
                ViewBag.IsLogedIn = false;
            }
            else
            {
                ViewBag.IsLogedIn = true;
            }

            if (group.Users.Contains(user))
            {
                hasJoined = true;
            }

            ViewBag.Moderator = moderator.UserName;
            ViewBag.Users = group.Users;
            ViewBag.CurrentId = currentId;
            ViewBag.HasJoined = hasJoined;

            if (User.IsInRole("admin"))
            {
                ViewBag.UserRole = "admin";
            }

            ViewBag.CurrentUserGroups = user.Groups;

            return View(group);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            GroupModels gr = db.Groups.Find(id);
            if(gr.moderatorId == User.Identity.GetUserId() || User.IsInRole("admin") || User.IsInRole("moderator"))
            {
                TempData["message"] = "Grupul a fost sters";
                foreach(var user in gr.Users)
                {
                    if (TryUpdateModel(gr))
                    {
                        user.Groups.Remove(gr);
                        db.SaveChanges();
                    }
                }
                foreach (var user in gr.InQueue)
                {
                    if (TryUpdateModel(gr))
                    {
                        user.Groups.Remove(gr);
                        db.SaveChanges();
                    }
                }
                db.Groups.Remove(gr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu ai dreptul sa stergi acest grup";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Kick (int groupId, string userId)
        {
            var user = db.Users.Find(userId);
            var group = db.Groups.Find(groupId);
            var id = groupId;

            if (TryUpdateModel(group))
            {
                group.Users.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction("Show", new { id });
        }

        [Authorize(Roles = "user,moderator,admin")]
        public ActionResult CreateGroup()
        {
            var group = new GroupModels();

            group.moderatorId = User.Identity.GetUserId();

            ViewBag.Categories = GetAllCategories();

            if (User.IsInRole("admin"))
            {
                ViewBag.UserRole = "admin";
            }

            if (!User.IsInRole("admin") && !User.IsInRole("moderator") && !User.IsInRole("user"))
            {
                ViewBag.IsLogedIn = false;
            }
            else
            {
                ViewBag.IsLogedIn = true;
            }

            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.CurrentUserGroups = user.Groups;

            return View(group);
        }
        
        [Authorize(Roles = "user,moderator,admin")]
        [HttpPost]
        public ActionResult CreateGroup(GroupModels group)
        {
            var user = db.Users.Find(group.moderatorId);
            group.Users.Add(user);

            var category = db.Categories.Find(group.CategoryId);

            try
            {
                db.Groups.Add(group);
                db.SaveChanges();
                TempData["message"] = "Grupul a fost adaugat!";

                // Register the user as member of this group
                if (TryUpdateModel(user))
                {
                    user.Groups.Add(group);
                    db.SaveChanges();
                }

                // Register the group as part of category
                if(TryUpdateModel(category))
                {
                    category.Groups.Add(group);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {

                TempData["message"] = e.ToString();
                return View(group);
            }
        }

        [Authorize(Roles = "user,moderator,admin")]
        [HttpPost]
        public ActionResult Join(int groupId, string userId)
        {
            var group = db.Groups.Find(groupId);
            var user = db.Users.Find(userId);
            var id = groupId;

            if (group.privateGroup)
            {
                if (group.InQueue.Contains(user))
                {
                    return RedirectToAction("Show", new { id });
                }
                else
                {
                    if (TryUpdateModel(group))
                    {
                        group.InQueue.Add(user);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Show", new { id });
                }
            }

            if (TryUpdateModel(group))
            {
                group.Users.Add(user);
                db.SaveChanges();
            }

            if (TryUpdateModel(user))
            {
                user.Groups.Add(group);
                db.SaveChanges();   
            }

            return RedirectToAction("Show", new { id });
        }

        [HttpGet]
        public ActionResult Approve (int groupId, string userId)
        {
            var user = db.Users.Find(userId);
            var group = db.Groups.Find(groupId);
            var id = groupId;

            if (TryUpdateModel(group))
            {
                group.InQueue.Remove(user);
                group.Users.Add(user);
                db.SaveChanges();
            }

            if (TryUpdateModel(user))
            {
                user.Groups.Add(group);
                db.SaveChanges();
            }

            return RedirectToAction("Show", new { id });
        }

        [HttpGet]
        public ActionResult Deny(int groupId, string userId)
        {
            var user = db.Users.Find(userId);
            var group = db.Groups.Find(groupId);
            var id = groupId;

            if (TryUpdateModel(group))
            {
                group.InQueue.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction("Show", new { id });
        }

        [HttpPost]
        public ActionResult Search (string search)
        {
            var Groups = (from gr in db.Groups where gr.groupName.ToUpper().Contains(search.ToUpper()) select gr).ToList();

            ViewBag.Groups = Groups;

            if (User.IsInRole("user") || User.IsInRole("moderator") || User.IsInRole("admin"))
            {
                ViewBag.IsLogedIn = true;
            }
            else
            {
                ViewBag.IsLogedIn = false;
            }

            if (User.IsInRole("admin"))
            {
                ViewBag.UserRole = "admin";
            }

            return View("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            
            var categories = from category in db.Categories
                             select category;
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name.ToString()
                });
            }
            return selectList;
        }
    }
}