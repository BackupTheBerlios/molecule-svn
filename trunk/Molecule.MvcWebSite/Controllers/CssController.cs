using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Molecule.Web;
using Mono.Rocks;
using System.Web.Caching;
using Molecule.IO;
using Molecule.WebSite.Services;
using System.IO;

namespace Molecule.MvcWebSite.Controllers
{
    

    public class CssController : Controller
    {
        static DateTime cacheDate = DateTime.MinValue;

        //
        // GET: /Css/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Molecule()
        {
            if (!System.IO.File.Exists(AdminService.CssCachePath) || AdminService.LastCssVariablesUpdate > cacheDate)
            {
                cacheDate = AdminService.LastCssVariablesUpdate;
                string css = System.IO.File.ReadAllText(Request.PhysicalPath);
                css = CssVariablesExtender.ExpandVariables(css,
                    pairs => AdminService.GetCssVariables().ForEach(
                        cvi => pairs[cvi.Key] = cvi.Value)
                    );
                new DirectoryInfo(Path.GetDirectoryName(AdminService.CssCachePath)).Create(true);
                System.IO.File.WriteAllText(AdminService.CssCachePath, css);
            }

            Response.Cache.SetExpires(DateTime.Now.Add(TimeSpan.FromDays(30)));
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetValidUntilExpires(false);

            Response.WriteFile(AdminService.CssCachePath);
            return null;
        }

    }
}
