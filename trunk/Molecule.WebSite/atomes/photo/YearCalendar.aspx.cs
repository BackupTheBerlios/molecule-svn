using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using WebPhoto.Services;

namespace Molecule.WebSite.atomes.photo
{
    public partial class YearCalendar : System.Web.UI.Page
    {
        const string requestFormat = "{0}?date={1}";

        protected string tagId;

        protected void Page_Load(object sender, EventArgs e)
        {
            tagId = Request.QueryString["tag"];
            DateTime year = DateTime.Today;
            string date = Request.QueryString["date"];
            if (!String.IsNullOrEmpty(date))
                year = DateTime.ParseExact(date, "yyyy", null);
            else if (!String.IsNullOrEmpty(tagId))
                year = PhotoLibrary.GetFirstPhotoByTag(tagId).Date;

            fillCalendar(year);
            initYearLinks(year);
            initTitle();
            this.TagHierarchy.Year = year.Year;
        }

        private void initTitle()
        {
            Title = "Photos";
        }

        private void initYearLinks(DateTime year)
        {
            initMonthLink(this.HyperLinkPrevious, year.Year - 1);
            initMonthLink(this.HyperLinkNext, year.Year + 1);
        }

        private void initMonthLink(HyperLink link, int year)
        {
            link.NavigateUrl = GetUrlFor(new DateTime(year, 1, 1), tagId);
            link.ToolTip = year.ToString();
        }
        

        public static string GetUrlFor(DateTime year, string tagId)
        {
            string url = String.Format(requestFormat, "YearCalendar.aspx", year.Year);
            if (!String.IsNullOrEmpty(tagId))
                url += "&tag=" + HttpUtility.UrlEncode(tagId);
            return url;
        }

        private void fillCalendar(DateTime year)
        {
            List<CalendarItem> items = new List<CalendarItem>();

            for(int i = 1; i <= 12; i++)
                items.Add(CalendarItem.CreateMonth(new DateTime(year.Year, i, 1), tagId));

            this.ListView1.DataSource = items;
            this.ListView1.DataBind();
        }

    }
}
