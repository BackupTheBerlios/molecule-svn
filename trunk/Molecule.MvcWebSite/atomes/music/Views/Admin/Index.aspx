<%@ Page Language="C#" MasterPageFile="~/Views/Shared/PreferencesPage.Master"
Inherits="System.Web.Mvc.ViewPage<Molecule.MvcWebSite.atomes.music.Data.AdminData>" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="preferencesContent" runat="server">
<%=Resources.webmusic.GetSongsFrom %>
<% Html.RenderPartial("ProviderSelector", Model); %>
   <%-- <molecule:ProviderSelector ID="ProviderSelector1" runat="server" 
        AtomeProviderTypeName="WebMusic.Services.MusicLibrary" />
--%>
<br/>
Last.Fm :<br />
<%--    <asp:CheckBox ID="lastfmEnabledCheckBox" runat="server" AutoPostBack="True" 
        oncheckedchanged="lastfmEnabledCheckBox_CheckedChanged" Text="Enable Lastfm" />
--%><br/>
<%--<asp:Literal runat="server" Text='<%$Resources:Common,User %>' /> : 
<asp:TextBox ID="lastFmUsername"  runat="server"></asp:TextBox>
<br/>
<asp:Literal ID="Literal1" runat="server" Text='<%$Resources:Common,Password %>' /> : 
<asp:TextBox ID="lastFmUserPassword"  TextMode="password" runat="server"></asp:TextBox>
<br/>

<asp:Button ID="preferencesButton" runat="server" Text="<%$ Resources:Common,Save %>" OnCommand="preferencesButton_Click" >
</asp:Button>--%>
</asp:Content>
