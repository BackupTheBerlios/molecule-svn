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

                RootTags = from tag in PhotoLibrary.AdminGetRootTags()
                           select new TagData(tag),

                ImageQuality = PhotoLibrary.ImageQualityLevel

            };
            return View(res);
        }

        public ActionResult Save(string provider,
            TagName tagName,
            string[] authorizations,
            string[] sharedTags,
            bool reloadProvider,
            int imageQuality)
        {

            

            if (!PhotoLibrary.Providers.Any(p => p.Id == provider))
                throw new ArgumentException("Invalid provider id.", "provider");

            var providerChanged = PhotoLibrary.CurrentProvider != provider;

            if(reloadProvider)  
                PhotoLibrary.CurrentProvider = null;
            PhotoLibrary.CurrentProvider = provider;
            PhotoLibrary.TagName = tagName;

            PhotoLibrary.ImageQualityLevel = imageQuality;

            if (providerChanged)
                PhotoLibrary.TagUserAuthorizations.Clear();
            else
            {
                //remove unchecked tag
                PhotoLibrary.TagUserAuthorizations.RemoveAll(tuai => sharedTags == null || !sharedTags.Contains(tuai.TagId));

                //add checked tag
                if (sharedTags != null)
                    foreach (var tagId in sharedTags)
                        if (!PhotoLibrary.TagUserAuthorizations.Any(tuai => tuai.TagId == tagId))
                            PhotoLibrary.TagUserAuthorizations.AddTag(tagId);

                //update autorizations
                foreach (var tua in PhotoLibrary.TagUserAuthorizations)
                    foreach (var auth in tua.Authorizations)
                    {
                        auth.Authorized = authorizations != null ?
                            authorizations.Contains(TagUserAuthorizationData.GetValue(auth.TagId, auth.User)) : false;
                        PhotoLibrary.TagUserAuthorizations.Set(auth);
                    }
            }
            PhotoLibrary.SaveTagUserAuthorizations();

            return RedirectToAction<AdminController>(c => c.Index());
        }
    }
}
