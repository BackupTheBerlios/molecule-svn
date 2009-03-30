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
    public partial class MonthCalendar : System.Web.UI.Page
    {
        const string requestFormat = "{0}?date={1}/{2:00}";

        protected string tagId;

        protected void Page_Load(object sender, EventArgs e)
        {
            tagId = Request.QueryString["tag"];
            DateTime day = DateTime.Today;
            string date = Request.QueryString["date"];
            if (!String.IsNullOrEmpty(date))
                day = DateTime.ParseExact(date, "yyyy/MM", null);
            else if (!String.IsNullOrEmpty(tagId))
            {
                day = PhotoLibrary.GetFirstPhotoByTag(tagId).Date;
            }

            fillCalendar(day);
            initMonthLinks(day);
            initTitle();
            TagHierarchy.Year = day.Year;
            TagHierarchy.Month = day.Month;

        }

        public static string GetUrlFor(DateTime month, string tagId)
        {
            string url = String.Format(requestFormat, "MonthCalendar.aspx", month.Year, month.Month);
            if(!String.IsNullOrEmpty(tagId))
                url += "&tag=" + HttpUtility.UrlEncode(tagId);
            return url;
        }

        private void initTitle()
        {
            Title = "Photos" + PhotoLibrary.GetTagFullPath(tagId);
        }

        private void initMonthLinks(DateTime day)
        {
            var previousMonth = day.Month == 1 ? 12 : day.Month - 1;
            var previousYear = day.Month == 1 ? day.Year - 1 : day.Year;
            initMonthLink(this.HyperLinkPrevious, previousMonth, previousYear);

            var nextMonth = day.Month == 12 ? 1 : day.Month + 1;
            var nextYear = day.Month == 12 ? day.Year + 1 : day.Year;
            initMonthLink(this.HyperLinkNext, nextMonth, nextYear);
        }

        private void initMonthLink(HyperLink link, int month, int year)
        {
            link.NavigateUrl = GetUrlFor(new DateTime(year, month, 1), tagId);
            link.ToolTip = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
        }

        private void fillCalendar(DateTime day)
        {
            var firstDayOfMonth = new DateTime(day.Year, day.Month, 1);
            var firstVisibleDay = firstDayOfMonth;
            while (firstVisibleDay.DayOfWeek != DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
                firstVisibleDay -= TimeSpan.FromDays(1);
            List<CalendarItem> items = new List<CalendarItem>();

            for (DateTime d = firstVisibleDay;
                d < firstVisibleDay + TimeSpan.FromDays(42);
                d += TimeSpan.FromDays(1))
                items.Add(CalendarItem.CreateDay(d, day.Month, tagId));
            
            this.ListView1.DataSource = items;
            this.ListView1.DataBind();
            
        }

       

        public static string FormatDay(int weekDay)
        {
            var day = (DayOfWeek)(((int)DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek + weekDay) % 7);
            return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(day);
        }
    }
}
