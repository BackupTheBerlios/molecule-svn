using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMusic.Providers;

namespace WebMusic.Services
{
    public class SearchResult
    {
        public IEnumerable<ISong> Songs { get; set; }
        public IEnumerable<IAlbum> Albums { get; set; }
        public IEnumerable<IArtist> Artists { get; set; }
    }
}
