using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;

namespace Molecule.WebSite.atomes.photo
{
    public partial class PhotoLink : System.Web.Mvc.ViewUserControl
    {
        public string TagId { get; set; }
        public string PhotoId { get; set; }
        public string Description { get; set; }
        public string HoverIconUrl { get; set; }
        public string HoverText { get; set; }
        public string NavigateUrl { get; set; }
    }
}