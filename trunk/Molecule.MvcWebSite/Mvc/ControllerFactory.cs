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
            base.RequestContext.RouteData.DataTokens["Namespaces"] = AtomeService.CurrentAtome.ControllerNamespaces;
                return base.GetControllerType(controllerName);
            
        }
    }
}
