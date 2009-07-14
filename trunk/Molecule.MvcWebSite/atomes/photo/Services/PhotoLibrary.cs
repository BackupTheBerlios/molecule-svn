using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.Web;
using WebPhoto.Providers;
using Molecule.Runtime;
using Molecule.Atome;
using System.Globalization;
using Molecule.Configuration;
using Molecule.WebSite.Services;
using System.Web.Security;
using Molecule;

namespace WebPhoto.Services
{
    public partial class PhotoLibrary : AtomeProviderBase<PhotoLibrary, IPhotoLibraryProvider>
    {
        IDictionary<string, UserPhotoLibrary> userLibraries;

        //needed by admin preference page
        IEnumerable<ITag> rootTags;

        private PhotoLibrary()
        {
        }

        #region AtomProviderBase implementation

        protected override string ProviderDirectory
        {
            get { return HttpContext.Current.Server.MapPath("~/atomes/photo/bin/providers"); }
        }

        protected override string DefaultProvider
        {
            get { return "Stub"; }
        }
        
        protected override void OnProviderUpdated()
        {
            System.Diagnostics.Trace.WriteLine("OnProviderUpdated");
            CallProvider(provider =>
                rootTags = provider.GetRootTags().ToList());

            initAuthorizations();
            buildUserLibraries();
        }

        private void buildUserLibraries()
        {
            userLibraries = (from user in Membership.GetAllUsers().Cast<MembershipUser>()
                         orderby user.UserName
                         select user.UserName)
                        .Concat(new string[] { AtomeUserAuthorizations.AnonymousUser })
                        .ToDictionary(s => s, s => new UserPhotoLibrary(s, rootTags, tagUserAuthorizations));
        }

        #endregion

        private static UserPhotoLibrary userInstance
        {
            get
            {
                return Instance.userLibraries[CurrentUser];
            }
        }

        private static string CurrentUser
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated ?
                    HttpContext.Current.User.Identity.Name : AtomeUserAuthorizations.AnonymousUser;
            }
        }

        #region Helpers

        private static IEnumerable<ITag> getAllTags(IEnumerable<ITag> rootTags)
        {
            foreach (var tag in rootTags)
            {
                yield return tag;
                foreach (var st in getAllTags(tag.ChildTags))
                    yield return st;
            }
        }

        private static bool isTagged(IPhoto photo, string tagId)
        {
            if (String.IsNullOrEmpty(tagId))
                return true;
            return photo.Tags.Any(t => t.Id == tagId);
        }

        private static IPhotoInfo getFirstPhotoByTag(ITag tag)
        {
            var res = tag.Photos.FirstOrDefault();
            return res ?? getFirstPhotoByTag(tag.ChildTags.FirstOrDefault());
        }

        private static IPhotoInfo getNeighborPhoto(string photoId, string tagId
            , Func<LinkedListNode<IPhoto>, LinkedListNode<IPhoto>> neighbor)
        {
            var current = neighbor(userInstance.GetPhotoById(photoId).Check(photoId));
            while (current != null)
            {
                if (isTagged(current.Value, tagId))
                        return current.Value;
                current = neighbor(current);
            }
            return null;
        }

        #endregion

        #region Service Methods

        public static ITagInfo GetTag(string tagId)
        {
            return userInstance.GetTagById(tagId).Check(tagId);
        }

        public static bool TagExists(string tagId)
        {
            return userInstance.TagExists(tagId);
        }

        public static IPhotoInfo GetPhoto(string photoId)
        {
            return userInstance.GetPhotoById(photoId).Check(photoId).Value;
        }

        public static IEnumerable<ITagInfo> GetTagsByTag(string tagId)
        {
            if (!String.IsNullOrEmpty(tagId))
                return userInstance.GetTagById(tagId).Check(tagId).ChildTags.Cast<ITagInfo>();
            else
                return GetRootTags();
        }

        public static IEnumerable<ITagInfo> GetTagHierarchy(string tagId)
        {
            var tags = new List<ITagInfo>();
            if (String.IsNullOrEmpty(tagId))
                return tags;

            var tag = userInstance.GetTagById(tagId);
            while (tag != null)
            {
                tags.Add(tag);
                tag = tag.Parent;
            }

            return tags.Reverse<ITagInfo>();
        }

        public static IEnumerable<ITagInfo> GetTagsByPhoto(string photoId)
        {
            return userInstance.GetPhotoById(photoId)
                .Check(photoId).Value.Tags.Cast<ITagInfo>();
        }

        public static IEnumerable<IPhotoInfo> GetPhotos()
        {
            return userInstance.GetPhotos().Cast<IPhotoInfo>();
        }

        public static IEnumerable<IPhotoInfo> GetPhotosByTag(string tagId)
        {
            if (!String.IsNullOrEmpty(tagId))
                return userInstance.GetTagById(tagId).Check(tagId).Photos.Cast<IPhotoInfo>();
            else return new List<IPhotoInfo>();
        }

        public static IPhotoInfo GetFirstPhotoByTag(string tagId)
        {
            return getFirstPhotoByTag(userInstance.GetTagById(tagId).Check(tagId));
        }

        public static IEnumerable<IPhotoInfo> GetPhotosByTags(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
                foreach (var photo in GetPhotosByTag(tag).Check(tag))
                    yield return photo;
        }

        public static IEnumerable<IPhotoInfo> GetPhotosByDayAndTag(DateTime d, string tagId)
        {
            var current = userInstance.GetPhotoByDay(d);

            //search for a photo that match specified tag.
            while (current != null && current.Value.Date.Date == d)
            {
                if (isTagged(current.Value, tagId))
                    yield return current.Value;
                current = current.Next;
            }
        }

        public static IEnumerable<IPhotoInfo> GetPhotosByMonthAndTag(DateTime month, string tagId)
        {
            LinkedListNode<IPhoto> current = null;

            //search for a photo in current month.
            for(DateTime dt = month; dt.Month == month.Month && current == null; dt = dt.AddDays(1))
                current = userInstance.GetPhotoByDay(dt);

            //search for a photo that match specified tag.
            while (current != null
                && current.Value.Date.Month == month.Month
                && current.Value.Date.Year == month.Year)
            {
                if (isTagged(current.Value, tagId))
                    yield return current.Value;
                current = current.Next;
            }
        }

        public static string GetTagFullPath(string tagId)
        {
            if (!String.IsNullOrEmpty(tagId))
            {
                //use GetPreferenceTag : can be used by preference page.
                var tag = AdminGetTag(tagId);
                string path = "";
                while (tag != null)
                {
                    path = " > " + tag.Name + path;
                    tag = tag.Parent;
                }
                return path;
            }
            return null;
        }

        public static IPhotoInfo GetNextPhoto(string photoId, string tagId)
        {
            return getNeighborPhoto(photoId, tagId, p => p.Next);
        }

        public static IPhotoInfo GetPreviousPhoto(string photoId, string tagId)
        {
            return getNeighborPhoto(photoId, tagId, p => p.Previous);
        }

        public static IEnumerable<ITagInfo> GetRootTags()
        {
            return userInstance.GetRootTags().Cast<ITagInfo>();
        }

        public static string GetLocalizedTagName()
        {
            return GetLocalizedTagName(TagName);
        }

        public static string GetLocalizedTagName(TagName tagName)
        {
            return (string)HttpContext.GetGlobalResourceObject("photo", tagName.ToString());
        }

        #endregion
    }
    public enum TagName
    {
        Tag,
        Folder,
        Collection,
        Album
    }
}
