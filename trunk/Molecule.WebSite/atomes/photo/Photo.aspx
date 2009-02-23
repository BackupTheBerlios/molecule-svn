<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Photo.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.Photo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <asp:HyperLink ID="ImagePreviousLink" runat="server">
        <asp:Image ID="ImagePrevious" runat="server" />
    </asp:HyperLink>
    <asp:Image ID="ImageCurrent" runat="server" />
    <asp:HyperLink ID="ImageNextLink" runat="server">
        <asp:Image ID="ImageNext" runat="server" />
    </asp:HyperLink>

</asp:Content>
