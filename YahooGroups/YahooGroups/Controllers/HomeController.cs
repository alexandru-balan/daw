using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YahooGroups.Models;

namespace YahooGroups.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string role = "unreg";

            if (User.IsInRole("admin"))
            {
                role = "admin";
            }
            else if (User.IsInRole("moderator"))
            {
                role = "moderator";
            }
            else if (User.IsInRole("user"))
            {
                role = "user";
            }

            ViewBag.UserRole = role;

            var user = db.Users.Find(User.Identity.GetUserId());

            ViewBag.CurrentUserGroups = user.Groups;

            if (!User.IsInRole("admin") && !User.IsInRole("moderator") && !User.IsInRole("user"))
            {
                ViewBag.IsLogedIn = false;
            }
            else
            {
                ViewBag.IsLogedIn = true;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}