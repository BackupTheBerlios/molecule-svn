//
// MusicLibrary.cs
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
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using WebMusic.Providers;
using System.IO;
using Molecule.Web;
using System.Web;
using Molecule.Runtime;
using Molecule.Atome;


namespace WebMusic.Services
{
    public class MusicLibrary : AtomeProviderBase<MusicLibrary, IMusicLibraryProvider>
    {

        static MusicLibrary()
        {
            
        }

        private MusicLibrary()
        {
        }

        IDictionary<string, IAlbum> albums;
        IDictionary<string, IArtist> artists;
        IDictionary<string, ISong> songs;


        protected override void OnProviderUpdated()
        {
            updateProviderArtists();          
        }

        private void buildIndexTables(IEnumerable<IArtist> providerArtists)
        {
            artists = new Dictionary<string, IArtist>();
            albums = new Dictionary<string, IAlbum>();
            songs = new Dictionary<string, ISong>();

            foreach (var artist in providerArtists)
            {
                artists[artist.Id] = artist;
                foreach (var album in artist.Albums)
                {
                    albums[album.Id] = album;
                    foreach (var song in album.Songs)
                        songs[song.Id] = song;
                }
            }
            if (log.IsDebugEnabled)
            {
                log.Debug(String.Format("Statistics : {0} artists, {1} albums and {2} songs"
                , artists.Count, albums.Count, songs.Count));
            }
        }

        private void updateProviderArtists()
        {
            IEnumerable<IArtist> providerArtists = null;
            IEnumerable<string> albumsRecentlyAdded = null;

            CallProvider(provider =>
            {
                providerArtists = provider.GetArtists();
                albumsRecentlyAdded = provider.AlbumsRecentlyAdded;
            });

            buildIndexTables(providerArtists);
            updateAlbumsRecentlyAdded(albumsRecentlyAdded);
        }

        private static void updateAlbumsRecentlyAdded(IEnumerable<string> albumsRecentlyAdded)
        {
            Molecule.Log.LogService.Instance.ClearType("Music");
            if (albumsRecentlyAdded != null)
            {
                foreach (string recentlyAdded in albumsRecentlyAdded)
                {
                    Molecule.Log.LogService.Instance.AddSemanticEvent("Music", DateTime.Now, recentlyAdded + " added", null, null, null);
                }
            }
        }

        public static IEnumerable<ISong> GetSongs()
        {
            return Instance.songs.Values;
        }

        public static IEnumerable<IArtist> GetArtists()
        {
			
            return  Instance.artists.Values.OrderBy(artist => artist.Name);
        }

        public static IEnumerable<IAlbum> GetAlbums()
        {
            return Instance.albums.Values.OrderBy(album => album.Name);
        }

        public static ISong GetSong(string id)
        {
            ISong res;
            Instance.songs.TryGetValue(id, out res);
            return res;
        }

        public static IEnumerable<IAlbum> GetAlbumsByArtist(string artist)
        {
			// if no artist specified we return all albums.
			if(String.IsNullOrEmpty (artist))
			{
				foreach (var album in  Instance.albums.Values.OrderBy(album => album.Name))
				         yield return album;
			}
            else if (Instance.artists.ContainsKey(artist))
            {
                foreach (var album in Instance.artists[artist].Albums.OrderBy(album => album.Name))
                    yield return album;
            }
        }

        public static IEnumerable<IAlbum> GetAlbumsByArtists(IEnumerable<string> artists)
        {
            if (artists != null)
            {
                foreach (var artist in artists)
                    foreach (var album in GetAlbumsByArtist(artist))
                        yield return album;
            }
        }

        public static IEnumerable<ISong> GetSongsByAlbum(string album)
        {
            if (!String.IsNullOrEmpty(album) && Instance.albums.ContainsKey(album))
            {
                foreach (var song in Instance.albums[album].Songs.OrderBy(song => song.AlbumTrack))
                    yield return song;
            }
        }

        public static IEnumerable<ISong> GetSongsByAlbums(IEnumerable<string> albums)
        {
            if (albums != null)
            {
                foreach (var album in albums)
                    foreach (var song in GetSongsByAlbum(album))
                        yield return song;
            }
        }

        public static IEnumerable<ISong> GetSongByArtistAndAlbum(string artist, string album)
        {
			// if we have not selected an artist we are looking for something like a compilation
			if(!String.IsNullOrEmpty(album) && String.IsNullOrEmpty(artist) && Instance.albums.ContainsKey(album)) 
			{
                foreach (var song in Instance.albums[album].Songs.OrderBy(song => song.AlbumTrack))
                    yield return song;				
			}
            else if (!String.IsNullOrEmpty(album) && !String.IsNullOrEmpty(artist) && Instance.artists.ContainsKey(artist) && Instance.albums.ContainsKey(album))
            {
                foreach (var song in Instance.albums[album].Songs.Where(song => song.Artist.Id == artist).OrderBy(song => song.AlbumTrack))
                    yield return song;
            }		
        }
		
		
        public static IEnumerable<ISong> SearchSongs(string pattern, bool inAlbums, bool inTitles, bool inArtists)
        {
            pattern = pattern.ToLower();
            return from song in Instance.songs.Values
                   let artists = from artist in Instance.artists.Values
                          where artist.Name.ToLower().Contains(pattern)
                          select artist
                   let albums = from album in Instance.albums.Values
                         where album.Name.ToLower().Contains(pattern)
                         select album
                   where inTitles && song.Title.ToLower().Contains(pattern)
                   || inAlbums && albums.Contains(song.Album)
                   || inArtists && artists.Contains(song.Artist)
                   orderby song.AlbumTrack
                   orderby song.Album.Name
                   orderby song.Artist.Name                   
                   select song;
        }

        public static IEnumerable<string> SearchStringContainingPattern(string pattern, bool inAlbums, bool inTitles, bool inArtists)
        {
            pattern = pattern.ToLower();
            return (from artist in Instance.artists.Values
                    where artist.Name.ToLower().Contains(pattern)
                    select artist.Name).Union<string>(
                   from album in Instance.albums.Values
                   where album.Name.ToLower().Contains(pattern)
                   select album.Name);
        }

        protected override string DefaultProvider
        {
            get { return "Stub"; }
        }

        protected override string ProviderDirectory
        {
            get { return HttpContext.Current.Server.MapPath("~/atomes/music/bin/providers"); }
        }
    }
}
