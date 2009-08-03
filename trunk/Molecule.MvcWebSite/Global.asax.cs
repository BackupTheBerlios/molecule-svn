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

            foreach (var atome in AtomeService.GetAtomes())
                atome.RegisterRoutes(routes);

            routes.MapRoute(
                "Default",                                              // Route name
                "{atome}/{controller}/{action}/{*id}",                           // URL with parameters
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