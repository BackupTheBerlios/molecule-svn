using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPhoto.Services;
using System.IO;
using WebPhoto.Providers;
using Mono.Rocks;

namespace Molecule.WebSite.atomes.photo
{
    

    public partial class PhotoFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];
            var size = Request.QueryString["size"].ToEnum<PhotoFileSize>();
			var photo = PhotoLibrary.GetPhoto(id);
            FileInfo fileInfo = null;
            Response.Clear();
            if (size != PhotoFileSize.Raw)
            {
                var thumbFile = PhotoFileProvider.GetResizedPhoto(photo.MediaFilePath, size);
                fileInfo = new FileInfo(thumbFile);
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileInfo.Name));
                fileInfo = new FileInfo(photo.MediaFilePath);
            }
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.ContentType = "image/jpeg";
            Response.Cache.SetExpires(DateTime.Now.Add(TimeSpan.FromDays(30)));
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetValidUntilExpires(false);
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();

        }

        public static string GetUrlFor(string photoId, PhotoFileSize size)
        {
            var imgFile = PhotoLibrary.GetPhoto(photoId).MediaFilePath;
            var thumbFile = PhotoFileSpec.GetPathForSize(imgFile, size, PhotoFileProvider.ThumbnailClip);
            var fileDate = DateTime.Now;
            if (File.Exists(thumbFile))
                fileDate = File.GetCreationTime(thumbFile);
            return String.Format("PhotoFile.aspx?id={0}&size={1}&date={2}",
                HttpUtility.UrlEncode(photoId), size.ToString(), fileDate.ToString("yyyyMMddHHmmss"));
        }
    }
}
