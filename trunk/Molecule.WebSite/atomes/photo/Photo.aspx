﻿<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Photo.aspx.cs"
    EnableViewState="False" EnableEventValidation="false" Inherits="Molecule.WebSite.atomes.photo.Photo" %>

<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<%@ Register Src="TagList.ascx" TagName="TagList" TagPrefix="photo" %>
<%@ Register src="TagHierarchy.ascx" tagname="TagHierarchy" tagprefix="photo" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>
<%@ Register Src="FullSizePhoto.ascx" TagName="FullSizePhoto" TagPrefix="photo" %>
<%@ Register Src="Map.ascx" TagName="Map" TagPrefix="photo" %>
<%@ Register Src="RawPhoto.ascx" TagName="RawPhoto" TagPrefix="photo" %>

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
    <div class="photoCurrent">   
        <photo:PhotoLink ID="PreviousPhotoLink" runat="server" Description="Photo précédente" />
        <photo:PhotoLink ID="NextPhotoLink" runat="server" Description="Photo suivante" />
        <br />
        <photo:FullSizePhoto ID="FullSizePhoto" runat="server" PhotoId='<%#CurrentPhoto.Id %>' /> 
        <div id="photoDescriptionContainer" style="font-size:110%">
            <p>
                <asp:Label runat="server"><%= CurrentPhoto.Description %></asp:Label>
            </p>
        </div>
        <br />
            
        <a href="<%= PhotoFile.GetUrlFor(CurrentPhoto.Id, PhotoFileSize.Raw) %>">
            <div class="ActionImage"><img src="images/zoom-original.png" /></div>
            <asp:Literal runat="server" Text="<%$ Resources:photo,OriginaleImage%>" />
        </a>
        
        <photo:Map ID="PhotoMap" runat="server" />
        
        <asp:HyperLink runat="server" id="CalendarLink">
            <div class="ActionImage">
                <img alt="calendar" src="images/office-calendar.png" />
            </div>
            <asp:Literal runat="server" Text='<%$Resources:photo,Calendar %>' />
        </asp:HyperLink>

        <h2><%= WebPhoto.Services.PhotoLibrary.GetLocalizedTagName() %>s</h2>
        <photo:TagList runat="server" ID="tagList" />
    </div>
</asp:Content>