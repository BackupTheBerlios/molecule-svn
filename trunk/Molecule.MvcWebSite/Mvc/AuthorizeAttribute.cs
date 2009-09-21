using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.WebSite.Services;

namespace Molecule.MvcWebSite.Mvc
{
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public AuthorizeAttribute()
        {
            
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return AtomeService.IsCurrentUserAuthorized();
        }
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
    }
}
