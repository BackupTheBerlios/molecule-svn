using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Molecule.WebSite.Services;

namespace Molecule.MvcWebSite.Mvc
{
    public class ControllerFactory : DefaultControllerFactory
    {
        protected override Type GetControllerType(string controllerName)
        {
            //if (!AtomeService.IsCurrentUserAuthorized())
                //return null;

            if(AtomeService.CurrentAtome != null)
                base.RequestContext.RouteData.DataTokens["Namespaces"] = AtomeService.CurrentAtome.ControllerNamespaces;

            return base.GetControllerType(controllerName);
            
        }
    }
}
