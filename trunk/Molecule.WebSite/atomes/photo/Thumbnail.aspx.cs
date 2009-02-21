using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPhoto.Services;
using System.IO;
using WebPhoto.Providers;

namespace Molecule.WebSite.atomes.photo
{
    public partial class Thumbnail : System.Web.UI.Page
    {
        public const int thumbnailSize = 80;
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];
            var photo = PhotoLibrary.GetPhoto(id);
            var thumbFile = ThumbnailProvider.GetThumbnailForImage(photo.MediaFilePath, thumbnailSize);
            var thumbInfo = new FileInfo(thumbFile);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(thumbInfo.Name));
            Response.ExpiresAbsolute = DateTime.Now.Add(new TimeSpan(2, 0, 0, 0, 0));
            Response.AddHeader("Content-Length", thumbInfo.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(thumbInfo.FullName);
        }

        public static string GetUrlFor(IPhoto photo)
        {
            return String.Format("Thumbnail.aspx?id={0}", HttpUtility.UrlEncode(photo.Id));
        }
    }
}
