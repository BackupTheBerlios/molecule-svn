<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
    Inherits="System.Web.Mvc.ViewPage<Molecule.Atomes.Documents.Data.FolderDisplayData>" %>
    <%@ Import Namespace="Molecule.Atomes.Documents.Controllers" %>
    <%@ Import Namespace="Molecule.Atomes.Documents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title>Template atome</title>
<script type="text/javascript" src="../../../../Scripts/jquery-1.3.2.js"></script>
<script type="text/javascript" src="../../../../Scripts/jquery.tools.min.js"></script>
<script type="text/javascript">
    $(document).ready(function() {
        
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<h2><%= Model.CurrentFolder.Name %></h2>
<%= Html.ActionLink<FolderController>("Créer un dossier", c => c.Create(Model.CurrentFolder.Id))%><br />
<%= Html.ActionLink<FolderController>("Ajouter un document", c => c.AddDocument(Model.CurrentFolder.Id))%>

<ul>
<% foreach (var folder in Model.Folders) { %>
    <li><%= Html.ActionLink<FolderController>(folder.Name, c => c.Display(folder.Id)) %></li>
<%} %>
</ul>
<ul>
<% foreach (var doc in Model.Documents) { %>
    <li><%= Html.ActionLink<FolderController>(doc.Name, c => c.File(doc.Id)) %></li>
<%} %>
</ul>
</asp:Content>
