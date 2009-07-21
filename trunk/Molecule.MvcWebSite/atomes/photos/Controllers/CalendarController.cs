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

namespace Molecule.MvcWebSite.atomes.photos.Controllers
{
    public class CalendarController : Controller
    {   
        public ActionResult Year(int? year, string tagId)
        {
            if (!year.HasValue)
                year = DateTime.Now.Year;

            ITagInfo tag = null;

            if (!String.IsNullOrEmpty(tagId))
            {
                year = PhotoLibrary.GetFirstPhotoByTag(tagId).Date.Year;
            }

            var data = new YearCalendarData()
            {
                Tag = tag,
                Year = year.Value,
                Items = (from i in 12.Times()
                         select CalendarItem.CreateMonth(new DateTime(year.Value, i+1, 1), tagId)).ToList()
            };

            return View(data);
        }

        public ActionResult Month(int year, int month)
        {
            return View();
        }

    }
}
