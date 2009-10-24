//
// BansheeProvider.cs
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
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;

using Mono.Data.Sqlite;
using WebMusic.Providers.Base;

using WebMusic;
using Molecule.Runtime;
using Molecule.IO;

[assembly: PluginContainer]

namespace WebMusic.Providers.Banshee
{

    [Plugin("Banshee Music Player")]
    public class BansheeProvider : IMusicLibraryProvider
    {

        private Dictionary<string, Artist> artists;
        private static string bansheeDatabase;
        private string defaultLibraryPath;
        public List<string> albumsRecentlyAdded;

        static BansheeProvider()
        {
            var configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".config");
            var bansheeDir = Path.Combine(configDir, "banshee-1");
            if (!Directory.Exists(bansheeDir))
                bansheeDir = Path.Combine(configDir, "banshee");

            bansheeDatabase = Path.Combine(bansheeDir, "banshee.db");
        }

        public BansheeProvider()
        {
            artists = new Dictionary<string, Artist>();
        }

        [IsUsablePlugin]
        public static bool IsUsable
        {
            get
            {
                return File.Exists(bansheeDatabase);
            }
        }

        public void Initialize()
        {
            //defaultLibraryPath = XdgBaseDirectorySpec.GetUserDirectory("XDG_MUSIC_DIR", "Music");


            defaultLibraryPath = Molecule.GConf.Helper.Read("/apps/banshee-1/library/base_location");
			
            string connectionString = String.Format("URI=file:{0},version=3", bansheeDatabase);

            using (IDbConnection dbcon = new SqliteConnection(connectionString))
            {
                dbcon.Open();

                this.FillArtistsList(dbcon);
                this.FillAlbumsList(dbcon);
                this.FillSongsList(dbcon);
                this.UpdateRecentlyAddedAlbums(dbcon);
            }
        }

        private void UpdateRecentlyAddedAlbums(IDbConnection dbcon)
        {
            albumsRecentlyAdded = new List<string>();
            using (IDbCommand dbcmd = dbcon.CreateCommand())
            {
                string sql =
                "select albums.albumTitle, CoreArtists.Name,  albums.ID,  CoreArtists.ArtistID " +
                "FROM (SELECT  DISTINCT albums.albumTitle albumTitle, albums.ID  ID " +
                "FROM (  " +
                "SELECT CoreAlbums.Title albumTitle, CoreAlbums.AlbumID ID, CoreTracks.DateAddedStamp dateAddedTimeStamp   " +
                "FROM CoreArtists, CoreTracks, CoreAlbums   " +
                "WHERE CoreArtists.ArtistID = CoreTracks.ArtistID and CoreTracks.AlbumID = CoreAlbums.AlbumID and PrimarySourceID = 1  " +
                "ORDER BY CoreTracks.DateAddedStamp DESC    " +
                ") albums  " +
                "LIMIT 5) albums, CoreTracks, CoreArtists " +
                "where albums.ID = CoreTracks.AlbumID and CoreArtists.ArtistID = CoreTracks.ArtistID ";
                

                dbcmd.CommandText = sql;

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    Dictionary<string, Dictionary<string, string>> recentAlbums = new Dictionary<string, Dictionary<string, string>>();
                    while (reader.Read())
                    {
                         string albumTitle = reader.GetValue(0).ToString();
                         string artist = reader.GetValue(1).ToString();
                         string artistID = reader.GetValue(3).ToString();
                        
                         
                         if (!recentAlbums.ContainsKey(albumTitle))
                         {
                             Dictionary<string, string> arts = new Dictionary<string, string>();
                             arts.Add(artistID,artist);
                             recentAlbums.Add(albumTitle, arts);
                         }
                         else
                         {
                             var arts = recentAlbums[albumTitle];
                             if (!arts.ContainsKey(artistID))
                             {
                                 arts.Add(artistID, artist);
                             }
                         }
                    }
                    foreach (KeyValuePair <string, Dictionary<string, string>> recent in recentAlbums)
                    {
                        string artistLabel =  String.Empty;
                        if (recent.Value.Count == 1)
                        {
                            foreach( var k in recent.Value)
                            {
                                artistLabel = k.Value;
                            }
                        }
                        else
                        {
                            artistLabel = "Various artists";
                        }
                        albumsRecentlyAdded.Add(String.Format("{0} - {1} ",artistLabel ,recent.Key));
                    }
                }
            }
        }

        public System.Collections.Generic.IEnumerable<string> AlbumsRecentlyAdded
        {
            get
            {
                return albumsRecentlyAdded;
            }
        }

        private void FillArtistsList(IDbConnection dbcon)
        {
            using (IDbCommand dbcmd = dbcon.CreateCommand())
            {
                string sql =
                    "SELECT ArtistID, Name " +
                    "FROM CoreArtists";
                dbcmd.CommandText = sql;

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Artist a;
                        string ArtistID = reader.GetValue(0).ToString();
                        string ArtistName = reader.GetValue(1).ToString();
                        if (artists.ContainsKey(ArtistID))
                        {
                            a = artists[ArtistID];
                        }
                        else
                        {
                            a = new Artist(ArtistID, ArtistName);
                            artists.Add(ArtistID, a);
                        }
                    }
                }
            }
        }


        private void FillAlbumsList(IDbConnection dbcon)
        {
            using (IDbCommand dbcmd = dbcon.CreateCommand())
            {
                string sql =
                    "SELECT CoreTracks.ArtistID, CoreTracks.AlbumID, CoreAlbums.Title " +
                    "FROM CoreTracks, CoreAlbums " +
                    "where CoreTracks.AlbumID = CoreAlbums.AlbumID";
                dbcmd.CommandText = sql;

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string ArtistID = reader.GetValue(0).ToString();
                        string AlbumID = reader.GetValue(1).ToString();
                        string Title = reader.GetValue(2).ToString();

                        if (artists.ContainsKey(ArtistID))
                        {
                            artists[ArtistID].AddAlbum(AlbumID, Title);
                        }
                    }
                }
            }
        }

        private void FillSongsList(IDbConnection dbcon)
        {
            using (IDbCommand dbcmd = dbcon.CreateCommand())
            {
                string sql =
                    "SELECT CoreTracks.UriType, CoreTracks.TrackID, CoreArtists.ArtistID, CoreAlbums.AlbumID, CoreTracks.Title, CoreTracks.Uri, CoreTracks.TrackNumber, CoreTracks.Duration " +
                    "FROM CoreArtists, CoreTracks, CoreAlbums " +
                    "WHERE CoreArtists.ArtistID = CoreTracks.ArtistID and CoreTracks.AlbumID = CoreAlbums.AlbumID and PrimarySourceID = 1 and MimeType = 'taglib/mp3'";
                dbcmd.CommandText = sql;
                

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //int uriType = reader.GetInt32(0);
                        string trackId = reader.GetValue(1).ToString();
                        string artistId = reader.GetValue(2).ToString();
                        string albumId = reader.GetValue(3).ToString();
                        string title = reader.GetValue(4).ToString();
                        string uri = reader.GetValue(5).ToString();
                        uint trackNumber = UInt32.Parse(reader.GetValue(6).ToString());
                        var duration = TimeSpan.FromMilliseconds(Int32.Parse(reader.GetValue(7).ToString()));
                        if (uri.StartsWith("file://"))
							uri = new Uri(uri).LocalPath;
                            
						else
							uri = Path.Combine(defaultLibraryPath, uri);
						
                        if (artists.ContainsKey(artistId))
                        {
                            artists[artistId].AddSong(albumId, title, uri, trackNumber, trackId, duration);
                        }
                    }
                }
            }
        }        


        public System.Collections.Generic.IEnumerable<WebMusic.Providers.IArtist> GetArtists()
        {
            return from artist in artists.Values
				where artist.Albums.Any()
				select artist as IArtist;
        }
    }
}
