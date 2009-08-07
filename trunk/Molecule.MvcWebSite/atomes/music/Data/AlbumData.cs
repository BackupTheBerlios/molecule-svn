using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMusic.Providers;

namespace Molecule.MvcWebSite.atomes.music.Data
{
    public class AlbumData : IAlbumInfo
    {
        IAlbumInfo album;

        public AlbumData(IAlbumInfo album)
        {
            this.album = album;
        }
        #region IAlbumInfo Members

        public string Name
        {
            get { return album.Name; }
        }

        public string Id
        {
            get { return album.Id; }
        }

        #endregion
    }
}
