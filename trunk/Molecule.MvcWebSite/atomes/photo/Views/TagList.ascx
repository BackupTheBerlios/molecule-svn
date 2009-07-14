<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagList.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.TagList" %>
<%@ Register Src="TagLink.ascx" TagName="TagLink" TagPrefix="photo" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<ul class="TagList">
<% foreach(var tag in Model){ %>
    <li>
    <% Html.RenderPartial("TagLink", new TagLinkData() { Tag = tag, TextOnly = true }); %>
    </li>
<% } %>
</ul>
