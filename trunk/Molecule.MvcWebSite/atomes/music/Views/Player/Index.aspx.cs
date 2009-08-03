////
//// Default.aspx.cs
////
//// Copyright (c) 2009 Pascal Fresnay (dev.molecule@free.fr) - Mickael Renault (dev.molecule@free.fr) 
////
//// Permission is hereby granted, free of charge, to any person obtaining a copy
//// of this software and associated documentation files (the "Software"), to deal
//// in the Software without restriction, including without limitation the rights
//// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//// copies of the Software, and to permit persons to whom the Software is
//// furnished to do so, subject to the following conditions:
////
//// The above copyright notice and this permission notice shall be included in
//// all copies or substantial portions of the Software.
////
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//// THE SOFTWARE.

//using System;
//using System.Collections;
//using System.Configuration;
//using System.Data;
//using System.Linq;
//using System.Web;
//using System.Text;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
//using WebMusic.Services;
//using System.Collections.Generic;
//using WebMusic.Providers;

//using nStuff.UpdateControls;

//using System.IO;

//namespace WebMusic
//{
//    public partial class _Default : System.Web.UI.Page, ICallbackEventHandler
//    {
//        private static log4net.ILog log = log4net.LogManager.GetLogger( typeof( _Default ) );

//        const string sessionCurrentArtist = "music.currentArtist";
//        const string sessionCurrentAlbum = "music.currentAlbum";
//        const string csname = "PlaySong";
		
//        public _Default()
//        {

//        }

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if(!Page.IsPostBack)
//            {
//                this.HandlePageArguments();
//            }			
			
//            string reference = Page.ClientScript.GetCallbackEventReference(this, "arg", "SongEvent", "context");
//            string script = "function UseCallback(arg,context) { "+ reference + ";}";
//            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "UseCallback", script, true);
			
//        }

//        private void HandlePageArguments()
//        {
//            // Define the name and type of the client scripts on the page.
//            Type cstype = this.GetType();
//            string albumId = Request.Params["album"];
//            if( ! String.IsNullOrEmpty(albumId))
//            {
//                IAlbum album = MusicLibrary.GetAlbums().First<IAlbum>( a => a.Id.Equals(albumId));
//                if( log.IsDebugEnabled)
//                {
					
//                    log.Debug("Play directly album " + album.Name );
//                }
//                Session[sessionCurrentAlbum] = albumId;
				
//                if( ! Page.ClientScript.IsStartupScriptRegistered(cstype, csname))
//                {
//                    StringBuilder appendSongsScript = new StringBuilder();
//                    appendSongsScript.Append("<script type=\"text/javascript\">");					
//                    foreach( ISong s in album.Songs)
//                    {
//                        Console.WriteLine(String.Format(" enqueueSong('{0}', '{1}', '{2}', '{3}'); ", Server.HtmlEncode( s.Id), Server.HtmlEncode(s.Artist.Name), Server.HtmlEncode(s.Title),Server.HtmlEncode( s.Album.Name )));

//                        appendSongsScript.Append(String.Format(" enqueueSong('{0}', '{1}', '{2}', '{3}'); ", 
//                                                               Server.HtmlEncode( s.Id).Replace(@"'",@"\'") , 
//                                                               Server.HtmlEncode(s.Artist.Name).Replace(@"'",@"\'") , 
//                                                               Server.HtmlEncode(s.Title).Replace(@"'",@"\'")  ,
//                                                               Server.HtmlEncode( s.Album.Name ).Replace(@"'",@"\'") ));
//                    }
       
//                    appendSongsScript.Append(" setPlaylistSelectedIndex(0); ");
//                    appendSongsScript.Append("  playSelectedSong(); ");
//                    appendSongsScript.Append(@"</script>");
//                    Page.ClientScript.RegisterStartupScript (cstype, csname,appendSongsScript.ToString() , false);
//                }	
//            }			
//        }
			
//        private void SongCurrentlyPlaying(string songId)
//        {
//            if( User.IsInRole("admin"))
//            {
//                ISong song = MusicLibrary.GetSong(songId);
//                if (log.IsDebugEnabled)
//                {
//                    log.Debug("Currently playing  " + song.Title + " of " + song.Artist.Name);
//                }
//                Lastfm.LastfmService.NowPlaying(song.Artist.Name, song.Title, song.Album.Name, (int)song.Duration.TotalSeconds , (int)song.AlbumTrack.Value);
//            }
//        }
		
//        private void SongPlayed(string songId)
//        {
//            if( User.IsInRole("admin"))
//            {
//                ISong song = MusicLibrary.GetSong(songId);
//                if (log.IsDebugEnabled)
//                {
//                    log.Debug("Scrobbling " + song.Title + " of " + song.Artist.Name);
//                }
//                Lastfm.LastfmService.Scrobble(song.Artist.Name, song.Album.Name, song.Title, (int)song.AlbumTrack.GetValueOrDefault(), (int)song.Duration.TotalSeconds, DateTime.Now.Subtract(song.Duration));
//            }
//        }

//        public void RaiseCallbackEvent(string eventArgument)
//        {
//            if (!String.IsNullOrEmpty(eventArgument))
//            {
//                string[] res = eventArgument.Split(';');
//                if (res[0].Equals("idSongPlayed"))
//                {
//                    SongPlayed(res[1]);
//                }
//                else if(res[0].Equals("idSongCurrentlyPlaying"))
//                {
//                    SongCurrentlyPlaying(res[1]);
//                }
//            }
//        }

//        public string FormatDuration(TimeSpan duration)
//        {
//            return duration.Minutes.ToString("00") + ":" + duration.Seconds.ToString("00");
//        }

//        public string GetCallbackResult()
//        {
//            return null;
//        }

//        protected void ArtistList_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            arl.DataBind();
//            Session[sessionCurrentArtist] = (string)arl.SelectedValue;
//        }

//        protected void AlbumList_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            all.DataBind();
//            Session[sessionCurrentAlbum] = (string)all.SelectedValue;
//            UpdateHistory.AddEntry(Session[sessionCurrentAlbum].ToString());
//        }
		
//        protected void HistoryNavigate(object sender, nStuff.UpdateControls.HistoryEventArgs e)
//        {
//            all.DataBind();		
//            for (int i = 0; i < all.Items.Count; i++)
//            {
//                if (((string)all.DataKeys[i]).Equals( e.EntryName) )
//                {
//                    all.SelectedIndex = i;
//                    break;
//                }
//            }
//            Session[sessionCurrentAlbum] = e.EntryName;
//            UpdatePanel1.Update();
            
//        }
//    }
//}
