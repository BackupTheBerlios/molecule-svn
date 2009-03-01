<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Tag.aspx.cs"
    Inherits="Molecule.WebSite.atomes.photo.Tag" Title="Untitled Page" EnableViewState="false" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2>Tags</h2>
<asp:ListView runat="server" DataSourceID="TagDataSource">
    <ItemTemplate>
        <a href='<%# Tag.GetUrlFor(((ITag)Container.DataItem).Id) %>'>
                <asp:Image ID="Image1" runat="server"
                    ImageUrl='<%# PhotoFile.GetUrlFor(
                        PhotoLibrary.GetPhotosByTag(((ITag)Container.DataItem).Id).First().Id
                        ,PhotoFileSize.Thumbnail) %>' />
            </a>
    </ItemTemplate>
    <LayoutTemplate>
    <div id="itemPlaceholder" runat="server">
            </div>
    </LayoutTemplate>
</asp:ListView>
<asp:ObjectDataSource ID="TagDataSource" runat="server" 
    SelectMethod="GetTagsByTag" TypeName="WebPhoto.Services.PhotoLibrary">
    <SelectParameters>
        <asp:QueryStringParameter Name="tagId" QueryStringField="id" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<h2>Photos</h2>
    <asp:ListView ID="PhotoListView" runat="server">
        <ItemTemplate>
            <a href='<%# Photo.GetUrlFor(((IPhoto)Container.DataItem).Id, tagId) %>'>
                <asp:Image runat="server"
                    ImageUrl='<%# PhotoFile.GetUrlFor(((IPhoto)Container.DataItem).Id, PhotoFileSize.Thumbnail) %>' />
            </a>
        </ItemTemplate>
        <LayoutTemplate>
            <div id="itemPlaceholder" runat="server">
            </div>
            <div>
                <asp:DataPager ID="DataPager1" runat="server" QueryStringField="page" PageSize="32">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Link"
                            ShowNextPageButton="false" ShowPreviousPageButton="True" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ButtonType="Link"
                            ShowPreviousPageButton="false" ShowNextPageButton="True" />
                    </Fields>
                </asp:DataPager>
            </div>
        </LayoutTemplate>
    </asp:ListView>
<h2><a href="<%= Calendar1.GetUrlFor(DateTime.Now, tagId) %>">Calendrier</a></h2>
</asp:Content>
