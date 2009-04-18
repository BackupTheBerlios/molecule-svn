<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Setup.aspx.cs" Inherits="Molecule.WebSite.admin.Setup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1><asp:Literal ID="Literal1" runat="server" Text='<%$Resources:molecule,SetupWelcome %>' /></h1>
    <h2><asp:Literal runat="server" Text='<%$Resources:molecule,Setup %>' /></h2>
    <asp:CreateUserWizard ID="createUserWizard" runat="server" RequireEmail="False" Visible="true"
        OnCreatedUser="createUserWizard_CreatedUser" FinishDestinationPageUrl="~/admin/Default.aspx"
        CancelDestinationPageUrl="~/">
        <WizardSteps>
            <asp:CreateUserWizardStep runat="server">
                <ContentTemplate>
                <div style="width:500px; margin:20px">
                    <p style="text-align:left"><asp:Literal runat="server" Text='<%$Resources:molecule,SetupInstructions %>' /></p>
                    <p style="text-align:left">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text='<%$Resources:Common,User %>' /> : 
                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" ErrorMessage='<%$Resources:molecule,UserRequired %>'
                         ToolTip='<%$Resources:molecule,UserRequired %>' ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                    </p><p style="text-align:left">
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text='<%$Resources:Common,Password %>' /> : 
                    <asp:TextBox ID="Password" runat="server" TextMode="SingleLine"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" ErrorMessage='<%$Resources:molecule,PasswordRequired %>'
                        ToolTip='<%$Resources:molecule,PasswordRequired %>' ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                    </p>
                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                    </div>   
                </ContentTemplate>
            </asp:CreateUserWizardStep>
<asp:CompleteWizardStep runat="server"></asp:CompleteWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>
    </div>
    </form>
</body>
</html>
