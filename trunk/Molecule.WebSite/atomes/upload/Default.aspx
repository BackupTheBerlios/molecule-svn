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

﻿<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="Upload.Default"
    MasterPageFile="~/Page.Master" EnableViewState="false" EnableSessionState="false" Culture="auto" UICulture="auto"
    Title="Upload file" %>
<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <link href="style/layout.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server">
        <Scripts>
            <asp:ScriptReference Path="scripts/default.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    
    <Upload:MultiFile id="multiUploadFile" runat="server" />
    <Upload:ProgressBar id="uploadProgressBar" runat="server" inline="false"  width="500px" height="100px" EnableTheming="true"/> 
    <asp:Button id="submitButton" runat="server" Text="Submit" onclick="submitButton_Click"  />
</asp:Content>

