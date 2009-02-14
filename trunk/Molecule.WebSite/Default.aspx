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

<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="Molecule.WebSite.WebForm1" Title="Untitled Page" EnableTheming="true" EnableViewState="false" %>
<%@ Import Namespace="Molecule.Log" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

<div class="messagesPanel">
    <asp:Repeater ID="typesRepeater" runat="server" OnItemDatabound="typesRepeater_ItemDatabound">
        <ItemTemplate>
                <div class="messagesPanelTitle"><%# (string)Container.DataItem %></div>
                <asp:Repeater ID="messagesRepeater" runat="server">
                    <ItemTemplate>
                        <div  class="message">
						    <%# ((SemanticEvent)Container.DataItem).Title %>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
        </ItemTemplate>
    </asp:Repeater>
</div>

<%--<div>
<div class="messagesPanel">
<div class="messagesPanelTitle">Music</div>
<div  class="message">
  <TD> <TR> <asp:Image ID="Image1" runat="server" width="30" Height="30" ImageUrl="http://tbn1.google.com/images?q=tbn:X_AdNprtYlq7cM:http://images-eu.amazon.com/images/P/B000A82IKW.01.LZZZZZZZ.jpg"/> <td id="plouf" valign="middle" style="height:200px;"> New album Live!</asp:HyperLink></td></TR> <TR class="messageDate" >08/12/2008</TR></TD>
  </div>
  <div class="message">
    <TD> <TR><asp:Image ID="Image2" runat="server" width="30" Height="30" ImageUrl="http://tbn1.google.com/images?q=tbn:X_AdNprtYlq7cM:http://images-eu.amazon.com/images/P/B000A82IKW.01.LZZZZZZZ.jpg"/>New album Live! <asp:HyperLink ID="HyperLink2" runat="server" Text="link" NavigateUrl="www.google.com"></asp:HyperLink></TR> <TR class="messageDate" >08/12/2008</TR></TD>
 </div>
  <div class="message">
    <TD> <TR><asp:Image ID="Image3" runat="server" width="30" Height="30" ImageUrl="http://tbn1.google.com/images?q=tbn:X_AdNprtYlq7cM:http://images-eu.amazon.com/images/P/B000A82IKW.01.LZZZZZZZ.jpg"/>New album Live! <asp:HyperLink ID="HyperLink3" runat="server" Text="link" NavigateUrl="www.google.com"></asp:HyperLink></TR> <TR class="messageDate" >08/12/2008</TR></TD>
 </div>
</div>

<div class="messagesPanel">

<div class="messagesPanelTitle">Todo</div>
<div  class="message">
    <TD> <TR> <asp:HyperLink ID="HyperLink4" runat="server" Text="Eplucher des carottes" NavigateUrl="www.google.com"></asp:HyperLink></TR> <TR class="messageDate" >08/12/2008</TR></TD>
    </div>
    <TD> <TR> <div class="message">
    <asp:HyperLink ID="HyperLink5" runat="server" Text="Péter les deux jambes à duhamel" NavigateUrl="www.google.com"></asp:HyperLink></TR> <TR class="messageDate">08/12/2008</TR></TD>
    </div>
    <div class="message">
    <TD> <TR> <asp:HyperLink ID="HyperLink6" runat="server" Text="Faire à manger" NavigateUrl="www.google.com"></asp:HyperLink></TR> <TR class="messageDate">08/12/2008</TR></TD>
    </div>
</div>
<div style="clear: both;"></div>

<div class="messagesPanel">
<div class="messagesPanelTitle">Photos</div>
<div  class="message">
  <TD> <TR> <asp:Image ID="Image4" runat="server" width="30" Height="30" ImageUrl="http://farm2.static.flickr.com/1096/1392121285_694f4791df_s.jpg"/><asp:HyperLink ID="HyperLink7" runat="server" Text="New photo" NavigateUrl="www.google.com"></asp:HyperLink> Photo de lessive</TR> <TR class="messageDate" >08/12/2008</TR></TD>
  </div>
  <div class="message">
    <TD> <TR><asp:Image ID="Image5" runat="server" width="30" Height="30" ImageUrl="http://farm2.static.flickr.com/1272/1393011868_9b9aec6aef_s.jpg"/><asp:HyperLink ID="HyperLink8" runat="server" Text="New photo" NavigateUrl="www.google.com"></asp:HyperLink> Photo de boules</TR> <TR class="messageDate" >08/12/2008</TR></TD>
 </div>
  <div class="message">
    <TD> <TR><asp:Image ID="Image6" runat="server" width="30" Height="30" ImageUrl="http://farm2.static.flickr.com/1423/1393011070_c9a846303f_s.jpg"/><asp:HyperLink ID="HyperLink9" runat="server" Text="New photo" NavigateUrl="www.google.com">Cacolac</asp:HyperLink></TR> <TR class="messageDate" >08/12/2008</TR></TD>
 </div>
</div>

<div class="spacer"> </div>
</div>--%>
</asp:Content>
