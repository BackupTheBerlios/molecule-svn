<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagLink.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.TagLink" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="System.Linq" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>
<% if(TextOnly){ %>
<asp:HyperLink runat="server" NavigateUrl='<%# Molecule.WebSite.atomes.photo.Tag.GetUrlFor(Tag.Id) %>'>
    <asp:Label runat="server"><%# Tag.Name %></asp:Label>
</asp:HyperLink>
<%}else{ %>
<photo:PhotoLink runat="server" Description='<%# Tag.Name %>'
    PhotoId='<%# WebPhoto.Services.PhotoLibrary.GetFirstPhotoByTag(Tag.Id).Id %>'
    NavigateUrl='<%# Molecule.WebSite.atomes.photo.Tag.GetUrlFor(Tag.Id) %>'
    HoverText='<%# Tag.Name %>' HoverIconUrl="/App_Themes/bloup/images/folder.png" />
<%} %>