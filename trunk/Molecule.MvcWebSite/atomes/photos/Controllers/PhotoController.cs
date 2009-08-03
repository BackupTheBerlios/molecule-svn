using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoto.Services;
using WebPhoto.Providers;
using Molecule.MvcWebSite.atomes.photos.Data;
using Molecule.MvcWebSite.atomes.photos.Controllers;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Molecule.MvcWebSite.atomes.photos.Views;
using Molecule.MvcWebSite.Controllers;

namespace Molecule.MvcWebSite.atomes.photos.Controllers
{
    [HandleError]
    public class PhotoController : PageControllerBase
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(PhotoController));

        public ActionResult Index(string id, string tagId)
        {
            return View(new PhotoIndexData()
                {
                    Photo = PhotoLibrary.GetPhoto(id),
                    NextPhoto = PhotoLibrary.GetNextPhoto(id, tagId),
                    PreviousPhoto = PhotoLibrary.GetPreviousPhoto(id, tagId),
                    CurrentTag = !String.IsNullOrEmpty(tagId) ? PhotoLibrary.GetTag(tagId) : null,
                    PhotoTags = PhotoLibrary.GetTagsByPhoto(id),
                });
        }

        public static string IndexUrl(UrlHelper helper, IPhotoInfo photo, ITagInfo tag)
        {
            return helper.Action("Index", "Photo", new { id = photo != null ? photo.Id : null, tagId = tag != null ? tag.Id : null });
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

        public static string FileUrl(UrlHelper helper, IPhotoInfo photo, PhotoFileSize size)
        {
            return helper.Action("File", "Photo", new { id = photo != null ? photo.Id : null, size = size });
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
