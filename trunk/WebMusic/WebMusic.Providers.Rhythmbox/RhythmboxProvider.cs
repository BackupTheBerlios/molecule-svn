//
// RhythmboxProvider.cs
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
using System.Collections.Generic;
using WebMusic.Providers;
using System.Linq;
using System.Xml.Linq;
using WebMusic.Providers.Base;
using Molecule.Runtime;

[assembly: PluginContainer]

namespace WebMusic.Providers.Rhythmbox
{
	
	[Plugin("Rhythmbox Music Player")]
	public class RhythmboxProvider :IMusicLibraryProvider
	{
        private static string rhythmboxDatabase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".gnome2/rhythmbox/rhythmdb.xml");
		private Dictionary<string, Artist> artists;
		
		private static log4net.ILog log = log4net.LogManager.GetLogger( typeof( RhythmboxProvider ) );     
		
		public RhythmboxProvider()
		{
			artists = new Dictionary<string, Artist>();
		}

        [IsUsablePlugin]
        public static bool IsUsable
        {
            get
            {
                return File.Exists(rhythmboxDatabase);
            }
        }

		public void Initialize()
		{
		
			XDocument xmlDoc = XDocument.Load(rhythmboxDatabase);
	
			var entries = from e in xmlDoc.Descendants("entry")
				where ((string)e.Attribute("type")).Equals("song")
				select new 
			{
				Title = (string)e.Element("title"),
				Artist = (string)e.Element("artist"),
				Album = (string)e.Element("album"),
				Location = (string) e.Element("location"),
				ArtistId = (string) e.Element("mb-artistid"),
				AlbumId = (string) e.Element("mb-albumid"),
				TrackNumber = (string) e.Element("track-number"),
				TrackId = (string) e.Element("mb-trackid"),
				Duration = (string) e.Element("duration")
			};

            int entryCount = 0;
			foreach( var entry in entries)
			{
                entryCount++;
				Artist a;
				if( artists.ContainsKey(entry.ArtistId))
				{
					a = artists[entry.ArtistId];
				}
				else
				{
					a = new Artist(entry.ArtistId, entry.Artist);
					artists.Add(entry.ArtistId,a);
				}				
				a.AddAlbum(entry.AlbumId, entry.Album);
                var duration = TimeSpan.FromSeconds(Int32.Parse(entry.Duration));
				a.AddSong(entry.AlbumId,entry.Title, entry.Location, Convert.ToUInt32(entry.TrackNumber), entry.TrackId, duration);
			}
			log.Debug(entryCount+ " entries found in rhythmbox user database.");
		}
		
		public System.Collections.Generic.IEnumerable<string> AlbumsRecentlyAdded
		{
			get
			{
				return null;
			}
		}		
		
		public IEnumerable<IArtist> GetArtists()
		{
            foreach(System.Collections.Generic.KeyValuePair<string,  Artist> key in artists)
			{
                yield return key.Value;
			}
		}

	}
}
