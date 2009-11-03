<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PhotoLinkData>" %>
<div class="PhotoLink">
<% var navigateUrl = Model.NavigateUrl ?? Url.Action<PhotoController>(c =>
       c.Index(Model.Photo.NotNull(p => p.Id), Model.Tag.NotNull(t => t.Id)),
       Molecule.MvcWebSite.atomes.photos.Atome.Id); %>
<a href="<%= navigateUrl %>">
    
    <%if (!String.IsNullOrEmpty(Model.HoverText)){ %>
    <div class="PhotoLink_HoverText"><%= Model.HoverText%></div>
    <%} %>
    <%if (Model.Photo != null)
      { %>
    <%--<div class="PhotoLink_HoverBackground"></div>--%>
        <img src="<%= Url.Action<PhotoController>(c => c.File(Model.Photo.Id, PhotoFileSize.Thumbnail), Molecule.MvcWebSite.atomes.photos.Atome.Id) %>"
        title="<%= Model.Description %>" alt="" class="PhotoLink_Thumbnail thinBox hover_background" />
    </a>
    <%} %>
    <% if (!String.IsNullOrEmpty(Model.HoverIconUrl)){ %>
    <div class="PhotoLink_HoverIcon">
        <a href='<%= navigateUrl %>'>
            <img title="<%= Model.Description %>" src="<%= Model.HoverIconUrl %>" alt="" />
        </a>
    </div>
    <%} %>
</div>