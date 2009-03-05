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
using WebPhoto.Providers;
using System.Collections.Generic;

namespace Molecule.WebSite.atomes.photo
{
    public partial class TagList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            list.DataSource = Tags;
            list.DataBind();
        }

        public IEnumerable<ITagInfo> Tags { get; set; }
    }
}