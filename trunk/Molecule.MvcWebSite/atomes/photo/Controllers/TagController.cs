//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Mvc.Ajax;
//using WebPhoto.Services;
//using WebPhoto.Providers;

//namespace Molecule.MvcWebSite.atomes.photo.Controllers
//{
//    public class TagController : Controller
//    {
//        //
//        // GET: /Tag/

//        public ActionResult Index(string id)
//        {
//            ITagInfo tag;
//            if (!String.IsNullOrEmpty(id))
//                tag = PhotoLibrary.GetTag(id);
//            var photos = PhotoLibrary.GetPhotosByTag(id).ToList();

//            if (photos.Any())
//            {
//                //TODO
//                //this.PhotoListView.DataSource = photos;
//                //this.PhotoListView.DataBind();
//                //this.CalendarLink.NavigateUrl = MonthCalendar.GetUrlFor(photos.First().Date, tagId);
//                //this.DownloadLink.NavigateUrl = Download.GetUrlFor(tagId, PhotoDataPager.StartRowIndex, PhotoDataPager.PageSize );				
//            }
//            //else
//            //this.photosPlaceHolder.Visible = false;

//            ViewData["subTags"] = from t in PhotoLibrary.GetTagsByTag(id) select t.Id;

//            //if (tags.Any())
//            //    this.subTagList.Tags = tags;
//            //else
//            //    this.tagsPlaceHolder.Visible = false;

//            //ViewData["subTags"] = 
//            return View();
//        }

//    }
//}
