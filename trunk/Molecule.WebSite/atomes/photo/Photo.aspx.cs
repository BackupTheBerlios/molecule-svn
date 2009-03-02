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
        protected string tagId;
        protected ITagInfo tag;

        protected void Page_Load(object sender, EventArgs e)
        {
            var photoId = Request.QueryString["id"];
            tagId = Request.QueryString["tag"];

            initContent(photoId);
            initTitle();
        }

        

        public IPhotoInfo CurrentPhoto { get; set; }
        public IPhotoInfo PreviousPhoto { get; set; }
        public IPhotoInfo NextPhoto { get; set; }

        private void initContent(string photoId)
        {
            if(!String.IsNullOrEmpty(tagId))
                tag = PhotoLibrary.GetTag(tagId);
            ImageCurrent.ImageUrl = PhotoFile.GetUrlFor(photoId, PhotoFileSize.Normal);
            CurrentPhoto = PhotoLibrary.GetPhoto(photoId);
            NextPhoto = PhotoLibrary.GetNextPhoto(photoId, tagId);
            PreviousPhoto = PhotoLibrary.GetPreviousPhoto(photoId, tagId);
            MetadatasGridView.DataSource = CurrentPhoto.Metadatas;
            MetadatasGridView.DataBind();
            TagsView.DataSource = PhotoLibrary.GetTagsByPhoto(CurrentPhoto.Id);
            TagsView.DataBind();
            
        }

        private void initTitle()
        {
            Title = "Photos"+PhotoLibrary.GetTagFullPath(tagId);
        }

        public static string GetUrlFor(string photoId)
        {
            return String.Format("Photo.aspx?id={0}", photoId);
        }

        public static string GetUrlFor(string photoId, string tagId)
        {
            if(String.IsNullOrEmpty(tagId))
                return GetUrlFor(photoId);
            return String.Format("Photo.aspx?id={0}&tag={1}", photoId, tagId);
        }
    }
}
