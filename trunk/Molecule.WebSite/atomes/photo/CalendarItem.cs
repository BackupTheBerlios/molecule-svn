using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using WebPhoto.Services;

namespace Molecule.WebSite.atomes.photo
{
    public class CalendarItem
    {
        public string PhotoId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateTime { get; set; }

        public bool IsEmpty { get { return String.IsNullOrEmpty(Name); } }

        public static CalendarItem CreateDay(DateTime d, int month, string tagId)
        {
            CalendarItem item = new CalendarItem();
            if (d.Month == month)
            {
                item.DateTime = d;
                item.Name = d.Day.ToString();
                var photos = PhotoLibrary.GetPhotosByDayAndTag(d, tagId);
                var photo = photos.FirstOrDefault();
                item.Description = photos.Count() + " photos";
                if (photo != null)
                {
                    item.PhotoId = photo.Id;
                }
            }
            return item;
        }

        public static CalendarItem CreateMonth(DateTime month, string tagId)
        {
            CalendarItem item = new CalendarItem();
            item.DateTime = month;
            item.Name = DateTimeFormatInfo.CurrentInfo.GetMonthName(month.Month);
            var photos = PhotoLibrary.GetPhotosByMonthAndTag(month, tagId);
            var photo = photos.FirstOrDefault();
            if (photo != null)
                item.PhotoId = photo.Id;
            return item;
        }
    }
}
