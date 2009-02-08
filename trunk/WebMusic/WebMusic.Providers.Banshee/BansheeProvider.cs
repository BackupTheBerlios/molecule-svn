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
		private static string bansheeDatabase = bansheeDatabase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),".config/banshee-1/banshee.db");
		private string defaultLibraryPath;
		public List<string> albumsRecentlyAdded;
		
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

		public void Initialize ()
		{
			defaultLibraryPath = XdgBaseDirectorySpec.GetUserDirectory("XDG_MUSIC_DIR", "Music");
			
			string connectionString = String.Format("URI=file:{0},version=3",bansheeDatabase);
			Console.WriteLine(connectionString);
		    IDbConnection dbcon;
			dbcon = (IDbConnection) new SqliteConnection(connectionString);
			dbcon.Open();
			
			this.FillArtistsList(dbcon);
			this.FillAlbumsList(dbcon);
			this.FillSongsList(dbcon);
			this.UpdateRecentlyAddedAlbums(dbcon);
			
			dbcon.Close();
			dbcon = null;
		}

		private void UpdateRecentlyAddedAlbums(IDbConnection dbcon)
		{
			albumsRecentlyAdded = new List<string>();
			IDbCommand dbcmd = dbcon.CreateCommand();			
			string sql =
			"SELECT  DISTINCT albums.albumTitle "+
            "FROM ( "+
            "SELECT CoreAlbums.Title albumTitle, CoreTracks.DateAddedStamp dateAddedTimeStamp  "+
            "FROM CoreArtists, CoreTracks, CoreAlbums  "+
            "WHERE CoreArtists.ArtistID = CoreTracks.ArtistID and CoreTracks.AlbumID = CoreAlbums.AlbumID and PrimarySourceID = 1 "+
            "ORDER BY CoreTracks.DateAddedStamp DESC   "+
            ") albums "+
            "LIMIT 5; ";
			dbcmd.CommandText = sql;
 

			IDataReader reader = dbcmd.ExecuteReader();
			while(reader.Read()) {
				albumsRecentlyAdded.Add(reader.GetValue (0).ToString());
			}
			// clean up
			reader.Close();
			reader = null;
			dbcmd.Dispose();
			dbcmd = null;
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
			IDbCommand dbcmd = dbcon.CreateCommand();			
			string sql =
				"SELECT ArtistID, Name " +
				"FROM CoreArtists";
			dbcmd.CommandText = sql;
 

			IDataReader reader = dbcmd.ExecuteReader();
			while(reader.Read()) {
				
				Artist a;
				string ArtistID = reader.GetValue (0).ToString();
				string ArtistName = reader.GetValue (1).ToString();
				if( artists.ContainsKey(ArtistID))
				{
					a = artists[ArtistID];
				}
				else
				{
					a = new Artist(ArtistID, ArtistName);
					artists.Add(ArtistID,a);
				}	
			}
			// clean up
			reader.Close();
			reader = null;
			dbcmd.Dispose();
			dbcmd = null;
			
		}
		
		
		private void FillAlbumsList(IDbConnection dbcon)
		{
			IDbCommand dbcmd = dbcon.CreateCommand();			
			string sql =
				"SELECT CoreTracks.ArtistID, CoreTracks.AlbumID, CoreAlbums.Title " +
				"FROM CoreTracks, CoreAlbums " +
				"where CoreTracks.AlbumID = CoreAlbums.AlbumID";
			dbcmd.CommandText = sql;
 

			IDataReader reader = dbcmd.ExecuteReader();
			while(reader.Read()) 
			{
				string ArtistID = reader.GetValue (0).ToString();
                string AlbumID = reader.GetValue(1).ToString();
                string Title = reader.GetValue(2).ToString();
				
				if( artists.ContainsKey(ArtistID))
				{
					artists[ArtistID].AddAlbum(AlbumID, Title);
				}
			}
			// clean up
			reader.Close();
			reader = null;
			dbcmd.Dispose();
			dbcmd = null;
		}
		
		private void FillSongsList(IDbConnection dbcon)
		{

			IDbCommand dbcmd = dbcon.CreateCommand();			
			string sql =
				"SELECT CoreTracks.UriType, CoreTracks.TrackID, CoreArtists.ArtistID, CoreAlbums.AlbumID, CoreTracks.Title, CoreTracks.Uri, CoreTracks.TrackNumber, CoreTracks.Duration " +
				"FROM CoreArtists, CoreTracks, CoreAlbums "+
				"WHERE CoreArtists.ArtistID = CoreTracks.ArtistID and CoreTracks.AlbumID = CoreAlbums.AlbumID and PrimarySourceID = 1";
			dbcmd.CommandText = sql;
 

			IDataReader reader = dbcmd.ExecuteReader();
			while(reader.Read()) 
			{
				int uriType = reader.GetInt32(0);
				string  trackId = reader.GetValue(1).ToString();
                string artistId = reader.GetValue(2).ToString();
                string albumId = reader.GetValue(3).ToString();
                string title = reader.GetValue(4).ToString();
                string uri = reader.GetValue(5).ToString();
				uint trackNumber = UInt32.Parse(reader.GetValue(6).ToString());
				var duration = TimeSpan.FromMilliseconds(Int32.Parse(reader.GetValue(7).ToString()));
				if( uriType == 1)
				{
					uri = Path.Combine(this.defaultLibraryPath, uri);
				}
				if( artists.ContainsKey(artistId))
				{
                    artists[artistId].AddSong(albumId, title, uri, trackNumber, trackId, duration);
				}
			}
			// clean up
			reader.Close();
			reader = null;
			dbcmd.Dispose();
			dbcmd = null;
		}		
		
		


		
		public System.Collections.Generic.IEnumerable<WebMusic.Providers.IArtist> GetArtists ()
		{
			foreach(System.Collections.Generic.KeyValuePair<string,Artist> key in artists)
			{
                yield return key.Value;
			}
		}
	}
}
