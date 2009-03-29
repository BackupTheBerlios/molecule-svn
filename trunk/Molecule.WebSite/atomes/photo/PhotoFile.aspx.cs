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
			if(size != PhotoFileSize.Raw)
			{
                var thumbFile = PhotoFileProvider.GetResizedPhoto(photo.MediaFilePath, size);
                var thumbInfo = new FileInfo(thumbFile);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(thumbInfo.Name));
			    Response.Clear();
                Response.ExpiresAbsolute = DateTime.Now.Add(new TimeSpan(2, 0, 0, 0, 0));
                Response.AddHeader("Content-Length", thumbInfo.Length.ToString());
                Response.ContentType = "image/jpeg";
                Response.WriteFile(thumbInfo.FullName);
				Response.Flush();
			}
			else
			{
                var rawInfo = new FileInfo(photo.MediaFilePath);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + id+".jpg");
			    Response.Clear();
                Response.ExpiresAbsolute = DateTime.Now.Add(new TimeSpan(2, 0, 0, 0, 0));
                Response.AddHeader("Content-Length", rawInfo.Length.ToString());
                Response.ContentType = "image/jpeg";
                Response.WriteFile(rawInfo.FullName);
				Response.Flush();
			}

        }

        public static string GetUrlFor(string photoId, PhotoFileSize size)
        {
            return String.Format("PhotoFile.aspx?id={0}&size={1}", HttpUtility.UrlEncode(photoId), size.ToString());
        }
    }
}
