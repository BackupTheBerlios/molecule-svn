using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;

namespace Molecule.MvcWebSite.atomes.music.Controllers
{
    public class PlayerController : PageControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
