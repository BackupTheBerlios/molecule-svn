using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoto.Providers;

namespace Molecule.MvcWebSite.atomes.photos.Data
{
    public class YearCalendarData
    {
        public IEnumerable<CalendarItem> Items { get; set; }
        public int Year { get; set; }
        public ITagInfo Tag { get; set; }
    }
}
