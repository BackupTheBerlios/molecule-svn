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

﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebMusic._Default"
    MasterPageFile="~/Page.Master" EnableViewState="false" Culture="auto" UICulture="auto"
    Title="Music" %>

<%@ Import Namespace="WebMusic.Providers" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <link rel="stylesheet" type="text/css" href="style/webmusic.css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server">
        <Scripts>
            <asp:ScriptReference Path="scripts/default.js" />
            <asp:ScriptReference Path="scripts/soundmanager2.js" />
            <%--<asp:ScriptReference Path="scripts/soundmanager2-nodebug-jsmin.js" />--%>
            <asp:ScriptReference Path="scripts/sm2player.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <div id="fileNotFoundPanel">
        <img src="images/dialog-warning.png" />
        <asp:Label runat="server" ID="LabelSongError" Text="<%$ Resources:SongError %>" />
    </div>
    <div id="playerpanel">
        <div id="playerControls">
            <img id="previousButton" alt="Précédent" src="images/media-skip-backward.png" onclick="return previousButton_onclick()" />&nbsp;
            <img id="playButton" alt="Jouer" src="images/media-playback-start.png" onclick="return playButton_onclick()" />
            <img id="pauseButton" alt="Pause" src="images/media-playback-pause.png" style="display: none;"
                onclick="return pauseButton_onclick()" />
            <img id="nextButton" alt="Suivant" src="images/media-skip-forward.png" onclick="return nextButton_onclick()" />
            <br />
            <div id="currentSongInfoLabel">
            </div>
            <div id="currentSongPositionLabel">
            </div>
        </div>
        <div id="playlistcontainer">
            <h2>
                <asp:Label runat="server" ID="playlistLabel" Text="<%$ Resources:Playlist %>" /></h2>
            <select size="10" multiple="multiple" id="playListBox" ondblclick="playSelectedSong()"
                onkeydown="playlist_onkeydown(event)" /> 
            <input id="repeatAllCheckBox" type="checkbox"/><label for="repeatAllCheckBox"><asp:Literal ID="repeatAllCheckBoxLiteral" runat="server" Text="<%$ Resources:RepeatAll%>" /></label>
        </div>
    </div>
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="navigationPanel">
                <div id="artistscontainer">
                    <h2>
                        <asp:Label runat="server" ID="aristsLabel2" Text="<%$ Resources:Artists %>" /></h2>
                    <asp:ListBox ID="artistsListBox" runat="server" AutoPostBack="True" DataTextField="Name"
                        DataValueField="Id" OnInit="artistsListBox_Init" OnSelectedIndexChanged="artistsListBox_SelectedIndexChanged"
                        SelectionMode="Multiple" />
                </div>
                <div id="albumscontainer">
                    <h2>
                        <asp:Label runat="server" ID="labelAlbums" Text="<%$ Resources:Albums %>" /></h2>
                    <asp:ListBox ID="albumsListBox" runat="server" AutoPostBack="True" DataTextField="Name"
                        DataValueField="Id" OnInit="albumsListBox_Init" SelectionMode="Multiple" OnSelectedIndexChanged="albumsListBox_SelectedIndexChanged" />
                </div>
            </div>
            <asp:TextBox ID="searchTextBox" runat="server" Visible="false"></asp:TextBox>
            <asp:CheckBox ID="searchInArtistsCheckBox" runat="server" Text="<%$ Resources:Artists %>"
                Checked="True" Visible="false" />
            <asp:CheckBox ID="searchInAlbumsCheckBox" runat="server" Text="<%$ Resources:Albums %>"
                Checked="True" Visible="false" />
            <asp:CheckBox ID="searchInTitlesCheckBox" runat="server" Text="<%$ Resources:Titles %>"
                Checked="True" Visible="false" />
            <asp:Button ID="searchButton" runat="server" OnClick="searchButton_Click" Text="<%$ Resources:Search %>" Visible="false" />
            <div id="songscontainer">
                <table id="songsView">
                    <asp:Repeater ID="SongsView" runat="server">
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="titleLabel" Text="<%$ Resources:Title %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="artistLabel" Text="<%$ Resources:Artist %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="albumLabel" Text="<%$ Resources:Album %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="trackLabel" Text="<%$ Resources:Track %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="durationLabel" Text="<%$ Resources:Duration %>" />
                                    </td>
                                    <td>
                                        <img alt="Play all" src="images/media-playback-start-small.png" onclick="songsView_onclick('playAll')">
                                        <img alt="Enqueue all" src="images/list-add.png" onclick="songsView_onclick('enqueueAll')" />
                                        <img alt="Download all" src="images/document-save.png" onclick="songsView_onclick('downloadAll')"/>
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="songsListRow">
                                <td style="display: none"><%# Server.UrlEncode(((ISong)(Container.DataItem)).Id)%></td>
                                <td>
                                    <%# Server.HtmlEncode(((ISong)Container.DataItem).Title) %>
                                </td>
                                <td>
                                    <%# Server.HtmlEncode(((ISong)Container.DataItem).Artist.Name) %>
                                </td>
                                <td>
                                    <%# Server.HtmlEncode(((ISong)Container.DataItem).Album.Name) %>
                                </td>
                                <td>
                                    <%# Eval("AlbumTrack") %>
                                </td>
                                <td>
                                    <%# FormatDuration(((ISong)Container.DataItem).Duration) %>
                                </td>
                                <td>
                                    <img alt="Play" src="images/media-playback-start-small.png" onclick="songsViewItem_onclick(this,'play')">
                                    <img alt="Enqueue" src="images/list-add.png" onclick="songsViewItem_onclick(this,'enqueue')" />
                                    <a href="Download.aspx?songId=<%# Server.UrlEncode(((ISong)(Container.DataItem)).Id)%>"><img alt="Download all" src="images/document-save.png"/></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Label runat="server" ID="chargingLabel" Text="<%$ Resources:Loading %>" />
        </ProgressTemplate>
    </asp:UpdateProgress>

    <script type="text/javascript">
    init();
    </script>

</asp:Content>
