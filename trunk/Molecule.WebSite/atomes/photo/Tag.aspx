<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Tag.aspx.cs"
    Inherits="Molecule.WebSite.atomes.photo.Tag" Title="Untitled Page" EnableViewState="false" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="System.Linq" %>
<%@ Register Src="TagLink.ascx" TagName="TagLink" TagPrefix="photo" %>
<%@ Register src="TagHierarchy.ascx" tagname="TagHierarchy" tagprefix="photo" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/common.css" rel="stylesheet" type="text/css" />
    <link href="style/tag.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2>
    <photo:TagHierarchy ID="tagHierarchy" runat="server" TagQueryStringField="id" />
</h2>
    <ul id="tagList">
        <asp:Repeater ID="TagsView" runat="server" DataSourceID="TagDataSource">
        <ItemTemplate>
            <li>
                <photo:TagLink runat="server" Tag="<%#(ITagInfo)Container.DataItem %>" />
            </li>
        </ItemTemplate>
        </asp:Repeater>
    </ul>

<asp:ObjectDataSource ID="TagDataSource" runat="server" 
    SelectMethod="GetTagsByTag" TypeName="WebPhoto.Services.PhotoLibrary">
    <SelectParameters>
        <asp:QueryStringParameter Name="tagId" QueryStringField="id" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<h2>Photos</h2>
    <asp:ListView ID="PhotoListView" runat="server" 
                        onpagepropertieschanging="PhotoListView_PagePropertiesChanging">
        <ItemTemplate>
            <photo:PhotoLink runat="server" TagId='<%# tagId %>' PhotoId='<%# Eval("Id") %>' />
        </ItemTemplate>
        <LayoutTemplate>
            <div id="itemPlaceholder" runat="server">
            </div>
        </LayoutTemplate>
    </asp:ListView>
    <br />
    <asp:DataPager runat="server" ID="PhotoDataPager" PagedControlID="PhotoListView"
        QueryStringField="page" PageSize="32">
        <Fields>
            <asp:NextPreviousPagerField ButtonType="Link"
                ShowNextPageButton="false" ShowPreviousPageButton="True" />
            <asp:NumericPagerField />
            <asp:NextPreviousPagerField ButtonType="Link"
                ShowPreviousPageButton="false" ShowNextPageButton="True" />
        </Fields>
    </asp:DataPager>
<h2><a href="<%= MonthCalendar.GetUrlFor(DateTime.Now, tagId) %>">Calendrier</a></h2>
</asp:Content>
