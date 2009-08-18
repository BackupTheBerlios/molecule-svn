using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using Molecule.MvcWebSite.atomes.admin.Data;
using System.Web.Mvc;

namespace Molecule.MvcWebSite.atomes.admin.Controllers
{
    public class PreferencesController : PreferencesPageControllerBase
    {
        public ActionResult Index()
        {
            var res = new PreferencesData();
            return View(res);
        }
    }
}
