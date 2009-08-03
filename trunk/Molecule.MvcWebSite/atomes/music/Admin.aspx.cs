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
//using System.Web.UI.WebControls;
//using WebMusic.Services;

//namespace WebMusic
//{
//    public partial class Preferences : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!IsPostBack)
//            {
//                lastFmUsername.Text = Lastfm.LastfmService.Username;


//                lastfmEnabledCheckBox.Checked = Lastfm.LastfmService.ScrobblingEnabled;
//                if (!Lastfm.LastfmService.ScrobblingEnabled)
//                {
//                    lastFmUserPassword.Enabled = false;
//                    lastFmUsername.Enabled = false;
//                }
//            }
//        }

//        protected void preferencesButton_Click(Object sender, CommandEventArgs e)
//        {
//            Lastfm.LastfmService.ScrobblingEnabled = lastfmEnabledCheckBox.Checked;
//            if (Lastfm.LastfmService.ScrobblingEnabled)
//            {
//                bool changed = false;
//                if (!String.IsNullOrEmpty(lastFmUserPassword.Text))
//                {
//                    changed = true;
//                    Lastfm.LastfmService.Password = lastFmUserPassword.Text;
//                }
//                if (!String.IsNullOrEmpty(lastFmUsername.Text) && !lastFmUsername.Text.Equals(Lastfm.LastfmService.Username))
//                {
//                    changed = true;
//                    Lastfm.LastfmService.Username = lastFmUsername.Text;
//                }
//                if (changed)
//                {
//                    Lastfm.LastfmService.Update();
//                }

//            }
//        }

//        protected void lastfmEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
//        {
//            if (lastfmEnabledCheckBox.Checked)
//            {
//                lastFmUserPassword.Enabled = true;
//                lastFmUsername.Enabled = true;
//            }
//            else
//            {
//                lastFmUserPassword.Enabled = false;
//                lastFmUsername.Enabled = false;
//            }
//        }
//    }
//}