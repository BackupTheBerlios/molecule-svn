<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
Inherits="System.Web.Mvc.ViewPage<TagIndexData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= Url.Content("~/atomes/photos/style/common.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/atomes/photos/style/tag.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<%--<photo:TagHierarchy ID="tagHierarchy" runat="server" TagQueryStringField="id" />>--%>
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
    
<%--<asp:PlaceHolder runat="server" ID="photosPlaceHolder">
<h2>Photos</h2>
    <asp:ListView ID="PhotoListView" runat="server" 
                        onpagepropertieschanging="PhotoListView_PagePropertiesChanging">
        <ItemTemplate>
            <photo:PhotoLink runat="server" TagId='<%# tagId %>' PhotoId='<%# Eval("Id") %>' />
        </ItemTemplate>
        <LayoutTemplate>
            <div id="itemPlaceholder" runat="server">
            </div>
        </LayoutTemplate>
    </asp:ListView>
    <br />
    <asp:DataPager runat="server" ID="PhotoDataPager" PagedControlID="PhotoListView"
        QueryStringField="page" PageSize="21">
        <Fields>
            <asp:NextPreviousPagerField ButtonType="Link" PreviousPageText="<"
                ShowNextPageButton="false" ShowPreviousPageButton="True" />
            <asp:NumericPagerField />
            <asp:NextPreviousPagerField ButtonType="Link" NextPageText=">"
                ShowPreviousPageButton="false" ShowNextPageButton="True"  />
        </Fields>
    </asp:DataPager>
    <br />
<asp:HyperLink runat="server" id="CalendarLink">
<div class="ActionImage">
    <img alt="calendar" src="images/office-calendar.png" />
</div>
<asp:Literal ID="Literal1" runat="server" Text='<%$Resources:photo,Calendar %>' />
</asp:HyperLink>

<asp:HyperLink runat="server" id="DownloadLink">
<div class="ActionImage">
    <img alt="download" src="../../App_Themes/<%= Theme %>/images/document-save.png" />
</div>
<asp:Literal ID="Literal2" runat="server" Text='<%$Resources:photo,DownloadPage %>' />
</asp:HyperLink>


</asp:PlaceHolder>
--%></asp:Content>
