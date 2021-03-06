<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
Inherits="System.Web.Mvc.ViewPage<Molecule.MvcWebSite.atomes.music.Data.IndexData>" %>
<%@ Import Namespace="Molecule.MvcWebSite.atomes.music.Controllers" %>
<%@ Import Namespace="WebMusic.Providers" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <link href="/atomes/music/style/layout.css" rel="stylesheet" type="text/css" />
    <link href="/atomes/music/style/jplayer.blue.monday.css" rel="stylesheet" type="text/css" /> 
        <style type="text/css">
        #playlistTable tr:hover .listRemove
        {
            background: url("<%= Url.Theme("images/list-remove.png")%>") no-repeat;
        }
    </style>
    <script type="text/javascript">
    <%= Url.JQueryProxyScript<PlayerController>("library") %>
    var covertArtUrl = "<%= Url.Action<PlayerController>(c => c.CovertArt("#id")) %>";
    var fileUrl = "<%= Url.Action<PlayerController>(c => c.File("#id")) %>";
    
    function songListRowTemplate(song){ return "<tr><td style='display: none'>" + song.Id + "</td>\
                    <td><span>" + song.Title + "</span></td>\
                    <td><span>" + song.ArtistName + "</span></td>\
                    <td><span>" + song.AlbumName + "</span></td>\
                    <td>" + song.AlbumTrack + "</td>\
                    <td>" + song.Duration + "</td>\
                    <td style='text-align: right'>\
                        <img alt='' src='<%= Url.Theme("images/media-playback-start-small.png")%>'\
                            onclick=\"songsViewItem_onclick(this,'play')\" />\
                        <img alt='' src=\"<%= Url.Theme("images/list-add.png")%>\" onclick=\"songsViewItem_onclick(this,'enqueue')\" />\
                        <a  href=\"" + fileUrl.replace("#id", song.Id) + "\">\
                            <img alt='' src='<%= Url.Theme("images/document-save.png")%>' /></a>\
                    </td>\
                </tr>";
     }
    
    </script>
    <script type="text/javascript" src="../../../../Scripts/jquery-1.3.2.js"></script>
    <script type="text/javascript" src="../../../../Scripts/jquery.history.js"></script>
    <script type="text/javascript" src="/atomes/music/scripts/default.js"></script>
<%--    <script type="text/javascript" src="/atomes/music/scripts/jplayer.js"></script>
    <script type="text/javascript" src="/atomes/music/scripts/jquery.jplayer.min.js"></script>--%>
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
<%--    <div id="jquery_jplayer"></div> --%>
    <div id="playerControls">
		<img id="previousButton" alt="Precedent" src="<%= Url.Theme("images/media-skip-backward.png")%>" />&nbsp;
        <img id="playButton" alt="Jouer" src="<%= Url.Theme("images/media-playback-start.png")%>" />
        <img id="pauseButton" alt="Pause" src="<%= Url.Theme("images/media-playback-pause.png")%>" />
        <img id="nextButton" alt="Suivant" src="<%= Url.Theme("images/media-skip-forward.png")%>" />
        <img id="volumeDownButton" alt="Diminuer volume" src="<%= Url.Theme("images/audio-volume-low.png")%>" />
        <div id="volumeBar" class="jp-volume-bar"> 
            <div id="volumeBarValue" class="jp-volume-bar-value backgroundHighlight"></div> 
        </div>
        <img id="volumeUpButton" alt="Augmenter volume" src="<%= Url.Theme("images/audio-volume-high.png")%>" />
        <div id="progressBar" class="jp-progress"> 
	        <div id="loadBar" class="jp-load-bar"> 
	            <div id="playBar" class="jp-play-bar backgroundHighlight"></div>     
	        </div> 
	        <div id="playTimeLabel" class="jp-play-time">00:00</div> 
             <div id="totalTimeLabel" class="jp-total-time">00:00</div> 
        </div> 
	        <br />
        <div id="currentSongTitleLabel" class="songInfo"></div>
        <div id="currentSongArtistLabel" class="songInfo"></div>
        <div id="coverArt" class="songInfo"></div>
    </div>
    
    <div id="playlistcontainer">
        <h2><%= Resources.webmusic.Playlist %></h2>
        <div id="playlistPanel" class="thinBox">
            <table id="playlistTable" class="itemList hoverTable"></table>
        </div>
        <input id="repeatAllCheckBox" type="checkbox" />
        <label for="repeatAllCheckBox"><%= Resources.webmusic.RepeatAll%></label>
        <span id="empty"><a id="emptyAction" href='#'><%= Resources.webmusic.Empty%></a></span>
        
    </div>

    <div id="artistscontainer">
        <h2><%= Resources.webmusic.Artists %></h2>
        <div class="navigationList thinBox">
            <span id="artistsWaiting"><img alt="waiting..." src="<%= Url.Theme("images/ajax-loader.gif") %>" /></span>
            <ul id="artistList" class="hoverList">
            </ul>
        </div>
    </div>
    
    <div id="albumscontainer">
        <h2><%= Resources.webmusic.Albums %></h2>
        <div class="navigationList thinBox">
            <span id="albumsWaiting"><img alt="waiting..." src="<%= Url.Theme("images/ajax-loader.gif") %>" /></span>
            <ul id="albumList" class="hoverList">
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
                    <td style="text-align: right; width:60px">
                        <img id="playAllButton" alt="" src="<%= Url.Theme("images/media-playback-start-small.png") %>" />
                        <img id="enqueueAllButton" alt="" src="<%= Url.Theme("images/list-add.png") %>" />
                        <a id="downloadAllLink" href=""><img id="downloadAllButton" alt="" src="<%= Url.Theme("images/document-save.png") %>" /></a>
                    </td>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
</asp:Content>