<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
    Inherits="System.Web.Mvc.ViewPage<Molecule.Atomes.Documents.Data.FolderIndexData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title>Template atome</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<h2><%= Model.CurrentFolder.Name %></h2>
<ul>
<% foreach (var folder in Model.Folders) { %>
    <li><%= Html.ActionLink<Molecule.Atomes.Documents.Controllers.FolderController>(
            folder.Name, c => c.Display(folder.Id),
            Molecule.Atomes.Documents.Atome.Id) %></li>
<%} %>
</ul>
<ul>
<% foreach (var doc in Model.Documents) { %>
    <li><%= Html.ActionLink<Molecule.Atomes.Documents.Controllers.FolderController>(
            doc.Name, c => c.File(doc.Id),
            Molecule.Atomes.Documents.Atome.Id) %></li>
<%} %>
</ul>
</asp:Content>
