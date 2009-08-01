using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using WebPhoto.Providers;
using WebPhoto.Services;
using Molecule.MvcWebSite.atomes.photos.Data;
using Molecule.MvcWebSite.atomes.photos.Views;

namespace Molecule.MvcWebSite.atomes.photos.Controllers
{
    public class TagController : Controller
    {
        public ActionResult Index(string id)
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
            return View(new TagIndexData()
            {
                SubTags = PhotoLibrary.GetTagsByTag(id),
                Photos = PhotoLibrary.GetPhotosByTag(id).ToList(),
                Tag = tag
            });
        }

        public static string IndexUrl(UrlHelper helper, ITagInfo tag)
        {
            return helper.Action("Index", "Tag", new { id = tag != null ? tag.Id : null });
        }

        public ActionResult Zip(string id)
        {
            return new ZipPhotoFilesResult()
            {
                Tag = PhotoLibrary.GetTag(id),
                Photos = PhotoLibrary.GetPhotosByTag(id)
            };
        }

        public static string ZipUrl(UrlHelper helper, ITagInfo tag)
        {
            return helper.Action("Zip", "Tag", new { id = tag.Id });
        }
    }
}
