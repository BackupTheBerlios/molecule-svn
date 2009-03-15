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
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

using Molecule.Runtime;
using Molecule.Collections;
using Mono.Rocks;
using Mono.Data.Sqlite;

[assembly:PluginContainer]

namespace WebPhoto.Providers.Fspot
{
    [Plugin("Fspot")]
    public class PhotoLibraryProvider : IPhotoLibraryProvider
    {
		
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(PhotoLibraryProvider));			
		private static string fspotDatabase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),".gnome2/f-spot/photos.db");	
		private static  string connectionString;		
		private List<Tag> rootTags;
		private static System.Collections.Generic.Dictionary<string,Photo> photos = new System.Collections.Generic.Dictionary<string,Photo>();		
		public PhotoLibraryProvider()
		{
		}		
		
        [IsUsablePlugin]
        public static bool IsUsable
        {
            get
            {
				bool fspotDataBaseExist = File.Exists(fspotDatabase);
			    if(log.IsInfoEnabled)
			    {
				    log.InfoFormat("Fspot photo provider usable : {0}",fspotDataBaseExist );
			    }				
                return fspotDataBaseExist ;
            }
        }

        #region IPhotoLibraryProvider Members

        static List<Photo> allPhotos;

        public void Initialize()
        {
			if(log.IsInfoEnabled)
			{
				log.Info("Initialize fspot photo provider");
			}
			
			rootTags = new List<Tag>();
			SqliteConnection conn = null;
			try
			{
				conn = new SqliteConnection(ConnectionString);
				conn.Open();
                RetrievePhotosFromDatabase(conn);
				RetrieveRootTagsFromDatabase(conn);

			}
			finally
			{
				conn.Close();
				conn = null;	
			}
			
        }

		public static string ConnectionString
		{
			get
			{
				if( connectionString == null)
				{
					connectionString = String.Format("URI=file:{0},version=3",fspotDatabase);
				}
				return connectionString;
			}
		}
		
		
		public 	static	IDictionary<string, Photo> AllPhotos
		{
			get
			{
			    return (IDictionary<string, Photo>) photos;	
		    }	
        }
		
		

        public IEnumerable<string> TagsRecentlyAdded
        {
            get { throw new NotImplementedException(); }
	    }

		private void RetrieveRootTagsFromDatabase(SqliteConnection conn)
		{

			IDataReader reader = null;
			IDbCommand dbcmd = null;
			try
			{
				dbcmd = conn.CreateCommand();			
				string sql =
					"SELECT id, name " +
						"FROM tags "+
						"WHERE category_id = 0";
				dbcmd.CommandText = sql;
				
				
				reader = dbcmd.ExecuteReader();
				while(reader.Read()) {
					string tagId = reader.GetValue (0).ToString();
					string tagName = reader.GetValue (1).ToString();
					Tag t = new Tag(tagId, tagName, null);
                    t.InitializeTag(conn);
					rootTags.Add(t);
				}
				//populate the childs
				foreach ( Tag tag in  rootTags) 
				{
					tag.RetrieveChildTagsFromDatabase(conn);
				}
			}
			finally
			{
				// clean up
				reader.Close();
				reader = null;
				dbcmd.Dispose();
				dbcmd = null;	

			}			
		}
		
		private void RetrievePhotosFromDatabase(SqliteConnection conn)
		{
			int numberOfPhotosRetrieved = 0;
			int numberofPhotosNotHandled = 0;
			
			if( log.IsDebugEnabled)
			 {
			    log.Debug("Retrieve photos from the fspot database");
            }
			
			SqliteCommand cmd = new SqliteCommand("SELECT  photos.id , photos.uri, photos.time, photos.description " +
			                                      "FROM  photos ", conn);
			
			
			IDataReader reader = null;
			try
			{
			    reader = cmd.ExecuteReader();
				
				while(reader.Read()) {
					string photoId = reader.GetValue (0).ToString();
					string photoUri = reader.GetValue (1).ToString();
					string photoTime = reader.GetValue(2).ToString();
					string photoDescription = reader.GetValue(3).ToString();					
					if( photoUri.ToLower().EndsWith(".jpg") && File.Exists(new Uri(photoUri).LocalPath))
					{
						Photo photo = new Photo(photoUri);
						photo.Date =  new DateTime (1970, 1, 1).ToLocalTime ().AddSeconds(Convert.ToInt64 (photoTime));
						photo.Id = photoId;
						photo.Description = photoDescription;
						photos.Add(photoId, photo); 
                        numberOfPhotosRetrieved ++;				
				    }
			        else
				    {
				        numberofPhotosNotHandled++;	
                    }
				}
			}
			finally
			{
				reader.Close();
				reader = null;
				cmd.Dispose();
				cmd = null;	
			}			
			if(log.IsInfoEnabled)
			{
                log.InfoFormat("Retrieved {0} photos from the fspot database. {1} photos not handled", numberOfPhotosRetrieved, numberofPhotosNotHandled);
            }
		}		
		
        IEnumerable<ITag> IPhotoLibraryProvider.GetRootTags()
        {
            foreach ( Tag tag in  rootTags) 
			{
				yield return (ITag) tag;
			}
        }
        #endregion
    }
}
