﻿<%--
 AdminControl.ascx

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

﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminControl.ascx.cs" Inherits="WebMusic.AdminControl" EnableViewState="true" %>
Récupérer la musique depuis :
    <asp:DropDownList ID="playerDropDownList" runat="server"
    DataValueField="Id" DataTextField="Name" />

<br/>
Last.Fm :<br />
    <asp:CheckBox ID="lastfmEnabledCheckBox" runat="server" AutoPostBack="True" 
        oncheckedchanged="lastfmEnabledCheckBox_CheckedChanged" Text="Enable Lastfm" />
<br/>
User : <asp:TextBox ID="lastFmUsername"  runat="server" Height="22px"></asp:TextBox>
<br/>
Password :<asp:TextBox ID="lastFmUserPassword"  TextMode="password" runat="server"></asp:TextBox>
<br/>

<asp:Button ID="preferencesButton" runat="server" Text="Save preferences" OnCommand="preferencesButton_Click" >
</asp:Button>
