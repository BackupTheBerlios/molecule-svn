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
	public partial class CoverArtFile : System.Web.UI.Page
	{
		private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(CoverArtFile));
		
        protected void Page_Load(object sender, EventArgs e)
        {
			
			var id = Request.QueryString["id"];
            if (log.IsDebugEnabled)
            {
                log.Debug("Requested cover art for the song with ID ");
            }
            var song = Services.MusicLibrary.GetSong(id);
            if (song == null)
                throw new ApplicationException("Unknown media with id " + id);
            string coverArtPath = CoverArtService.FetchCoverArt(song.Artist.Name, song.Album.Name);

            if (!String.IsNullOrEmpty(coverArtPath) && File.Exists(coverArtPath))
            {
                FileInfo file = new FileInfo(coverArtPath);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name));
                Response.ExpiresAbsolute = DateTime.Now.Add(new TimeSpan(365,0,0,0,0));
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(coverArtPath);
            }
            else
            {
                if (log.IsWarnEnabled)
                {
                    log.Warn("File " + coverArtPath + " doesn't exist");
                    Response.StatusCode = 404;
                    Response.End();
                }
            }
        }		
	}
}
