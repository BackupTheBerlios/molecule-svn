using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Molecule.WebSite.Services;

namespace Molecule.MvcWebSite.Controllers
{
    [HandleError]
    public class HomeController : PageControllerBase
    {
        public ActionResult Index()
        {
            if (AdminService.IsSetupAuthorized)
                return RedirectToAction("Index", "Setup", new { atome = "admin" });

            return View();
        }
    }
}
