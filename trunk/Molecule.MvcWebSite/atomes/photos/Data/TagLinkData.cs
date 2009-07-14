using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoto.Providers;

namespace Molecule.MvcWebSite.atomes.photos.Data
{
    public class TagLinkData
    {
        public ITagInfo Tag { get; set; }
        public bool TextOnly { get; set; }
    }
}
