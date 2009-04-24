//
// Mp3MetadataReader.ashx.cs
//
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
using TagLib;

namespace Molecule.Metadata
{
	public class Mp3MetadataReader
	{
		private string filePath;
		
		public Mp3MetadataReader(string filePath)
		{
			this.filePath = filePath;
			
		}
		
		public void RetrieveMetadatas()
		{
			TagLib.File file = TagLib.File.Create(this.filePath);
			this.Bitrate = file.Properties.AudioBitrate;
			this.Duration = file.Properties.Duration;
			this.Album = file.Tag.Album;
			this.Artist = file.Tag.FirstPerformer;
			this.Title = file.Tag.Title;
		}
		

		public string Artist
		{
			get;
			private set;	
		}
				
		public string Album
		{
		    get;
			private set;
				
		}
				
	    public string Title
		{
		    get;
			private set;
		}
				
		public TimeSpan Duration
		{
		    get;
			private set;
		}
		
        public int Bitrate
		{
			get;
			private set;
		}
		
	}
}
