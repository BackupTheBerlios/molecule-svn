using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Molecule.MvcWebSite.Models;
using Molecule.WebSite.Services;
using System.Web.Security;

namespace Molecule.MvcWebSite.Controllers
{
    [Molecule.MvcWebSite.Mvc.Authorize()]
    public abstract class PageControllerBase : PublicPageControllerBase
    {
        public PageControllerBase()
            : base()
        {
        }
    }

    public abstract class PublicPageControllerBase : Controller
    {
        public PublicPageControllerBase()
        {
            ViewData["PageData"] = new PageData()
            {
                Atomes = AtomeService.GetAuthorizedAtomes()
            };
        }
    }
}
