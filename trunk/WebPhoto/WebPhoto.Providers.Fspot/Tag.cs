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
		List<Photo> photos;
		
		public Tag(string id, string name)
		{
			this.Id = id;
			this.Name = name;
			childTags = new List<Tag>();
			photos = new List<Photo>();
		}			
		

		public void InitializeTag(SqliteConnection conn)
		{
			GetPhotos(conn);
			
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
		
		
		private void GetPhotos(SqliteConnection conn)
		{
			if( log.IsDebugEnabled)
			{
				log.DebugFormat("Get photo list for the tag {0} with id {1}", this.Name, this.Id);
			}
				
			SqliteCommand cmd = new SqliteCommand("SELECT  photo_tags.photo_id " +
			                                      "FROM  photo_tags "+
			                                      "WHERE photo_tags.tag_id = $TagId;", conn);			
			
			cmd.Parameters.Add("$TagId", DbType.String).Value = this.Id;
			IDataReader reader = null;
			try 
			{
				reader = cmd.ExecuteReader();
				System.Collections.Generic.IDictionary<string, Photo> allPhotos = PhotoLibraryProvider.AllPhotos;
				while(reader.Read()) {
					string photoId = reader.GetValue (0).ToString();
					Photo p;
					if( allPhotos.TryGetValue(photoId, out p))
					{
					    this.photos.Add(p);
						p.AddTag(this);
					}
				}
			}
			finally
			{
				reader.Close();
				reader = null;
				
				cmd.Dispose();
				cmd= null;
			}			
			
			if(log.IsDebugEnabled)
			{
				log.DebugFormat("Tag {0}  with id {1} contains {2} photos", this.Name, this.Id, photos.Count);
			}
		}
		
		public IEnumerable<IPhoto> Photos{ 
			get
			{	
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
