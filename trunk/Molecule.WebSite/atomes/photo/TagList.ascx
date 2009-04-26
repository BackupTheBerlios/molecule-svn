<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagList.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.TagList" %>
<%@ Register Src="TagLink.ascx" TagName="TagLink" TagPrefix="photo" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<ul class="TagList">
<asp:Repeater ID="list" runat="server">
<ItemTemplate>
    <li>
         <photo:TagLink runat="server" Tag="<%#(ITagInfo)Container.DataItem %>" TextOnly="true" />
    </li>
</ItemTemplate>
</asp:Repeater>
</ul>