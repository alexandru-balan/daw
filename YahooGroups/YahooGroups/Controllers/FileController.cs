using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YahooGroups.Models;

namespace YahooGroups.Controllers
{
    public class FileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: File
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Show(int groupId)
        {
            var files = from fl in db.Files select fl;
            var users = from us in db.Users select us;
            ViewBag.Users = users.ToList();
            ViewBag.Files = files.ToList();
            ViewBag.GroupId = groupId;
            return View();
        }
        
    }
}