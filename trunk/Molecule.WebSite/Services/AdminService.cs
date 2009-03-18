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
using Molecule.Web;
using System.Collections.Generic;
using System.Web.Configuration;
using System.IO;
using Molecule.Configuration;
using Mono.Rocks;

namespace Molecule.WebSite.Services
{
    public class AdminService
    {
        private AdminService()
        {
            moleculeTitle = Configuration.ConfigurationClient.Get<string>("Molecule.WebSite", "Title", "Molecule");
        }

        private string moleculeTitle;

        protected IEnumerable<CssVariableInfo> CssVariables = null;

        private static AdminService instance { get { return Singleton<AdminService>.Instance; } }

        static PagesSection pagesSection = (PagesSection)WebConfigurationManager.GetWebApplicationSection("system.web/pages");
        static string cssVariablesKeyConf = "CssVariables." + pagesSection.Theme;
        const string confAtomeUserAuthorizationsKey = "AtomeUserAuthorizations";
        const string confNamespace = "Molecule.Admin";

        static object cssVariablesLock = new object();

        public static DateTime LastCssVariablesUpdate { get; set; }

        public static IEnumerable<CssVariableInfo> GetCssVariables()
        {
            if (instance.CssVariables == null)
            {
                lock (cssVariablesLock)
                {
                    if (instance.CssVariables == null)
                    {
                        //init css variables from main css content.
                        string mainCssFile = HttpContext.Current.Server.MapPath("~/App_Themes/" + pagesSection.Theme + "/molecule.css");

                        instance.CssVariables = (from kvp in CssVariablesExtender.ExtractVariables(File.ReadAllText(mainCssFile))
                                                 select new CssVariableInfo() { Key = kvp.Key, Value = kvp.Value }
                                                ).ToList();

                        //override with saved configuration
                        string savedVariablesConf = ConfigurationClient.Client.Get<string>(confNamespace, cssVariablesKeyConf, "");

                        if (!String.IsNullOrEmpty(savedVariablesConf))
                        {
                            var savedVariables = CssVariableInfo.Deserialize(savedVariablesConf);
                            instance.CssVariables.ForEach(cvi =>
                            {
                                var over = savedVariables.FirstOrDefault(sv => sv.Key == cvi.Key);
                                if (over != null)
                                    cvi.Value = over.Value;
                            });
                        }
                    }
                }
            }
            return instance.CssVariables;
        }

        

        public static void UpdateCssVariable(CssVariableInfo info)
        {
            var cssVariables = GetCssVariables();
            cssVariables.First(cvi => cvi.Key == info.Key).Value = info.Value;
            string conf = CssVariableInfo.Serialize(cssVariables);
            ConfigurationClient.Client.Set<string>(confNamespace, cssVariablesKeyConf, conf);
            LastCssVariablesUpdate = DateTime.Now;
        }

        internal static void ResetCssVariables()
        {
            LastCssVariablesUpdate = DateTime.Now;
            ConfigurationClient.Client.Set<string>(confNamespace, cssVariablesKeyConf, "");
            instance.CssVariables = null;
        }

        public static string MoleculeTitle
        {
            get { return instance.moleculeTitle; }
            set { instance.updateMoleculeTitle(value); }
        }

        private void updateMoleculeTitle(string value)
        {
            if (value.Length == 0 || value.Length > 100)
                return;
            instance.moleculeTitle = value;
            Configuration.ConfigurationClient.Set<string>("Molecule.WebSite", "Title", value);
        }

        private AtomeUserAuthorizations atomeUserAuthorizations;
        private object authLock = new object();

        public static AtomeUserAuthorizations AtomeUserAuthorizations
        {
            get
            {
                if (instance.atomeUserAuthorizations == null)
                    instance.initAuthorizations();

                return instance.atomeUserAuthorizations;
            }
        }

        private void initAuthorizations()
        {
            lock (authLock)
            {
                if (atomeUserAuthorizations == null)
                {
                    var oldData = ConfigurationClient.Get<AtomeUserAuthorizations>(
                        confNamespace, confAtomeUserAuthorizationsKey, null);
                    atomeUserAuthorizations = new AtomeUserAuthorizations(oldData);
                }
            }
        }

        public static void SetAtomeUserAuthorization(string user, string atome, bool auth)
        {
            lock (instance.authLock)
            {
                instance.atomeUserAuthorizations.Set(user, atome, auth);
                instance.saveAuthorizations();
            }
        }

        protected void saveAuthorizations()
        {
            ConfigurationClient.Set(confNamespace, confAtomeUserAuthorizationsKey,
            atomeUserAuthorizations);
        }

        public static void SaveAtomeUserAuthorizations()
        {
            instance.saveAuthorizations();
        }
    }
}
