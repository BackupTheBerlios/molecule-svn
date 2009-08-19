using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebPhoto.Services;
using Molecule.MvcWebSite.atomes.photos.Controllers;


namespace Molecule.MvcWebSite.atomes.photos
{
    public class Atome : Molecule.MvcWebSite.IAtome
    {
        public Atome()
        {
        }

        #region IAtome Members

        public void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {

            routes.MapRoute(
                "PhotosRoute",                                              // Route name
                "photos",                           // URL with parameters
                new { atome = "photos", controller = "Tag", action = "Index" }  // Parameter defaults
            );
        }

        public Type PreferencesController
        {
            get { return typeof(AdminController); }
        }

        #endregion

        #region IAtome Members


        public IEnumerable<string> ControllerNamespaces
        {
            get { yield return "Molecule.MvcWebSite.atomes.photos.Controllers"; }
        }

        #endregion
    }
}
