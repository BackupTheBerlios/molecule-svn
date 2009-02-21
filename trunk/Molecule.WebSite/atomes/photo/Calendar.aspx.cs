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
using WebPhoto.Providers;
using System.Globalization;
using System.Collections.Generic;
using WebPhoto.Services;

namespace Molecule.WebSite.atomes.photo
{
    public partial class Calendar1 : System.Web.UI.Page
    {
        const string requestFormat = "{0}?date={1}/{2:00}";

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime day = DateTime.Today;
            string date = Request.QueryString["date"];
            if (!String.IsNullOrEmpty(date))
                day = DateTime.ParseExact(date, "yyyy/MM", null);

            fillCalendar(day);

            var previousMonth = day.Month == 1 ? 12 : day.Month - 1;
            var previousYear = day.Month == 1 ? day.Year - 1 : day.Year;
            this.HyperLinkPrevious.NavigateUrl = String.Format(requestFormat,
                Request.Path, previousYear, previousMonth);

            var nextMonth = day.Month == 12 ? 1 : day.Month + 1;
            var nextYear = day.Month == 12 ? day.Year + 1 : day.Year;
            this.HyperLinkNext.NavigateUrl= String.Format(requestFormat,
                Request.Path, nextYear, nextMonth);

        }

        private void fillCalendar(DateTime day)
        {
            var firstDayOfMonth = new DateTime(day.Year, day.Month, 1);
            var firstVisibleDay = firstDayOfMonth;
            while (firstVisibleDay.DayOfWeek != DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
                firstVisibleDay -= TimeSpan.FromDays(1);
            List<CalendarItem> items = new List<CalendarItem>();

            //fake data
            var photos = PhotoLibrary.GetPhotos().ToArray();
            int i = 0;

            for (DateTime d = firstVisibleDay;
                d < firstVisibleDay + TimeSpan.FromDays(42);
                d += TimeSpan.FromDays(1))
            {
                if (d.Month == day.Month)
                    items.Add(new CalendarItem() { Day = d.Day, ThumbnailUrl = Thumbnail.GetUrlFor(photos[i]) });
                else
                    items.Add(new CalendarItem() { Day = d.Day, ThumbnailUrl = null });
                i++;
            }
            
            this.ListView1.DataSource = items;
            this.ListView1.DataBind();
            this.LabelMonth.Text = String.Format("{0} {1}",
                DateTimeFormatInfo.CurrentInfo.GetMonthName(firstDayOfMonth.Month),
                firstDayOfMonth.Year.ToString());
        }

        public static string FormatDay(int weekDay)
        {
            var day = (DayOfWeek)(((int)DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek + weekDay) % 7);
            return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(day);
        }
    }

    public class CalendarItem
    {
        public bool HasThumbnail { get { return !String.IsNullOrEmpty(ThumbnailUrl); } }
        public string ThumbnailUrl { get; set; }
        public int Day { get; set; }
    }
}
