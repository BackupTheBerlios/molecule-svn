<%@ Page Language="C#" MasterPageFile="~/Views/Shared/PreferencesPage.Master"
Inherits="System.Web.Mvc.ViewPage<Molecule.MvcWebSite.atomes.admin.Data.PreferencesData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="preferencesContent" runat="server">
<%var form = Html.BeginForm("Save", "Admin", new { atome = "admin" }, FormMethod.Post); %>
<br />
   <h2><%=Resources.Common.Users%></h2>
   <table>
        <thead>
            <tr>
                <td><%= Resources.Common.User %></td>
                <td><%= Resources.molecule.LastLogin %></td>
            </tr>
        </thead>
        <tbody>
            <% foreach (var user in Model.Users)
               { %>
            <tr><td><%= user.UserName %></td><td><%= user.LastLoginDate %></td></tr>
            <%} %>
        </tbody>
    </table>
    <h2><%=Resources.molecule.NewUser %></h2>
    <label for="username"><%= Resources.Common.User %> : </label>
    <%= Html.TextBox("username") %>
    <label for="password"><%= Resources.Common.Password %> : </label>
    <%= Html.TextBox("password") %>
    <input type="submit" value="<%= Resources.molecule.NewUser %>" />
<%--    <asp:ListView ID="UserListView" runat="server" DataKeyNames="UserName"
        DataSourceID="usersObjectDataSource">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("UserName") %>' />
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("LastLoginDate") %>' />
                </td>
                 <td>
                    <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Delete" AlternateText="Delete"
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
                        <td><asp:Literal ID="Literal1" runat="server" Text='<%$Resources:Common,User%>' /></td>
                        <td><asp:Literal ID="Literal2" runat="server" Text='<%$Resources:molecule,LastLogin%>' /></td>
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
    
    <h2><asp:Literal ID="Literal3" runat="server" Text='<%$Resources:molecule,NewUser %>' /></h2>
    <asp:CreateUserWizard ID="createUserWizard" runat="server" RequireEmail="False" Visible="true"
        OnCreatedUser="createUserWizard_CreatedUser" LoginCreatedUser="False" ContinueDestinationPageUrl="~/admin/Default.aspx"
        CancelDestinationPageUrl="~/admin/Default.aspx">
        <WizardSteps>
            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
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
<% form.EndForm(); %>
</asp:Content>
