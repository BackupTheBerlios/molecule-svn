<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
Inherits="System.Web.Mvc.ViewPage<Molecule.MvcWebSite.atomes.music.Data.IndexData>" %>

<%@ Import Namespace="Molecule.MvcWebSite.atomes.music.Controllers" %>

<%@ Import Namespace="WebMusic.Providers" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <link href="/atomes/music/style/layout.css" rel="stylesheet" type="text/css" />
        <style id="Style1" type="text/css" runat="server">
        #playlistTable tr:hover .listRemove
        {
            background: url("/App_Themes/bloup/images/list-remove.png") no-repeat;
        }
    </style>
    <script type="text/javascript">
    <%= Url.JQueryProxyScript<PlayerController>("library") %>
    </script>
    <script type="text/javascript" src="../../../../Scripts/jquery-1.3.2.js"></script>
    <script type="text/javascript" src="/atomes/music/scripts/filter.js"></script>
    <script type="text/javascript" src="/atomes/music/scripts/default.js"></script>
    <script type="text/javascript" src="/atomes/music/scripts/sm2player.js"></script>
    <script type="text/javascript" src="/atomes/music/scripts/soundmanager2.js"></script>

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
    <input id="search" type="text"/><button id="searchButton">Rechercher</button>
    <div id="navigationPanel">
        
        <div id="artistscontainer">
            <h2><%= Resources.webmusic.Artists %></h2>
            <div class="navigationList thinBox">
                <ul id="artistList">
                </ul>
            </div>
        </div>
        <div id="albumscontainer">
            <h2><%= Resources.webmusic.Albums %></h2>
            <div class="navigationList thinBox">
                <ul id="albumList">
                </ul>
            </div>
        </div>
    </div>
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
                        <img alt="" src="/App_Themes/bloup/images/media-playback-start-small.png" onclick="songsView_onclick('playAll')" />
                        <img alt="" src="/App_Themes/bloup/images/list-add.png" onclick="songsView_onclick('enqueueAll')" />
                        <img alt="" src="/App_Themes/bloup/images/document-save.png" onclick="songsView_onclick('downloadAll')" />
                    </td>
                </tr>
            </thead>
            <tbody>
            <%--<%foreach (var song in Model.Songs)
              { %>
                <tr>
                    <td style="display: none"><%= Server.UrlEncode(song.Id)%></td>
                    <td><%= Server.HtmlEncode(song.Title) %></td>
                    <td><%= Server.HtmlEncode(song.Artist.Name)%></td>
                    <td><%= Server.HtmlEncode(song.Album.Name)%></td>
                    <td><%= song.AlbumTrack.ToString() %></td>
                    <td><%= song.Duration.Minutes.ToString("00") + ":" + song.Duration.Seconds.ToString("00") %></td>
                    <td style="text-align: right">
                        <img alt="" src="/App_Themes/bloup/images/media-playback-start-small.png"
                            onclick="songsViewItem_onclick(this,'play')" />
                        <img alt="" src="/App_Themes/bloup/images/list-add.png" onclick="songsViewItem_onclick(this,'enqueue')" />
                        <a href="Download.aspx?songId=<%= Server.UrlEncode(song.Id)%>">
                            <img alt="" src="/App_Themes/bloup/images/document-save.png" /></a>
                    </td>
                </tr>
             <%} %>--%>
            </tbody>
        </table>
    </div>
    
    
    
    <script type="text/javascript">
        
    </script>
</asp:Content>