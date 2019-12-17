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

            var groups = from gr in db.Groups
                             orderby gr.groupName
                             select gr;
            ViewBag.Groups = groups;

            bool IsLogedIn = false;

            if (User.IsInRole("user") || User.IsInRole("moderator") || User.IsInRole("admin"))
            {
                IsLogedIn = true;
            }

            ViewBag.IsLogedIn = IsLogedIn;

            return View();
        }
        public ActionResult Show(int id)
        {
            GroupModels group = db.Groups.Find(id);
            return View(group);
        }
        public ActionResult Delete(int id)
        {
            GroupModels gr = db.Groups.Find(id);
            if(gr.moderatorId == User.Identity.GetUserId())
            {
                TempData["message"] = "Grupul a fost sters";
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
        public ActionResult CreateGroup(GroupModels group)
        {
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