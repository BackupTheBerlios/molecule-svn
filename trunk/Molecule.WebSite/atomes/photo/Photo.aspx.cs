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
            var photo = PhotoLibrary.GetPhoto(photoId);
            ImageCurrent.ImageUrl = Thumbnail.GetUrlFor(photo);

            var nextPhoto = PhotoLibrary.GetNextPhoto(photoId);
            if (nextPhoto != null)
                ImageNext.ImageUrl = Thumbnail.GetUrlFor(nextPhoto);
            else
                ImageNext.Visible = false;

            var previousPhoto = PhotoLibrary.GetPreviousPhoto(photoId);
            if (previousPhoto != null)
                ImagePrevious.ImageUrl = Thumbnail.GetUrlFor(previousPhoto);
            else
                ImagePrevious.Visible = false;
        }
    }
}
