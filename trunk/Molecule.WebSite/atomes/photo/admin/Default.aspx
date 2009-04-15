<%@ Page Language="C#" MasterPageFile="~/PreferencesPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.admin.Default" Title="Untitled Page" %>
<%@ Register src="~/ProviderSelector.ascx" tagname="ProviderSelector" tagprefix="molecule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="preferencesContent" runat="server">
<h2>Source</h2>
    <p><asp:Literal runat="server" Text='<%$Resources:photo,GetPhotosFrom %>' /> 
     <molecule:ProviderSelector ID="ProviderSelector1" runat="server" 
        AtomeProviderTypeName="WebPhoto.Services.PhotoLibrary" />
    </p>
    <p><asp:Literal runat="server" Text='<%$Resources:photo,PhotoCollectionName %>' />
    <asp:DropDownList ID="TagNameList" runat="server" DataValueField="Value" DataTextField="Name"
        OnSelectedIndexChanged="TagNameList_SelectedIndexChanged" />
    </p>
    <asp:Button runat="server" Text="<%$ Resources:Common,Save %>" 
        OnCommand="OkButton_OnClick" />
    
    <h2><%= WebPhoto.Services.PhotoLibrary.GetLocalizedTagName() %>s partagés</h2>
    <ajaxToolkit:ModalPopupExtender ID="MPE" runat="server"
        TargetControlID="TagLink" PopupControlID="TagSelectPanel"
        CancelControlID="CancelButton"
        BackgroundCssClass="modalBackground" />
    <table>
        <thead><tr><td><asp:Button runat="server" ID="TagLink"
            Text='<%# WebPhoto.Services.PhotoLibrary.GetLocalizedTagName()+"s" %>'/></td>
            <asp:Repeater ID="AuthHeaderRepeater"  runat="server">
                <ItemTemplate>
                    <td><asp:Label runat="server"><%# (string)Container.DataItem %></asp:Label></td>
                </ItemTemplate>
            </asp:Repeater> 
            <td><asp:Literal runat="server" Text='<%$ Resources:Common,Anonymous %>' /></td>
            </tr>
        </thead>
        <asp:ListView ID="AuthListView" runat="server" EnableViewState="true">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label runat="server" ToolTip='<%# WebPhoto.Services.PhotoLibrary.GetTagFullPath((string)Eval("TagId")) %>'>
                        <%# WebPhoto.Services.PhotoLibrary.AdminGetTag((string)Eval("TagId")).Name %>
                    </asp:Label>
                </td>
                <asp:ListView runat="server" DataSource='<%# Eval("Authorizations") %>' EnableViewState="true">
                    <ItemTemplate>
                        <td>
                            <asp:CheckBox runat="server"
                            ToolTip='<%# Eval("TagId") +","+ Eval("User") %>' Checked='<%# Eval("Authorized") %>'
                             OnCheckedChanged="OnAuthListView_CheckedChanged" />
                        </td>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <td id="itemPlaceHolder" runat="server"></td>
                    </LayoutTemplate>
                </asp:ListView>
            </tr>
        </ItemTemplate>
        <LayoutTemplate>
            <tbody>
                <tr ID="itemPlaceHolder" runat="server">
                </tr>
            </tbody>
        </LayoutTemplate>
        </asp:ListView>
    </table>
    <asp:Panel runat="server" ID="TagSelectPanel" CssClass="popup" DefaultButton="OkButton" Height="300" Width="300">
        <div style="height:259px;overflow:auto; margin:5px">
            <asp:TreeView ID="TagTreeView" runat="server" NodeIndent="10" ExpandDepth="20"
                ontreenodecheckchanged="TagTreeView_TreeNodeCheckChanged" ShowCheckBoxes="All">
            </asp:TreeView>
        </div>
        <div style="margin:5px; text-align:right;">
            <asp:Button runat="server" ID="OkButton" Text="Ok" OnClick="OkButton_OnClick"/>
            <asp:Button runat="server" ID="CancelButton" Text="Annuler" />
        </div>
    </asp:Panel>
    <br />
    <asp:Button Text="<%$ Resources:Common,Save %>" runat="server" OnClick="save_onclick" />
</asp:Content>
