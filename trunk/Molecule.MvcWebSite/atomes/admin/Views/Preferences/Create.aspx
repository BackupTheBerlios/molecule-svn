<%@ Page Language="C#" MasterPageFile="~/Views/Shared/PreferencesPage.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="preferencesContent" runat="server">
    <h2>
        <%=Resources.molecule.NewUser %></h2>
    <% var createForm = Html.BeginForm("Create", "Preferences", new { atome = "admin" }, FormMethod.Post); %>
    <label for="username"><%= Resources.Common.User %> : </label>
    <%= Html.TextBox("username") %>
    <label for="password"><%= Resources.Common.Password %> : </label>
    <%= Html.TextBox("password") %>
    <input type="submit" value="<%= Resources.molecule.NewUser %>" />
    <% createForm.EndForm(); %>
</asp:Content>
