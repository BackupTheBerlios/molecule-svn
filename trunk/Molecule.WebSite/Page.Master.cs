﻿//
// Page.Master.cs
//
// Copyright (c) 2009 Pascal Fresnay (dev.molecule@free.fr) - Mickael Renault (dev.molecule@free.fr) 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Molecule.WebSite.Services;

namespace Molecule.WebSite
{
    public partial class Page : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Services.AdminService.IsSetupAuthorized)
                Context.Response.Redirect("~/admin/Setup.aspx");

            
            if (String.IsNullOrEmpty(Page.Title) || Page.Title.StartsWith("Untitle"))
                if (AtomeService.CurrentPathIsAtome)
                    Page.Title = AtomeService.CurrentAtome.Name;
                else Page.Title = "Molecule";

            logoPlaceHolder.Visible = AdminService.DisplayLogo;
        }

        protected void logsView_OnLoad(object sender, EventArgs args)
        {
            var logsView = sender as ListView;
            
            logsView.DataSource = Molecule.Log.LogService.Instance.Events;
            logsView.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            moleculeCssLink.Attributes["href"] = "/App_Themes/" + Page.Theme + "/molecule.cssx?date=" + AdminService.LastCssVariablesUpdate.ToString("yyyyMMddHHmmss");

        }

        protected void updateLogPanel_DataBound(object sender, EventArgs e)
        {
            //fix : no more reference
            //logsViewDataPager.Visible = (logsViewDataPager.PageSize < logsViewDataPager.TotalRowCount);
        }
    }
}
