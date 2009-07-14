using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using WebPhoto.Providers;
using WebPhoto.Services;
using Molecule.MvcWebSite.atomes.photos.Data;

namespace Molecule.MvcWebSite.atomes.photos.Controllers
{
    public class TagController : Controller
    {
        public ActionResult Explore(string id)
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
            return View(new TagExploreData()
            {
                SubTags = PhotoLibrary.GetTagsByTag(id),
                Photos = PhotoLibrary.GetPhotosByTag(id).ToList(),
                Tag = tag
            });
        }

    }
}
