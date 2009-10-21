using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Molecule.WebSite.Services;
using Molecule.MvcWebSite.atomes.admin.Controllers;

namespace Molecule.MvcWebSite.Controllers
{
    [HandleError]
    public class HomeController : PageControllerBase
    {


        public ActionResult Index()
        {
            if (AdminService.IsSetupAuthorized)
                return RedirectToAction<SetupController>(c => c.Index(), Molecule.MvcWebSite.atomes.admin.Atome.Id);

            Console.WriteLine("UserLanguages : " + Request.UserLanguages.Aggregate("", (s1, s2) => s1 + " " + s2));
            Console.WriteLine("UICulture : "+System.Globalization.CultureInfo.CurrentUICulture.Name);
            return View();
        }
    }
}
