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
            return RedirectToAction<FolderController>(c => c.Display(""));
        }

        public ActionResult Display(string path)
        {
            if (path == null)
                path = "";
            var folder = Service.GetFolder(path);

            return View(new FolderIndexData() {
                CurrentFolder = folder,
                Folders = Service.GetFolders(folder),
                Documents = Service.GetDocuments(folder)
            });
        }

        public FileResult File(string filePath)
        {
            return new FilePathResult(Service.GetDocument(filePath).Path, "application/octet-stream");
        }
    }
}
