using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using WebPhoto.Providers;
using WebPhoto.Services;
using Molecule.MvcWebSite.atomes.photos.Data;
using Molecule.MvcWebSite.atomes.photos.Views;
using Molecule.MvcWebSite.Controllers;
using Molecule.Web.Mvc;

namespace Molecule.MvcWebSite.atomes.photos.Controllers
{
    public class TagController : PageControllerBase
    {


        public ActionResult Index(string id)
        {
            ITagInfo tag = null;
            if (!String.IsNullOrEmpty(id))
                tag = PhotoLibrary.GetTag(id);

            return View(new TagIndexData()
            {
                SubTags = PhotoLibrary.GetTagsByTag(id),
                Photos = PhotoLibrary.GetPhotosByTag(id).ToList(),
                Tag = tag
            });
        }

        public ActionResult Zip(string id)
        {
            return new ZipPhotoFilesResult()
            {
                Tag = PhotoLibrary.GetTag(id),
                Photos = PhotoLibrary.GetPhotosByTag(id)
            };
        }
    }
}
