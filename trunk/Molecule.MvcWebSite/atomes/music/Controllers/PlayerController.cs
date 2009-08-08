using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;
using Molecule.MvcWebSite.atomes.music.Data;
using WebMusic.Services;
using WebMusic.Providers;

namespace Molecule.MvcWebSite.atomes.music.Controllers
{
    public class PlayerController : PageControllerBase
    {
        public ActionResult Index()
        {
            var data = new IndexData()
            {
                Artists = MusicLibrary.GetArtists().Cast<IArtistInfo>(),
                Albums = MusicLibrary.GetAlbums().Cast<IAlbumInfo>(),
                Songs = new List<ISongInfo>().Cast<ISongInfo>()
            };
            return View(data);
        }

        public ActionResult File(string id)
        {
            return new FilePathResult(MusicLibrary.GetSong(id).MediaFilePath, "audio/mpeg");
        }
    }
}
