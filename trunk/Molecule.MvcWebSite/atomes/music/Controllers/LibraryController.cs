using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMusic.Services;
using Molecule.MvcWebSite.atomes.music.Data;

namespace Molecule.MvcWebSite.atomes.music.Controllers
{
    public class LibraryController : Controller
    {
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
    }
}
