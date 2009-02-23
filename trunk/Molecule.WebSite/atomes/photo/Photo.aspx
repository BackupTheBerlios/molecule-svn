<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Photo.aspx.cs"
    Inherits="Molecule.WebSite.atomes.photo.Photo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/common.css" rel="stylesheet" type="text/css" />
    <link href="style/photo.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    
    <asp:HyperLink ID="ImageNextLink" runat="server">
    <div id="photoNext">
        
            <img src="" alt="" runat="server" class="photoLink" id="ImageNext" />
            <div id="nextLinkIcon">
                <img src="/App_Themes/<%= Theme %>/images/go-next.png" alt="" /></div>
    </div>
    </asp:HyperLink>
    
    <asp:HyperLink ID="ImagePreviousLink" runat="server">
        <div id="photoPrevious">
            <img src="" alt="" runat="server" class="photoLink" id="ImagePrevious" />
            <div id="previousLinkIcon">
                <img src="/App_Themes/<%= Theme %>/images/go-previous.png" alt="" /></div>
        </div>
    </asp:HyperLink>
    
    <div id="photoCurrent">
        <asp:Image ID="ImageCurrent" style="border:solid 1px" runat="server" />
    </div>
</asp:Content>
