using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMusic.Services;
using Molecule.MvcWebSite.atomes.music.Data;
using WebMusic.Providers;
using log4net;

namespace Molecule.MvcWebSite.atomes.music.Controllers
{
    public class LibraryController : Controller
    {
        static ILog log = LogManager.GetLogger(typeof(LibraryController));

        public JsonResult Artists()
        {
            return new JsonResult() { Data = MusicLibrary.GetArtists().Select((a)=>new ArtistData(a)) };
        }

        public JsonResult Albums()
        {
            return new JsonResult() { Data = MusicLibrary.GetAlbums().Select((a) => new AlbumData(a)) };
        }

        public JsonResult AlbumsByArtist(string id)
        {
            return new JsonResult() { Data = MusicLibrary.GetAlbumsByArtist(id).Select((a) => new AlbumData(a)) };
        }

        public JsonResult SongsByAlbum(string id)
        {
            return new JsonResult() { Data = MusicLibrary.GetSongsByAlbum(id).Select((s) => new SongData(s, s.Artist.Name, s.Album.Name)) };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void SongPlayed(string id)
        {
            if (User.IsInRole("admin"))//not using AuthorizeAttribute to avoid 401 error.
            {
                ISong song = MusicLibrary.GetSong(id);
                if (log.IsDebugEnabled)
                    log.Debug("Scrobbling " + song.Title + " of " + song.Artist.Name);

                WebMusic.Lastfm.LastfmService.Scrobble(song.Artist.Name, song.Album.Name, song.Title, (int)song.AlbumTrack.GetValueOrDefault(), (int)song.Duration.TotalSeconds, DateTime.Now.Subtract(song.Duration));
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void SongCurrentlyPlaying(string id)
        {
            if (User.IsInRole("admin"))//not using AuthorizeAttribute to avoid 401 error.
            {
                ISong song = MusicLibrary.GetSong(id);
                if (log.IsDebugEnabled)
                    log.Debug("Currently playing  " + song.Title + " of " + song.Artist.Name);

                WebMusic.Lastfm.LastfmService.NowPlaying(song.Artist.Name, song.Title, song.Album.Name, (int)song.Duration.TotalSeconds, (int)song.AlbumTrack.Value);
            }
        }
    }
}
