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

<%@ Page Language="C#" Inherits="Molecule.WebSite.Admin.CreateUser" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
	<title>CreateUser</title>
</head>
<body>
	<form id="form1" runat="server">

<asp:CreateUserWizard ID="CreateUserWizard1" runat="server" AnswerLabelText="Réponse de sécurité:" AnswerRequiredErrorMessage="Une réponse de sécurité est obligatoire." CancelButtonText="Annuler" CompleteSuccessText="Votre compte a été créé avec succès !" ConfirmPasswordCompareErrorMessage="Le mot de passe et la confirmation doivent correspondre" ConfirmPasswordLabelText="Confirmez le mot de passe:" ConfirmPasswordRequiredErrorMessage="Le mot de passe de confirmation est obligatoire." ContinueButtonText="Continuer" CreateUserButtonText="Créer utilisateur" DuplicateEmailErrorMessage="L'adresse e-mail que vous avez saisie est déjà utilisée ! Saisissez-en une autre." DuplicateUserNameErrorMessage="Saisissez un nom d'utilisateur différent." EmailRegularExpression="^.+@[^\.].*\.[a-z]{2,}$" EmailRegularExpressionErrorMessage="Saisissez une adresse e-mail différente." EmailRequiredErrorMessage="Un E-mail valide est obligatoire." InvalidAnswerErrorMessage="Saisissez une réponse différente" InvalidEmailErrorMessage="Saisissez une adresse e-mail valide." InvalidPasswordErrorMessage="Longueur du mot de passe minimum: {0}. Caractères non alpha-numériques requis: {1}." InvalidQuestionErrorMessage="Entrez une question de sécurité différente." PasswordLabelText="Mot de passe:" PasswordRegularExpressionErrorMessage="Saisissez un mot de passe différent." PasswordRequiredErrorMessage="Un mot de passe est obligatoire." QuestionLabelText="Question de sécurité:" QuestionRequiredErrorMessage="Une question de sécurité est obligatoire." StartNextButtonText="Suivant" UnknownErrorMessage="Votre compte n'a pas été créé. Essayez à nouveau." UserNameLabelText="Nom d'utilisateur:" UserNameRequiredErrorMessage="Un nom d'utilisateur est obligatoire." OnCreatedUser="CreateUserWizard1_CreatedUser">
		<WizardSteps>
			<asp:CreateUserWizardStep runat="server" Title="Cr&#233;ez un nouveau compte utilisateur">
			</asp:CreateUserWizardStep>
			<asp:CompleteWizardStep runat="server" Title="Nouveau compte">
			</asp:CompleteWizardStep>
		</WizardSteps>
        <StepNavigationTemplate>
            <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                Text="Previous" />
            <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="Next" />
        </StepNavigationTemplate>
	</asp:CreateUserWizard>


	</form>
</body>
</html>