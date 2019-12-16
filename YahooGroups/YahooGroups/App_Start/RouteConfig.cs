using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace YahooGroups
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "CategoryIndex",
                url: "{controller}/{action}",
                defaults: new {controller = "Category", action = "Index"}
            );

            routes.MapRoute(
                name: "CategoryShow",
                url: "{controller}/{action}/{categoryId}",
                defaults: new { controller = "Category", action = "Show" }
            );

            routes.MapRoute(
                name: "CategoryNewGET",
                url: "{controller}/{action}",
                defaults: new { controller = "Category", action = "New" }
            );

            routes.MapRoute(
                name: "CategoryNewPOST",
                url: "{controller}/{action}/{category}",
                defaults: new { controller = "Category", action = "New" }
            );

            routes.MapRoute(
                name: "CategoryEditGET",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Category", action = "Edit" }
            );

            routes.MapRoute(
                name: "CategoryEditPUT",
                url: "{controller}/{action}/{categoryId}/{category}",
                defaults: new { controller = "Category", action = "Edit" }
            );

            routes.MapRoute(
                name: "CategoryEditNoEnt",
                url: "{controller}/{action}",
                defaults: new { controller = "Category", action = "Edit" }
            );

            routes.MapRoute(
                name: "CategoryDelete",
                url: "{controller}/{action}/{categoryId}",
                defaults: new { controller = "Category", action = "Delete" }
            );

            routes.MapRoute(
                name: "IdentityIndex",
                url: "{controller}/{action}",
                defaults: new { controller = "Identity", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
