<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PhotoLinkData>" %>
<div class="PhotoLink">
<% var navigateUrl = Model.NavigateUrl ?? (Model.Photo != null ?
       Model.Tag != null ? Url.RouteUrl("TagPhoto", new {id = Model.Photo.Id, tagId = Model.Tag.Id })
                        : Url.RouteUrl("Photo", new { id = Model.Photo.Id })
       : "");  %>
<a href='<%= navigateUrl %>'>
    
    <%if (!String.IsNullOrEmpty(Model.HoverText)){ %>
    <div class="PhotoLink_HoverText"><%= Model.HoverText%></div>
    <%} %>
    <%if (Model.Photo != null)
      { %>
    <div class="PhotoLink_HoverBackground"></div>
        <img src="<%= Url.Action("File", "photo", new { id = Model.Photo.Id, size= PhotoFileSize.Thumbnail }, null) %>"
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