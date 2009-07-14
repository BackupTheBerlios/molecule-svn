<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<%@ Import Namespace="MvcContrib" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/common.css" rel="stylesheet" type="text/css" />
    <link href="style/photo.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <%--<photo:TagHierarchy ID="TagHierarchy" runat="server" TagQueryStringField="tag" />--%>
    <div class="photoCurrent">   
        <%--<photo:PhotoLink ID="PreviousPhotoLink" runat="server" Description="Photo précédente" />
        <photo:PhotoLink ID="NextPhotoLink" runat="server" Description="Photo suivante" />
        <br />
        <photo:FullSizePhoto ID="FullSizePhoto" runat="server" PhotoId='<%#CurrentPhoto.Id %>' /> --%>
        
        <div id="photo">
        <div id="metadatas" class="metadatas">
            <%--<asp:GridView ID="MetadatasGridView" runat="server" AutoGenerateColumns="true" ShowHeader="false">
            </asp:GridView>--%>
        </div>
        <%--<a id="image" href="<%= PhotoFile.GetUrlFor(value, PhotoFileSize.Normal); %>" />--%>
        </div>
        
        <div id="photoDescriptionContainer" style="font-size:110%">
            <p>
                <%= (ViewData.Get<IPhotoInfo>("CurrentPhoto")).Description %>
            </p>
        </div>
        <br />
            
<%--        <a href="<%= PhotoFile.GetUrlFor(CurrentPhoto.Id, PhotoFileSize.Raw) %>">
            <div class="ActionImage"><img src="images/zoom-original.png" /></div>
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:photo,OriginaleImage%>" />
        </a>
        
        <photo:Map ID="PhotoMap" runat="server" />
        
        <asp:HyperLink runat="server" id="CalendarLink">
            <div class="ActionImage">
                <img alt="calendar" src="images/office-calendar.png" />
            </div>
            <asp:Literal ID="Literal2" runat="server" Text='<%$Resources:photo,Calendar %>' />
        </asp:HyperLink>
--%>
        <h2><%= WebPhoto.Services.PhotoLibrary.GetLocalizedTagName() %>s</h2>
<%--        <photo:TagList runat="server" ID="tagList" />--%>
    </div>
</asp:Content>