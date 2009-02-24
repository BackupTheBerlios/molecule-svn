using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.Web;
using WebPhoto.Providers;
using Molecule.Runtime;

namespace WebPhoto.Services
{
    public class PhotoLibrary
    {
        static object instanceLock = new object();

        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(PhotoLibrary));

        static string providerName;

        Dictionary<string, ITag> tags;
        IEnumerable<ITag> rootTags;
        Dictionary<string, LinkedListNode<IPhoto>> photosByIds;
        LinkedList<IPhoto> timelinePhotos;
        Dictionary<DateTime, LinkedListNode<IPhoto>> photosByDay;

        static PhotoLibrary()
        {
            providerName = Molecule.Configuration.ConfigurationClient.Client.Get<string>("WebPhoto", "LibraryProvider", "Stub");
        }

        static PhotoLibrary instance
        {
            get
            {
                return Singleton<PhotoLibrary>.Instance;
            }
        }

        private PhotoLibrary()
        {
            UpdateProvider();
        }

        public static string CurrentProvider
        {
            get { return providerName; }
            set
            {
                providerName = value;
                Molecule.Configuration.ConfigurationClient.Client.Set<string>("WebPhoto", "LibraryProvider", value);
                instance.UpdateProvider();
            }
        }

        public static IEnumerable<ProviderInfo> Providers
        {
            get
            {
                foreach (var provider in Plugin<IPhotoLibraryProvider>.List(providerDirectory))
                    yield return new ProviderInfo() { Description = provider.Description, Name = provider.Name };
            }
        }

        static string providerDirectory
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/atomes/photo/bin/providers");
            }
        }

        protected void UpdateProvider()
        {
            if (log.IsDebugEnabled)
                log.Debug("Provider used : " + providerName);

            updateProviderTags();
            
        }

        private void updateProviderTags()
        {
            try
            {
                var provider = Plugin<IPhotoLibraryProvider>.CreateInstance(
                   providerName, providerDirectory);
                provider.Initialize();
                rootTags = provider.GetRootTags();

                // TODO : recent tags

            }
            catch (Exception e)
            {
                throw new ProviderException(providerName, e);
            }

            buildIndexTables();
        }

        protected static IEnumerable<ITag> GetAllTags(IEnumerable<ITag> tags)
        {
            foreach (ITag tag in tags)
            {
                yield return tag;
                foreach (ITag subTag in tag.ChildTags)
                    yield return subTag;
            }
        }
        
        
        private void buildIndexTables()
        {
            tags = new Dictionary<string, ITag>();
            Dictionary<string, IPhoto> tempPhotos = new Dictionary<string,IPhoto>();

            //id index
            foreach (var tag in GetAllTags(rootTags))
            {
                tags[tag.Id] = tag;
                foreach (var photo in tag.Photos)
                    tempPhotos[photo.Id] = photo;
            }
            
            if (log.IsDebugEnabled)
            {
                log.Debug(String.Format("Statistics : {0} tags, {1} photos"
                , tags.Count, tempPhotos.Count));
            }
            
            //timeline
            var tempTimeline = new List<IPhoto>(tempPhotos.Values);
            tempTimeline.Sort((p1, p2) => p1.Date.CompareTo(p2.Date));
            timelinePhotos = new LinkedList<IPhoto>(tempTimeline);
            

            //day index
            photosByDay = new Dictionary<DateTime, LinkedListNode<IPhoto>>();
            for (var item = timelinePhotos.First; item != timelinePhotos.Last; item = item.Next)
                if (item == timelinePhotos.First || item.Previous.Value.Date.Date != item.Value.Date.Date)
                    photosByDay[item.Value.Date.Date] = item;

            //id index
            photosByIds = new Dictionary<string, LinkedListNode<IPhoto>>();
            for (var item = timelinePhotos.First; item != timelinePhotos.Last; item = item.Next)
                photosByIds[item.Value.Id] = item;

        }

        public static ITagInfo GetTag(string tag)
        {
            return instance.tags[tag];
        }

        public static IPhotoInfo GetPhoto(string photoId)
        {
            return instance.photosByIds[photoId].Value;
        }

        public static IEnumerable<ITagInfo> GetTags()
        {
            return instance.tags.Values.Cast<ITagInfo>();
        }

        public static IEnumerable<ITagInfo> GetTagsByPhoto(string photoId)
        {
            return instance.photosByIds[photoId].Value.Tags.Cast<ITagInfo>();
        }

        public static IEnumerable<IPhotoInfo> GetPhotos()
        {
            return instance.timelinePhotos.Cast<IPhotoInfo>();
        }

        public static IEnumerable<IPhotoInfo> GetPhotosByTag(string tag)
        {
            if (!String.IsNullOrEmpty(tag))
                return instance.tags[tag].Photos.OrderBy(photo => photo.Date).Cast<IPhotoInfo>();
            else
                return GetPhotos();
        }



        public static IEnumerable<IPhotoInfo> GetPhotosByTags(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
                foreach (var photo in GetPhotosByTag(tag))
                    yield return photo;
        }

        public static IEnumerable<IPhotoInfo> GetPhotosByDayAndTag(DateTime d, string tagId)
        {
            LinkedListNode<IPhoto> current;
            instance.photosByDay.TryGetValue(d, out current);
            while (current != null && current.Value.Date.Date == d)
            {
                if (String.IsNullOrEmpty(tagId) || current.Value.Tags.Any(t => t.Id == tagId))
                    yield return current.Value;
                current = current.Next;
            }
        }



        public static IPhotoInfo GetNextPhoto(string photoId, string tagId)
        {
            return getNeighborPhoto(photoId, tagId, p => p.Next);
        }

        public static IPhotoInfo GetPreviousPhoto(string photoId, string tagId)
        {
            return getNeighborPhoto(photoId, tagId, p => p.Previous);
        }

        private static IPhotoInfo getNeighborPhoto(string photoId, string tagId
            , Func<LinkedListNode<IPhoto>, LinkedListNode<IPhoto>> neighbor)
        {
            var current = neighbor(instance.photosByIds[photoId]);
            while (current != null)
            {
                if (String.IsNullOrEmpty(tagId) || current.Value.Tags.Any(t => t.Id == tagId))
                    return current.Value;
                current = neighbor(current);
            }
            return null;
        }

        public static IEnumerable<ITag> GetRootTags()
        {
            return instance.rootTags;
        }
    }
}
