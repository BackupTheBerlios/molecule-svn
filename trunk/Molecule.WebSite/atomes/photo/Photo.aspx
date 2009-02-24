<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Photo.aspx.cs"
    Inherits="Molecule.WebSite.atomes.photo.Photo" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Services" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/common.css" rel="stylesheet" type="text/css" />
    <link href="style/photo.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    
    <% if (NextPhoto != null){ %>
    <div id="photoNext">
        <a href='<%= GetUrlFor(NextPhoto.Id) %>'>
            <img src="<%= PhotoFile.GetUrlFor(NextPhoto.Id, PhotoFileSize.Thumbnail) %>"
                alt="" class="photoLink" />
        </a>
            <div id="nextLinkIcon">
                <a href='<%= GetUrlFor(NextPhoto.Id) %>'>
                    <img src="/App_Themes/<%= Theme %>/images/go-next.png" alt="" />
                    </a>
            </div>
    </div>
    <%} %>
    
    
    <% if (PreviousPhoto != null){ %>
    <div id="photoPrevious">
        <a href='<%= GetUrlFor(PreviousPhoto.Id) %>'>
            <img src="<%= PhotoFile.GetUrlFor(PreviousPhoto.Id, PhotoFileSize.Thumbnail) %>"
                alt="" class="photoLink" />
        </a>
        <div id="previousLinkIcon">
            <a href='<%= GetUrlFor(PreviousPhoto.Id) %>'>
                <img src="/App_Themes/<%= Theme %>/images/go-previous.png" alt="" />
                </a>
        </div>
    </div>
     <%} %>
    
    <div id="photoCurrent">
        <asp:Image ID="ImageCurrent" style="border:solid 1px" runat="server"
            ImageUrl="<%=PhotoFile.GetUrlFor(CurrentPhoto.Id, PhotoFileSize.Normal) %>"/>
        <div id="photoDescriptionContainer">

        </div>
    </div>
    
    
</asp:Content>
