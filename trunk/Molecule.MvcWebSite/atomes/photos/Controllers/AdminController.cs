using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;
using Molecule.MvcWebSite.atomes.photos.Data;
using WebPhoto.Services;

namespace Molecule.MvcWebSite.atomes.photos.Controllers
{
    public class AdminController : PreferencesPageControllerBase
    {
        public ActionResult Index()
        {
            var res = new AdminData()
            {
                 Providers = PhotoLibrary.Providers,
                 SelectedProviderId = PhotoLibrary.CurrentProvider
            };
            return View(res);
        }
    }
}
