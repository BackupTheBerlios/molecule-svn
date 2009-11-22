using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Molecule.WebSite.Services;
using Molecule.MvcWebSite.atomes.admin.Controllers;
using Molecule.MvcWebSite.atomes.admin;

namespace Molecule.MvcWebSite.Controllers
{
    [HandleError]
    public class HomeController : PublicPageControllerBase<Molecule.MvcWebSite.atomes.admin.Atome>
    {

        public ActionResult Index()
        {
            if (AdminService.IsSetupAuthorized)
                return RedirectToAction<SetupController>(c => c.Index());
           return View();
        }
    }
}
