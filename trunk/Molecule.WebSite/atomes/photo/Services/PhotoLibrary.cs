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
        Dictionary<string, IPhoto> photos;
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
            IEnumerable<ITag> providerRootTags;
            try
            {
                var provider = Plugin<IPhotoLibraryProvider>.CreateInstance(
                   providerName, providerDirectory);
                provider.Initialize();
                providerRootTags = provider.GetRootTags();

                // TODO : recent tags

            }
            catch (Exception e)
            {
                throw new ProviderException(providerName, e);
            }

            buildIndexTables(providerRootTags);
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
        
        
        private void buildIndexTables(IEnumerable<ITag> providerRootTags)
        {
            tags = new Dictionary<string, ITag>();
            photos = new Dictionary<string, IPhoto>();

            //id index
            foreach (var tag in GetAllTags(providerRootTags))
            {
                tags[tag.Id] = tag;
                foreach (var photo in tag.Photos)
                    photos[photo.Id] = photo;
            }
            
            if (log.IsDebugEnabled)
            {
                log.Debug(String.Format("Statistics : {0} tags, {1} photos"
                , tags.Count, photos.Count));
            }

            //timeline
            var tempTimeline = new List<IPhoto>(photos.Values);
            tempTimeline.Sort((p1, p2) => p1.Date.CompareTo(p2.Date));
            timelinePhotos = new LinkedList<IPhoto>(tempTimeline);
            photosByDay = new Dictionary<DateTime, LinkedListNode<IPhoto>>();

            //day index
            for (var item = timelinePhotos.First; item != timelinePhotos.Last; item = item.Next)
                if (item == timelinePhotos.First || item.Previous.Value.Date.Date != item.Value.Date.Date)
                    photosByDay[item.Value.Date.Date] = item;
        }

        public static ITag GetTag(string tag)
        {
            return instance.tags[tag];
        }

        public static IPhoto GetPhoto(string photo)
        {
            return instance.photos[photo];
        }

        public static IEnumerable<ITag> GetTags()
        {
            return instance.tags.Values;
        }

        public static IEnumerable<IPhoto> GetPhotos()
        {
            return instance.timelinePhotos;
        }


        public static IEnumerable<IPhoto> GetPhotosByTag(string tag)
        {
            if (!String.IsNullOrEmpty(tag) && instance.tags.ContainsKey(tag))
            {
                foreach(var photo in instance.tags[tag].Photos.OrderBy(photo => photo.Date))
                    yield return photo;
            }
        }

        public static IEnumerable<IPhoto> GetPhotosByTags(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
                foreach (var photo in GetPhotosByTag(tag))
                    yield return photo;
        }

        public static IEnumerable<IPhoto> GetPhotosByDay(DateTime d)
        {
            LinkedListNode<IPhoto> current;
            instance.photosByDay.TryGetValue(d, out current);
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
    }
}
