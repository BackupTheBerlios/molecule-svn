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
                PreviousYear = year.Value - 1,
                NextYear = year.Value + 1,
                Items = (from i in 12.Times()
                         select CalendarItem.CreateMonth(new DateTime(year.Value, i + 1, 1), tagId)).ToList()
            });
        }

        public static string YearUrl(UrlHelper helper, int? year, ITagInfo tag)
        {
            return helper.Action("Year", "Calendar", new { year = year, tagId = tag != null ? tag.Id : null });
        }

        public ActionResult Month(int year, int month, string tagId)
        {
            return View(new MonthCalendarData()
            {
                Tag = !String.IsNullOrEmpty(tagId) ? PhotoLibrary.GetTag(tagId) : null,
                Year = year,
                PreviousMonth = month == 1 ? 12 : month - 1,
                NextMonth = month == 12 ? 1 : month + 1,
                PreviousYear = month == 1 ? year - 1 : year,
                NextYear = month == 12 ? year + 1 : year,
                Month = month,
                Items = getCalendarItems(year, month, tagId)
            });
        }

        public static string MonthUrl(UrlHelper helper, int year, int month, ITagInfo tag)
        {
            return helper.Action("Month", "Calendar", new { year = year, month = month, tagId = tag != null ? tag.Id : null });
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
