using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Molecule.MvcWebSite.Models;
using Molecule.WebSite.Services;
using System.Web.Security;
using System.Linq.Expressions;

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


        protected RedirectToRouteResult RedirectToAction<T>(Expression<Action<T>> action, string atomeId)
            where T : PublicPageControllerBase
        {
            var res = MvcContrib.ControllerExtensions.RedirectToAction<T>(this, action);
            if (!String.IsNullOrEmpty(atomeId))
                res.RouteValues.Add("atome", atomeId);
            return res;
        }

        protected RedirectToRouteResult RedirectToAction<T>(Expression<Action<T>> action)
            where T : PublicPageControllerBase
        {
            var currentAtome = AtomeService.CurrentAtome;
            return RedirectToAction<T>(action, currentAtome != null ? currentAtome.Id : null);
        }
    }
}
