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

namespace Upload
{
    public partial class Preferences : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.destinationDirectoryPathTextBox.Attributes.Add("onkeyup", String.Format(@"destinationDirectoryTextChanged({0},{1});",this.destinationDirectoryPathTextBox.ClientID, this.preferencesButton.ClientID));
        }

        protected void preferencesButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.destinationDirectoryPathTextBox.Text))
            {
                Upload.UploadService.DestinationPath = this.destinationDirectoryPathTextBox.Text;
                this.destinationDirectoryPathTextBox.Text = String.Empty;
            }
        }


    }
}
