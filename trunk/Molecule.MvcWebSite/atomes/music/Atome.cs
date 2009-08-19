using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Molecule.MvcWebSite.atomes.music.Controllers;

namespace Molecule.MvcWebSite.atomes.music
{
    public class Atome : IAtome
    {
        public void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {

            routes.MapRoute(
                "MusicRoute",                                              // Route name
                "music",                           // URL with parameters
                new { atome = "music", controller = "Player", action = "Index" }  // Parameter defaults
            );
        }



        #region IAtome Members


        public Type PreferencesController
        {
            get { return typeof(AdminController); }
        }

        #endregion

        #region IAtome Members


        public IEnumerable<string> ControllerNamespaces
        {
            get 
            {
                yield return "Molecule.MvcWebSite.atomes.music.Controllers";
            }
        }

        #endregion
    }
}
