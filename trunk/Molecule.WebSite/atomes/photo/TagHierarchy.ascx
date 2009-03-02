<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagHierarchy.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.TagHierarchy" %>
<%@ Register Src="~/atomes/photo/TagLink.ascx" TagName="TagLink" TagPrefix="photo" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<asp:Repeater runat="server" ID="TagHierarchyView">
    <ItemTemplate>
        <photo:TagLink runat="server" Tag="<%#(ITagInfo)Container.DataItem %>" />
    </ItemTemplate>
    <SeparatorTemplate> > </SeparatorTemplate>
</asp:Repeater>