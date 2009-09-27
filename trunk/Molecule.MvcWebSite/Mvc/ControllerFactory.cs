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

            if(AtomeService.CurrentPathIsAtome)
                Console.WriteLine("ControllerFactory:" + AtomeService.CurrentAtome.Name);
            Console.WriteLine("ControllerFactory:"+controllerName);
            Console.WriteLine("ControllerFactory:" + HttpContext.Current.Request.Url);
            
            return base.GetControllerType(controllerName);
            
        }
    }
}
