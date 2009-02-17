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
using Molecule.Runtime;

namespace WebPhoto.Services
{
    public class ProviderInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Id { get { return Name; } }
    }
}
