using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPhoto.Services;
using System.IO;

namespace Molecule.WebSite.atomes.photo
{
    public partial class Thumbnail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];
            var photo = PhotoLibrary.GetPhoto(id);
            var thumbFile = ThumbnailProvider.GetThumbnailForImage(photo.MediaFilePath, 120);
            var thumbInfo = new FileInfo(thumbFile);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(thumbInfo.Name));
            Response.ExpiresAbsolute = DateTime.Now.Add(new TimeSpan(2, 0, 0, 0, 0));
            Response.AddHeader("Content-Length", thumbInfo.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(thumbInfo.FullName);
        }
    }
}
