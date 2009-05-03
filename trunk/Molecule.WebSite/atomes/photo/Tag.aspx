<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Tag.aspx.cs"
    Inherits="Molecule.WebSite.atomes.photo.Tag" Title="Untitled Page" EnableViewState="false" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="System.Linq" %>
<%@ Register Src="TagLink.ascx" TagName="TagLink" TagPrefix="photo" %>
<%@ Register src="TagHierarchy.ascx" tagname="TagHierarchy" tagprefix="photo" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>
<%@ Register Src="TagList.ascx" TagName="TagList" TagPrefix="photo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/common.css" rel="stylesheet" type="text/css" />
    <link href="style/tag.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <photo:TagHierarchy ID="tagHierarchy" runat="server" TagQueryStringField="id" />
    <asp:PlaceHolder runat="server" ID="tagsPlaceHolder">
    <h2><%= WebPhoto.Services.PhotoLibrary.GetLocalizedTagName() %>s</h2>
    <photo:TagList runat="server" ID="subTagList" />
    </asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="photosPlaceHolder">
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
        QueryStringField="page" PageSize="21">
        <Fields>
            <asp:NextPreviousPagerField ButtonType="Link" PreviousPageText="<"
                ShowNextPageButton="false" ShowPreviousPageButton="True" />
            <asp:NumericPagerField />
            <asp:NextPreviousPagerField ButtonType="Link" NextPageText=">"
                ShowPreviousPageButton="false" ShowNextPageButton="True"  />
        </Fields>
    </asp:DataPager>
    <br />
<asp:HyperLink runat="server" id="CalendarLink">
<div class="ActionImage">
    <img alt="calendar" src="images/office-calendar.png" />
</div>
<asp:Literal runat="server" Text='<%$Resources:photo,Calendar %>' />
</asp:HyperLink>

<asp:HyperLink runat="server" id="DownloadLink">
<div class="ActionImage">
    <img alt="download" src="../../App_Themes/<%= Theme %>/images/document-save.png" />
</div>
<asp:Literal runat="server" Text='<%$Resources:photo,DownloadPage %>' />
</asp:HyperLink>


</asp:PlaceHolder>
</asp:Content>
