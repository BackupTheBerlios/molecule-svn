<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagLinkData>" %>

<% if(Model.TextOnly){
       %>
       <a href="<%= TagController.IndexUrl(Url, Model.Tag) %>"><%= Model.Tag.Name %></a>
   <%}else{ %>
<%--<photo:PhotoLink runat="server" Description='<%= Model.Tag.Name %>'
    PhotoId='<%= PhotoLibrary.GetFirstPhotoByTag(Model.Tag.Id).Id %>'
    NavigateUrl='/*<%# Molecule.WebSite.atomes.photo.Tag.GetUrlFor(Model.Tag.Id)*/ "" %>'
    HoverText='<%= tag.Name %>' HoverIconUrl="/App_Themes/bloup/images/folder.png" />--%>
<%} %>