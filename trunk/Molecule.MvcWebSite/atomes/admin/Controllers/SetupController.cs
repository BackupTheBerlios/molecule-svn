using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;
using Molecule.WebSite.Services;
using System.Web.Security;
using MvcContrib;

namespace Molecule.MvcWebSite.atomes.admin.Controllers
{
    public class SetupController : PublicPageControllerBase
    {

        public ActionResult Index()
        {
            return this.RedirectToAction(c => c.CreateAdmin());
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CreateAdmin()
        {
            if (!AdminService.IsSetupAuthorized)
                return new RedirectResult("~/");
            
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAdmin(string username, string password)
        {
            AdminService.CreateAdmin(username, password);
            return this.RedirectToAction<PreferencesController>(c => c.Index());
        }
    }
}
