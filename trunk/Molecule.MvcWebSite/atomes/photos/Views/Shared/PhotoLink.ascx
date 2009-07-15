<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PhotoLinkData>" %>
<div class="PhotoLink">
<a href='<%= Model.NavigateUrl ?? Url.Action("Index", "photo", new { id = Model.Photo.Id }) %>'>
    
    <%if (!String.IsNullOrEmpty(Model.HoverText)){ %>
    
    <div class="PhotoLink_HoverText"><%= Model.HoverText%></div>
    <%} %>
    <%if (Model.Photo != null)
      { %>
    <div class="PhotoLink_HoverBackground"></div>
        <img src="<%= Url.Action("File", "photo", new { id = Model.Photo.Id, size= PhotoFileSize.Thumbnail }, null) %>" title="<%= Model.Description %>" alt="" class="PhotoLink_Thumbnail" />
    </a>
    <%} %>
    <% if (!String.IsNullOrEmpty(Model.HoverIconUrl)){ %>
    <div class="PhotoLink_HoverIcon">
        <a href='<%= Model.NavigateUrl ?? Url.Action("Index", "photo", new { id = Model.Photo.Id }) %>'>
            <img title="<%= Model.Description %>" src="<%= Model.HoverIconUrl %>" alt="" />
        </a>
    </div>
    <%} %>
</div>