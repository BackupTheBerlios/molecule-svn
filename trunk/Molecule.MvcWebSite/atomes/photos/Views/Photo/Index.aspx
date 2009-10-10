<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master" Inherits="System.Web.Mvc.ViewPage<PhotoIndexData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/atomes/photos/style/common.css" rel="stylesheet" type="text/css" />
    <link href="/atomes/photos/style/photo.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% Html.RenderPartial("TagHierarchy", new TagHierarchyData() { Tag = Model.CurrentTag }); %>
    <div class="photoCurrent">
        <%if (Model.PreviousPhoto != null)
              Html.RenderPartial("PhotoLink", new PhotoLinkData()
              {
                  Photo = Model.PreviousPhoto,
                  Tag = Model.CurrentTag
                  ,
                  HoverIconUrl = Url.Theme("images/go-previous.png"),
                  Description = "Photo Précédente"
              });
          if (Model.NextPhoto != null)
              Html.RenderPartial("PhotoLink", new PhotoLinkData()
              {
                  Photo = Model.NextPhoto,
                  Tag = Model.CurrentTag
                  ,
                  HoverIconUrl = Url.Theme("images/go-next.png"),
                  Description = "Photo suivante"
              });
        %><br />
        <div id="photo">
            <div id="metadatas" class="metadatas">
                <%= Html.Grid(Model.Photo.Metadatas.Select((kvp) => new {Key = kvp.Key, Value = kvp.Value}))
                    .Columns(column => {
                        column.For(x => x.Key);
     		            column.For(x => x.Value);
     	            }) %>
            </div>
            <img src="<%= Url.Action<PhotoController>(c => c.File(Model.Photo.Id, PhotoFileSize.Normal)) %>"
                alt="" width="<%=Model.PhotoSize.Width %>" height="<%=Model.PhotoSize.Height %>" />
        </div>
        <div id="photoDescriptionContainer" style="font-size: 110%">
            <p>
                <%= Model.Photo.Description %>
            </p>
        </div>
        <br />
        <a href="<%= Url.Action<PhotoController>(c => c.File(Model.Photo.Id, PhotoFileSize.Raw)) %>">
            <div class="ActionImage">
                <img alt="Original" src="/atomes/photos/images/zoom-original.png" /></div>
            <%= Resources.photo.OriginaleImage %>
        </a>
        <%--<photo:Map ID="PhotoMap" runat="server" />--%>
        
        <a href="<%= Url.Action<CalendarController>(c => c.Month(Model.Photo.Date.Year,
            Model.Photo.Date.Month, Model.CurrentTag != null ? Model.CurrentTag.Id : null)) %>">
            <div class="ActionImage">
                <img alt="calendar" src="/atomes/photos/images/office-calendar.png" />
            </div>
            <%= Resources.photo.Calendar %>
        </a>
        <h2>
            <%= WebPhoto.Services.PhotoLibrary.GetLocalizedTagName() %>s</h2>
        <% Html.RenderPartial("TagList", Model.PhotoTags); %>
    </div>
</asp:Content>
