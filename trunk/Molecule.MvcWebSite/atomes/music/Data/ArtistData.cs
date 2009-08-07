using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMusic.Providers;

namespace Molecule.MvcWebSite.atomes.music.Data
{
    public class ArtistData : IArtistInfo
    {
        IArtistInfo artist;

        public ArtistData(IArtistInfo artist)
        {
            this.artist = artist;
        }
        #region IArtistInfo Members

        public string Name
        {
            get { return artist.Name; }
        }

        public string Id
        {
            get { return artist.Id; }
        }

        #endregion
    }
}
