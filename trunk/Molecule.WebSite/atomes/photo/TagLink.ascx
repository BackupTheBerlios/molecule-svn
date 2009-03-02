<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagLink.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.TagLink" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<asp:HyperLink runat="server" NavigateUrl='<%# Molecule.WebSite.atomes.photo.Tag.GetUrlFor(Tag.Id) %>'>
    <asp:Label runat="server"><%# Tag.Name %></asp:Label>
</asp:HyperLink>