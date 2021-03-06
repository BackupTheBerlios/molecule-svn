﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;
using Molecule.Atomes.Documents.Data;
using System.IO;

namespace Molecule.Atomes.Documents.Controllers
{
    public class FolderController : PageControllerBase<Atome>
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

            return View(new FolderDisplayData() {
                CurrentFolder = folder,
                Folders = Service.GetFolders(folder),
                Documents = Service.GetDocuments(folder),
                CurrentFolderHierarchy = Service.GetFolderHierarchy(folder)
            });
        }

        public FileResult File(string filePath)
        {
            return new FilePathResult(Service.GetDocument(filePath).Path, "application/octet-stream");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create(string parentPath)
        {
            return View("Create", new FolderCreateData(){
                CurrentFolder= Service.GetFolder(parentPath)
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string parentPath, string name)
        {
            var fi = Service.CreateSubdirectory(parentPath, name);
            return RedirectToAction<FolderController>(c => c.Display(fi.Id));
        }

        public ActionResult Delete(string path)
        {
            Service.Delete(path);
            return RedirectToAction<FolderController>(c => c.Display(path));
        }


        public ActionResult AddDocument(string folderPath)
        {
            if (Request.RequestType == HttpVerbs.Post.ToString() && Request.Files.Count > 0) {
                foreach (string fileId in Request.Files) {
                    var file = Request.Files[fileId];
                    file.SaveAs(Path.Combine(Service.GetFolder(folderPath).Path, file.FileName));
                }

                return RedirectToAction<FolderController>(c => c.Display(folderPath));
            }
            else return View("AddDocument", new FolderAddDocumentData(){
                CurrentFolder= Service.GetFolder(folderPath)
            });
        }
    }
}
