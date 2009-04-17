<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Molecule.WebSite.setup.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:CreateUserWizard ID="createUserWizard" runat="server" RequireEmail="False" Visible="true"
        OnCreatedUser="createUserWizard_CreatedUser" ContinueDestinationPageUrl="~/admin/Default.aspx"
        CancelDestinationPageUrl="~/">
        <WizardSteps>
            <asp:CreateUserWizardStep runat="server">
                <ContentTemplate>
                    <p style="text-align:right">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text='<%$Resources:Common,User %>' /> : 
                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserName" ErrorMessage='<%$Resources:molecule,UserRequired %>'
                         ToolTip='<%$Resources:molecule,UserRequired %>' ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                    </p><p style="text-align:right">
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text='<%$Resources:Common,Password %>' /> : 
                    <asp:TextBox ID="Password" runat="server" TextMode="SingleLine"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Password" ErrorMessage='<%$Resources:molecule,PasswordRequired %>'
                        ToolTip='<%$Resources:molecule,PasswordRequired %>' ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                    </p>
                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>       
                </ContentTemplate>
            </asp:CreateUserWizardStep>
<asp:CompleteWizardStep runat="server"></asp:CompleteWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>
    </div>
    </form>
</body>
</html>
