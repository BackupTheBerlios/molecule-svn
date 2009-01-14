//
// Download.aspx.cs
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
using System.Xml.Linq;
using log4net;
using WebMusic.Providers;
using WebMusic.Services;
using System.IO;
using WebMusic.IO;

namespace WebMusic
{
    public partial class Download : System.Web.UI.Page
    {
        ILog log = LogManager.GetLogger(typeof(Download));

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            HandleParameters();
            Response.End();
        }

        private void HandleParameters()
        {
            string songId = Request.Params["songId"];

            if (!string.IsNullOrEmpty(songId))
            {
                WriteSongFile(songId);
                return;
            }

            string songList = Request.Params["songList"];
            if (!string.IsNullOrEmpty(songList))
            {
                WriteZippedSongFiles(songList);
            }
        }

        private void WriteZippedSongFiles(string songList)
        {
            string[] songIds = songList.Split(new char[]{'{', ',', '}'}, StringSplitOptions.RemoveEmptyEntries);
            //generate an unique file name
            string fileName = Path.GetTempFileName();
            CompressionHelper.CompressFiles(songIds.Select(id => Services.MusicLibrary.GetSong(id)), fileName);

            System.IO.FileInfo file = new System.IO.FileInfo(fileName);
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "MusicSelection.zip");
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.End();
            File.Delete(fileName);
        }

        private void WriteSongFile(string songId)
        {
            Context.Response.Redirect(MediaFileHandler.GetVirtualPath(songId), true);
        }
    }
}