<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagLinkData>" %>

<% if(Model.TextOnly){
       %>
       <a href="<%= Url.Action<TagController>(c => c.Index(Model.Tag != null ? Model.Tag.Id : null), Atome.Id) %>"><%= Model.Tag.Name %></a>
   <%}else{ %>
<%--<photo:PhotoLink runat="server" Description='<%= Model.Tag.Name %>'
    PhotoId='<%= PhotoLibrary.GetFirstPhotoByTag(Model.Tag.Id).Id %>'
    NavigateUrl='/*<%# Molecule.WebSite.atomes.photo.Tag.GetUrlFor(Model.Tag.Id)*/ "" %>'
    HoverText='<%= tag.Name %>' HoverIconUrl="/App_Themes/bloup/images/folder.png" />--%>
<%} %>