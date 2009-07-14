using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Molecule.WebSite.Services;
using Molecule.MvcWebSite.Mvc;

namespace Molecule.MvcWebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

            routes.MapRoute("photoFile",
                "{controller}/{action}/{id}/{size}",
                new { controller = "photos", action = "File", id = "", size = "" }
            );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            var viewEngine = ViewEngines.Engines[0] as WebFormViewEngine;

            viewEngine.ViewLocationFormats = new string[] { 
                "~/atomes/{1}/Views/{0}.aspx", 
                "~/atomes/{1}/Views/{0}.ascx", 
                "~/Views/{1}/{0}.aspx",
                "~/Views/{1}/{0}.ascx",
                "~/Views/Shared/{0}.aspx", 
                "~/Views/Shared/{0}.ascx" 
                };

            viewEngine.PartialViewLocationFormats = viewEngine.ViewLocationFormats;
            viewEngine.MasterLocationFormats = viewEngine.ViewLocationFormats;
        }
    }
}