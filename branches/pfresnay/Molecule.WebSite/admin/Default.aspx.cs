﻿//
// Default.aspx.cs
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
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Molecule.WebSite.Services;
using AjaxControlToolkit;

namespace Molecule.WebSite.Admin
{
    public partial class Default : System.Web.UI.Page
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void createUserButton_Click(object sender, EventArgs e)
        {
            createUserWizard.Visible = true;
        }

        protected void createUserWizard_CreatedUser(object sender, EventArgs e)
        {
            usersGridView.DataBind();
        }

        protected void atomesAdminPlaceHolder_Init(object sender, EventArgs e)
        {
            
            foreach (var atome in AtomeService.GetAtomesWithAdminWebControl())
            {
                TabPanel tabPanel = new TabPanel();
                var ctrl = (AdminWebControlContainer)LoadControl("AdminWebControlContainer.ascx");
                ctrl.AtomeInfo = atome;
                tabPanel.Controls.Add(ctrl);
                tabPanel.HeaderText = atome.Name;
                this.TabContainer.Tabs.Add(tabPanel);
            }
        }
    }
}
