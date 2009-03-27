<%@ Page Language="C#" MasterPageFile="~/PreferencesPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.admin.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="preferencesContent" runat="server">
Récupérer les photos depuis :
    <asp:DropDownList ID="ProviderList" runat="server"
    DataValueField="Id" DataTextField="Name" />
    <asp:Button ID="preferencesButton" runat="server" Text="Save preferences" OnCommand="preferencesButton_Click" />
    
    <h2>Autorisations</h2>

    <table>
        <thead><tr><td>Tag</td>
            <asp:Repeater ID="AuthHeaderRepeater"  runat="server">
                <ItemTemplate>
                    <td><asp:Label runat="server"><%# (string)Container.DataItem %></asp:Label></td>
                </ItemTemplate>
            </asp:Repeater>
            <td>Anonymous</td>
            </tr>
        </thead>
        <asp:ListView ID="AuthListView" runat="server">
        <ItemTemplate>
            <tr>
                <td><asp:Label runat="server"><%# Eval("Tag") %></asp:Label></td>
                <asp:ListView runat="server" DataSource='<%# Eval("Authorizations") %>'>
                    <ItemTemplate>
                        <td>
                            <asp:CheckBox runat="server"
                            ToolTip='<%# Eval("Tag") +","+ Eval("User") %>' Checked='<%# Eval("Authorized") %>'
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
    <asp:TreeView ID="TagTreeView" runat="server" 
        ontreenodecheckchanged="TagTreeView_TreeNodeCheckChanged" ShowCheckBoxes="All">
    </asp:TreeView>
    <br />
    <asp:Button Text="Save" runat="server" OnClick="save_onclick" />

</asp:Content>
