<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
Inherits="System.Web.Mvc.ViewPage<TagIndexData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/atomes/photos/style/common.css" rel="stylesheet" type="text/css" />
    <link href="/atomes/photos/style/tag.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<% Html.RenderPartial("TagHierarchy", new TagHierarchyData() { Tag = Model.Tag }); %>
<% if(Model.SubTags.Any()){ %>
<h2><%= PhotoLibrary.GetLocalizedTagName() %>s</h2>
<% Html.RenderPartial("TagList", Model.SubTags);
}%>
       
<%if (Model.Photos.Any()){ %>
 <h2>Photos</h2>
 <ul class="PhotoList">
<% foreach(var photo in Model.Photos){ %>
    <li>
    <% Html.RenderPartial("PhotoLink", new PhotoLinkData() { Photo = photo, Tag = Model.Tag }); %>
    </li>
<% } %>
</ul>
 <%}%>

<br />
<%  var first = Model.Photos.FirstOrDefault();
    if (first != null)
    {%>
<a href="<%= TagController.ZipUrl(Url, Model.Tag) %>">
            <div class="ActionImage">
    <img alt="download" src="<%= Url.Theme("images/document-save.png")%>" />
</div>
            <%= Resources.photo.DownloadPage%>
        </a>
        
<a href="<%= CalendarController.MonthUrl(Url, first.Date.Year, first.Date.Month, Model.Tag) %>">
            <div class="ActionImage">
                <img alt="calendar" src="/atomes/photos/images/office-calendar.png" />
            </div>
            <%= Resources.photo.Calendar%>
        </a>
<%} %>
</asp:Content>
