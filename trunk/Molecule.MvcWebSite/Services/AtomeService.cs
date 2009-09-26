//
// AtomeService.cs
//
// Copyright (c) 2009 Pascal Fresnay (dev.molecule@free.fr) - Mickael Renault (dev.molecule@free.fr) 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Molecule.Web;
using Molecule.Serialization;
using System.IO;
using log4net;
using System.Text.RegularExpressions;
using Molecule.Configuration;
using Molecule.Collections;
using Mono.Rocks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Molecule.WebSite.Services
{
    public class AtomeService
    {
        static ILog log = LogManager.GetLogger(typeof(AtomeService));
        List<AtomeInfo> atomes;
        const string atomesBaseDir = "~/atomes/";
        private AtomeService()
        {
            initAtomes();
        }

        private void initAtomes()
        {
            atomes = new List<AtomeInfo>();
            string atomesPath = HttpContext.Current.Server.MapPath(atomesBaseDir);
            var atomesPathInfo = new DirectoryInfo(atomesPath);
            if (log.IsDebugEnabled)
                log.Debug("Search for atomes in directory " + atomesPath);
            foreach (var atomeDir in atomesPathInfo.GetDirectories())
            {
                string virtualAtomeDir = VirtualPathUtility.Combine(atomesBaseDir, atomeDir.Name);
                string atomeDescriptionPath = Path.Combine(atomeDir.FullName, "Atome.xml");
                if (File.Exists(atomeDescriptionPath))
                {
                    var atomeInfo = new AtomeInfo(Molecule.Serialization.Atome.LoadFrom(atomeDescriptionPath), virtualAtomeDir);
                    atomes.Add(atomeInfo);
                    if (log.IsInfoEnabled)
                        log.Info("Found atome " + atomeInfo.Id);
                }
                else
                    log.Warn("Atome contained in " + atomeDir.Name + " directory doesn't contain an Atome.xml description file, will not be used.");
            }
        }

        private static AtomeService instance { get { return Singleton<AtomeService>.Instance; } }

        public static IEnumerable<IAtomeInfo> GetAtomes()
        {
            return instance.atomes.Cast<IAtomeInfo>();
        }

        static Regex virtualPathRegex = new Regex(@"/atomes/(?<atome>[^/]+)", RegexOptions.Compiled);

        public static bool IsAtome(HttpContext context)
        {
            return GetAtome(context) != null;
        }

        public static IAtomeInfo CurrentAtome
        {
            get
            {

                return GetAtome(HttpContext.Current);
            }
        }

        public static bool CurrentPathIsAtome
        {
            get
            {
                
                return IsAtome(HttpContext.Current);
            }
        }

        public static IAtomeInfo GetAtome(HttpContext context)
        {
            var rd = RouteTable.Routes.GetRouteData(new HttpContextWrapper(context));
            object atomeName;
            rd.Values.TryGetValue("atome", out atomeName);
            if (atomeName == null || String.IsNullOrEmpty((string)atomeName))
                return null;
            else
                return GetAtome((string)atomeName);
            //Match match = virtualPathRegex.Match(virtualPath);
            //if (match.Success)
            //{
            //    var atomeDir = match.Groups[1].Value;
            //    var atomeVirtualPath = VirtualPathUtility.Combine(atomesBaseDir, atomeDir);
            //    return GetAtomes().First(atome => atome.Path.Equals(atomeVirtualPath));
            //}
            //else return null;
        }

        public static IAtomeInfo GetAtome(string atomeId)
        {
            return GetAtomes().First(a => String.Compare(a.Id, atomeId, true) == 0);
        }

        public static IEnumerable<IAtomeInfo> GetAtomesWithPreferences()
        {
            return from atome in GetAtomes()
                   where atome.HasPreferences
                   select atome;
        }

		public static bool IsCurrentUserAuthorizedForAtome(string atomeId)
		{
            var currentUser = HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "";
            if (currentUser == null)
                currentUser = AtomeUserAuthorizations.AnonymousUser;

            if (HttpContext.Current.User != null && HttpContext.Current.User.IsInRole(SQLiteProvidersHelper.AdminRoleName))
                return true;

            var auth = AdminService.AtomeUserAuthorizations.TryGet(atomeId, currentUser);
            return auth != null ? auth.Authorized : false;			
			
		}

		
		
        public static bool IsCurrentUserAuthorized(HttpContext context)
        {
            var atome = GetAtome(context);

            if (atome == null)
            {
                //if (url.Contains("admin") && !HttpContext.Current.User.IsInRole(SQLiteProvidersHelper.AdminRoleName))
                //    //preference path (workaround a mono bug that does not filter sitemap correctly)
                //    return false;

                return true; //only handle atome authorizations
            }

            return IsCurrentUserAuthorizedForAtome(atome.Id);
        }

		
        public static bool IsCurrentUserAuthorized()
        {
            var currentAtome = CurrentAtome;
            if (currentAtome == null)
                return true; //only handle atome authorizations

            return IsCurrentUserAuthorizedForAtome(currentAtome.Id);
        }

        public static IEnumerable<IAtomeInfo> GetAuthorizedAtomes()
        {
            return from atome in instance.atomes
                   where IsCurrentUserAuthorizedForAtome(atome.Id)
                   select atome as IAtomeInfo;
        }
    }
}