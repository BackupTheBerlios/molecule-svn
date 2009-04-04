<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoLink.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.PhotoLink" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<div id="<%= ID %>" class="PhotoLink">
<a href='<%= NavigateUrl ?? Photo.GetUrlFor(PhotoId, TagId) %>'>
    
    <%if (!String.IsNullOrEmpty(HoverText)){ %>
    
    <div class="PhotoLink_HoverText"><%= HoverText%></div>
    <%} %>
    <%if (!String.IsNullOrEmpty(PhotoId)){ %>
    <div class="PhotoLink_HoverBackground"></div>
        <img src="<%= PhotoFile.GetUrlFor(PhotoId, PhotoFileSize.Thumbnail) %>" title="<%= Description %>" alt="" class="PhotoLink_Thumbnail" />
    </a>
    <%} %>
    <% if (!String.IsNullOrEmpty(HoverIconUrl)){ %>
    <div class="PhotoLink_HoverIcon">
        <a href='<%= NavigateUrl ?? Photo.GetUrlFor(PhotoId, TagId) %>'>
            <img title="<%= Description %>" src="<%= HoverIconUrl %>" alt="" />
        </a>
    </div>
    <%} %>
</div>