<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagLink.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.TagLink" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="System.Linq" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>

<% if(Model.TextOnly)
       Writer.Write(Html.ActionLink(Model.Tag.Name, "Tag", "photos",new { id = Model.Tag.Id }));
   else{ %>
<photo:PhotoLink runat="server" Description='<%= Model.Tag.Name %>'
    PhotoId='<%= PhotoLibrary.GetFirstPhotoByTag(Model.Tag.Id).Id %>'
    NavigateUrl='/*<%# Molecule.WebSite.atomes.photo.Tag.GetUrlFor(Model.Tag.Id)*/ "" %>'
    HoverText='<%= tag.Name %>' HoverIconUrl="/App_Themes/bloup/images/folder.png" />
<%} %>