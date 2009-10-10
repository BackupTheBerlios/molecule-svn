<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PhotoLinkData>" %>
<div class="PhotoLink">
<% var navigateUrl = Model.NavigateUrl ?? Url.Action<PhotoController>(c => c.Index(Model.Photo.Id,
       Model.Tag != null ? Model.Tag.Id : null), Atome.Id); %>
<a href="<%= navigateUrl %>">
    
    <%if (!String.IsNullOrEmpty(Model.HoverText)){ %>
    <div class="PhotoLink_HoverText"><%= Model.HoverText%></div>
    <%} %>
    <%if (Model.Photo != null)
      { %>
    <div class="PhotoLink_HoverBackground"></div>
        <img src="<%= Url.Action<PhotoController>(c => c.File(Model.Photo.Id, PhotoFileSize.Thumbnail)) %>"
        title="<%= Model.Description %>" alt="" class="PhotoLink_Thumbnail" />
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