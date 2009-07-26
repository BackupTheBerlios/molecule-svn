using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoto.Providers;

namespace Molecule.MvcWebSite.atomes.photos.Data
{
    public class TagHierarchyData
    {
        public ITagInfo Tag { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }
    }
}
