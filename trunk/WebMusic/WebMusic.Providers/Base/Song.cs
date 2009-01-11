//
// Song.cs
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

namespace WebMusic.Providers.Base
{
	
	
	public class Song : ISong
	{
		Album album;
		Artist artist;
		string title;
		string id;
		string mediaFilePath;
		Nullable<uint> albumTrack;
        TimeSpan duration;

		public Song(string title, Album album, Artist artist, string mediaFilePath,Nullable<uint> albumTrack, string trackId, TimeSpan duration )
		{
			this.title = title;
			this.album = album;
			this.artist = artist;
			this.mediaFilePath = mediaFilePath;
			this.albumTrack = albumTrack;
			this.id = trackId;
            this.duration = duration;
		}		
		
		public string Id {
			get {
				return id;
			}
		}

		public string Title {
			get {
				return title;
			}
		}

		public WebMusic.Providers.IArtist Artist {
			get {
				return artist;
			}
		}

		public WebMusic.Providers.IAlbum Album {
			get {
				return album;
			}
		}

		public string MediaFilePath {
			get {
				return mediaFilePath;
			}
		}

		public uint? AlbumTrack {
			get {
				return albumTrack;
			}
		}

        public TimeSpan Duration
        {
            get { return duration; }
        }
    }
}
