using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YahooGroups.Models;
namespace YahooGroups.Controllers
{
    public class GroupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
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

            if (group.Users.Contains(user))
            {
                hasJoined = true;
            }

            ViewBag.Moderator = moderator.UserName;
            ViewBag.Users = group.Users;
            ViewBag.CurrentId = currentId;
            ViewBag.HasJoined = hasJoined;

            return View(group);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            GroupModels gr = db.Groups.Find(id);
            if(gr.moderatorId == User.Identity.GetUserId() || User.IsInRole("admin") || User.IsInRole("moderator"))
            {
                TempData["message"] = "Grupul a fost sters";
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

        [Authorize(Roles = "user,moderator,admin")]
        public ActionResult CreateGroup()
        {
            var group = new GroupModels();

            group.moderatorId = User.Identity.GetUserId();

            ViewBag.Categories = GetAllCategories();

            return View(group);
        }

        [Authorize(Roles = "user,moderator,admin")]
        [HttpPost]
        public ActionResult Join(int groupId, string userId)
        {
            var group = db.Groups.Find(groupId);
            var user = db.Users.Find(userId);   

            if (TryUpdateModel(group))
            {
                group.Users.Add(user);
                db.SaveChanges();
            }

            var id = groupId;

            return RedirectToAction("Show", new { id });
        }
        
        [Authorize(Roles = "user,moderator,admin")]
        [HttpPost]
        public ActionResult CreateGroup(GroupModels group)
        {
            var user = db.Users.Find(group.moderatorId);
            group.Users = new List<ApplicationUser>();
            group.Users.Add(user);

            try
            {
                db.Groups.Add(group);
                db.SaveChanges();
                TempData["message"] = "Grupul a fost adaugat!";
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {

                TempData["message"] = e.ToString();
                return View(group);
            }
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