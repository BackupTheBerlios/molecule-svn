using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;

namespace Molecule.Template.Controllers
{
    public class PreferencesController : PreferencesPageControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
