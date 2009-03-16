<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Photo.aspx.cs"
 EnableViewState="False" EnableEventValidation="false"
    Inherits="Molecule.WebSite.atomes.photo.Photo" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<%@ Register Src="TagLink.ascx" TagName="TagLink" TagPrefix="photo" %>
<%@ Register src="TagHierarchy.ascx" tagname="TagHierarchy" tagprefix="photo" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>
<%@ Register Src="TagList.ascx" TagName="TagList" TagPrefix="photo" %>
<%@ Register Src="Map.ascx" tagname="Map" TagPrefix="photo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/common.css" rel="stylesheet" type="text/css" />
    <link href="style/photo.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server">
        <Scripts>
            <asp:ScriptReference Path="scripts/default.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    
    <photo:TagHierarchy ID="TagHierarchy" runat="server" TagQueryStringField="tag" />
    
    <photo:PhotoLink ID="NextPhotoLink" runat="server" Description="Photo suivante" />
    <photo:PhotoLink ID="PreviousPhotoLink" runat="server" Description="Photo précédente" />

    <div id="photoCurrent">
        <img alt="<%=CurrentPhoto.Id %>"  
             src="<%=PhotoFile.GetUrlFor(CurrentPhoto.Id, PhotoFileSize.Normal) %>" />
        <div id="photoDescriptionContainer">
            <p>
                <asp:Label runat="server"><%= CurrentPhoto.Description %></asp:Label>
                              
            </p>
            <h2>Tags</h2>
            <photo:TagList runat="server" ID="tagList" />
            <h2>Metadatas</h2>
            <asp:GridView ID="MetadatasGridView" runat="server" AutoGenerateColumns="true" ShowHeader="false">
            </asp:GridView>
            <h2><asp:Label ID="LocationText" runat="server">Location</asp:Label></h2>
            <photo:Map ID="PhotoMap" runat="server" Latitude="47.809944" Longitude="-1.9465"></photo:Map>
        </div>
    </div>
</asp:Content>
