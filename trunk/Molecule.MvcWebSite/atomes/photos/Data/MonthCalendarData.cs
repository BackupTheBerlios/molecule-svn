﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Molecule.MvcWebSite.atomes.photos.Data
{
    public class MonthCalendarData : YearCalendarData
    {
        public int Month { get; set; }
        public int PreviousMonth { get; set; }
        public int NextMonth { get; set; }
    }
}
