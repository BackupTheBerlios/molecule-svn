using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPhoto.Services;

namespace Molecule.WebSite.atomes.photo
{
    public partial class TagHierarchy : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!String.IsNullOrEmpty(TagQueryStringField))
                TagId = Request.QueryString[TagQueryStringField];

            if (!String.IsNullOrEmpty(TagId))
            {
                this.TagHierarchyView.DataSource =
                    PhotoLibrary.GetTagHierarchy(TagId);
                this.TagHierarchyView.DataBind();
            }

            if (Year.HasValue)
                YearLink.NavigateUrl = YearCalendar.GetUrlFor(new DateTime(Year.Value, 1, 1), TagId);
            else
                YearLink.Visible = false;

            if (Month.HasValue)
                MonthLink.NavigateUrl = MonthCalendar.GetUrlFor(new DateTime(Year.Value, Month.Value, 1), TagId);
            else
                MonthLink.Visible = false;

        }

        public string TagId { get; set; }

        public string TagQueryStringField { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }
    }
}