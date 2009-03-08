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

namespace Molecule.WebSite
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CssHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/css";
            
            string css = File.ReadAllText(context.Request.PhysicalPath);
            css = CssVariablesExtender.ExpandVariables(css, context.Request.QueryString);

            context.Response.Write(css);
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
