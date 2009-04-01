using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Molecule.WebSite.Services;

namespace Molecule.WebSite
{
    public partial class PreferencesPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.User.IsInRole(SQLiteProvidersHelper.AdminRoleName))
                FormsAuthentication.RedirectToLoginPage();
            this.tabDefault.CssClass = AtomeService.CurrentPathIsAtome ? "tabTitle" : "tabSelectedTitle";
            Page.Title = this.GetLocalResourceObject("Preferences").ToString();
        }
    }
}
