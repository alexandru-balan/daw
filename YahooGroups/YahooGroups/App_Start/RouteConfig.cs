﻿using System;
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
                name: "GroupJoin",
                url: "{controller}/{action}/{groupId}/{userId}",
                defaults: new {controller = "Group", action = "Join"}
            );

            routes.MapRoute(
                name: "GroupSearch",
                url: "{controller}/{action}/{groupId}/{search}",
                defaults: new { controller = "Group", action = "Search" }
            );

            routes.MapRoute(
                name: "CategoryEdit",
                url: "{controller}/{action}/{id}/{category}",
                defaults: new { controller = "Category", action = "Edit" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
