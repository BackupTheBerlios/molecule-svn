<%@ Page Language="C#" MasterPageFile="~/Views/Shared/PreferencesPage.Master"
Inherits="System.Web.Mvc.ViewPage<Molecule.MvcWebSite.atomes.admin.Data.PreferencesData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="preferencesContent" runat="server">
<%var form = Html.BeginForm("Save", "Admin", new { atome = "admin" }, FormMethod.Post); %>
<br />
   
<% form.EndForm(); %>
</asp:Content>
