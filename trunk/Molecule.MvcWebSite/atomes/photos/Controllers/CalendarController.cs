using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using WebPhoto.Services;
using Molecule.MvcWebSite.atomes.photos.Data;
using WebPhoto.Providers;
using Mono.Rocks;
using System.Globalization;

namespace Molecule.MvcWebSite.atomes.photos.Controllers
{
    public class CalendarController : Controller
    {   
        public ActionResult Year(int? year, string tagId)
        {
            ITagInfo tag = null;

            if (!String.IsNullOrEmpty(tagId))
            {
                if(!year.HasValue)
                    year = PhotoLibrary.GetFirstPhotoByTag(tagId).Date.Year;
                tag = PhotoLibrary.GetTag(tagId);
            }
            else if (!year.HasValue)
                year = DateTime.Now.Year;

            return View(new YearCalendarData()
            {
                Tag = tag,
                Year = year.Value,
                Items = (from i in 12.Times()
                         select CalendarItem.CreateMonth(new DateTime(year.Value, i + 1, 1), tagId)).ToList()
            });
        }

        public ActionResult Month(int year, int month, string tagId)
        {
            return View(new MonthCalendarData()
            {
                Tag = !String.IsNullOrEmpty(tagId) ? PhotoLibrary.GetTag(tagId) : null,
                Year = year,
                Month = month,
                Items = getCalendarItems(year, month, tagId)
            });
        }

        private IEnumerable<CalendarItem> getCalendarItems(int year, int month, string tagId)
        {
            var firstDayOfMonth = new DateTime(year, month, 1);
            var firstVisibleDay = firstDayOfMonth;
            while (firstVisibleDay.DayOfWeek != DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
                firstVisibleDay -= TimeSpan.FromDays(1);

            for (DateTime d = firstVisibleDay;
                d < firstVisibleDay + TimeSpan.FromDays(42);
                d += TimeSpan.FromDays(1))
                    yield return CalendarItem.CreateDay(d, month, tagId);
        }
    }
}
