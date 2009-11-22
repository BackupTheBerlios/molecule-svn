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
    public abstract class PageControllerBase<TA> : PublicPageControllerBase<TA>
        where TA : IAtome
    {
        public PageControllerBase()
            : base()
        {
        }
    }

    public class PublicPageControllerBase : Controller
    {
        public static IAtomeInfo GetAtome<TC>()
            where TC : PublicPageControllerBase
        {
            return AtomeService.GetAtome(typeof(TC).BaseType.GetGenericArguments().First());
        }   
    }

    public abstract class PublicPageControllerBase<TA> : PublicPageControllerBase
        where TA : IAtome
    {
        public PublicPageControllerBase()
        {
            ViewData["PageData"] = new PageData()
            {
                Atomes = AtomeService.GetAuthorizedAtomes()
            };
        }

        //public IAtomeInfo Atome
        //{
        //    get {
        //        return AtomeService.GetAtome<TA>();
        //    }
        //}

        protected RedirectToRouteResult RedirectToAction<TC>(Expression<Action<TC>> action)
            where TC : PublicPageControllerBase
        {
            var res = MvcContrib.ControllerExtensions.RedirectToAction<TC>(this, action);
            res.RouteValues.Add("atome", GetAtome<TC>().Id);
            return res;
        }

        
    }
    public class test : PublicPageControllerBase<Molecule.Atomes.Documents.Atome>
    {
    }
}
