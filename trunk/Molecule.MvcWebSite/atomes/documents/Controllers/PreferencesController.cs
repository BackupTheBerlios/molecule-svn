using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;

namespace Molecule.Atomes.Documents.Controllers
{
    public class PreferencesController : PreferencesPageControllerBase<Atome>
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save()
        {
            return RedirectToAction<PreferencesController>(c => c.Index());
        }
    }
}
