using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoto.Providers;
using log4net;
using Mono.Rocks;
using Molecule;

namespace WebPhoto.Services
{
    internal class UserPhotoLibrary
    {
        Dictionary<string, InternalTag> tags;
        IEnumerable<InternalTag> rootTags;
        Dictionary<string, LinkedListNode<IPhoto>> photosByIds;
        LinkedList<IPhoto> timelinePhotos;
        Dictionary<DateTime, LinkedListNode<IPhoto>> photosByDays;

        string user;
        public UserPhotoLibrary(string user, IEnumerable<ITag> originalRootTags, TagUserAuthorizations auths)
        {
            this.user = user;
            buildIndexTables(originalRootTags, auths);
        }

        private void buildIndexTables(IEnumerable<ITag> originalRootTags, TagUserAuthorizations auths)
        {
            //Search for visible tags and authorized photos
            Dictionary<string, IPhoto> authorizedPhotos = new Dictionary<string, IPhoto>();
            List<ITag> authorizedTags = getAuthorizedTags(originalRootTags, auths).ToList();
            HashSet<ITag> visibleTags = new HashSet<ITag>();
            foreach (var tag in authorizedTags)
            {
                foreach (var photo in tag.Photos)
                {
                    //even not directly authorized tags can be traversed or can reference authorized photo.
                    //We register them, but not theirs photos.
                    photo.Tags
                        .ForEach(t => getTagsHierarchy(t)
                            .ForEach(t2 => visibleTags.Add(t2)));

                    //Register photos from a directly authorized tag.
                    authorizedPhotos[photo.Id] = photo;
                }
            }

            //root tags
            this.rootTags = (from rootTag in originalRootTags.Intersect(visibleTags)
                                select new InternalTag(rootTag, authorizedTags, visibleTags, null)).ToList();

            //tag index
            tags = new Dictionary<string, InternalTag>();
            foreach (var rootTag in rootTags)
            {
                tags[rootTag.Id] = rootTag;
                foreach(var subTag in rootTag.AllTags)
                    tags[subTag.Id] = subTag;
            }

            //Order photo in a timeline list.
            var tempTimeline = new List<IPhoto>(authorizedPhotos.Values);
            tempTimeline.Sort((p1, p2) => p1.Date.CompareTo(p2.Date));
            timelinePhotos = new LinkedList<IPhoto>(tempTimeline);

            //day index for timeline.
            photosByDays = new Dictionary<DateTime, LinkedListNode<IPhoto>>();
            for (var item = timelinePhotos.First; item != null; item = item.Next)
                if (item == timelinePhotos.First || item.Previous.Value.Date.Date != item.Value.Date.Date)
                    photosByDays[item.Value.Date.Date] = item;

            //photo ids index for timeline.
            photosByIds = new Dictionary<string, LinkedListNode<IPhoto>>();
            for (var item = timelinePhotos.First; item != null; item = item.Next)
                photosByIds[item.Value.Id] = item;

        }

        private IEnumerable<ITag> getTagsHierarchy(ITag tag)
        {
            for(ITag current = tag; current != null; current = current.Parent)
                yield return current;
        }

        private IEnumerable<ITag> getAuthorizedTags(IEnumerable<ITag> originalRootTags, TagUserAuthorizations auths)
        {
            return from tagId in auths.GetTags(user)
                   select getTagById(originalRootTags, tagId);
        }

        private ITag getTagById(IEnumerable<ITag> tags, string tagId)
        {
            foreach (var tag in tags)
            {
                if (tag.Id == tagId)
                    return tag;
                var cTag = getTagById(tag.ChildTags, tagId);
                if (cTag != null)
                    return cTag;
            }
            return null;
        }

        internal IEnumerable<InternalTag> GetRootTags()
        {
            return rootTags;
        }

        internal LinkedListNode<IPhoto> GetPhotoById(string photoId)
        {
            LinkedListNode<IPhoto> res = null;
            photosByIds.TryGetValue(photoId, out res);
            return res;
        }

        internal InternalTag GetTagById(string tagId)
        {
            InternalTag res = null;
            tags.TryGetValue(tagId, out res);
            return res;
        }

        internal bool TagExists(string tagId)
        {
            return tags.ContainsKey(tagId);
        }

        internal IEnumerable<IPhoto> GetPhotos()
        {
            return timelinePhotos;
        }

        internal LinkedListNode<IPhoto> GetPhotoByDay(DateTime d)
        {
            LinkedListNode<IPhoto> res;
            photosByDays.TryGetValue(d, out res);
            return res;
        }
    }
}
