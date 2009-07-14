using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoto.Services;
using WebPhoto.Providers;
using Molecule.MvcWebSite.atomes.photos.Data;
using Molecule.MvcWebSite.atomes.photos.Controllers;

namespace Molecule.MvcWebSite.photo.Controllers
{
    [HandleError]
    public class PhotosController : Controller
    {
        public ActionResult Display(string id, string tagId)
        {
            ITagInfo tag;
            if (!String.IsNullOrEmpty(tagId))
                tag = PhotoLibrary.GetTag(tagId);
            var currentPhoto = PhotoLibrary.GetPhoto(id);
            var nextPhoto = PhotoLibrary.GetNextPhoto(id, tagId);
            var previousPhoto = PhotoLibrary.GetPreviousPhoto(id, tagId);
            var tags = PhotoLibrary.GetTagsByPhoto(id);
            ViewData["CurrentPhoto"] = currentPhoto;
 
            return View();
        }

        public ActionResult Tag(string id)
        {
            ITagInfo tag = null;
            if (!String.IsNullOrEmpty(id))
                tag = PhotoLibrary.GetTag(id);

            //if (photos.Any())
            //{
                //TODO
                //this.PhotoListView.DataSource = photos;
                //this.PhotoListView.DataBind();
                //this.CalendarLink.NavigateUrl = MonthCalendar.GetUrlFor(photos.First().Date, tagId);
                //this.DownloadLink.NavigateUrl = Download.GetUrlFor(tagId, PhotoDataPager.StartRowIndex, PhotoDataPager.PageSize );				
            //}
            //else
            //this.photosPlaceHolder.Visible = false;
            return View(new TagData()
            {
                SubTags = PhotoLibrary.GetTagsByTag(id),
                Photos = PhotoLibrary.GetPhotosByTag(id).ToList(),
                Tag = tag
            });
        }

        public ActionResult File(string id, PhotoFileSize size)
        {
            //TODO : data cache
            return new PhotoFileResult()
            {
                Photo = PhotoLibrary.GetPhoto(id),
                Size = size
            };
        }
    }

    //public partial class Photo : System.Web.UI.Page
    //{
    //    protected string tagId;
    //    protected ITagInfo tag;

    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        var photoId = Request.QueryString["id"];
    //        tagId = Request.QueryString["tag"];

    //        initContent(photoId);
    //        initTitle();
    //    }

    //    public IPhotoInfo CurrentPhoto { get; set; }

    //    public IPhotoInfo NextPhoto { get; set; }

    //    private void initContent(string photoId)
    //    {
    //        if (!String.IsNullOrEmpty(tagId))
    //            tag = PhotoLibrary.GetTag(tagId);
    //        CurrentPhoto = PhotoLibrary.GetPhoto(photoId);
    //        var nextPhoto = PhotoLibrary.GetNextPhoto(photoId, tagId);
    //        if (nextPhoto != null)
    //        {
    //            NextPhotoLink.PhotoId = nextPhoto.Id;
    //            NextPhotoLink.TagId = tagId;
    //            NextPhotoLink.HoverIconUrl = "/App_Themes/" + Theme + "/images/go-next.png";
    //            NextPhoto = nextPhoto;
    //            FullSizePhoto.Loaded = String.Format(@"preload('{0}')", PhotoFile.GetUrlFor(nextPhoto.Id, PhotoFileSize.Normal));
    //        }
    //        else
    //            NextPhotoLink.Visible = false;
    //        var previousPhoto = PhotoLibrary.GetPreviousPhoto(photoId, tagId);
    //        if (previousPhoto != null)
    //        {
    //            PreviousPhotoLink.TagId = tagId;
    //            PreviousPhotoLink.PhotoId = previousPhoto.Id;
    //            PreviousPhotoLink.HoverIconUrl = "/App_Themes/" + Theme + "/images/go-previous.png";
    //        }
    //        else
    //            PreviousPhotoLink.Visible = false;

    //        FullSizePhoto.PhotoId = photoId;
    //        FullSizePhoto.Metadatas = CurrentPhoto.Metadatas;

    //        this.CalendarLink.NavigateUrl = MonthCalendar.GetUrlFor(CurrentPhoto.Date, tagId);

    //        tagList.Tags = PhotoLibrary.GetTagsByPhoto(CurrentPhoto.Id);

    //        if (CurrentPhoto.Latitude.HasValue && CurrentPhoto.Longitude.HasValue)
    //        {
    //            this.PhotoMap.Visible = true;
    //            this.PhotoMap.Latitude = CurrentPhoto.Latitude.Value;
    //            this.PhotoMap.Longitude = CurrentPhoto.Longitude.Value;
    //            this.PhotoMap.ThumbnailUrl = PhotoFile.GetUrlFor(photoId, PhotoFileSize.Thumbnail);
    //        }
    //        else
    //        {
    //            this.PhotoMap.Visible = false;
    //        }
    //    }

    //    private void initTitle()
    //    {
    //        Title = "Photos";
    //    }

    //    public static string GetUrlFor(string photoId)
    //    {
    //        return String.Format("Photo.aspx?id={0}", photoId);
    //    }

    //    public static string GetUrlFor(string photoId, string tagId)
    //    {
    //        if (String.IsNullOrEmpty(tagId))
    //            return GetUrlFor(photoId);
    //        return String.Format("Photo.aspx?id={0}&tag={1}", photoId, tagId);
    //    }
    //}
}
