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
            //var photoId = Request.QueryString["id"];
            tagId = Request.QueryString["tag"];

            //initContent(photoId);
            initTitle();
        }

        

        public IPhotoInfo CurrentPhoto { get; set; }

        public IPhotoInfo NextPhoto { get; set; }

        private void initContent(string photoId)
        {
            if(!String.IsNullOrEmpty(tagId))
                tag = PhotoLibrary.GetTag(tagId);
            CurrentPhoto = PhotoLibrary.GetPhoto(photoId);
            var nextPhoto = PhotoLibrary.GetNextPhoto(photoId, tagId);

            NextPhotoLink.Visible = nextPhoto != null;

            var previousPhoto = PhotoLibrary.GetPreviousPhoto(photoId, tagId);
            PreviousPhotoLink.Visible = previousPhoto != null;

            FullSizePhoto.PhotoId = photoId;
            FullSizePhoto.Metadatas = CurrentPhoto.Metadatas;
            LabelDescription.Text = CurrentPhoto.Description;
            ViewState["currentPhotoId"] = photoId;
			
            tagList.Tags = PhotoLibrary.GetTagsByPhoto(CurrentPhoto.Id);
            
			if( CurrentPhoto.Latitude.HasValue && CurrentPhoto.Longitude.HasValue )
			{
				this.PhotoMap.Visible = true;
				this.PhotoMap.Latitude =  CurrentPhoto.Latitude.Value;
				this.PhotoMap.Longitude =  CurrentPhoto.Longitude.Value;
                this.PhotoMap.ThumbnailUrl = PhotoFile.GetUrlFor(photoId, PhotoFileSize.Thumbnail);
			}
			else
			{
				this.PhotoMap.Visible = false;				
			}
            updateHistory.AddEntry(CurrentPhoto.Id);
        }

        protected void OnUpdateHistoryNavigate(object sender, nStuff.UpdateControls.HistoryEventArgs e)
        {
            // Raised when the user navigates back/forward or
            // loads a bookmark to a specific view.
            if (!String.IsNullOrEmpty(e.EntryName))
            {
                ViewState["currentPhotoId"] = e.EntryName;
                initContent(e.EntryName);
            }
            mainUP.Update();
        }

        protected void OnPreviousClick(object sender, EventArgs args)
        {
            string photoId = (string)ViewState["currentPhotoId"];
            var previousPhoto = PhotoLibrary.GetPreviousPhoto(photoId, tagId);
            initContent(previousPhoto.Id);
            //updateHistory.AddEntry(previousPhoto.Id);
        }

        protected void OnNextClick(object sender, EventArgs args)
        {
            string photoId = (string)ViewState["currentPhotoId"];
            var nextPhoto = PhotoLibrary.GetNextPhoto(photoId, tagId);
            initContent(nextPhoto.Id);
            //updateHistory.AddEntry(nextPhoto.Id);
        }

        private void initTitle()
        {
            Title = "Photos"+PhotoLibrary.GetTagFullPath(tagId);
        }

        public static string GetUrlFor(string photoId)
        {
            return String.Format("Photo.aspx#{0}", photoId);
        }

        public static string GetUrlFor(string photoId, string tagId)
        {
            if(String.IsNullOrEmpty(tagId))
                return GetUrlFor(photoId);
            return String.Format("Photo.aspx?tag={0}#{1}", tagId, photoId);
        }
    }
}
