using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Molecule.MvcWebSite.Mvc
{
    public class ControllerFactory : DefaultControllerFactory
    {
        protected override Type GetControllerType(string controllerName)
        {
            //var atome = base.RequestContext.RouteData.Values["atome"];
            //var ctrlBaseName = atome != null ? atome.ToString() : "";
            //var type = base.GetControllerType(ctrlBaseName + controllerName);
            return base.GetControllerType(controllerName);
        }
    }
}
