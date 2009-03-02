<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Photo.aspx.cs"
 EnableViewState="False" EnableEventValidation="false"
    Inherits="Molecule.WebSite.atomes.photo.Photo" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="WebPhoto.Providers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/common.css" rel="stylesheet" type="text/css" />
    <link href="style/photo.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if(tag != null){ %>
    Tag courant : <a href='<%= Tag.GetUrlFor(tagId) %>'>
                        <asp:Label runat="server"><%= tag.Name %></asp:Label>
                    </a>
    <% } %>
    
    <% if (NextPhoto != null){ %>
    <div id="photoNext">
        <a href='<%= GetUrlFor(NextPhoto.Id, tagId) %>'>
            <img src="<%= PhotoFile.GetUrlFor(NextPhoto.Id, PhotoFileSize.Thumbnail) %>"
                alt="" class="photoLink" />
        </a>
            <div id="nextLinkIcon">
                <a href='<%= GetUrlFor(NextPhoto.Id, tagId) %>'>
                    <img src="/App_Themes/<%= Theme %>/images/go-next.png" alt="" />
                    </a>
            </div>
    </div>
    <%} %>
    
    
    <% if (PreviousPhoto != null){ %>
    <div id="photoPrevious">
        <a href='<%= GetUrlFor(PreviousPhoto.Id, tagId) %>'>
            <img src="<%= PhotoFile.GetUrlFor(PreviousPhoto.Id, PhotoFileSize.Thumbnail) %>"
                alt="" class="photoLink" />
        </a>
        <div id="previousLinkIcon">
            <a href='<%= GetUrlFor(PreviousPhoto.Id, tagId) %>'>
                <img src="/App_Themes/<%= Theme %>/images/go-previous.png" alt="" />
                </a>
        </div>
    </div>
     <%} %>
    
    <div id="photoCurrent">
        <asp:Image ID="ImageCurrent" style="border:solid 1px" runat="server"
            ImageUrl="<%=PhotoFile.GetUrlFor(CurrentPhoto.Id, PhotoFileSize.Normal) %>"/>
        <div id="photoDescriptionContainer">
            <p>
                <asp:Label runat="server"><%= CurrentPhoto.Description %></asp:Label>
            </p>
            <h2>Tags</h2>
            <ul id="tagList">
            <asp:Repeater ID="TagsView" runat="server">
            <ItemTemplate>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl='<%# Tag.GetUrlFor(((ITagInfo)Container.DataItem).Id) %>'>
                        <asp:Label runat="server"><%#Eval("Name")%></asp:Label>
                    </asp:HyperLink>
                </li>
            </ItemTemplate>
            </asp:Repeater>
            </ul>
            <h2>Metadatas</h2>
            <asp:GridView ID="MetadatasGridView" runat="server" AutoGenerateColumns="true" ShowHeader="false">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
