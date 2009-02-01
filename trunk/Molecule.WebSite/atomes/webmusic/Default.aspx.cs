//
// Default.aspx.cs
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
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebMusic.Services;
using System.Collections.Generic;
using WebMusic.Providers;
using System.IO;

namespace WebMusic
{
    public partial class _Default : System.Web.UI.Page, ICallbackEventHandler
    {
		private static log4net.ILog log = log4net.LogManager.GetLogger( typeof( _Default ) );

        const string sessionCurrentArtist = "music.currentArtist";
        const string sessionCurrentAlbum = "music.currentAlbum";

        public _Default()
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string reference = Page.ClientScript.GetCallbackEventReference(this, "arg", "SongEvent", "context");
            string script = "function UseCallback(arg,context) { "+ reference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "UseCallback", script, true);
        }

        private void SongCurrentlyPlaying(string songId)
        {
			if( User.IsInRole("admin"))
			{
				ISong song = MusicLibrary.GetSong(songId);
				if (log.IsDebugEnabled)
				{
					log.Debug("Currently playing  " + song.Title + " of " + song.Artist.Name);
				}
                Lastfm.LastfmService.NowPlaying(song.Artist.Name, song.Title, song.Album.Name, (int)song.Duration.TotalSeconds , (int)song.AlbumTrack.Value);
			}
		}
		
        private void SongPlayed(string songId)
		{
			if( User.IsInRole("admin"))
			{
				ISong song = MusicLibrary.GetSong(songId);
				if (log.IsDebugEnabled)
				{
					log.Debug("Scrobbling " + song.Title + " of " + song.Artist.Name);
				}
				Lastfm.LastfmService.Scrobble(song.Artist.Name, song.Album.Name, song.Title, (int)song.AlbumTrack.GetValueOrDefault(), (int)song.Duration.TotalSeconds, DateTime.Now.Subtract(song.Duration));
			}
		}
		
        protected void albumsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //List<string> selectedAlbums = new List<string>();
            //foreach (ListItem item in albumsListBox.Items)
            //    if (item.Selected)
            //        selectedAlbums.Add(item.Value);

            //IEnumerable<WebMusic.Providers.ISong> songsAlbum = MusicLibrary.GetSongsByAlbums(selectedAlbums);
            //songsList = songsAlbum;
            //this.SongsView.DataSource = songsList;
            //this.SongsView.DataBind();
        }

        protected void artistsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //List<string> selectedArtists = new List<string>();
            //foreach (ListItem item in artistsListBox.Items)
            //    if (item.Selected)
            //        selectedArtists.Add(item.Value);

            //List<string> selectedAlbums = new List<string>();
            //foreach (ListItem item in albumsListBox.Items)
            //    if (item.Selected)
            //    {
            //        selectedAlbums.Add(item.Value);
            //    }
			
            //this.albumsListBox.DataSource = MusicLibrary.GetAlbumsByArtists(selectedArtists);
            //this.albumsListBox.DataBind();

            ////restore selected albums, overriden by previous databinding.
            //List<string> newSelectedAlbums = new List<string>();
            //foreach (string album in selectedAlbums)
            //{
            //    var item = this.albumsListBox.Items.FindByValue(album);
            //    if (item != null)
            //    {
            //        item.Selected = true;
            //        if( log.IsDebugEnabled)
            //        {
            //            log.Debug("New selected album "+album);
            //        }
            //        newSelectedAlbums.Add(album);
            //    }
            //}

			
            //IEnumerable<WebMusic.Providers.ISong> songsAlbum = MusicLibrary.GetSongsByAlbums(newSelectedAlbums);
            //songsList = songsAlbum;
            //this.SongsView.DataSource = songsList;
            //this.SongsView.DataBind();
        }

        protected void artistsListBox_Init(object sender, EventArgs e)
        {
            //this.artistsListBox.DataSource = MusicLibrary.GetArtists();
            //this.artistsListBox.DataBind();
        }

        protected void albumsListBox_Init(object sender, EventArgs e)
        {
            //this.albumsListBox.DataSource = MusicLibrary.GetAlbums();
            //this.albumsListBox.DataBind();
        }

        protected void searchButton_Click(object sender, EventArgs e)
        {
            //songsList = Services.MusicLibrary.SearchSongs(searchTextBox.Text,
            //    searchInAlbumsCheckBox.Checked,
            //    searchInTitlesCheckBox.Checked,
            //    searchInArtistsCheckBox.Checked);
            //this.SongsView.DataSource = songsList;
            //this.SongsView.DataBind();
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            if (!String.IsNullOrEmpty(eventArgument))
            {
                string[] res = eventArgument.Split(';');
                if (res[0].Equals("idSongPlayed"))
                {
                    SongPlayed(res[1]);
                }
                else if(res[0].Equals("idSongCurrentlyPlaying"))
                {
                    SongCurrentlyPlaying(res[1]);
                }
            }
        }

        public string FormatDuration(TimeSpan duration)
        {
            return duration.Minutes.ToString("00") + ":" + duration.Seconds.ToString("00");
        }

        public string GetCallbackResult()
        {
            return null;
        }

        protected void ArtistList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArtistList.DataBind();
            Session[sessionCurrentArtist] = (string)ArtistList.SelectedValue;
        }

        protected void AlbumList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlbumList.DataBind();
            Session[sessionCurrentAlbum] = (string)AlbumList.SelectedValue;
        }
    }
}
