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
            var tagId = Request.QueryString["tag"];

            init(photoId, tagId);
        }

        public IPhotoInfo CurrentPhoto { get; set; }
        public IPhotoInfo PreviousPhoto { get; set; }
        public IPhotoInfo NextPhoto { get; set; }

        private void init(string photoId, string tagId)
        {
            ImageCurrent.ImageUrl = PhotoFile.GetUrlFor(photoId, PhotoFileSize.Normal);
            CurrentPhoto = PhotoLibrary.GetPhoto(photoId);
            NextPhoto = PhotoLibrary.GetNextPhoto(photoId, tagId);
            PreviousPhoto = PhotoLibrary.GetPreviousPhoto(photoId, tagId);
            //if (NextPhoto != null)
            //{
            //    ImageNext.Src = PhotoFile.GetUrlFor(NextPhoto.Id, PhotoFileSize.Thumbnail);
            //    ImageNextLink.NavigateUrl = GetUrlFor(NextPhoto.Id);
            //}
            //else
            //    ImageNext.Visible = false;

            
            //if (PreviousPhoto != null)
            //{
            //    ImagePrevious.Src = PhotoFile.GetUrlFor(PreviousPhoto.Id, PhotoFileSize.Thumbnail);
            //    ImagePreviousLink.NavigateUrl = GetUrlFor(PreviousPhoto.Id);
            //}
            //else
            //    ImagePrevious.Visible = false;
        }

        public static string GetUrlFor(string photoId)
        {
            return String.Format("Photo.aspx?id={0}", photoId);
        }
    }
}
