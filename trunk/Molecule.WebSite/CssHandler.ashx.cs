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

namespace Molecule.WebSite
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CssHandler : IHttpHandler
    {
        static Regex variablesRegex = new Regex(@"@variables\s*\{(?<decl>[^\}]*)\}", RegexOptions.Compiled);
        static Regex keyValueRegex = new Regex(@"\s*(?<key>[^:]+)\s*:\s*(?<value>[^;]+)\s*;", RegexOptions.Compiled);
        static Regex variableRefRegex = new Regex(@"var\((?<var>[^\)]+)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/css";
            
            //Replace CSS Variables by CSS 2.1
            //http://disruptive-innovations.com/zoo/cssvariables/
            //TODO : cache
            //TODO : handle @import

            //search for variables definition & remove it from css
            var pairs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            string css = File.ReadAllText(context.Request.PhysicalPath);
            css = variablesRegex.Replace(css, delegate(Match m){
                foreach (Match mkv in keyValueRegex.Matches(m.Groups[1].Value))
                    pairs[mkv.Groups["key"].Value] = mkv.Groups["value"].Value;
                return "";
            });

            var queryString = context.Request.QueryString;
            for (int i = 0; i < queryString.Count; i++)
                pairs[queryString.GetKey(i)] = queryString.Get(i);

            //search for variable reference & replace it by its definition.
            css = variableRefRegex.Replace(css, m => pairs[m.Groups["var"].Value]);

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
