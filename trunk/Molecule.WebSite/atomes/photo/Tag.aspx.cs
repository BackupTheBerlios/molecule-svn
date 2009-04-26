using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using WebPhoto.Services;
using WebPhoto.Providers;
using System.Collections.Generic;

namespace Molecule.WebSite.atomes.photo
{
    public partial class Tag : System.Web.UI.Page
    {
        protected string tagId;
        protected ITagInfo tag;
        List<IPhotoInfo> photos;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            tagId = Request.QueryString["id"];
            if(!String.IsNullOrEmpty(tagId))
                tag = PhotoLibrary.GetTag(tagId);
            photos = PhotoLibrary.GetPhotosByTag(tagId).ToList();
            initContent();
            initTitle();
        }

        private void initContent()
        {
            this.PhotoListView.DataSource = photos;
            this.PhotoListView.DataBind();
            this.subTagList.Tags = PhotoLibrary.GetTagsByTag(tagId);
            if (this.photos.Any())
                this.CalendarLink.NavigateUrl = MonthCalendar.GetUrlFor(photos.First().Date, tagId);
            else
                this.CalendarLink.Visible = false;
                
        }

        private void initTitle()
        {
            Title = "Photos";
        }

        public static string GetUrlFor(string tagId)
        {
            return String.Format("Tag.aspx?id={0}", tagId);
        }

        protected void PhotoListView_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.PhotoDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows >= 1 ? e.MaximumRows:32, false);
            this.PhotoListView.DataSource = photos;
            this.PhotoListView.DataBind();
        }
    }
}
