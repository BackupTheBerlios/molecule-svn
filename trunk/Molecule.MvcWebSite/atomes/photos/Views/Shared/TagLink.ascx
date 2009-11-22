<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagLinkData>" %>

<% if(Model.TextOnly){%>
       <%= Html.ActionLink<TagController>(Model.Tag.Name,
                      c => c.Index(Model.Tag.NotNull(t => t.Id)))%>
   <%}else{ %>
<%--<photo:PhotoLink runat="server" Description='<%= Model.Tag.Name %>'
    PhotoId='<%= PhotoLibrary.GetFirstPhotoByTag(Model.Tag.Id).Id %>'
    NavigateUrl='/*<%# Molecule.WebSite.atomes.photo.Tag.GetUrlFor(Model.Tag.Id)*/ "" %>'
    HoverText='<%= tag.Name %>' HoverIconUrl="/App_Themes/bloup/images/folder.png" />--%>
<%} %>