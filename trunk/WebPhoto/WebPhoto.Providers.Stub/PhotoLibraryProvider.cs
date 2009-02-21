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

[assembly:PluginContainer]

namespace WebPhoto.Providers.Stub
{
    [Plugin("Stub")]
    public class PhotoLibraryProvider : IPhotoLibraryProvider
    {
        const int nbRootTags = 5;
        const int nbMaxSubTagsByTags = 3;
        const int nbMaxPhotosByTags = 30;
        const int maxDepth = 3;
        const int nbJpg = 10;

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
                yield return new Tag(i.ToString(), 0, null);
            }
        }

        #endregion

        public class Tag : ITag
        {
            int depth;
            ITag parentTag;

            public Tag(string id, int depth, ITag parentTag)
            {
                this.parentTag = parentTag;
                this.Id = id;
                Name = "Tag" + Id;
                this.depth = depth;
            }
            #region ITag Members

            public IEnumerable<ITag> ChildTags
            {
                get
                {
                    if (depth < maxDepth)
                    {
                        for (int i = 0; i < nbMaxSubTagsByTags; i++)
                            yield return new Tag(Id + "." + i, depth + 1, this);
                    }
                }
            }

            public ITag Parent { get; set; }

            public IEnumerable<IPhoto> Photos
            {
                get
                {
                    int nbItem = new Random(Math.Abs(Id.GetHashCode())).Next(nbMaxPhotosByTags);
                    for (int i = 0; i < nbItem; i++)
                        yield return new Photo(Id+".photo"+i, this);
                }
            }

            #endregion

            #region ITag Members

            public string Id { get; set; }

            public string Name { get; set; }
        }

            #endregion

        class Photo : IPhoto
        {
            ITag parentTag;

            public Photo(string id, ITag parentTag)
            {
                this.Id = id;
                this.parentTag = parentTag;
                Metadatas = new Molecule.Collections.Dictionary<string, string>();
                Date = DateTime.Today - TimeSpan.FromDays(new Random(this.Id.GetHashCode()).Next(60));
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

            #endregion
        }
    }
}
