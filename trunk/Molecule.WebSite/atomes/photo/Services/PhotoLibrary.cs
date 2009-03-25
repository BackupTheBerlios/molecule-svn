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

namespace WebPhoto.Services
{
    public class PhotoLibrary : AtomeProviderBase<PhotoLibrary, IPhotoLibraryProvider>
    {

        Dictionary<string, ITagInfo> tags;
        IEnumerable<ITagInfo> rootTags;
        Dictionary<string, LinkedListNode<IPhotoInfo>> photosByIds;
        LinkedList<IPhotoInfo> timelinePhotos;
        Dictionary<DateTime, LinkedListNode<IPhotoInfo>> photosByDay;

        private PhotoLibrary()
        {
        }

        #region AtomProviderBase implementation
        protected override string ConfigurationNamespace
        {
            get { return "WebPhoto"; }
        }

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
            CallProvider(provider =>
                    rootTags = provider.GetRootTags().Cast<ITagInfo>());

            buildIndexTables();
        }

        private void buildIndexTables()
        {
            tags = new Dictionary<string, ITagInfo>();
            Dictionary<string, IPhotoInfo> tempPhotos = new Dictionary<string, IPhotoInfo>();

            //id index
            foreach (var tag in getAllTags(rootTags, false).Cast<ITag>())
            {

                foreach (var photo in tag.Photos)
                    tempPhotos[photo.Id] = photo;

                if (isCandidate(tag, false))
                    tags[tag.Id] = tag;
            }

            if (log.IsDebugEnabled)
            {
                log.Debug(String.Format("Statistics : {0} tags, {1} photos"
                , tags.Count, tempPhotos.Count));
            }

            //timeline
            var tempTimeline = new List<IPhotoInfo>(tempPhotos.Values);
            tempTimeline.Sort((p1, p2) => p2.Date.CompareTo(p1.Date));
            timelinePhotos = new LinkedList<IPhotoInfo>(tempTimeline);


            //day index
            photosByDay = new Dictionary<DateTime, LinkedListNode<IPhotoInfo>>();
            for (var item = timelinePhotos.First; item != null; item = item.Next)
                if (item == timelinePhotos.First || item.Previous.Value.Date.Date != item.Value.Date.Date)
                    photosByDay[item.Value.Date.Date] = item;

            //id index
            photosByIds = new Dictionary<string, LinkedListNode<IPhotoInfo>>();
            for (var item = timelinePhotos.First; item != null; item = item.Next)
                photosByIds[item.Value.Id] = item;

        }

        #endregion

        #region Authorization Helpers
        private static bool isCandidate(ITagInfo tag, bool filter)
        {
            return getAllTags(tag, filter)
                .Any(t => getPhotosByTag(t, filter).Any());
        }

        private static bool isCandidate(IPhotoInfo photo, bool filter)
        {
            return isCandidate(photo, null, filter);
        }

        private static bool isCandidate(IPhotoInfo photo, string tagId, bool filter)
        {
            return true;//still not fully implemented :-(
            //search for an authorized tag for photo
            //if tagId is specified, tagId is mandatory.

            bool tagIdFound = String.IsNullOrEmpty(tagId);
            bool authorizedTagFound = false;
            var authorizedTags = getCurrentUserAuthorizedTags();
            
            foreach (var photoTagId in ((IPhoto)photo).Tags.Select(t => t.Id))
            {
                if(!authorizedTagFound)
                    authorizedTagFound = authorizedTags.Contains(photoTagId);

                if(!tagIdFound)
                    tagIdFound = photoTagId == tagId;

                if (authorizedTagFound && tagIdFound)
                    return true;
            }
            return false;
        }

        static List<string> alltags;
        private static List<string> getCurrentUserAuthorizedTags()
        {
            if (alltags == null)
            {
                alltags = new List<string>();
                alltags.Add("53a9bdcd0e93d746eb0fc3865fc5e02b");
            }
            return alltags;
        }

        private static string CurrentUser
        {
            get
            {
                var currentUser = HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "";
                return currentUser ?? AtomeUserAuthorizations.AnonymousUser;
            }
        }

        private static bool isCurrentUserAuthorized(IPhoto photo)
        {
            return true;
            
        }

        #endregion

        #region Helpers
        private static IEnumerable<ITagInfo> getAllTags(ITagInfo tag, bool filter)
        {
            return getAllTags(new[] { tag }, filter);
        }

        private static IEnumerable<ITagInfo> getAllTags(IEnumerable<ITagInfo> tags, bool filter)
        {
            foreach (var tag in tags)
            {
                yield return tag;
                foreach (var t in getAllTags(getTagsByTag(tag, filter), filter))
                    yield return t;
            }
        }

        private static IEnumerable<ITagInfo> getAllTags(bool filter)
        {
            return getAllTags(instance.rootTags, filter);
        }

        private static IEnumerable<ITagInfo> getRootTags(bool filter)
        {
            return instance.rootTags
                .Where(t => isCandidate(t, filter));
        }

        private static IEnumerable<ITagInfo> getTagsByTag(ITagInfo tag, bool filter)
        {
            return getTagsByTag(tag as ITag, filter).Cast<ITagInfo>();
        }

        private static IEnumerable<ITag> getTagsByTag(ITag tag, bool filter)
        {
            return tag.ChildTags.Where(t => isCandidate(t, filter));
        }
        
        private static IEnumerable<IPhotoInfo> getPhotosByTag(ITagInfo tag, bool filter)
        {
            return from photo in (tag as ITag).Photos
                   where isCandidate(photo, filter)
                   select photo as IPhotoInfo;
        }

        private static IPhotoInfo getFirstPhotoByTag(ITagInfo tag, bool filter)
        {
            var res = getPhotosByTag(tag, filter).FirstOrDefault();
            return res ?? getFirstPhotoByTag(getTagsByTag(tag, filter)
                .FirstOrDefault(), filter);
        }

        private static IEnumerable<ITagInfo> getTagsByPhoto(IPhotoInfo photo)
        {
            return (photo as IPhoto).Tags.Cast<ITagInfo>();
        }

        private static IPhotoInfo getNeighborPhoto(string photoId, string tagId
            , Func<LinkedListNode<IPhotoInfo>, LinkedListNode<IPhotoInfo>> neighbor
            , bool filter)
        {
            var current = neighbor(instance.photosByIds[photoId]);
            while (current != null)
            {
                if (isCandidate(current.Value, tagId, filter))
                        return current.Value;
                current = neighbor(current);
            }
            return null;
        }

        #endregion

        #region Service Methods

        public static ITagInfo GetTag(string tagId)
        {
            return instance.tags[tagId];
        }

        public static IPhotoInfo GetPhoto(string photoId)
        {
            return instance.photosByIds[photoId].Value;
        }

        public static IEnumerable<ITagInfo> GetTagsByTag(string tagId)
        {
            if (!String.IsNullOrEmpty(tagId))
                return getTagsByTag(instance.tags[tagId], true);
            else
                return GetRootTags();
        }

        public static IEnumerable<ITagInfo> GetTagHierarchy(string tagId)
        {
            var tags = new List<ITagInfo>();
            if (instance.tags.ContainsKey(tagId))
            {
                var tag = instance.tags[tagId];
                while (tag != null)
                {
                    tags.Add(tag);
                    tag = tag.Parent;
                }
            }
            return tags.Reverse<ITagInfo>();
        }

        public static IEnumerable<ITagInfo> GetTagsByPhoto(string photoId)
        {
            return getTagsByPhoto(instance.photosByIds[photoId].Value);
        }

        public static IEnumerable<IPhotoInfo> GetPhotos()
        {
            return instance.timelinePhotos
                .Where(photo => isCandidate(photo, true));
        }

        public static IEnumerable<IPhotoInfo> GetPhotosByTag(string tag)
        {
            if (!String.IsNullOrEmpty(tag))
                return getPhotosByTag(instance.tags[tag], true)
                    .OrderByDescending(p=>p.Date);//should be optimized if photo are ordered by default.
            else
                return GetPhotos().OrderByDescending(p=>p.Date);
        }

        public static IPhotoInfo GetFirstPhotoByTag(string tagId)
        {
            return getFirstPhotoByTag(instance.tags[tagId], true);
        }

        public static IEnumerable<IPhotoInfo> GetPhotosByTags(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
                foreach (var photo in GetPhotosByTag(tag))
                    yield return photo;
        }

        public static IEnumerable<IPhotoInfo> GetPhotosByDayAndTag(DateTime d, string tagId)
        {
            LinkedListNode<IPhotoInfo> current;

            //search for a photo in current day.
            instance.photosByDay.TryGetValue(d, out current);

            //search for a photo that match specified tag.
            while (current != null && current.Value.Date.Date == d)
            {
                if (isCandidate(current.Value, tagId, true))
                    yield return current.Value;
                current = current.Next;
            }
        }

        public static IEnumerable<IPhotoInfo> GetPhotosByMonthAndTag(DateTime month, string tagId)
        {
            LinkedListNode<IPhotoInfo> current = null;

            //search for a photo in current month.
            for(DateTime dt = month; dt.Month == month.Month && current == null; dt = dt.AddDays(1))
                instance.photosByDay.TryGetValue(month, out current);

            //search for a photo that match specified tag.
            while (current != null
                && current.Value.Date.Month == month.Month
                && current.Value.Date.Year == month.Year)
            {
                if (isCandidate(current.Value, tagId, true))
                    yield return current.Value;
                current = current.Next;
            }
        }

        public static string GetTagFullPath(string tagId)
        {
            if (!String.IsNullOrEmpty(tagId))
            {
                var tag = GetTag(tagId);
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
            return getNeighborPhoto(photoId, tagId, p => p.Next, true);
        }

        public static IPhotoInfo GetPreviousPhoto(string photoId, string tagId)
        {
            return getNeighborPhoto(photoId, tagId, p => p.Previous, true);
        }

        public static IEnumerable<ITagInfo> GetRootTags()
        {
            return getRootTags(true);
        }

        public static IEnumerable<ITagInfo> GetAllTags()
        {
            return getAllTags(true);
        }

        #endregion
    }
}
