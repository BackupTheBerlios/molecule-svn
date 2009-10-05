using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;
using Molecule.MvcWebSite.atomes.music.Data;
using WebMusic.Services;
using WebMusic.Lastfm;

namespace Molecule.MvcWebSite.atomes.music.Controllers
{
    public class AdminController : PreferencesPageControllerBase
    {

        public ActionResult Index()
        {
            return View(new AdminData()
            {
                Providers = MusicLibrary.Providers,
                SelectedProviderId = MusicLibrary.CurrentProvider,
                LastfmEnabled = LastfmService.ScrobblingEnabled,
                LastfmUsername = LastfmService.Username,
                LastfmPassword = LastfmService.Password
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(string provider, bool LastfmEnabled, string LastfmUsername, string LastfmPassword, string reload)
        {
            if(reload != null)
                return Reload(provider);

            if (!MusicLibrary.Providers.Any(p => p.Id == provider))
                throw new ArgumentException("Invalid provider id.", "provider");

            MusicLibrary.CurrentProvider = provider;
            LastfmService.ScrobblingEnabled = LastfmEnabled;

            if (LastfmUsername.Length > 200)
                throw new ArgumentException("Lastfm username is too long.", "LastfmUsername");
            LastfmService.Username = LastfmUsername;

            if (LastfmPassword.Length > 200)
                throw new ArgumentException("Lastfm password is too long.", "LastfmPassword");
            LastfmService.Password = LastfmPassword;

            return RedirectToAction<AdminController>(c => c.Index());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Reload(string provider)
        {
            MusicLibrary.CurrentProvider = null;
            MusicLibrary.CurrentProvider = provider;
            return RedirectToAction<AdminController>(c => c.Index());
        }
    }
}
