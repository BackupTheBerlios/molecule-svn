using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Molecule.MvcWebSite.Controllers;
using WebMusic.Services;
using WebMusic.Providers.Base;
using WebMusic.Providers;
using Molecule.MvcWebSite.atomes.music.Views.Player;

namespace Molecule.MvcWebSite.atomes.music.Controllers
{
    [Authorize]
    public class DownloadController : PageControllerBase<Atome>
    {

        private enum ArchiveResult
        {
            Ok,
            TooBig,
            FileMissing,
            UnknownError
        }


        private ArchiveResult CheckResult(SearchResult searchResult)
        {
            if (searchResult.Songs.Count<ISong>() > 15)
            {
                return ArchiveResult.TooBig; 
            }
            return ArchiveResult.Ok;
        }

        
        public ActionResult Files(string id)
        {
            string[] idParts = id.Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            SearchResult searchResult = null;

            if (idParts != null && idParts.Length == 2)
            {
                searchResult = MusicLibrary.Search(String.Format("{0}:{1}",idParts[0], idParts[1]));
                if (CheckResult(searchResult) == ArchiveResult.TooBig)
                {
                    ViewData["ErrorMessage"] = "Archive will be too big. Select less files.";
                    ViewData["ShowDownloadLink"] = false;
                    return View();
                }
            }
            ViewData["ShowDownloadLink"] = true;
            ViewData["DownloadId"] = id;
            return View();
        }


        public ActionResult Archive(string id)
        {
            string[] idParts = id.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (idParts != null && idParts.Length == 2)
            {
                SearchResult searchResult = null;
                searchResult = MusicLibrary.Search(String.Format("{0}:{1}", idParts[0], idParts[1]));
                if (CheckResult(searchResult) == ArchiveResult.Ok)
                {
                    return new SongsZipResult(from i in searchResult.Songs
                                              let s = MusicLibrary.GetSong(i.Id)
                                              where s != null
                                              select s);
                }
            }
            return new EmptyResult();

        }



    }
}
