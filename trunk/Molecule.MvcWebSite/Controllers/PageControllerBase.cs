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
    public abstract class PageControllerBase : Controller
    {
        public PageControllerBase()
        {
            var pageData = new PageData()
            {
                Atomes = AtomeService.GetAtomes()
            };
            ViewData["PageData"] = pageData;
        }
    }
}
