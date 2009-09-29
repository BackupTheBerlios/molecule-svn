<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h1><%= Resources.molecule.SetupWelcome %></h1>
    <h2><%= Resources.molecule.Setup %></h2>
    <%= Resources.molecule.SetupInstructions %><br />
    <% var createForm = Html.BeginForm(); %>
    <label for="username"><%= Resources.Common.User %> : </label><br />
    <%= Html.TextBox("username", Environment.UserName)%><br />
    <label for="password"><%= Resources.Common.Password %> : </label><br />
    <%= Html.TextBox("password") %><br />
    <input type="submit" value="<%= Resources.molecule.NewUser %>" />
    <% createForm.EndForm(); %>
</asp:Content>
