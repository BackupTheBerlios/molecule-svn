//
// LastfmService.cs
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
using Molecule.Web;

namespace WebMusic.Lastfm
{
    public class LastfmService
    {
        IQueue queue;
		
        private string username;

        // md5
        private string password;

        private bool scrobblingEnabled;
				
        static LastfmService instance
        {
            get
            {
                return Singleton<LastfmService>.Instance;
            }
        }

        private LastfmService()
        {
            username = Molecule.Configuration.ConfigurationClient.Client.Get<string>("WebMusic", "LastfmUsername", String.Empty);
            password = Molecule.Configuration.ConfigurationClient.Client.Get<string>("WebMusic", "LastfmPassword", "");
            scrobblingEnabled = Molecule.Configuration.ConfigurationClient.Client.Get<bool>("WebMusic", "ScrobblingEnabled", false);
			if(scrobblingEnabled)
			{
				queue = new Lastfm.Queue();
				Lastfm.LastfmCore.AudioscrobblerQueue = queue;
				Lastfm.LastfmCore.Account.UserName = username;
				Lastfm.LastfmCore.Account.Password = password;
				Lastfm.LastfmCore.Audioscrobbler.UpdateNetworkState(true);
				Lastfm.LastfmCore.Audioscrobbler.Start();
			}
        }

        public static string Username
        {
            get
            {
                return instance.username;
            }
            set
            {
                //save username
                Molecule.Configuration.ConfigurationClient.Client.Set<string>("WebMusic", "LastfmUsername", value);
                instance.username = value;
            }
        }

        public static string Password
        {
            get
            {
                return instance.password;
            }
            set
            {
                //save password
                Molecule.Configuration.ConfigurationClient.Client.Set<string>("WebMusic", "LastfmPassword", value);
                instance.password = value;
            }
        }

        public static bool ScrobblingEnabled
        {
            get
            {
                return instance.scrobblingEnabled;
            }
            set
            {
                Molecule.Configuration.ConfigurationClient.Client.Set<bool>("WebMusic", "ScrobblingEnabled", value);
                instance.scrobblingEnabled = value;
            }
        }

		
		
		public static void Update()
		{	
			
			if( instance.scrobblingEnabled )
			{
				if( instance.queue == null )
				{
					instance.queue = new Queue();
				}
				Lastfm.LastfmCore.Audioscrobbler.Stop();
				Lastfm.LastfmCore.AudioscrobblerQueue = instance.queue;
				Lastfm.LastfmCore.Account.UserName = instance.username;
				Lastfm.LastfmCore.Account.Password = instance.password;
				Lastfm.LastfmCore.Audioscrobbler.UpdateNetworkState(true);
				Lastfm.LastfmCore.Audioscrobbler.Start();
			}	
		}
		
		public static void Scrobble(string artistName, string albumTitle, string trackTitle, int trackNumber, int durationTotalSeconds, DateTime started )
		{
			if( instance.scrobblingEnabled )
			{
				instance.queue.Add(artistName, albumTitle, trackTitle, trackNumber, durationTotalSeconds, started);
				instance.queue.Save();

			}
		}
		
		public static void NowPlaying(string artist, string title, string album, double duration, int trackNum)
		{
			if( instance.scrobblingEnabled )
			{
				LastfmCore.Audioscrobbler.NowPlaying(artist, title, album, duration, trackNum);
			}		
		}
		
    }
}
