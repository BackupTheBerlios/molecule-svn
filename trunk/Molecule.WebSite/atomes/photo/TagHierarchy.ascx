<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagHierarchy.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.TagHierarchy" %>
<%@ Register Src="~/atomes/photo/TagLink.ascx" TagName="TagLink" TagPrefix="photo" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="System.Globalization" %>
<span style="font-size:120%">
<asp:HyperLink runat="server" NavigateUrl="~/atomes/photo/Default.aspx" Text="Accueil" />
<asp:Repeater runat="server" ID="TagHierarchyView">
    <HeaderTemplate> > 
    </HeaderTemplate>
    <ItemTemplate>
        <photo:TagLink runat="server" Tag="<%#(ITagInfo)Container.DataItem %>" TextOnly="true" />
    </ItemTemplate>
    <SeparatorTemplate> > </SeparatorTemplate>
</asp:Repeater>
<% if (Year.HasValue){ %>
 > <asp:HyperLink ID="YearLink" runat="server" >
    <asp:Label runat="server"><%= Year.Value %></asp:Label>
</asp:HyperLink>
    <% if (Month.HasValue)
       { %>
 > <asp:HyperLink ID="MonthLink" runat="server" >
    <asp:Label runat="server"><%= DateTimeFormatInfo.CurrentInfo.GetMonthName(Month.Value) %></asp:Label>
</asp:HyperLink>
<%     }
 } %>
 </span>

