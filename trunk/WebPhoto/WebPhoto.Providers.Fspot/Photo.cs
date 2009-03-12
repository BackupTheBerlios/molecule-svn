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
using Molecule.Collections;
using System.Collections.Generic;

namespace WebPhoto.Providers.Stub
{
    public class Photo : IPhoto
	{	
        static int photoIdBase;
        Random random;
        string uri;
		
        public Photo(string uri)
        {
            this.uri = uri;
            Metadatas = new Molecule.Collections.Dictionary<string, string>();
        }

#region IPhoto Members
		
        List<ITag> tags = new List<ITag>();
        public IEnumerable<ITag> Tags
        {
    		get 
            {
                return tags; 
            }
    	}

        public IPhoto AddTag(ITag tag)
        {
            tags.Add(tag);
            return this;
        }

        public string Id 
		{ 
			get; 
			set; 
		}

        public string MediaFilePath
        {
            get
            {
                return   new Uri(uri).LocalPath;
            }
        }

        public IKeyedEnumerable<string, string> Metadatas 
		{ 
			get; 
			set; 
		}

        public DateTime Date 
		{ 
			get; 
			set; 
		}

		public string Description
		{
			get;
			set;
	    }

#endregion
    }
}
