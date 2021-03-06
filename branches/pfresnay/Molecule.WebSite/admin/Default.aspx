﻿﻿<%--
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

<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Molecule.WebSite.Admin.Default" Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">

    <cc1:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="0" CssClass="customtabstyle" 
    xmlns:cc1="ajaxcontroltoolkit">
          <cc1:TabPanel runat="server" HeaderText="Users" ID="TabPanel1"> <ContentTemplate> <asp:GridView ID="usersGridView" runat="server" AllowPaging="True" 
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
    oncreateduser="createUserWizard_CreatedUser" LoginCreatedUser="False" 
        ContinueDestinationPageUrl="~/admin/Default.aspx"><WizardSteps><asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server"><ContentTemplate><table border="0"><tr><td align="center" colspan="2" class="style1">Create a new user account</td></tr><tr><td align="right"><asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User 
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
    </ContentTemplate> </cc1:TabPanel> </cc1:TabContainer>
    
    
    <br />
    
    
    <asp:PlaceHolder ID="atomesAdminPlaceHolder" runat="server" 
        oninit="atomesAdminPlaceHolder_Init"></asp:PlaceHolder>
    
    
</asp:Content>


<asp:Content ID="Content2" runat="server" contentplaceholderid="head">

    
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
    </style>

    
</asp:Content>