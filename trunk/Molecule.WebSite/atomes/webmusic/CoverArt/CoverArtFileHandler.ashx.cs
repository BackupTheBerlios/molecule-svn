using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using WebMusic.Providers;
using System.IO;

namespace WebMusic.CoverArt
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CoverArtFileHandler : IHttpHandler
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(CoverArtFileHandler));

        public void ProcessRequest(HttpContext context)
        {
            string id = Path.GetFileNameWithoutExtension(context.Request.FilePath).Trim(' ', '\n');
            if (log.IsDebugEnabled)
            {
                log.Debug("Requested cover art for the song with ID ");
            }
            ISong song = Services.MusicLibrary.GetSong(id);
            if (song == null)
                throw new ApplicationException("Unknown media with id " + id);
            string coverArtPath = CoverArtService.Instance.FetchCoverArt(song.Artist.Name, song.Album.Name);

            if (!String.IsNullOrEmpty(coverArtPath) && File.Exists(coverArtPath))
            {
                FileInfo file = new FileInfo(coverArtPath);
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + context.Server.UrlEncode(file.Name));
                context.Response.ExpiresAbsolute = DateTime.Now.Add(new TimeSpan(365,0,0,0,0));
                context.Response.AddHeader("Content-Length", file.Length.ToString());
                context.Response.ContentType = "application/octet-stream";
                context.Response.WriteFile(coverArtPath);

                /*
                HTTP/1.1 301 Moved Permanently
      Location: http://example.com/newuri
      Content-Type: text/html
                */
            }
            else
            {
                if (log.IsWarnEnabled)
                {
                    log.Warn("File " + coverArtPath + " doesn't exist");
                    context.Response.StatusCode = 404;
                    context.Response.End();
                }
            }
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
