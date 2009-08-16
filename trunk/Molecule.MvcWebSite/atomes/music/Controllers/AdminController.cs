using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;
using Molecule.MvcWebSite.atomes.music.Data;
using WebMusic.Services;

namespace Molecule.MvcWebSite.atomes.music.Controllers
{
    public class AdminController : PreferencesPageControllerBase
    {
        public ActionResult Index()
        {
            return View(new AdminData() { Providers = MusicLibrary.Providers });
        }

    }
}
