﻿using System;
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

namespace Molecule.WebSite.atomes.photo
{
    public partial class Tag : System.Web.UI.Page
    {
        protected string tagId;
        ICollection photos;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            tagId = Request.QueryString["id"];
            photos = PhotoLibrary.GetPhotosByTag(tagId).ToList();
            initContent();
            initTitle();
        }

        private void initContent()
        {
            this.PhotoListView.DataSource = photos;
            this.PhotoListView.DataBind();
        }

        private void initTitle()
        {
            Title = "Photos" + PhotoLibrary.GetTagFullPath(tagId);
        }

        public static string GetUrlFor(string tagId)
        {
            return String.Format("Tag.aspx?id={0}", tagId);
        }

        protected void PhotoListView_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.PhotoDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            this.PhotoListView.DataSource = photos;
            this.PhotoListView.DataBind();
        }
    }
}
