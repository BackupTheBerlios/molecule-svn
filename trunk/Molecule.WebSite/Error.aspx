<%--
 Error.aspx

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

<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Molecule.WebSite.Error" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2><asp:Label runat="server" ID="LabelAtomeName" /></h2>
<p>
<asp:Label runat="server" ID="LabelError" />
</p>
    <p>
        <asp:LoginView ID="LoginView1" runat="server">
            <RoleGroups>
                <asp:RoleGroup Roles="admin">
                    <ContentTemplate>

                        <asp:HyperLink id="linkDetails" runat="server">
                            <asp:Label ID="labelDetails" runat="server"/>
                        </asp:HyperLink>
                        
                        <asp:Panel runat="server" ID="errorDetails">
                            <%= Server.HtmlEncode(this.exceptionDetails.Replace("\n","<br/>")) %></asp:Panel>
                        
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server"
                            TargetControlID="errorDetails" CollapsedSize="0"
                            Collapsed="True"
                            TextLabelID="labelDetails"
                            CollapsedText="Show Details..."
                            ExpandedText="Hide Details" 
                            ExpandControlID="linkDetails"
                            CollapseControlID="linkDetails"
                            ExpandDirection="Vertical" SuppressPostBack="true" />
                    </ContentTemplate>
                </asp:RoleGroup>
            </RoleGroups>
        </asp:LoginView>
</p>

</asp:Content>
