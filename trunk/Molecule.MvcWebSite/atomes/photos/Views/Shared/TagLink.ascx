<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagLinkData>" %>

<% if(Model.TextOnly)
       Writer.Write(Html.RouteLink(Model.Tag.Name, "Tag", new { id = Model.Tag.Id}));
   else{ %>
<%--<photo:PhotoLink runat="server" Description='<%= Model.Tag.Name %>'
    PhotoId='<%= PhotoLibrary.GetFirstPhotoByTag(Model.Tag.Id).Id %>'
    NavigateUrl='/*<%# Molecule.WebSite.atomes.photo.Tag.GetUrlFor(Model.Tag.Id)*/ "" %>'
    HoverText='<%= tag.Name %>' HoverIconUrl="/App_Themes/bloup/images/folder.png" />--%>
<%} %>