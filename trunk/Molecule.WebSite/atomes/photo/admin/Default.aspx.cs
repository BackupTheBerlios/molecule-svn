using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using WebPhoto.Services;

namespace Molecule.WebSite.atomes.photo.admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProviderList.DataSource = PhotoLibrary.Providers;
                ProviderList.DataBind();
                ProviderList.SelectedValue = PhotoLibrary.CurrentProvider;
            }
        }

        protected void preferencesButton_Click(Object sender, CommandEventArgs e)
        {
            PhotoLibrary.CurrentProvider = ProviderList.SelectedValue;

        }
    }
}
