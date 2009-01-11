//
// Album.cs
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
	
	
	public class Album : IAlbum
	{
		string name;
		string id;
		Artist artist;
		Dictionary<string, Song> songs;
		
		
		public Album(string id, string name, Artist artist)
		{
			this.id = id;
			this.name = name;
			this.artist = artist;
			songs = new Dictionary<string, Song>();
		}		
			
		public void AddSong(Song song)
		{
			if( !songs.ContainsKey(song.Id))
			{
				songs.Add(song.Id, song);
			}
		}
		
		public System.Collections.Generic.IEnumerable<WebMusic.Providers.ISong> Songs {
			get {
				foreach(System.Collections.Generic.KeyValuePair<string, Song> key in songs)
				{
					yield return key.Value;
				}
			}
		}

		public string Name {
			get {
				return name;
			}
		}

		public WebMusic.Providers.IArtist Artist {
			get {
				return artist;
			}
		}

		public string Id {
			get {
				return id;
			}
		}


	}
}
