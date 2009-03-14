// Copyright (c) 2009 Pascal Fresnay (dev.molecule@free.fr) - Mickael Renault (dev.molecule@free.fr) 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Molecule.Runtime;
using Molecule.Collections;
using Mono.Rocks;

[assembly:PluginContainer]

namespace WebPhoto.Providers.Stub
{
    [Plugin("Stub")]
    public class PhotoLibraryProvider : IPhotoLibraryProvider
    {
        const int nbRootTags = 5;
        const int nbMaxSubTagsByTags = 3;
        const int nbMaxPhotosByTags = 40;
        const int maxDepth = 2;
        const int nbJpg = 10;
        const int nbMaxTagByPhoto = 3;
        const int nbPhotos = 500;

        const string fakeText = @"Sed ut perspiciatis unde omnis iste natus error 
            sit voluptatem accusantium doloremque laudantium, totam rem aperiam,
            eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae
            vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas
            sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores
            eos qui ratione voluptatem sequi nesciunt.";
        [IsUsablePlugin]
        public static bool IsUsable
        {
            get
            {
                return true;
            }
        }

        #region IPhotoLibraryProvider Members

        static List<Photo> allPhotos;
        static IEnumerable<Tag> rootTags;

        public void Initialize()
        {
            allPhotos = (from nb in nbPhotos.Times()
                     select new Photo()).ToList();
            rootTags = nbRootTags.Times().Select(i => new Tag(i, null));
        }

        public IEnumerable<string> TagsRecentlyAdded
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerable<ITag> IPhotoLibraryProvider.GetRootTags()
        {
            return rootTags.Cast<ITag>();
        }

        #endregion

        public class Tag : ITag
        {
            static Random rand = new Random();

            int depth;
            ITag parentTag;

            public Tag(int i, Tag parentTag)
            {
                this.parentTag = parentTag;
                if (parentTag != null)
                {
                    this.Id = parentTag.Id + "." + i;
                    this.depth = parentTag.depth + 1;
                }
                else
                    this.Id = i.ToString();

                Name = "Tag" + Id;

                int nbItem = rand.Next(nbMaxPhotosByTags);

                Photos = from nb in nbItem.Times()
                         select allPhotos[rand.Next(nbPhotos)].AddTag(this);

                int nbChilds = depth < maxDepth ? nbMaxSubTagsByTags : 0;

                ChildTags = from nb in nbChilds.Times()
                            select new Tag(nb, this) as ITag;
            }
            #region ITag Members

            public IEnumerable<ITag> ChildTags { get; set; }

            ITag ITag.Parent { get { return parentTag; } }
            ITagInfo ITagInfo.Parent { get { return parentTag; } }

            public IEnumerable<IPhoto> Photos{ get; set; }

            #endregion

            #region ITag Members

            public string Id { get; set; }

            public string Name { get; set; }
        }

            #endregion

        class Photo : IPhoto
        {
            static int photoIdBase;
            Random random;
            string description;

            public Photo()
            {
                this.Id = "photo" + photoIdBase++;
                this.random = new Random(this.Id.GetHashCode());
                this.description = fakeText.Substring(random.Next(50), 10 + random.Next(fakeText.Length - 60));
                Date = DateTime.Today - TimeSpan.FromDays(random.Next(180));
                Metadatas = new Molecule.Collections.Dictionary<string, string>();
                Metadatas["Name"] = Id + ".jpg";
                Metadatas["Exposure"] = "f/2.8 1/100 sec.";
                Metadatas["Size"] = "2592x1944";
                Metadatas["Camera"] = "Canone PaweurShote";
                int r = random.Next(nbJpg);
                string virtualPath = "stublibrary/photo"+r+".jpg";
                MediaFilePath = System.Web.HttpContext.Current.Request.MapPath(virtualPath);
            }
            #region IPhoto Members

            List<ITag> tags = new List<ITag>();
            public IEnumerable<ITag> Tags
            {
                get { return tags; }
            }

            public IPhoto AddTag(ITag tag)
            {
                tags.Add(tag);
                return this;
            }

            public string Id { get; set; }

            public string MediaFilePath
            {
                get;
                set;
            }

            public IKeyedEnumerable<string, string> Metadatas { get; set; }

            public DateTime Date { get; set; }

            public string Description
            {
                get
                {
                    return description;
                }
            }

            #endregion
        }
    }
}
