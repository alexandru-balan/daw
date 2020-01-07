using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YahooGroups.Models;

namespace YahooGroups.Controllers
{
    
    public class CommentController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Conversation(int groupId)
        {
            var com = from cm in db.Comments select cm;
            var users = from us in db.Users select us;
            ViewBag.Users = users.ToList();
            ViewBag.Comments = com.ToList();
            ViewBag.groupId = groupId;
            return View();
        }
        
    }
}