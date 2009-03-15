<%@ Page Language="C#" MasterPageFile="~/PreferencesPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.admin.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="preferencesContent" runat="server">
Récupérer les photos depuis :
    <asp:DropDownList ID="ProviderList" runat="server"
    DataValueField="Id" DataTextField="Name" />
    <asp:Button ID="preferencesButton" runat="server" Text="Save preferences" OnCommand="preferencesButton_Click" />
</asp:Content>
