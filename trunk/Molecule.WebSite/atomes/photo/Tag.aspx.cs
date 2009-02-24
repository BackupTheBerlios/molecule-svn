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

namespace Molecule.WebSite.atomes.photo
{
    public partial class Tag : System.Web.UI.Page
    {
        protected string tagId;

        protected void Page_Load(object sender, EventArgs e)
        {
            tagId = Request.QueryString["id"];
            initContent();
            initTitle();
        }

        private void initContent()
        {
            this.PhotoListView.DataSource = PhotoLibrary.GetPhotosByTag(tagId).ToList();
            this.PhotoListView.DataBind();
        }

        private void initTitle()
        {
            Title = "Photos" + PhotoLibrary.GetTagFullPath(tagId);
        }
    }
}
