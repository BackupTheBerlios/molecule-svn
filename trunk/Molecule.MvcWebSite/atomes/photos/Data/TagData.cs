using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoto.Providers;

namespace Molecule.MvcWebSite.atomes.photos.Data
{
    public class TagData
    {
        public ITagInfo Tag { get; set; }
        public IEnumerable<ITagInfo> SubTags { get; set; }
        public IEnumerable<IPhotoInfo> Photos { get; set; }
    }
}
