﻿//
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
                        log.Info("Found atome " + atomeInfo.Name);
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

        static Regex virtualPathRegex = new Regex(@"/atomes/(?<atome>\w+)/.*$", RegexOptions.Compiled);

        public static bool IsAtomeVirtualPath(string virtualPath)
        {
            return virtualPathRegex.Match(virtualPath).Success;
        }

        public static IAtomeInfo CurrentAtome
        {
            get
            {
                
                return GetAtomeByVirtualPath(HttpContext.Current.Request.Path);
            }
        }

        public static bool CurrentPathIsAtome
        {
            get
            {
                return IsAtomeVirtualPath(HttpContext.Current.Request.Path);
            }
        }

        public static IAtomeInfo GetAtomeByVirtualPath(string virtualPath)
        {
            Match match = virtualPathRegex.Match(virtualPath);
            if (match.Success)
            {
                var atomeDir = match.Groups[1].Value;
                var atomeVirtualPath = VirtualPathUtility.Combine(atomesBaseDir, atomeDir);
                return GetAtomes().First(atome => atome.Path.Equals(atomeVirtualPath));
            }
            else return null;
        }

        public static IEnumerable<IAtomeInfo> GetAtomesWithAdminWebControl()
        {
            return from atome in GetAtomes()
                   where atome.HasPreferencesPage
                   select atome;
        }

        internal static bool IsCurrentUserAuthorized()
        {
            var currentAtome = CurrentAtome;
            if (currentAtome == null)
                return true; //only handle atome authorizations

            var currentUser = HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "";
            if (currentUser == null)
                return false;

            //todo use atomesAuthorizedUsers
            return true;
        }

        private DataTable atomesAuthorizedUsers;
        private object authLock = new object();

        const string confNamespace = "Molecule";
        const string confAtomesUsersAuthorizationsKey = "AtomesUsersAuthorizations";

        public static DataTable GetAtomesAuthorizations()
        {
            if (instance.atomesAuthorizedUsers == null)
            {
                lock (instance.authLock)
                {
                    if (instance.atomesAuthorizedUsers == null)
                    {
                        instance.atomesAuthorizedUsers = ConfigurationClient.Get(
                            confNamespace, confAtomesUsersAuthorizationsKey,
                            new DataTable());
                        instance.syncAuthorizations();
                    }
                }
            }
            return instance.atomesAuthorizedUsers;
        }

        public static void SetAtomesAuthorizations(DataTable auth)
        {
            lock(instance.authLock)
            {
                instance.atomesAuthorizedUsers = auth;
                ConfigurationClient.Set(confNamespace, confAtomesUsersAuthorizationsKey,
                instance.atomesAuthorizedUsers);
            }
        }

        //sync with known user and atome
        private void syncAuthorizations()
        {
            var oldData = atomesAuthorizedUsers;
            var res = new DataTable();
            res.Columns.Add("atome_name", typeof(string));

            foreach (MembershipUser user in Membership.GetAllUsers())
                res.Columns.Add(user.UserName, typeof(bool));

            foreach (var atome in AtomeService.GetAtomes())
            {
                var values = (from row in oldData.AsEnumerable()
                              where (string)row["row_name"] == atome.Name
                              select row.ItemArray).FirstOrDefault();

                if (values == null)
                {
                    values = new object[res.Columns.Count];
                    values[0] = atome.Name;
                    for (int i = 1; i < values.Length; i++)
                        values[i] = false;
                }

                var newRow = res.NewRow();
                newRow.ItemArray = values;
                res.Rows.Add(newRow);
            }
            atomesAuthorizedUsers = res;
        }
    }
}
