<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
    Inherits="System.Web.Mvc.ViewPage<Molecule.Atomes.Documents.Data.FolderIndexData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title>Template atome</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<%= Model.CurrentFolder.Name %>
</asp:Content>
