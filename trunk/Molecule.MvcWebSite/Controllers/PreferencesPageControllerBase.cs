using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Molecule.MvcWebSite.Models;
using Molecule.WebSite.Services;

namespace Molecule.MvcWebSite.Controllers
{
    [Authorize(Roles = SQLiteProvidersHelper.AdminRoleName)]
    public abstract class PreferencesPageControllerBase : PageControllerBase
    {
        public PreferencesPageControllerBase()
        {
            ViewData["PreferencesPageData"] = new PreferencesPageData()
            {
                Atomes = AtomeService.GetAtomesWithPreferences()
            };
        }
    }
}
