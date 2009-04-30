using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using Molecule.Web;
using Mono.Rocks;
using System.Web.Caching;
using Molecule.IO;
using Molecule.WebSite.Services;

namespace Molecule.WebSite
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CssHandler : IHttpHandler
    {//Still not generic, must only handle molecule.css
        static DateTime cacheDate = DateTime.MinValue;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/css";
            if (!File.Exists(AdminService.CssCachePath) || AdminService.LastCssVariablesUpdate > cacheDate)
            {
                cacheDate = Services.AdminService.LastCssVariablesUpdate;
                string css = File.ReadAllText(context.Request.PhysicalPath);
                css = CssVariablesExtender.ExpandVariables(css,
                    pairs => Services.AdminService.GetCssVariables().ForEach(
                        cvi => pairs[cvi.Key] = cvi.Value)
                    );
                new DirectoryInfo(Path.GetDirectoryName(AdminService.CssCachePath)).Create(true);
                File.WriteAllText(AdminService.CssCachePath, css);
            }

            context.Response.Cache.SetExpires(DateTime.Now.Add(TimeSpan.FromDays(30)));
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetValidUntilExpires(false);

            context.Response.WriteFile(AdminService.CssCachePath);
            return;
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
