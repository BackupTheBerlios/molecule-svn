using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;
using Molecule.MvcWebSite.atomes.music.Data;
using WebMusic.Services;
using WebMusic.Providers;
using log4net;
using WebMusic.CoverArt;
using System.IO;
using WebMusic.IO;
using Molecule.MvcWebSite.atomes.music.Views.Player;

namespace Molecule.MvcWebSite.atomes.music.Controllers
{
    [Authorize]
    public class PlayerController : PageControllerBase<Atome>
    {

       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult File(string id)
        {
            return new SecureFileResult(MusicLibrary.GetSong(id).MediaFilePath, "audio/mpeg");
        }



        static ILog log = LogManager.GetLogger(typeof(PlayerController));

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Artists()
        {
            return new JsonResult() { Data = MusicLibrary.GetArtists().Select((a) => new ArtistData(a)) };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Albums()
        {
            return new JsonResult() { Data = MusicLibrary.GetAlbums().Select((a) => new AlbumData(a)) };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AlbumsByArtist(string id)
        {
            return new JsonResult() { Data = MusicLibrary.GetAlbumsByArtist(id).Select((a) => new AlbumData(a)) };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SongsByAlbum(string id)
        {
            return new JsonResult() { Data = MusicLibrary.GetSongsByAlbum(id).Select((s) => new SongData(s)) };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Search(string search)
        {
            return new JsonResult() { Data = new SearchData(MusicLibrary.Search(search)) };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void SongPlayed(string id)
        {
            if (User.IsInRole("admin"))//not using AuthorizeAttribute to avoid 401 error when other people are listening music.
            {
                ISong song = MusicLibrary.GetSong(id);
                if (song == null)
                    return;
                if (log.IsDebugEnabled)
                    log.Debug("Scrobbling " + song.Title + " of " + song.Artist.Name);

                WebMusic.Lastfm.LastfmService.Scrobble(song.Artist.Name, song.Album.Name, song.Title, (int)song.AlbumTrack.GetValueOrDefault(), (int)song.Duration.TotalSeconds, DateTime.Now.Subtract(song.Duration));
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void SongCurrentlyPlaying(string id)
        {
            if (User.IsInRole("admin"))//not using AuthorizeAttribute to avoid 401 error when other people are listening music.
            {
                ISong song = MusicLibrary.GetSong(id);
                if (song == null)
                    return;
                if (log.IsDebugEnabled)
                    log.Debug("Currently playing  " + song.Title + " of " + song.Artist.Name);

                WebMusic.Lastfm.LastfmService.NowPlaying(song.Artist.Name, song.Title, song.Album.Name, (int)song.Duration.TotalSeconds, (int)song.AlbumTrack.Value);
            }
        }

        public FileResult CovertArt(string id)
        {
            if (log.IsDebugEnabled)
                log.Debug("Requested cover art for the song with ID ");

            var song = MusicLibrary.GetSong(id).Check(id);

            string coverArtPath = CoverArtService.FetchCoverArt(song.Artist.Name, song.Album.Name);

            if (!String.IsNullOrEmpty(coverArtPath) && System.IO.File.Exists(coverArtPath))
                return new FilePathResult(coverArtPath, "image/jpeg");
            else
                throw new ResourceNotFoundException(id);
        }
    }
}
