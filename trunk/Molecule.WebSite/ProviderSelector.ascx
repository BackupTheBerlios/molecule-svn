<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProviderSelector.ascx.cs" Inherits="Molecule.WebSite.ProviderSelector" %>
<asp:DropDownList ID="ProviderList" runat="server" DataValueField="Id" DataTextField="Name"
        OnSelectedIndexChanged="ProviderList_SelectedIndexChanged"/>
        
<asp:LinkButton ID="server" runat="server" Text="<%$ Resources:Common,Reload %>" 
OnCommand="ReinitButton_OnClick" />