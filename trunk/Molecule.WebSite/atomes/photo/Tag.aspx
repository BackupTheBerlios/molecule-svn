<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Tag.aspx.cs"
    Inherits="Molecule.WebSite.atomes.photo.Tag" Title="Untitled Page" EnableViewState="false" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<h2>Tags</h2>

<h2>Photos</h2>
    <asp:ListView ID="PhotoListView" runat="server">
        <ItemTemplate>
            <a href='<%# Photo.GetUrlFor(((IPhoto)Container.DataItem).Id, tagId) %>'>
                <asp:Image runat="server" ImageUrl='<%# PhotoFile.GetUrlFor(((IPhoto)Container.DataItem).Id, WebPhoto.Services.PhotoFileSize.Thumbnail) %>' />
            </a>
        </ItemTemplate>
        <EmptyDataTemplate>
            <span>No data was returned.</span>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <div id="itemPlaceholderContainer" runat="server">
                <span id="itemPlaceholder" runat="server" />
            </div>
            <div>
                <asp:DataPager ID="DataPager1" runat="server" QueryStringField="page" PageSize="32">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Link" ShowNextPageButton="false" ShowPreviousPageButton="True" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ButtonType="Link" ShowPreviousPageButton="false" ShowNextPageButton="True" />
                    </Fields>
                </asp:DataPager>
            </div>
        </LayoutTemplate>
    </asp:ListView>
<h2><a href="<%= Calendar1.GetUrlFor(DateTime.Now, tagId) %>">Calendrier</a></h2>
</asp:Content>
