<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<ITagInfo>>" %>
<ul class="TagList">
<% foreach(var tag in Model){ %>
    <li>
    <% Html.RenderPartial("TagLink", new TagLinkData() { Tag = tag, TextOnly = true }); %>
    </li>
<% } %>
</ul>
