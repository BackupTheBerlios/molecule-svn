//
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
using System.Collections.Generic;
using Mono.Rocks;

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
               
            if (!IsPostBack)
            {
                this.titleTextBox.Text = AdminService.MoleculeTitle;
                initAuthList();
            }
        }

        private void initAuthList()
        {
            AuthListView.DataSource = AdminService.AtomeUserAuthorizations;
            AuthListView.DataBind();
            AuthHeaderRepeater.DataSource = Membership.GetAllUsers().Cast<MembershipUser>().Select(u => u.UserName);
            AuthHeaderRepeater.DataBind();
        }

        protected void createUserWizard_CreatedUser(object sender, EventArgs e)
        {
            UserListView.DataBind();
            AdminService.UpdateAtomeUserAuthorizations();
            initAuthList();
        }

        protected void ButtonReset_Click(object sender, EventArgs e)
        {
            Services.AdminService.ResetCssVariables();
            CssVariableList.DataBind();
        }

        protected void OnAuthListView_CheckedChanged(object sender, EventArgs e)
        {
            var cbx = ((CheckBox)sender);
            var atomeUser = cbx.ToolTip.Split(',');
            AdminService.AtomeUserAuthorizations.Set(atomeUser[0], atomeUser[1], cbx.Checked);
        }
    
        protected void Variable_TextChanged(object sender, EventArgs e)
        {
            CssVariableList.UpdateItem(((sender as TextBox).Parent as ListViewDataItem).DataItemIndex,false);
        }

        protected void save_onclick(object sender, EventArgs e)
        {
            AdminService.MoleculeTitle = this.titleTextBox.Text;
            AdminService.SaveAtomeUserAuthorizations();
        }
    }
}
