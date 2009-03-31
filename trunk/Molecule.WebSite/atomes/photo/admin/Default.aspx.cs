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

namespace Molecule.WebSite.atomes.photo.admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProviderList.DataSource = PhotoLibrary.Providers;
                ProviderList.DataBind();
                ProviderList.SelectedValue = PhotoLibrary.CurrentProvider;
                initAuthorizationPart();
            }
        }

        private void initAuthorizationPart()
        {
            AuthListView.DataSource = PhotoLibrary.TagUserAuthorizations;
            AuthListView.DataBind();
            AuthHeaderRepeater.DataSource = Membership.GetAllUsers().Cast<MembershipUser>().Select(u => u.UserName);
            AuthHeaderRepeater.DataBind();
            initTagTreeView();
        }

        private void initTagTreeView()
        {
            var tags = PhotoLibrary.AdminGetRootTags();

            TagTreeView.Nodes.Clear();

            foreach (var tag in tags)
                TagTreeView.Nodes.Add(createTreeNode(tag));
        }

        private TreeNode createTreeNode(WebPhoto.Providers.ITagInfo tag)
        {
            TreeNode node = new TreeNode(tag.Name, tag.Id);
            node.Checked = PhotoLibrary.TagUserAuthorizations.ContainsTag(tag.Id);
            foreach (var childTag in PhotoLibrary.AdminGetTagsByTag(tag.Id))
                node.ChildNodes.Add(createTreeNode(childTag));
            return node;
                
        }

        protected void OnAuthListView_CheckedChanged(object sender, EventArgs e)
        {
            var cbx = ((CheckBox)sender);
            var tagUser = cbx.ToolTip.Split(',');
            PhotoLibrary.TagUserAuthorizations.Set(tagUser[0], tagUser[1], cbx.Checked);
        }

        protected void save_onclick(object sender, EventArgs e)
        {
            PhotoLibrary.SaveTagUserAuthorizations();
        }

        protected void preferencesButton_Click(Object sender, CommandEventArgs e)
        {
            PhotoLibrary.CurrentProvider = ProviderList.SelectedValue;
            initAuthorizationPart();
        }

        protected void TagTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.Checked)
                PhotoLibrary.TagUserAuthorizations.AddTag(e.Node.Value);
            else
                PhotoLibrary.TagUserAuthorizations.RemoveTag(e.Node.Value);
            
        }

        protected void OkButton_OnClick(object sender, EventArgs args)
        {
            initAuthorizationPart();
        }
    }
}
