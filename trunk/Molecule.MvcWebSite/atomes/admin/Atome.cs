using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Molecule.MvcWebSite.atomes.admin.Controllers;

namespace Molecule.MvcWebSite.atomes.admin
{
    public class Atome : IAtome
    {
        //public void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        //{
        //    routes.MapRoute(
        //        "PreferencesRoute",                                              // Route name
        //        "Preferences",                           // URL with parameters
        //        new { atome = "admin", controller = "Preferences", action = "Index" }  // Parameter defaults
        //    );
        //}

        #region IAtome Members


        public Type PreferencesController
        {
            get { return typeof(PreferencesController); }
        }

        public IEnumerable<string> ControllerNamespaces
        {
            get { yield return "Molecule.MvcWebSite.atomes.admin.Controllers"; }
        }

        #endregion


        public Type DefaultController
        {
            get { return PreferencesController; }
        }


        public bool AdminOnly
        {
            get { return true; }
        }

        public string Name
        {
            get { return Resources.Common.Preferences; }
        }
    }
}
