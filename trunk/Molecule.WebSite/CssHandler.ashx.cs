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

namespace Molecule.WebSite
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CssHandler : IHttpHandler
    {
        static string cssCachePath = Path.Combine(Path.Combine(XdgBaseDirectorySpec.GetUserDirectory("XDG_CACHE_HOME", ".molecule"), "www"), "molecule.css");
        static DateTime cacheDate = DateTime.MinValue;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/css";
            if (!File.Exists(cssCachePath) || Services.AdminService.LastCssVariablesUpdate > cacheDate)
            {
                cacheDate = Services.AdminService.LastCssVariablesUpdate;
                string css = File.ReadAllText(context.Request.PhysicalPath);
                css = CssVariablesExtender.ExpandVariables(css,
                    pairs => Services.AdminService.GetCssVariables().ForEach(
                        cvi => pairs[cvi.Key] = cvi.Value)
                    );
                FileInfo fi = new FileInfo(cssCachePath);
                new DirectoryInfo(Path.GetDirectoryName(cssCachePath)).Create(true);
                File.WriteAllText(cssCachePath, css); 
            }

            //context.Response.Cache.SetExpires(DateTime.Now.Add(new TimeSpan(1, 0, 0, 0)));
            //context.Response.Cache.SetCacheability(HttpCacheability.Public);
            //context.Response.Cache.SetValidUntilExpires(false);
            
            context.Response.WriteFile(cssCachePath);
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
