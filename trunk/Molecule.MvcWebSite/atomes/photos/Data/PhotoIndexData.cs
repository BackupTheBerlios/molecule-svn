using System;
using System.Collections.Generic;
using System.Linq;
using WebPhoto.Providers;
using System.Drawing;


namespace Molecule.MvcWebSite.atomes.photos.Data
{
    public class PhotoIndexData
    {
        public Size PhotoSize { get; set; }
        public IPhotoInfo Photo { get; set; }
        public ITagInfo CurrentTag { get; set; }
        public IPhotoInfo PreviousPhoto { get; set; }
        public IPhotoInfo NextPhoto { get; set; }
        public IEnumerable<ITagInfo> PhotoTags { get; set; }
    }
}
