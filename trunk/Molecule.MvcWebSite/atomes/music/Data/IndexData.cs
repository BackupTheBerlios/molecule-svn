using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMusic.Providers;

namespace Molecule.MvcWebSite.atomes.music.Data
{
    public class IndexData
    {
        public IEnumerable<IArtistInfo> Artists { get; set; }
        public IEnumerable<IAlbumInfo> Albums { get; set; }
        public IEnumerable<ISongInfo> Songs { get; set; }
    }
}
