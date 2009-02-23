using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPhoto.Services;
using WebPhoto.Providers;

namespace Molecule.WebSite.atomes.photo
{
    public partial class Photo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var photoId = Request.QueryString["id"];

            ImageCurrent.ImageUrl = PhotoFile.GetUrlFor(photoId, PhotoFileSize.Normal);

            var nextPhoto = PhotoLibrary.GetNextPhoto(photoId);
            if (nextPhoto != null)
                ImageNext.ImageUrl = PhotoFile.GetUrlFor(nextPhoto.Id, PhotoFileSize.Thumbnail);
            else
                ImageNext.Visible = false;

            var previousPhoto = PhotoLibrary.GetPreviousPhoto(photoId);
            if (previousPhoto != null)
                ImagePrevious.ImageUrl = PhotoFile.GetUrlFor(previousPhoto.Id, PhotoFileSize.Thumbnail);
            else
                ImagePrevious.Visible = false;
        }

        public static string GetUrlFor(string photoId)
        {
            return String.Format("Photo.aspx?id={0}", photoId);
        }
    }
}
