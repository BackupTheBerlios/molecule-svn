using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMusic.Providers;

namespace Molecule.MvcWebSite.atomes.music.Data
{
    public class SongData : ISongInfo
    {
        ISongInfo song;

        public SongData(ISongInfo song)
        {
            this.song = song;
        }
        #region ISongInfo Members

        public string Id
        {
            get { return song.Id; }
        }

        public string Title
        {
            get { return song.Title; }
        }

        public string MediaFilePath
        {
            get { return song.MediaFilePath; }
        }

        public uint? AlbumTrack
        {
            get { return song.AlbumTrack; }
        }

        public TimeSpan Duration
        {
            get { return song.Duration; }
        }

        #endregion
    }
}
