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

namespace Molecule.WebSite.atomes.photo
{
    public partial class Map : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}