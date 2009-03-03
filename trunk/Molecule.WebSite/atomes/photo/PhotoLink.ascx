<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoLink.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.PhotoLink" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<div id="<%= ID %>" class="PhotoLink">
    <a href='<%= Photo.GetUrlFor(PhotoId, TagId) %>'>
        <img src="<%= PhotoFile.GetUrlFor(PhotoId, PhotoFileSize.Thumbnail) %>" title="<%= Description %>" alt="" class="photoLink" />
    </a>
    <% if (!String.IsNullOrEmpty(HoverIconUrl)){ %>
    <div>
        <a href='<%= Photo.GetUrlFor(PhotoId, TagId) %>'>
            <img title="<%= Description %>" src="<%= HoverIconUrl %>" class="PhotoLink_HoverIcon" alt="" />
        </a>
    </div>
    <%} %>
</div>