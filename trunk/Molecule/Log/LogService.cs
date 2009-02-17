//
// LogService.cs
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
using Molecule;
using Molecule.Web;
using System.Data;
using System.Data.SqlClient;
using Mono.Data.Sqlite;


namespace Molecule.Log
{
	public class LogService
	{
		private static log4net.ILog log = log4net.LogManager.GetLogger( typeof( LogService ) );
		private List<Event> events; 
		
		private LogService()
		{
			events = new List<Event>();
		}
		
		public static LogService Instance
        {
            get
            {
                return Singleton<LogService>.Instance;
            }
        }
		
		public void AddEvent(string content)
		{
			events.Add(new Event(content));
			events.Sort((x, y) =>  x.CreationDate.CompareTo(y.CreationDate)*-1);
		}		
			           
		public IEnumerable<Event> Events
		{
			get
			{
				return (IEnumerable<Event>) events;
			}
		}
		
		public void AddSemanticEvent(string type, DateTime creationDate,string title, string description, string imageUri, string url)
		{
            using (SqliteConnection conn = new SqliteConnection(SQLiteProvidersHelper.ConnectionString))
            {
                SqliteCommand cmd = new SqliteCommand("INSERT INTO semantic_event_messages " +
                                                      " (PKID, Type, Title, CreationDate, ImageUri, Description, " +
                                                      " Url) " +
                                                      " Values(NULL, $Type, $Title, $CreationDate, $ImageUri, $Description, " +
                                                      " $Url)", conn);
                cmd.Parameters.Add("$Type", DbType.String).Value = type;
                cmd.Parameters.Add("$Title", DbType.String).Value = title;
                cmd.Parameters.Add("$CreationDate", DbType.DateTime).Value = creationDate;
                cmd.Parameters.Add("$ImageUri", DbType.String).Value = imageUri;
                cmd.Parameters.Add("$Description", DbType.String).Value = description;
                cmd.Parameters.Add("$Url", DbType.String).Value = url;

                conn.Open();
                int recAdded = cmd.ExecuteNonQuery();

                if (recAdded <= 0)
                {
                    throw new System.Data.DataException(@"Cant insert message on the database.");
                }
            }
		}
		
		
		public List<string> GetSemanticTypes()
		{
            using (SqliteConnection conn = new SqliteConnection(SQLiteProvidersHelper.ConnectionString))
            {
                SqliteCommand cmd = new SqliteCommand("SELECT DISTINCT Type FROM semantic_event_messages", conn);

                List<string> types = new List<string>();

                SqliteDataReader reader = null;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    types.Add(reader.GetString(0));
                }
                return types;
            }	
    	}
		
		public List<SemanticEvent> GetSemanticEventByType(string type)
		{
            using (SqliteConnection conn = new SqliteConnection(SQLiteProvidersHelper.ConnectionString))
            {
                SqliteCommand cmd = new SqliteCommand("SELECT Type, CreationDate,Title ,Description, ImageUri,  Url  FROM semantic_event_messages " +
                                                      "WHERE type = $Type", conn);

                cmd.Parameters.Add("$Type", DbType.String).Value = type;
                List<SemanticEvent> semanticEvents = new List<SemanticEvent>();

                SqliteDataReader reader = null;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SemanticEvent ev = new SemanticEvent(reader.GetString(0), reader.GetDateTime(1),
                                                         reader.GetString(2),
                                                         reader.IsDBNull(3) ? String.Empty : reader.GetString(3),
                                                         reader.IsDBNull(4) ? String.Empty : reader.GetString(4),
                                                         reader.IsDBNull(5) ? String.Empty : reader.GetString(5));
                    semanticEvents.Add(ev);
                }
                return semanticEvents;	
            }
    	}

		public void ClearType(string type)
		{
            using (SqliteConnection conn = new SqliteConnection(SQLiteProvidersHelper.ConnectionString))
            {
                SqliteCommand cmd = new SqliteCommand("DELETE  FROM semantic_event_messages WHERE type = $Type", conn);

                cmd.Parameters.Add("$Type", DbType.String).Value = type;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
    	}
	}
}
