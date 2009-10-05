<%@ Page Language="C#" MasterPageFile="~/Views/Shared/PreferencesPage.Master"
Inherits="System.Web.Mvc.ViewPage<Molecule.MvcWebSite.atomes.music.Data.AdminData>" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="preferencesContent" runat="server">
<%var form = Html.BeginForm("Save", "Admin", new { atome = "Music" }, FormMethod.Post); %>
<%= Resources.webmusic.GetSongsFrom %>
<br />
<% Html.RenderPartial("ProviderSelector", Model); %>
<br/>
<h2>Last.fm</h2>
<%= Html.CheckBox("lastfmEnabled", Model.LastfmEnabled) %> <label for="lastfmEnabled"><%= Resources.webmusic.EnableLastFmNotification %></label>
<br/>
<label for="lastfmUsername"><%= Resources.Common.User %></label> : <%= Html.TextBox("lastfmUsername", Model.LastfmUsername) %>
<br/>
<label for="lastfmPassword"><%= Resources.Common.Password %></label> : <%= Html.TextBox("lastfmPassword", Model.LastfmPassword)%>
<br/>
<input type="submit" value="<%= Resources.Common.Save %>" />
<% form.EndForm(); %>
</asp:Content>
