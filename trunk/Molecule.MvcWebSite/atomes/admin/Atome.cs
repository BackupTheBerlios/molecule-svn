using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Molecule.MvcWebSite.atomes.music.Controllers;

namespace Molecule.MvcWebSite.atomes.admin
{
    public class Atome : IAtome
    {
        public void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {

            routes.MapRoute(
                "PreferencesRoute",                                              // Route name
                "Preferences",                           // URL with parameters
                new { atome = "admin", controller = "Preferences", action = "Index" }  // Parameter defaults
            );
        }



        #region IAtome Members


        public Type PreferencesController
        {
            get { return typeof(AdminController); }
        }

        #endregion
    }
}
