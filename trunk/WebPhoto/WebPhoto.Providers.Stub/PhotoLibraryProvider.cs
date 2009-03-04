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

        const string fakeText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus vitae lorem ornare erat rhoncus gravida. Nam non arcu. Sed accumsan risus a nulla. Cras ante. Nullam varius eros a dolor. Praesent pretium. Nullam mollis, est elementum cursus facilisis, orci purus euismod turpis, vel facilisis quam erat id velit. Morbi lacus justo, vestibulum et, aliquet sed, blandit non, nulla. Aenean molestie. Mauris nunc. ";

        [IsUsablePlugin]
        public static bool IsUsable
        {
            get
            {
                return true;
            }
        }

        #region IPhotoLibraryProvider Members

        public void Initialize()
        {
            
        }

        public IEnumerable<string> TagsRecentlyAdded
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerable<ITag> IPhotoLibraryProvider.GetRootTags()
        {
            for (int i = 0; i < nbRootTags; i++)
            {
                yield return new Tag(i, null);
            }
        }

        #endregion

        public class Tag : ITag
        {
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
                
                int nbItem = new Random(Math.Abs(Id.GetHashCode())).Next(nbMaxPhotosByTags);
                Photos = from nb in nbItem.Times()
                         select new Photo(nb, this) as IPhoto;
            }
            #region ITag Members

            public IEnumerable<ITag> ChildTags
            {
                get
                {
                    return from i in nbMaxSubTagsByTags.Times()
                        where depth < maxDepth
                        select new Tag(i, this) as ITag;
                }
            }

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
            ITag parentTag;
            Random random;
            string description;

            public Photo(int i, ITag parentTag)
            {
                this.Id = parentTag.Id + ".photo" + i;
                this.random = new Random(this.Id.GetHashCode());
                this.description = fakeText.Substring(random.Next(50), 10 + random.Next(40));
                this.parentTag = parentTag;
                Metadatas = new Molecule.Collections.Dictionary<string, string>();
                Date = DateTime.Today - TimeSpan.FromDays(random.Next(180));
                Metadatas = new Molecule.Collections.Dictionary<string, string>();
                Metadatas["Name"] = Id + ".jpg";
                Metadatas["Exposure"] = "f/2.8 1/100 sec.";
                Metadatas["Size"] = "2592x1944";
                Metadatas["Camera"] = "Canone PaweurShote";
            }
            #region IPhoto Members

            public IEnumerable<ITag> Tags
            {
                get { yield return parentTag; }
            }

            public string Id { get; set; }

            public string MediaFilePath
            {
                get
                {
                    int r = Math.Abs(Id.GetHashCode()) % nbJpg;
                    string virtualPath = "stublibrary/photo"+r+".jpg";
                    return System.Web.HttpContext.Current.Request.MapPath(virtualPath);
                }
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
