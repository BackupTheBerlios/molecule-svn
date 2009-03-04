﻿using System;
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

            fillCalendar(day);
            initMonthLinks(day);
            initTitle();

        }

        public static string GetUrlFor(DateTime month, string tagId)
        {
            string url = String.Format(requestFormat, "MonthCalendar.aspx", month.Year, month.Month);
            if(!String.IsNullOrEmpty(tagId))
                url += "&tag=" + HttpUtility.UrlEncode(tagId);
            return url;
        }

        private void getPhotos(DateTime day)
        {
            throw new NotImplementedException();
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
            link.Text = String.Format("{0} {1}",
                DateTimeFormatInfo.CurrentInfo.GetMonthName(month),
                year);
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
                items.Add(new CalendarItem(d, day.Month, tagId));
            
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
        public CalendarItem(DateTime d, int month, string tagId)
        {
            if (d.Month == month)
            {
                Day = d.Day;
                var photos = PhotoLibrary.GetPhotosByDayAndTag(d, tagId);
                var photo = photos.FirstOrDefault();
                PhotoCount = photos.Count();
                if (photo != null)
                {
                    PhotoId = photo.Id;
                }
            }
        }

        public string PhotoId { get; set; }
        
        public bool IsCurrentMonth { get { return Day.HasValue; } }
        
        public int? Day { get; set; }
        
        public int PhotoCount { get; set; }
    }
}
