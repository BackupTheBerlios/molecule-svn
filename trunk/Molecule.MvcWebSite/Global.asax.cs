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
                "Tag",                                              // Route name
                "{atome}/Tag/{tagId}/{controller}/{id}",                           // URL with parameters
                new { atome = "", controller = "Home", action = "Index", id = "", tagId="" }  // Parameter defaults
            );

            routes.MapRoute(
                "Year",                                              // Route name
                "{atome}/Calendar/{year}",                           // URL with parameters
                new { atome = "", controller = "Calendar", action = "Year" }  // Parameter defaults
            );

            routes.MapRoute(
                "Month",                                              // Route name
                "{atome}/Calendar/{year}/{month}",                           // URL with parameters
                new { atome = "", controller = "Calendar", action = "Month" }  // Parameter defaults
            );

            routes.MapRoute(
                "Default",                                              // Route name
                "{atome}/{controller}/{id}/{action}",                           // URL with parameters
                new { atome = "", controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new Molecule.MvcWebSite.Mvc.ViewEngine());
        }
    }
}