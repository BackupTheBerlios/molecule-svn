<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
Inherits="System.Web.Mvc.ViewPage<Molecule.MvcWebSite.atomes.music.Data.IndexData>" %>
<%@ Import Namespace="Molecule.MvcWebSite.atomes.music.Controllers" %>
<%@ Import Namespace="WebMusic.Providers" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <link href="/atomes/music/style/layout.css" rel="stylesheet" type="text/css" />
        <style id="Style1" type="text/css" runat="server">
        #playlistTable tr:hover .listRemove
        {
            background: url("<%= Url.Theme("images/list-remove.png")%>" no-repeat;
        }
    </style>
    <script type="text/javascript">
    <%= Url.JQueryProxyScript<PlayerController>("library") %>
    </script>
    <script type="text/javascript" src="../../../../Scripts/jquery-1.3.2.js"></script>
    <script type="text/javascript" src="/atomes/music/scripts/default.js"></script>
    <script type="text/javascript" src="/atomes/music/scripts/sm2player.js"></script>
    <script type="text/javascript" src="/atomes/music/scripts/soundmanager2-nodebug-jsmin.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div id="fileNotFoundPanel" class="informationPanel">
        <img alt="Warning" src="<%= Url.Theme("images/dialog-warning.png")%>" />
        <%= Resources.webmusic.SongError %>
    </div>
    <div id="fileAddedToPlaylistPanel" class="informationPanel">
        <%=Resources.webmusic.SongsAdded %>
    </div>
    
    <div id="playerControls">
        <img id="previousButton" alt="Précédent" src="<%= Url.Theme("images/media-skip-backward.png")%>" />&nbsp;
        <img id="playButton" alt="Jouer" src="<%= Url.Theme("images/media-playback-start.png")%>" />
        <img id="pauseButton" alt="Pause" src="<%= Url.Theme("images/media-playback-pause.png")%>" />
        <img id="nextButton" alt="Suivant" src="<%= Url.Theme("images/media-skip-forward.png")%>" />
        <img id="volumeDownButton" alt="Diminuer volume" src="<%= Url.Theme("images/audio-volume-low.png")%>" />
        <div id="currentVolumeLabel"></div>
        <img id="volumeUpButton" alt="Augmenter volume" src="<%= Url.Theme("images/audio-volume-high.png")%>" />
        <div id="currentSongTitleLabel"></div>
        <div id="currentSongArtistLabel"></div>
        <div id="currentSongPositionLabel"></div>
        <img alt="" id="coverArtImage" class="thinBorder" src="" />
    </div>
    
    
    <div id="playlistcontainer">
        <h2><%= Resources.webmusic.Playlist %></h2>
        <div id="playlistPanel" class="thinBox">
            <table id="playlistTable" class="itemList hoverTable"></table>
        </div>
        <input id="repeatAllCheckBox" type="checkbox" />
        <label for="repeatAllCheckBox"><%= Resources.webmusic.RepeatAll%></label>
    </div>

    <div id="artistscontainer">
        <h2><%= Resources.webmusic.Artists %></h2>
        <div class="navigationList thinBox">
            <span id="artistsWaiting">Waiting...</span>
            <ul id="artistList">
            </ul>
        </div>
    </div>
    
    <div id="albumscontainer">
        <h2><%= Resources.webmusic.Albums %></h2>
        <div class="navigationList thinBox">
            <span id="albumsWaiting">Waiting...</span>
            <ul id="albumList">
            </ul>
        </div>
    </div>

     <input id="search" class="searchTextBox" type="text" />
     
    <div id="songscontainer">
        <table id="songsView" class="hoverTable">
            <thead>
                <tr>
                    <td><%= Resources.webmusic.Title %></td>
                    <td><%= Resources.webmusic.Artist %></td>
                    <td><%= Resources.webmusic.Album %></td>
                    <td><%= Resources.webmusic.Track %></td>
                    <td><%= Resources.webmusic.Duration %></td>
                    <td style="text-align: right">
                        <img id="playAllButton" alt="" src="<%= Url.Theme("images/media-playback-start-small.png") %>" />
                        <img id="enqueueAllButton" alt="" src="<%= Url.Theme("images/list-add.png") %>" />
                        <img id="downloadAllButton" alt="" src="<%= Url.Theme("images/document-save.png") %>" />
                    </td>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
</asp:Content>