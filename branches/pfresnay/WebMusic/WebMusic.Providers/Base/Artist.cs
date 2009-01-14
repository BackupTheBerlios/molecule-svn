//
// Artist.cs
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
using System.Collections.Generic;

namespace WebMusic.Providers.Base
{

	public class Artist : IArtist
	{
		private string name;
		private string id;
		
		private Dictionary<string, Album> albums;
		
		public System.Collections.Generic.IEnumerable<WebMusic.Providers.IAlbum> Albums {
			get {
				foreach(System.Collections.Generic.KeyValuePair<string,  Album> key in albums)
				{
					yield return key.Value;
				}
			}
		}

		public string Name {
			get
			{
				return name;
			}
		}

		public string Id {
			get
			{
				return id;
			}
		}

		
		public Album AddAlbum(string AlbumId, string AlbumName)
		{
			if( !albums.ContainsKey(AlbumId))
			{
                Album album = new Album(AlbumId, AlbumName, this);
                albums.Add(AlbumId, album);
                return album;
			}
            return null;
		}
	
		public void AddSong(string albumId, string title, string mediaPath, uint? trackNumber, string trackId, TimeSpan duration)   
		{
			if( albums.ContainsKey(albumId))
			{
				Song s = new Song(title, albums[albumId], this, mediaPath, trackNumber, trackId, duration);
				albums[albumId].AddSong(s);
			}
		}
		
		public Artist(string id, string name)
		{
			this.name = name;
			this.id = id;
			this.albums = new Dictionary<string, Album>();
		}
	}
}
