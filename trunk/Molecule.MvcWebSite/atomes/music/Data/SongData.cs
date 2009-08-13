using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMusic.Providers;

namespace Molecule.MvcWebSite.atomes.music.Data
{
    public class SongData
    {
        ISong song;

        public SongData(ISong song)
        {
            this.song = song;
        }

        public string Id
        {
            get { return song.Id; }
        }

        public string ArtistName
        {
            get{ return song.Artist.Name;}
        }

        public string AlbumName
        {
            get { return song.Album.Name; }
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
            get { return ((int)Math.Truncate(song.Duration.TotalMinutes)).ToString("00")
                + ":" + song.Duration.Seconds.ToString("00"); }
        }
    }
}
