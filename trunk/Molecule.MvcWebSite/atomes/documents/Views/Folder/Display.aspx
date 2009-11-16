<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
    Inherits="System.Web.Mvc.ViewPage<Molecule.Atomes.Documents.Data.FolderIndexData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title>Template atome</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<h2><%= Model.CurrentFolder.Name %></h2>
<%var cForm = Html.BeginForm("Create", "Folder", new { atome = Molecule.Atomes.Documents.Atome.Id }, FormMethod.Get); %>
<%= Html.TextBox("name") %>
<input type="hidden" name="parentPath" value="<%= Model.CurrentFolder.Id %>" />
<input type="submit" value="Create" />
<% cForm.EndForm(); %>

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
