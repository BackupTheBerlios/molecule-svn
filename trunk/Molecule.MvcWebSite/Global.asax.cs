using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Molecule.WebSite.Services;
using Molecule.MvcWebSite.Mvc;
using Molecule.MvcWebSite.RouteHandlers;
using log4net;

namespace Molecule.MvcWebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static ILog log = LogManager.GetLogger(typeof(MvcApplication));

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
            routes.MapRoute("webdav", "webdav/{*path}", new { controller = "Webdav", action = "Index" }).RouteHandler = new WebdavRouteHandler();

            routes.MapRoute(
                "Default",                                              // Route name
                "{atome}/{controller}/{action}/{*id}",                           // URL with parameters
                new { atome = "", controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            log4net.GlobalContext.Properties["currentuser"] = Environment.UserName;
            log4net.Config.XmlConfigurator.Configure();

            if (log.IsInfoEnabled)
                log.Info("\nApplication starting...");
            ControllerBuilder.Current.SetControllerFactory(typeof(ControllerFactory));

            RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new Molecule.MvcWebSite.Mvc.ViewEngine());
            
            if (log.IsInfoEnabled)
                log.Info("Application started.");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (log.IsInfoEnabled)
            {
                if (HttpContext.Current.Session != null)
                    log.Info("Session start, Session ID : " + HttpContext.Current.Session.SessionID);
                if (HttpContext.Current.User != null)
                    log.Info("     Current user: " + HttpContext.Current.User.Identity.Name);
                log.Info("     Current request: " + HttpContext.Current.Request.RawUrl);
                log.Info("     User Agent: " + HttpContext.Current.Request.UserAgent);
                log.Info("     User address: " + HttpContext.Current.Request.UserHostAddress);
                log.Info("     User hostname: " + HttpContext.Current.Request.UserHostName);
            }
        }

        protected virtual void Session_End(object sender, EventArgs e)
        {
            if (log.IsInfoEnabled)
                log.Info("Session end.");
            log.Info("Session ID : " + HttpContext.Current.Session.SessionID);
        }

        protected virtual void Application_End(object sender, EventArgs e)
        {
            if (log.IsInfoEnabled)
                log.Info("Application end.\n");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();  // Log the exception.
            log.Error("Unhandled exception", exception);
            //Response.Clear();
            //HttpException httpException = exception as HttpException;
            //RouteData routeData = new RouteData();
            //routeData.Values.Add("controller", "Error");
            //if (httpException == null) {
            //    routeData.Values.Add("action", "Index");
            //}
            //else //It's an Http Exception, Let's handle it.
            //{
            //    switch (httpException.GetHttpCode())  {    case 404:      // Page not found.
            //        routeData.Values.Add("action", "HttpError404");      break;     case 500:     // Server error.
            //        routeData.Values.Add("action", "HttpError500");       break;       // Here you can handle Views to other error codes.        // I choose a General error template
            //        default:        routeData.Values.Add("action", "General");        break;     }  }  // Pass exception details to the target error View.
            //routeData.Values.Add("error", exception);  // Clear the error on server. Server.ClearError(); // Call target Controller and pass the routeData.
            ////IController errorController = new ErrorController();
            ////errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData)); }
        }
    }
}