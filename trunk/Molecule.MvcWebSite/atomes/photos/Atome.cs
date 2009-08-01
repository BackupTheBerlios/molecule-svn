using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebPhoto.Services;

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
            //routes.MapRoute(
            //    "Tag",                                              // Route name
            //    "photos/tag/{id}",                           // URL with parameters
            //    new { atome = "photos", controller = "Tag", action = "Index", id = "" }  // Parameter defaults
            //);

            //routes.MapRoute(
            //    "PhotoFile",
            //    "photos/photo/{id}/file/{size}",
            //    new { atome = "photos", controller = "Photo", action = "File" }
            //    );

            //routes.MapRoute(
            //    "Photo",                                              // Route name
            //    "photos/photo/{id}",                           // URL with parameters
            //    new { atome = "photos", controller = "Photo", action = "Index", id = "" }  // Parameter defaults
            //    );

            //routes.MapRoute(
            //    "TagPhoto",                                              // Route name
            //    "photos/tag/{tagId}/photo/{id}",                           // URL with parameters
            //    new { atome = "photos", controller = "Photo", action = "Index", id = "", tagId = "" }  // Parameter defaults
            //    );

            //routes.MapRoute(
            //    "Year",                                              // Route name
            //    "photos/calendar/{year}",                           // URL with parameters
            //    new { atome = "photos", controller = "Calendar", action = "Year", year = DateTime.Now.Year }  // Parameter defaults
            //);

            //routes.MapRoute(
            //    "Month",                                              // Route name
            //    "photos/calendar/{year}/{month}",                           // URL with parameters
            //    new { atome = "photos", controller = "Calendar", action = "Month" }  // Parameter defaults
            //);

            //routes.MapRoute(
            //    "TagYear",                                              // Route name
            //    "photos/tag/{tagId}/calendar/{year}",                           // URL with parameters
            //    new { atome = "photos", controller = "Calendar", action = "Year", year = DateTime.Now.Year }  // Parameter defaults
            //);

            routes.MapRoute(
                "PhotosRoute",                                              // Route name
                "photos/{controller}/{action}/{*id}",                           // URL with parameters
                new { atome = "photos", controller = "Tag", action = "Index" }  // Parameter defaults
            );
        }

        #endregion
    }
}
