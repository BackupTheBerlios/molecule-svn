using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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

        
    }
}
