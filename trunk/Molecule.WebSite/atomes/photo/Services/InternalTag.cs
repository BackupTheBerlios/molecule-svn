using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoto.Providers;

namespace WebPhoto.Services
{
    internal class InternalTag : ITag
    {
        ITag originalTag;
        IEnumerable<IPhoto> authorizedPhotos;
        IEnumerable<InternalTag> childTags;
        InternalTag parentTag;

        public InternalTag(ITag originalTag, IEnumerable<ITag> authorizedTags, IEnumerable<ITag> visibleTags, InternalTag parentTag)
        {
            this.originalTag = originalTag;
            this.parentTag = parentTag;

            authorizedPhotos = (from photo in originalTag.Photos
                                   where photo.Tags.Any(t => authorizedTags.Any(t2 => t2.Id == t.Id))
                                   orderby photo.Date //descending
                                   select photo
                               ).ToList();

            this.childTags = (from tag in originalTag.ChildTags
                              join vTag in visibleTags on tag.Id equals vTag.Id
                              select new InternalTag(tag, authorizedTags, visibleTags, this)).ToList();
        }

        #region ITagInfo Members

        public string Id
        {
            get { return originalTag.Id; }
        }

        public string Name
        {
            get { return originalTag.Name; }
        }

        ITagInfo ITagInfo.Parent
        {
            get { return parentTag; }
        }

        #endregion

        internal InternalTag Parent
        {
            get { return parentTag; }
        }

        #region ITag Members

        public IEnumerable<ITag> ChildTags
        {
            get { return childTags.Cast<ITag>(); }
        }

        ITag ITag.Parent
        {
            get { return parentTag; }
        }

        public IEnumerable<IPhoto> Photos
        {
            get { return authorizedPhotos; }
        }

        #endregion

        internal IEnumerable<InternalTag> AllTags
        {
            get
            {
                foreach (var t in childTags)
                {
                    yield return t;
                    foreach (var subT in t.AllTags)
                        yield return subT;
                }
            }
        }
    }
}
