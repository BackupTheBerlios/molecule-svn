<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
 Inherits="System.Web.Mvc.ViewPage<Molecule.Atomes.Documents.Data.FolderCreateData>" %>
    <%@ Import Namespace="Molecule.Atomes.Documents.Controllers" %>
    <%@ Import Namespace="Molecule.Atomes.Documents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<%var cForm = Html.BeginForm<FolderController>(c => c.Create(Model.CurrentFolder.Id)); %>
<%= Html.TextBox("name") %>
<input type="hidden" name="parentPath" value="<%= Model.CurrentFolder.Id %>" />
<input type="submit" value="Create" />
<% cForm.EndForm(); %>
</asp:Content>
