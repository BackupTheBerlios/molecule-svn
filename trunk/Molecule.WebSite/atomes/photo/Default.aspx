<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.Default" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:TreeView ID="TreeView1" runat="server" DataSourceID="tagSiteMapDataSource">
    </asp:TreeView>
    <asp:SiteMapDataSource ID="tagSiteMapDataSource" runat="server" 
        SiteMapProvider="tagSiteMapProvider" />
    <asp:ListView ID="ListView1" runat="server" DataSourceID="PhotoDataSource">
            <ItemTemplate>
                <span style="">
                    <asp:Image ID="Image1" runat="server"
                        ImageUrl='<%# PhotoFile.GetUrlFor(((IPhoto)Container.DataItem).Id, WebPhoto.Services.PhotoFileSize.Thumbnail) %>' />
                </span>
            </ItemTemplate>
            
            <EmptyDataTemplate>
                <span>No data was returned.</span>
            </EmptyDataTemplate>
            
            <LayoutTemplate>
                <div ID="itemPlaceholderContainer" runat="server" style="">
                    <span ID="itemPlaceholder" runat="server" />
                </div>
                <div style="">
                    <asp:DataPager ID="DataPager1" runat="server">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" 
                                ShowNextPageButton="False" ShowPreviousPageButton="False" />
                            <asp:NumericPagerField />
                            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" 
                                ShowNextPageButton="False" ShowPreviousPageButton="False" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </LayoutTemplate>
        </asp:ListView>
    <asp:ObjectDataSource ID="PhotoDataSource" runat="server" 
        SelectMethod="GetPhotos" TypeName="WebPhoto.Services.PhotoLibrary">
    </asp:ObjectDataSource>
</asp:Content>
