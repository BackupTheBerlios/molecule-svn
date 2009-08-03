using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMusic.Providers;

namespace Molecule.MvcWebSite.atomes.music.Data
{
    public class IndexData
    {
        public IEnumerable<IArtist> Artists { get; set; }
        public IEnumerable<IAlbum> Albums { get; set; }
        public IEnumerable<ISong> Songs { get; set; }
    }
}
