using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMusic.Providers;

namespace Molecule.MvcWebSite.atomes.music.Data
{
    public class SongData
    {
        ISongInfo song;

        public SongData(ISongInfo song, string artistName, string albumName)
        {
            this.song = song;
            ArtistName = artistName;
            AlbumName = albumName;
        }

        public string Id
        {
            get { return song.Id; }
        }

        public string ArtistName
        {
            get;
            private set;
        }

        public string AlbumName
        {
            get;
            private set;
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

        public string Duration
        {
            get { return ((int)Math.Truncate(song.Duration.TotalMinutes)) + ":" + song.Duration.Seconds; }
        }
    }
}
