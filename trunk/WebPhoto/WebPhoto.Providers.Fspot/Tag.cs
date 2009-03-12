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
using System.Collections.Generic;
using System.IO;

using Mono.Data.Sqlite;

namespace WebPhoto.Providers.Stub
{
	public class Tag : ITag
	{
		
	    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Tag));		
		static Random rand = new Random();
		List<Tag> childTags;
		int depth;
		ITag parentTag;
		
		
		public Tag(string id, string name)
		{
			this.Id = id;
			this.Name = name;
			childTags = new List<Tag>();
		}			
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		public void RetrieveChildTagsFromDatabase(SqliteConnection conn)
		{
			if( log.IsDebugEnabled)
			{
				log.DebugFormat("Get child tags for the tag with id {0}", this.Id);
			}
			SqliteCommand cmd = new SqliteCommand("SELECT id, name " +
			                                      "FROM tags "+
			                                      "WHERE category_id = $TagId;", conn);
			
			
			cmd.Parameters.Add("$TagId", DbType.String).Value = this.Id;
			
			IDataReader reader = null;
			try
			{
				//conn.Open();
				
				reader = cmd.ExecuteReader();
				
				while(reader.Read()) {
					
					string tagId = reader.GetValue (0).ToString();
					string tagName = reader.GetValue (1).ToString();

					childTags.Add(new Tag(tagId, tagName));
					if( log.IsDebugEnabled)
					{
						log.DebugFormat("Child tags for the tag {0} : id {1} name {2}", this.Id, tagId, tagName);
					}
				}
			}
			finally
			{
				reader.Close();
				reader = null;
			}			
			
			if( log.IsDebugEnabled)
			{
				log.DebugFormat("Get photo list for the tag {0} with id {1} : Done",  this.Name,this.Id);
			}

		}
		
		public IEnumerable<ITag> ChildTags { 
			get
			{
				List<Tag> childTags = new List<Tag>();
				foreach(Tag childTag in childTags)
				{
					yield return childTag;
				}
			}
			set
			{
			
			
			}
		}
		
		ITag ITag.Parent { get { return parentTag; } }
		ITagInfo ITagInfo.Parent { get { return parentTag; } }
		
		
		// have to be optimized.
		// We open and close the connexion each time we get photos of one tag
		private List<Photo> GetPhotos()
		{
			if( log.IsDebugEnabled)
			{
				log.DebugFormat("Get photo list for the tag {0} with id {1}", this.Name, this.Id);
			}

			List<Photo> photos = new List<Photo>();
				
			SqliteConnection conn = new SqliteConnection(PhotoLibraryProvider.ConnectionString);
			SqliteCommand cmd = new SqliteCommand("SELECT  photos.id , photos.uri, photos.time, photos.description " +
			                                      "FROM  tags , photo_tags, photos "+
			                                      "WHERE tags.id = photo_tags.tag_id and photos.id = photo_tags.photo_id  and tags.id = $TagId;", conn);
			
			
			cmd.Parameters.Add("$TagId", DbType.String).Value = this.Id;
			IDataReader reader = null;
			try
			{
				conn.Open();
				
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
						photo.AddTag(this);
						photo.Id = photoId;
						photo.Description = photoDescription;
						photos.Add(photo); 					
					}
				}
			}
			finally
			{
				reader.Close();
				reader = null;

				conn.Close();
				conn = null;
			}			
			
			if( log.IsDebugEnabled)
			{
				log.DebugFormat("Get photo list for the tag {0} with id {1} : Done",  this.Name,this.Id);
			}
			return photos;
		}
		
		public IEnumerable<IPhoto> Photos{ 
			get
			{
				List<Photo> photos = GetPhotos();
				
				if(log.IsDebugEnabled)
				{
					log.DebugFormat("Tag {0}  with id {1} contains {2} photos", this.Name, this.Id, photos.Count);
				}
				
				foreach(Photo photo in photos)
				{
					yield return (IPhoto) photo;
				}
			}
		}

		
		public string Id { get; set; }
		public string Name { get; set; }
	}
	
}
