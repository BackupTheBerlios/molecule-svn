<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/PreferencesPage.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="preferencesContent" runat="server">
<% Html.BeginForm("Save", "Preferences", new { atome = Molecule.Atomes.Documents.Atome.Id }, FormMethod.Post); %>

<% Html.EndForm(); %>
</asp:Content>
