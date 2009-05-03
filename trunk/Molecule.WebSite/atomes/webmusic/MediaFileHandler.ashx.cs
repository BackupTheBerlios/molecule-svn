//
// MediaFileHandler.ashx.cs
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
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.IO;
using WebMusic;
using WebMusic.Services;
using WebMusic.Providers;

namespace WebMusic
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MediaFileHandler : IHttpHandler
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(MediaFileHandler));

        public void ProcessRequest(HttpContext context)
        {
            string id = Path.GetFileNameWithoutExtension(context.Request.FilePath).Trim(' ','\n');
            if (log.IsDebugEnabled)
            {
                log.Debug("Requested song media file with ID " + id + " and the file path " + context.Request.FilePath);
            }

            ISong song = Services.MusicLibrary.GetSong(id);
            if (song == null)
                throw new ApplicationException("Unknown media with id " + id);

            string mediaFilePath = new Uri(song.MediaFilePath).LocalPath;

            if (!Path.IsPathRooted(mediaFilePath))
                mediaFilePath = context.Server.MapPath(song.MediaFilePath);
            if (File.Exists(mediaFilePath))
            {
                FileInfo file = new FileInfo(mediaFilePath);
                context.Response.ContentType = "application/octet-stream";
				context.Response.WriteFile(mediaFilePath, true);
            }
            else
            {
                if (log.IsWarnEnabled)
                {
                    log.Warn("File " + mediaFilePath + " doesn't exist");
                    context.Response.StatusCode = 404;
                    context.Response.End();
                }
            }
        }

        public static string GetVirtualPath(ISong song)
        {
            return GetVirtualPath(song.Id);
        }

        public static string GetVirtualPath(string songId)
        {
            return songId + ".media";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
