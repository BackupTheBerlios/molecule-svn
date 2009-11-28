<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/PreferencesPage.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Molecule.Atomes.Documents.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="preferencesContent" runat="server">
<% Html.BeginForm<PreferencesController>(c => c.Save()); %>

<% Html.EndForm(); %>
</asp:Content>
