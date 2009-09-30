<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<h2><%= Resources.molecule.Login %></h2>
    <p>
        <%= Resources.molecule.LoginInstructions %>
    </p>
    <%= Html.ValidationSummary(Resources.molecule.LoginError) %>

    <% using (Html.BeginForm()) { %>
        <div>
            <p>
                <label for="username"><%= Resources.Common.User %> :</label>
                <%= Html.TextBox("username") %>
                <%= Html.ValidationMessage("username") %>
            </p>
            <p>
                <label for="password"><%= Resources.Common.Password %> :</label>
                <%= Html.Password("password") %>
                <%= Html.ValidationMessage("password") %>
            </p>
            <p>
                <%= Html.CheckBox("rememberMe") %> <label for="rememberMe"><%= Resources.molecule.RememberMe %></label>
            </p>
            <p>
                <input type="submit" value="<%= Resources.molecule.Login %>" />
            </p>
        </div>
    <% } %>
</asp:Content>
