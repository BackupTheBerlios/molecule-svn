using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPhoto.Services;
using WebPhoto.Providers;

namespace Molecule.WebSite.atomes.photo
{
    public partial class TagLink : System.Web.Mvc.ViewUserControl<TagLinkData>
    {
    }

    public class TagLinkData
    {
        public string TagId { get; set; }
        public bool TextOnly { get; set; }
    }
}