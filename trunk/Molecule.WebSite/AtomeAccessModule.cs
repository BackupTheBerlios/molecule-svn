using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Molecule.WebSite.Services;

namespace Molecule.WebSite
{
    public class AtomeAccessModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.AuthorizeRequest += new EventHandler(context_AuthorizeRequest);
        }

        void context_AuthorizeRequest(object sender, EventArgs e)
        {
            if (!AtomeService.IsCurrentUserAuthorized())
                FormsAuthentication.RedirectToLoginPage();
        }

        #endregion
    }
}
