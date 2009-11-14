using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;
using Molecule.Atomes.Documents.Data;

namespace Molecule.Atomes.Documents.Controllers
{
    public class FolderController : PageControllerBase
    {
        /// <summary>
        /// Default action.
        /// </summary>
        public ActionResult Index()
        {
            return View(new FolderIndexData() {
                CurrentFolder = Service.GetRootFolder()
            });
        }
    }
}
