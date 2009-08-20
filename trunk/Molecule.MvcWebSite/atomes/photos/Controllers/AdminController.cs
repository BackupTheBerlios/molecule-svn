using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using System.Web.Mvc;
using Molecule.MvcWebSite.atomes.photos.Data;
using WebPhoto.Services;
using System.Web.Security;

namespace Molecule.MvcWebSite.atomes.photos.Controllers
{
    public class AdminController : PreferencesPageControllerBase
    {
        public ActionResult Index()
        {
            
            var res = new AdminData()
            {
                Providers = PhotoLibrary.Providers,
                
                SelectedProviderId = PhotoLibrary.CurrentProvider,
                
                TagNames = from tagName in EnumHelper.GetValues<WebPhoto.Services.TagName>()
                           select new KeyValuePair<TagName, string>(tagName, PhotoLibrary.GetLocalizedTagName(tagName)),

                SelectedTagName = new KeyValuePair<TagName, string>(PhotoLibrary.TagName, PhotoLibrary.GetLocalizedTagName()),
                
                UserNames = Membership.GetAllUsers().Cast<MembershipUser>().Select(u => u.UserName)
                            .Concat(new string[] { Resources.Common.Anonymous}),

                TagUserAuthorizations = from tuai in PhotoLibrary.TagUserAuthorizations
                                        select new TagUserAuthorizationItemData(tuai),

                RootTags = from tag in PhotoLibrary.GetRootTags()
                           select new TagData(tag)

            };
            return View(res);
        }

        public ActionResult Save(string provider, TagName tagName, string[] authorizations, string[] sharedTags)
        {
            PhotoLibrary.CurrentProvider = provider;
            PhotoLibrary.TagName = tagName;

            //remove unchecked tag
            PhotoLibrary.TagUserAuthorizations.RemoveAll(tuai => !sharedTags.Contains(tuai.TagId));

            //add checked tag
            foreach (var tagId in sharedTags)
                if (!PhotoLibrary.TagUserAuthorizations.Any(tuai => tuai.TagId == tagId))
                    PhotoLibrary.TagUserAuthorizations.AddTag(tagId);

            //update autorizations
            foreach (var tua in PhotoLibrary.TagUserAuthorizations)
            {
                foreach (var auth in tua.Authorizations)
                {
                    auth.Authorized = authorizations.Contains(TagUserAuthorizationData.GetValue(auth.TagId, auth.User));
                    PhotoLibrary.TagUserAuthorizations.Set(auth);
                }
            }

            PhotoLibrary.SaveTagUserAuthorizations();

            return RedirectToAction("Index");
        }
    }
}
