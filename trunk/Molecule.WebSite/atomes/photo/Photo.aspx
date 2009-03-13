<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Photo.aspx.cs"
 EnableViewState="False" EnableEventValidation="false"
    Inherits="Molecule.WebSite.atomes.photo.Photo" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<%@ Register Src="TagLink.ascx" TagName="TagLink" TagPrefix="photo" %>
<%@ Register src="TagHierarchy.ascx" tagname="TagHierarchy" tagprefix="photo" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>
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
            <ul class="TagList">
            <asp:Repeater ID="TagsView" runat="server">
            <ItemTemplate>
                <li>
                     <photo:TagLink runat="server" Tag="<%#(ITagInfo)Container.DataItem %>" />
                </li>
            </ItemTemplate>
            </asp:Repeater>
            </ul>
            <h2>Metadatas</h2>
            <asp:GridView ID="MetadatasGridView" runat="server" AutoGenerateColumns="true" ShowHeader="false">
            </asp:GridView>
            <h2>Location</h2>
            <photo:Map runat="server" Latitude="48.1181" Longitude="-1.6707"></photo:Map>
        </div>
    </div>
</asp:Content>
