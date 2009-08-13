using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMusic.Services;

namespace Molecule.MvcWebSite.atomes.music.Data
{
    public class SearchData
    {
        private SearchResult result;
        public SearchData(SearchResult searchResult)
        {
            this.result = searchResult;
        }

        public IEnumerable<ArtistData> Artists { get { return from a in result.Artists select new ArtistData(a); } }
        public IEnumerable<AlbumData> Albums { get { return from a in result.Albums select new AlbumData(a); } }
        public IEnumerable<SongData> Songs { get { return (from s in result.Songs select new SongData(s)).Take(50); } }
    }
}
