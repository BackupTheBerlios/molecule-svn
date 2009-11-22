<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
 Inherits="System.Web.Mvc.ViewPage<Molecule.Atomes.Documents.Data.FolderAddDocumentData>" %>
    <%@ Import Namespace="Molecule.Atomes.Documents.Controllers" %>
    <%@ Import Namespace="Molecule.Atomes.Documents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<%var aForm = Html.BeginForm<FolderController>(c => c.AddDocument(Model.CurrentFolder.Id),
      FormMethod.Post, new { enctype = "multipart/form-data" }); %>
<input type="file" name="file" />
<input type="hidden" name="folderPath" value="<%= Model.CurrentFolder.Id %>" />
<input type="submit" value="Upload" />
<% aForm.EndForm(); %>
</asp:Content>
