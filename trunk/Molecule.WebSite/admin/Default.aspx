<%--
 Default.aspx

 Copyright (c) 2009 Pascal Fresnay (dev.molecule@free.fr) - Mickael Renault (dev.molecule@free.fr) 

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
 --%>

<%@ Page Language="C#" MasterPageFile="~/PreferencesPage.Master" AutoEventWireup="true" EnableViewState="true"
 CodeBehind="Default.aspx.cs" Inherits="Molecule.WebSite.Admin.Default" Title="Général" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagPrefix="cdt" Assembly="CDT.ColorPickerExtender" Namespace="CDT" %>
<%@ Import Namespace="System.Web.Security" %>
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.ajax__cp_container
{
	z-index:3;
}
.ajax__cp_container table td
{
	padding:0px;
}
</style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="preferencesContent" runat="server">

<h2><asp:Label runat="server" Text="<%$Resources:Common,Users%>" /></h2>
    <asp:ListView ID="UserListView" runat="server" DataKeyNames="UserName"
        DataSourceID="usersObjectDataSource">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label runat="server" Text='<%# Eval("UserName") %>' />
                </td>
                <td>
                    <asp:Label runat="server" Text='<%# Eval("LastLoginDate") %>' />
                </td>
                 <td>
                    <asp:ImageButton runat="server" CommandName="Delete" AlternateText="Delete"
                        ImageUrl='<%# "~/App_Themes/"+ Theme + "/images/list-remove.png"  %>'
                        Visible='<%# !Roles.IsUserInRole((string)Eval("UserName"),
                            Molecule.SQLiteProvidersHelper.AdminRoleName) %>'/>
                </td>
            </tr>
        </ItemTemplate>
        <InsertItemTemplate>
                  
        </InsertItemTemplate>
        <LayoutTemplate>
            <table>
                <thead>
                    <tr>
                        <td><asp:Literal runat="server" Text='<%$Resources:Common,User%>' /></td>
                        <td><asp:Literal runat="server" Text='<%$Resources:molecule,LastLogin%>' /></td>
                    </tr>
                </thead>
                <tbody>
                    <tr ID="itemPlaceholder" runat="server">
                    </tr>
                </tbody>
            </table>
        </LayoutTemplate>
    </asp:ListView>
    
      <asp:ObjectDataSource ID="usersObjectDataSource" runat="server" 
        DeleteMethod="DeleteUser" SelectMethod="GetAllUsers" 
        TypeName="System.Web.Security.Membership"><DeleteParameters><asp:Parameter Name="username" Type="String" />
        </DeleteParameters>
    </asp:ObjectDataSource>
    
    <h2><asp:Literal runat="server" Text='<%$Resources:molecule,NewUser %>' /></h2>
    <asp:CreateUserWizard ID="createUserWizard" runat="server" RequireEmail="False" Visible="true"
        OnCreatedUser="createUserWizard_CreatedUser" LoginCreatedUser="False" ContinueDestinationPageUrl="~/admin/Default.aspx"
        CancelDestinationPageUrl="~/admin/Default.aspx">
        <WizardSteps>
            <asp:CreateUserWizardStep runat="server">
                <ContentTemplate>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="createUserWizard" />
                    <p style="text-align:left">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text='<%$Resources:Common,User %>' /> : 
                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserName" ErrorMessage='<%$Resources:molecule,UserRequired %>'
                         ToolTip='<%$Resources:molecule,UserRequired %>' ValidationGroup="createUserWizard">*</asp:RequiredFieldValidator>
                    </p><p style="text-align:left">
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text='<%$Resources:Common,Password %>' /> : 
                    <asp:TextBox ID="Password" runat="server" TextMode="SingleLine"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Password" ErrorMessage='<%$Resources:molecule,PasswordRequired %>'
                        ToolTip='<%$Resources:molecule,PasswordRequired %>' ValidationGroup="createUserWizard">*</asp:RequiredFieldValidator>
                    </p>     
                </ContentTemplate>
            </asp:CreateUserWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>

    <h2><asp:Literal runat="server" Text='<%$Resources:Common,Authorizations %>' /></h2>

    <table>
        <thead><tr><td>Atome</td>
            <asp:Repeater ID="AuthHeaderRepeater"  runat="server">
                <ItemTemplate>
                    <td><asp:Label runat="server"><%# (string)Container.DataItem %></asp:Label></td>
                </ItemTemplate>
            </asp:Repeater>
            <td><asp:Literal ID="Literal1" runat="server" Text='<%$Resources:Common,Anonymous %>' /></td>
            </tr>
        </thead>
        <asp:ListView ID="AuthListView" runat="server">
        <ItemTemplate>
            <tr>
                <td><asp:Label runat="server"><%# Eval("Atome") %></asp:Label></td>
                <asp:ListView runat="server" DataSource='<%# Eval("Authorizations") %>'>
                    <ItemTemplate>
                        <td>
                            <asp:CheckBox runat="server"
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
    <asp:Button Text="<%$ Resources:Common,Save %>" runat="server" OnClick="save_onclick" />

    <h2><asp:Label runat="server" Text="<%$Resources:molecule,Theme%>" /></h2>
    <p>
    Titre : <asp:TextBox runat="server" ID="titleTextBox"></asp:TextBox>
    </p>
    <p>
        <asp:ListView ID="CssVariableList" runat="server" DataSourceID="CssVariablesSource" DataKeyNames="Key">
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label runat="server" Text='<%# Eval("Key") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="tb" runat="server" Text='<%# Bind("Value") %>' OnTextChanged="Variable_TextChanged" />
                        <asp:PlaceHolder runat="server" Visible='<%# ((string)Eval("Key")).Contains("Color") %>'>
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
        <asp:Button ID="Button1" Text="<%$ Resources:Common,Save %>" runat="server" OnClick="save_onclick" />
    </p>
    
</asp:Content>