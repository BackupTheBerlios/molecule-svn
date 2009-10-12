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
            //routes.Clear();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //if (Type.GetType("Mono.Runtime") != null)
            //    defaultRoute(routes);

            //foreach (var atome in AtomeService.GetAtomes())
            //    atome.RegisterRoutes(routes);

            //if (Type.GetType("Mono.Runtime") == null)
                defaultRoute(routes);
        }

        private static void defaultRoute(RouteCollection routes)
        {
            routes.MapRoute(
                "Default",                                              // Route name
                "{atome}/{controller}/{action}/{*id}",                           // URL with parameters
                new { atome = "", controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            ControllerBuilder.Current.SetControllerFactory(typeof(ControllerFactory));

            RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new Molecule.MvcWebSite.Mvc.ViewEngine());
        }
    }
}