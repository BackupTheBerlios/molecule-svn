﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/PreferencesPage.Master" Inherits="System.Web.Mvc.ViewPage<Molecule.MvcWebSite.atomes.admin.Data.PreferencesData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="preferencesContent" runat="server">
    <%var form = Html.BeginForm("Save", "Preferences", new { atome = "admin" }, FormMethod.Post); %>
    <br />
    <h2>
        <%=Resources.Common.Users%></h2>
    <table>
        <thead>
            <tr>
                <td><%= Resources.Common.User %></td>
                <td><%= Resources.molecule.LastLogin %></td>
                <td></td>
            </tr>
        </thead>
        <tbody>
            <% foreach (var user in Model.DeletableUsers) { %>
            <tr>
                <td><%= user.Name %></td>
                <td><%= user.LastLoginDate %></td>
                <td><a href="<%= Url.Action("Delete", "Preferences", new { id = user.Name }) %>">
                    <img alt="Delete" src="/App_Themes/bloup/images/list-remove.png" /></a> </td>
            </tr>
            <%} %>
        </tbody>
    </table>
    <a href="<%= Url.Action("Create", "Preferences", new { atome = "admin" }) %>">
        <%= Resources.molecule.NewUser%></a>
    <h2>
        <%= Resources.Common.Authorizations %></h2>
    <table>
        <thead>
            <tr>
                <td>Atome</td>
                <% foreach (var user in Model.AuthorizableUsers) { %>
                <td><%= user %></td>
                <%} %>
            </tr>
        </thead>
        <tbody>
            <% foreach (var auAuth in Model.Authorizations) { %>
            <tr>
                <td>
                    <%= auAuth.Atome%></td>
                <% foreach (var auth in auAuth.Authorizations) { %>
                <td><input <%=auth.Authorized?"checked=\"checked\"":"" %> value="<%= auth.Value %>"
                        type="checkbox" name="authorizations" /></td>
                <%}
               } %>
            </tr>
        </tbody>
    </table>
    <%--    
    <h2><asp:Literal ID="Literal4" runat="server" Text='<%$Resources:Common,Authorizations %>' /></h2>

    <table>
        <thead><tr><td>Atome</td>
            <asp:Repeater ID="AuthHeaderRepeater"  runat="server">
                <ItemTemplate>
                    <td><asp:Label ID="Label4" runat="server"><%# (string)Container.DataItem %></asp:Label></td>
                </ItemTemplate>
            </asp:Repeater>
            <td><asp:Literal ID="Literal5" runat="server" Text='<%$Resources:Common,Anonymous %>' /></td>
            </tr>
        </thead>
        <asp:ListView ID="AuthListView" runat="server">
        <ItemTemplate>
            <tr>
                <td><asp:Label ID="Label5" runat="server"><%# Eval("Atome") %></asp:Label></td>
                <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("Authorizations") %>'>
                    <ItemTemplate>
                        <td>
                            <asp:CheckBox ID="CheckBox1" runat="server"
                            ToolTip='<%# Eval("Atome") +","+ Eval("User") %>' Checked='<%# Eval("Authorized") %>'
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
    <asp:Button ID="Button1" Text="<%$ Resources:Common,Save %>" runat="server" OnClick="save_onclick" />

    <h2><asp:Label ID="Label6" runat="server" Text="<%$Resources:molecule,Theme%>" /></h2>
    <p>
    Titre : <asp:TextBox runat="server" ID="titleTextBox"></asp:TextBox>
    </p>
    <p>
        <asp:CheckBox ID="LogoCheckBox" runat="server" Text="<%$Resources:molecule,DisplayLogo%>" 
            oncheckedchanged="LogoCheckBox_CheckedChanged" />
    </p>
    <p>
        <asp:ListView ID="CssVariableList" runat="server" DataSourceID="CssVariablesSource" DataKeyNames="Key">
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("Key") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="tb" runat="server" Text='<%# Bind("Value") %>' OnTextChanged="Variable_TextChanged" />
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# ((string)Eval("Key")).Contains("Color") %>'>
                            <asp:ImageButton id="cpb" runat="server" 
                              ImageUrl='<%# "~/App_Themes/"+ Theme +"/images/colorpicker.png" %>' />
                            <cdt:colorpickerextender runat="server"
                                 targetcontrolid="tb" samplecontrolid="cpb" popupbuttonid="cpb"
                                 SelectedColor='<%# ((string)Eval("Value")).Replace("#","") %>' />
                        </asp:PlaceHolder>
                    </td>
                </tr>
            </ItemTemplate>
            <LayoutTemplate>
                <table>
                    <thead>
                        <tr>
                            <td>Key</td>
                            <td>Value</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </tbody>
                </table>
            </LayoutTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="CssVariablesSource" runat="server" 
            DataObjectTypeName="Molecule.WebSite.Services.CssVariableInfo" 
            SelectMethod="GetCssVariables" 
            TypeName="Molecule.WebSite.Services.AdminService" 
            UpdateMethod="UpdateCssVariable"></asp:ObjectDataSource>
        <asp:Button ID="ButtonReset" runat="server" Text="Réinitialiser" 
            onclick="ButtonReset_Click" />
        <asp:Button ID="Button2" Text="<%$ Resources:Common,Save %>" runat="server" OnClick="save_onclick" />
    </p>--%>
    <br />
    <input type="submit" value="<%= Resources.Common.Save %>" />
    <% form.EndForm(); %>
</asp:Content>
