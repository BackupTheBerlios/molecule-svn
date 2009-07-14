//
// MusicLibraryProvider.cs
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
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using Molecule.Runtime;
using System.Web;

[assembly: PluginContainer]

namespace WebMusic.Providers.Stub
{
    [Plugin("Stub")]
    public class StubProvider : IMusicLibraryProvider
    {
        const int nbArtists = 100;
        const int nbAlbumsByArtist = 3;
        const int nbSongsByAlbum = 10;


        [IsUsablePlugin]
        public static bool IsUsablePlugin
        {
            get
            {
                return true;
            }
        }

        #region IMusicLibraryProvider Members

        public void Initialize()
        {
        }

		public System.Collections.Generic.IEnumerable<string> AlbumsRecentlyAdded
		{
			get
			{
				return null;
			}
		}		
		
        public System.Collections.Generic.IEnumerable<IArtist> GetArtists()
        {
            for (int i = 0; i < nbArtists; i++)
                yield return new Artist("artist" + i);
        }

        #endregion



        class Artist : IArtist
        {
            private string name;
            private List<IAlbum> albums;

            public Artist(string name)
            {
                this.name = name;
                albums = new List<IAlbum>();
                for (int i = 0; i < nbAlbumsByArtist; i++)
                    albums.Add(new Album("albumFrom" + name + i, this));
            }

            #region IArtist Members

            public System.Collections.Generic.IEnumerable<IAlbum> Albums
            {
                get
                {
                    return albums;
                }
            }

            public string Name
            {
                get { return name; }
            }

            public string Id
            {
                get { return name; }
            }

            #endregion
        }

        class Album : IAlbum
        {
            private string name;
            private Artist artist;
            private List<ISong> songs;


            public Album(string name, Artist artist)
            {
                this.name = name;
                this.artist = artist;
                songs = new List<ISong>();
                for (int i = 0; i < nbSongsByAlbum; i++)
                    this.songs.Add(
                        new Song()
                        {
                            Title = "songFrom" + name + i,
                            Artist = artist,
                            Album = this,
                            Id = "songFrom" + name + i,
                            AlbumTrack = (uint)i
                        });
            }

            #region IAlbum Members

            public IEnumerable<ISong> Songs
            {
                get { return songs; }
            }

            public string Name
            {
                get { return name; }
            }

            public IArtist Artist
            {
                get { return artist; }
            }

            public string Id
            {
                get { return artist.Id + "." + name; }
            }

            #endregion
        }

        class Song : ISong
        {

            #region ISong Members

            public string Id { get; set; }

            public string Title { get; set; }

            public IArtist Artist { get; set; }

            public IAlbum Album { get; set; }

            public uint? AlbumTrack { get; set; }

            public TimeSpan Duration { get; set; }

            public string MediaFilePath
            {
                get
                {
                    string virtualPath = AlbumTrack % 2 == 1 ? "song1.mp3" : "song2.mp3";
                    return HttpContext.Current.Server.MapPath("/atomes/webmusic/stublibrary/" + virtualPath);
                }
            }

            #endregion
        }
    }
}