using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Molecule.Collections;
using WebPhoto.Services;

namespace Molecule.WebSite.atomes.photo
{
    public partial class FullSizePhoto : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IKeyedEnumerable<string, string> Metadatas
        {
            set
            {
                MetadatasGridView.DataSource = value;
                MetadatasGridView.DataBind();
            }
        }

		public string Loaded
		{
			set
			{
				image.Attributes.Add("onLoad", value);
			}
		}
		
        public string PhotoId
        {
            set { image.ImageUrl = PhotoFile.GetUrlFor(value, PhotoFileSize.Normal); }
        }
    }
}