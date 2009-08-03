﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
Inherits="System.Web.Mvc.ViewPage<Molecule.MvcWebSite.atomes.music.Data.IndexData>" %>

<%@ Import Namespace="WebMusic.Providers" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <link href="/atomes/music/style/layout.css" rel="stylesheet" type="text/css" />
    <style type="text/css" runat="server">
        #playlistTable tr:hover .listRemove
        {
            background: url("/App_Themes/bloup/images/list-remove.png") no-repeat;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div id="fileNotFoundPanel" class="informationPanel">
        <img alt="Warning" src="/App_Themes/bloup/images/dialog-warning.png" />
        <%= Resources.webmusic.SongError %>
    </div>
    <div id="fileAddedToPlaylistPanel" class="informationPanel">
        <%=Resources.webmusic.SongsAdded %>
    </div>
    <div id="playerpanel">
        <div id="playerControls">
            <img id="previousButton" alt="Précédent" src="/App_Themes/bloup/images/media-skip-backward.png"
                onclick="return previousButton_onclick()" />&nbsp;
            <img id="playButton" alt="Jouer" src="/App_Themes/bloup/images/media-playback-start.png"
                onclick="return playButton_onclick()" />
            <img id="pauseButton" alt="Pause" src="/App_Themes/bloup/images/media-playback-pause.png"
                style="display: none;" onclick="return pauseButton_onclick()" />
            <img id="nextButton" alt="Suivant" src="/App_Themes/bloup/images/media-skip-forward.png"
                onclick="return nextButton_onclick()" />
            <img alt="Diminuer volume" src="/App_Themes/bloup/images/audio-volume-low.png" onclick="return volumeDownButton_onclick()" />
            <div id="currentVolumeLabel">
            </div>
            <img alt="Augmenter volume" src="/App_Themes/bloup/images/audio-volume-high.png"
                onclick="return volumeUpButton_onclick()" />
            <div id="songInformationPanel">
                <div id="songsLabelPanel">
                    <div id="currentSongTitleLabel"></div>
                    <div id="currentSongArtistLabel"></div>
                    <div id="currentSongPositionLabel" style="font-family: Courier New"></div>
                </div>
                <div id="coverArtPanel">
                    <img alt="" id="coverArtImage" class="thinBorder" src="" style="display: none;" />
                </div>
            </div>
        </div>
        <div id="playlistcontainer">
            <h2><%= Resources.webmusic.Playlist %></h2>
            <div id="playlistPanel" style="overflow: auto; height: 150px;" class="thinBox">
                <table id="playlistTable" class="itemList hoverTable" onkeydown="playlist_onkeydown(event)"></table>
            </div>
            <input id="repeatAllCheckBox" type="checkbox" />
            <label for="repeatAllCheckBox"><%= Resources.webmusic.RepeatAll%></label>
        </div>
    </div>
    <div id="navigationPanel">
        <div id="artistscontainer">
            <h2><%= Resources.webmusic.Artists %></h2>
            <div id="artistList" style="overflow: auto; height: 150px;" class="thinBox">
                <%--<asp:DataList ID="arl" runat="server" DataSourceID="ArtistDataSource" DataKeyField="Id"
                    OnSelectedIndexChanged="ArtistList_SelectedIndexChanged" CssClass="itemList hoverTable">
                    <ItemTemplate>
                        <asp:LinkButton ID="aib" runat="server" Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("Id") %>'
                            CommandName="Select" />
                    </ItemTemplate>
                </asp:DataList>
                <asp:ObjectDataSource ID="ArtistDataSource" runat="server" SelectMethod="GetArtists"
                    TypeName="WebMusic.Services.MusicLibrary"></asp:ObjectDataSource>--%>
            </div>
        </div>
        <div id="albumscontainer">
            <h2><%= Resources.webmusic.Albums %></h2>
            <div id="albumList" style="overflow: auto; height: 150px;" class="thinBox">
                <%--<asp:UpdatePanel ID="albumUpdatePanel" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <asp:DataList ID="all" runat="server" DataSourceID="AlbumDataSource" DataKeyField="Id"
                            OnSelectedIndexChanged="AlbumList_SelectedIndexChanged" CssClass="itemList hoverTable">
                            <ItemTemplate>
                                <asp:LinkButton ID="aib" runat="server" Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("Id") %>'
                                    CommandName="Select" />
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:ObjectDataSource ID="AlbumDataSource" runat="server" SelectMethod="GetAlbumsByArtist"
                            TypeName="WebMusic.Services.MusicLibrary">
                            <SelectParameters>
                                <asp:SessionParameter Name="artist" SessionField="music.currentArtist" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="arl" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </div>
        </div>
    </div>
    <div id="songscontainer">
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <table id="songsView" class="hoverTable">
                    <asp:Repeater ID="sv" runat="server" DataSourceID="SongDataSource">
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="til" Text="<%= Resources.webmusic.Title %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="arl" Text="<%= Resources.webmusic.Artist %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="all" Text="<%$ Resources:webmusic,Album %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="trl" Text="<%$ Resources:webmusic,Track %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="dl" Text="<%$ Resources:webmusic,Duration %>" />
                                    </td>
                                    <td style="text-align: right">
                                        <img alt="" src="/App_Themes/<%= Theme %>/images/media-playback-start-small.png"
                                            onclick="songsView_onclick('playAll')" />
                                        <img alt="" src="/App_Themes/<%= Theme %>/images/list-add.png" onclick="songsView_onclick('enqueueAll')" />
                                        <img alt="" src="/App_Themes/<%= Theme %>/images/document-save.png" onclick="songsView_onclick('downloadAll')" />
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="display: none">
                                    <%# Server.UrlEncode(((ISong)(Container.DataItem)).Id)%>
                                </td>
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
                                <td style="text-align: right">
                                    <img alt="" src="/App_Themes/<%= Theme %>/images/media-playback-start-small.png"
                                        onclick="songsViewItem_onclick(this,'play')" />
                                    <img alt="" src="/App_Themes/<%= Theme %>/images/list-add.png" onclick="songsViewItem_onclick(this,'enqueue')" />
                                    <a href="Download.aspx?songId=<%# Server.UrlEncode(((ISong)(Container.DataItem)).Id)%>">
                                        <img alt="" src="/App_Themes/<%= Theme %>/images/document-save.png" /></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:ObjectDataSource ID="SongDataSource" runat="server" SelectMethod="GetSongByArtistAndAlbum"
                        TypeName="WebMusic.Services.MusicLibrary">
                        <SelectParameters>
                            <asp:SessionParameter Name="album" SessionField="music.currentAlbum" Type="String" />
                            <asp:SessionParameter Name="artist" SessionField="music.currentArtist" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="all" EventName="ItemCommand" />
            </Triggers>
        </asp:UpdatePanel>--%>
    </div>
    
    <script type="text/javascript" src="/atomes/music/scripts/default.js" />
    <script type="text/javascript" src="/atomes/music/scripts/soundmanager2.js" />
    <script type="text/javascript" src="/atomes/music/scripts/sm2player.js" />
    
    <script type="text/javascript">
        init();
    </script>
</asp:Content>
