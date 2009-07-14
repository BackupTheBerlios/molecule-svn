<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagLink.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.TagLink" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="System.Linq" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>

<% var tag = PhotoLibrary.GetTag(Model.TagId); %>
<% if(Model.TextOnly)
       Writer.Write(Html.ActionLink(tag.Name,null, new {Controller="photo", Action="Tag", id=tag.Id}));
   else{ %>
<photo:PhotoLink runat="server" Description='<%= tag.Name %>'
    PhotoId='<%= PhotoLibrary.GetFirstPhotoByTag(tag.Id).Id %>'
    NavigateUrl='/*<%# Molecule.WebSite.atomes.photo.Tag.GetUrlFor(tag.Id)*/ "" %>'
    HoverText='<%= tag.Name %>' HoverIconUrl="/App_Themes/bloup/images/folder.png" />
<%} %>