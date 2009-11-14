using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;

namespace Molecule.Atomes.Documents.Controllers
{
    public class DefaultController : PageControllerBase
    {
        /// <summary>
        /// Default action.
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
    }
}
