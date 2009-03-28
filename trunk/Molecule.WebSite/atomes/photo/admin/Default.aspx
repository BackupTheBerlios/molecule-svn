<%@ Page Language="C#" MasterPageFile="~/PreferencesPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.admin.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="preferencesContent" runat="server">
Récupérer les photos depuis :
    <asp:DropDownList ID="ProviderList" runat="server"
    DataValueField="Id" DataTextField="Name" />
    <asp:Button ID="preferencesButton" runat="server" Text="Save preferences" OnCommand="preferencesButton_Click" />
    
    <h2>Tags partagés</h2>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <ajaxToolkit:ModalPopupExtender ID="MPE" runat="server"
                TargetControlID="TagLink" PopupControlID="TagSelectPanel"
                CancelControlID="CancelButton"
                BackgroundCssClass="modalBackground" />
            <table>
                <thead><tr><td><asp:Button runat="server" ID="TagLink" Text="Tags"></asp:Button></td>
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
                        <td>
                            <asp:Label runat="server" ToolTip='<%# WebPhoto.Services.PhotoLibrary.GetTagFullPath((string)Eval("TagId")) %>'>
                                <%# WebPhoto.Services.PhotoLibrary.GetTag((string)Eval("TagId")).Name %>
                            </asp:Label>
                        </td>
                        <asp:ListView runat="server" DataSource='<%# Eval("Authorizations") %>'>
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
                    <asp:TreeView ID="TagTreeView" runat="server" NodeIndent="10" ExpandDepth="FullyExpand"
                        ontreenodecheckchanged="TagTreeView_TreeNodeCheckChanged" ShowCheckBoxes="All">
                    </asp:TreeView>
                </div>
                <div style="margin:5px; text-align:right;">
                    <asp:Button runat="server" ID="OkButton" Text="Ok" OnClick="OkButton_OnClick"/>
                    <asp:Button runat="server" ID="CancelButton" Text="Annuler" />
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="OkButton" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:Button Text="Save" runat="server" OnClick="save_onclick" />
</asp:Content>
