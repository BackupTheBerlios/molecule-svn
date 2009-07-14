using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoto.Providers;

namespace Molecule.MvcWebSite.atomes.photos.Data
{
    public class PhotoLinkData
    {
        public ITagInfo Tag { get; set; }
        public IPhotoInfo Photo { get; set; }
        public string Description { get; set; }
        public string HoverIconUrl { get; set; }
        public string HoverText { get; set; }
        public string NavigateUrl { get; set; }
    }
}
