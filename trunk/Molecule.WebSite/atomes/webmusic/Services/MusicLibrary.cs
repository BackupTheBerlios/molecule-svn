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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using WebMusic.Providers;
using System.IO;
using Molecule.Web;


namespace WebMusic.Services
{
    public class MusicLibrary
    {
        static object instanceLock = new object();

		private static log4net.ILog log = log4net.LogManager.GetLogger( typeof( MusicLibrary ) );

        static string providerName;

        static MusicLibrary()
        {
            providerName = Molecule.Configuration.ConfigurationClient.Client.Get<string>("WebMusic", "LibraryProvider", "Stub");
        }

        static MusicLibrary instance
        {
            get
            {
                return Singleton<MusicLibrary>.Instance;
            }
        }

        private MusicLibrary()
        {
            UpdateProvider();
        }

        IDictionary<string, IAlbum> albums;
        IDictionary<string, IArtist> artists;
        IDictionary<string, ISong> songs;
        

        public static string CurrentProvider
        {
            get { return providerName; }
            set
            {
                providerName = value;
                Molecule.Configuration.ConfigurationClient.Client.Set<string>("WebMusic", "LibraryProvider", value);
                instance.UpdateProvider();
            }
        }

        public static IEnumerable<MusicLibraryProviderInfo> Providers
        {
            get
            {
                return MusicLibraryProviderLoader.GetProviders(providerDirectory);
            }
        }

        static string providerDirectory
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/atomes/webmusic/bin/providers");
            }
        }

        protected void UpdateProvider()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug("Provider used : " + providerName);
            }

            IEnumerable<IArtist> providerArtists;
            try
            {
                var provider = MusicLibraryProviderLoader.CreateInstance(
                   providerName, providerDirectory);
                provider.Initialize();
                providerArtists = provider.GetArtists();
				// update informations about the albums recently added
				System.Collections.Generic.IEnumerable<string> albumsRecentlyAdded = provider.AlbumsRecentlyAdded;
			    Molecule.Log.LogService.Instance.ClearType("Music");
                if (albumsRecentlyAdded != null)
                {
                    foreach (string recentlyAdded in albumsRecentlyAdded)
                    {
                        Molecule.Log.LogService.Instance.AddSemanticEvent("Music", DateTime.Now, recentlyAdded + " added", null, null, null);
                    }
                }
            }
            catch (Exception e)
            {
                throw new ProviderException(providerName, e);
            }
            
            //build index tables
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

        public static IEnumerable<ISong> GetSongs()
        {
            return instance.songs.Values;
        }

        public static IEnumerable<IArtist> GetArtists()
        {
            return instance.artists.Values.OrderBy(artist => artist.Name);
        }

        public static IEnumerable<IAlbum> GetAlbums()
        {
            return instance.albums.Values.OrderBy(album => album.Name);
        }

        public static ISong GetSong(string id)
        {
            ISong res;
            instance.songs.TryGetValue(id, out res);
            return res;
        }

        public static IEnumerable<IAlbum> GetAlbumsByArtist(string artist)
        {
            if (artist != null)
            {
                foreach (var album in instance.artists[artist].Albums.OrderBy(album => album.Name))
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
            if (album != null)
            {
                foreach (var song in instance.albums[album].Songs.OrderBy(song => song.AlbumTrack))
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

        public static IEnumerable<ISong> SearchSongs(string pattern, bool inAlbums, bool inTitles, bool inArtists)
        {
            pattern = pattern.ToLower();
            return from song in instance.songs.Values
                   let artists = from artist in instance.artists.Values
                          where artist.Name.ToLower().Contains(pattern)
                          select artist
                   let albums = from album in instance.albums.Values
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
            return (from artist in instance.artists.Values
                    where artist.Name.ToLower().Contains(pattern)
                    select artist.Name).Union<string>(
                   from album in instance.albums.Values
                   where album.Name.ToLower().Contains(pattern)
                   select album.Name);
 
            /*
            return from song in instance.songs.Values
                   let artists = from artist in instance.artists.Values
                                 where artist.Name.ToLower().Contains(pattern)
                                 select artist.Name
                   let albums = from album in instance.albums.Values
                                where album.Name.ToLower().Contains(pattern)
                                select album.Name
                   where inTitles && song.Title.ToLower().Contains(pattern)
                   || inAlbums && albums.Contains(song.Album)
                   || inArtists && artists.Contains(song.Artist)
                   orderby song.AlbumTrack
                   orderby song.Album.Name
                   orderby song.Artist.Name
                   select song;
             * */
        }

    }
}
