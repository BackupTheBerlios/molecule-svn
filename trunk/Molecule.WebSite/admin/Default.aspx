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
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="preferencesContent" runat="server">

<h2><asp:Label ID="Label1" runat="server" Text="<%$Resources:Users%>" /></h2>
    <asp:GridView ID="usersGridView" runat="server" AllowPaging="True" 
        AutoGenerateColumns="False" DataSourceID="usersObjectDataSource" 
        DataKeyNames="UserName"><Columns><asp:BoundField DataField="UserName" HeaderText="User Name" ReadOnly="True" 
                SortExpression="UserName" />
            <asp:CheckBoxField DataField="IsOnline" HeaderText="Is Online ?" ReadOnly="True" 
                SortExpression="IsOnline" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
    
      <asp:ObjectDataSource ID="usersObjectDataSource" runat="server" 
        DeleteMethod="DeleteUser" SelectMethod="GetAllUsers" 
        TypeName="System.Web.Security.Membership"><DeleteParameters><asp:Parameter Name="username" Type="String" />
        </DeleteParameters>
    </asp:ObjectDataSource>
      <asp:Button ID="createUserButton" runat="server" Text="Create User" 
        onclick="createUserButton_Click" />
  
      <asp:CreateUserWizard ID="createUserWizard" runat="server" 
                            RequireEmail="False" Visible="False" 
                            oncreateduser="createUserWizard_CreatedUser" 
                            LoginCreatedUser="False" ContinueDestinationPageUrl="~/admin/Default.aspx" 
                            CancelDestinationPageUrl="~/admin/Default.aspx" 
                            DisplayCancelButton="True"><WizardSteps><asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server"><ContentTemplate><table border="0"><tr><td align="center" colspan="2">Create a new user account</td></tr><tr><td align="right"><asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User 
                                Name:</asp:Label></td><td><asp:TextBox ID="UserName" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                    ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                    ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></td></tr><tr><td align="right"><asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label></td><td><asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                    ControlToValidate="Password" ErrorMessage="Password is required." 
                                    ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></td></tr><tr><td align="right"><asp:Label ID="ConfirmPasswordLabel" runat="server" 
                                    AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label></td><td><asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" 
                                    ControlToValidate="ConfirmPassword" 
                                    ErrorMessage="Confirm Password is required." 
                                    ToolTip="Confirm Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></td></tr><tr><td align="center" colspan="2"><asp:CompareValidator ID="PasswordCompare" runat="server" 
                                    ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                                    Display="Dynamic" 
                                    ErrorMessage="The Password and Confirmation Password must match." 
                                    ValidationGroup="CreateUserWizard1"></asp:CompareValidator></td></tr><tr><td align="center" colspan="2" style="color:Red;"><asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal></td></tr></table>
                </ContentTemplate>
            </asp:CreateUserWizardStep><asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server"></asp:CompleteWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>  

    <h2><asp:Label runat="server" Text="<%$Resources:Theme%>" /></h2>
    <p>
        <asp:ListView ID="ListView1" runat="server" DataSourceID="CssVariablesSource" 
            DataKeyNames="Key">
            <ItemTemplate>
                <tr style="">
                    <td>
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    </td>
                    <td>
                        <asp:Label ID="KeyLabel" runat="server" Text='<%# Eval("Key") %>' />
                    </td>
                    <td>
                        <asp:Label ID="ValueLabel" runat="server" Text='<%# Eval("Value") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <table runat="server" style="">
                    <tr>
                        <td>
                            No data was returned.</td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table runat="server">
                    <tr runat="server">
                        <td runat="server">
                            <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr runat="server" style="">
                                    <th runat="server">
                                    </th>
                                    <th runat="server">
                                        Key</th>
                                    <th runat="server">
                                        Value</th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" style="">
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <EditItemTemplate>
                <tr style="">
                    <td>
                        <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                            Text="Update" />
                        <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                            Text="Cancel" />
                    </td>
                    <td>
                        <asp:Label ID="KeyLabel" runat="server" Text='<%# Bind("Key") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="ValueTextBox" runat="server" Text='<%# Bind("Value") %>' />
                    </td>
                </tr>
            </EditItemTemplate>
            <SelectedItemTemplate>
                <tr style="">
                    <td>
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    </td>
                    <td>
                        <asp:Label ID="KeyLabel" runat="server" Text='<%# Eval("Key") %>' />
                    </td>
                    <td>
                        <asp:Label ID="ValueLabel" runat="server" Text='<%# Eval("Value") %>' />
                    </td>
                </tr>
            </SelectedItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="CssVariablesSource" runat="server" 
            DataObjectTypeName="Molecule.WebSite.Services.CssVariableInfo" 
            SelectMethod="GetCssVariables" 
            TypeName="Molecule.WebSite.Services.AdminService" 
            UpdateMethod="UpdateCssVariable"></asp:ObjectDataSource>
        <asp:Button ID="ButtonReset" runat="server" Text="Réinitialiser" 
            onclick="ButtonReset_Click" />    
    </p>
    
</asp:Content>