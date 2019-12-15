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
        private GroupDBContext db = new GroupDBContext();
        
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
            return View();
        }
        public ActionResult Show(int id)
        {
            GroupModels gr = db.Groups.Find(id);
            return View(gr);
        }
        public ActionResult New()
        {
            return View();
        }
        [Authorize]
        public ActionResult CreateGroup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateGroup(GroupModels Gr)
        {
            try
            {
                Console.Write(Gr);
                db.Groups.Add(Gr);
                db.SaveChanges();
                TempData["message"] = "Grupul a fost adaugat!";
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {

                TempData["message"] = e.ToString();
                return RedirectToAction("Index");
            }
            }
        [NonAction]
        public IEnumerable<SelectListItem> GetAllGroups()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            
            var groups = from gr in db.Groups
                             select gr;
            foreach (var group in groups)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = group.groupId.ToString(),
                    Text = group.groupName.ToString()
                });
            }
            return selectList;
        }
    }
}