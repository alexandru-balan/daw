using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YahooGroups.Controllers
{
    public class HomeController : Controller
    {
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