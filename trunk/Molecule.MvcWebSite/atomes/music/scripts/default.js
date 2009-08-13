/// <reference path="~/Scripts/jquery-1.3.2-vsdoc.js" />
$(document).ready(function() {
    library.Artists(updateArtistList);
    library.Albums(updateAlbumList);
    $("#searchButton").click(function() {
        var result = library.Search($("#search").attr("value"), function(result) {
            updateAlbumList(result.Albums);
            updateArtistList(result.Artists);
            updateSongList(result.Songs);
        });
    });
    init();
});

function updateAlbumList(albums) {
    $("#albumList li").remove();
    var albumList = $("#albumList");
    $.each(albums, function(i, item) {
        var a = $("<a href='#'>" + item.Name + "</a>");
        a.click(function(e) {
            e.preventDefault();
            library.SongsByAlbum(item.Id, updateSongList);
        });
        albumList.append($("<li/>").append(a));
    });
}

function updateArtistList(artists) {
    $("#artistList li").remove();
    var artistList = $("#artistList");
    $.each(artists, function(i, item) {
        var a = $("<a href='#'>" + item.Name + "</a>");
        a.click(function(e) {
            e.preventDefault();
            library.AlbumsByArtist(item.Id, updateAlbumList);
        });
        artistList.append($("<li/>").append(a));
    });
}

function updateSongList(songs) {
    $("#songsView > tbody tr").remove();
    $.each(songs, function(i, song) {
        $("#songsView").append("<tr><td style='display: none'>" + song.Id + "</td>\
                    <td>" + song.Title + "</td>\
                    <td>" + song.ArtistName + "</td>\
                    <td>" + song.AlbumName + "</td>\
                    <td>" + song.AlbumTrack + "</td>\
                    <td>" + song.Duration + "</td>\
                    <td style='text-align: right'>\
                        <img alt='' src='/App_Themes/bloup/images/media-playback-start-small.png'\
                            onclick=\"songsViewItem_onclick(this,'play')\" />\
                        <img alt='' src=\"/App_Themes/bloup/images/list-add.png\" onclick=\"songsViewItem_onclick(this,'enqueue')\" />\
                        <a href=\"\">\
                            <img alt='' src='/App_Themes/bloup/images/document-save.png' /></a>\
                    </td>\
                </tr>");
    });
}

var player;
var playButton;
var pauseButton;
var repeatAllCheckBox;
var currentSongArtistLabel;
var currentSongTitleLabel;
var currentSongPositionLabel;
var currentVolumeLabel;
var errorLoadingMessage;
var songAddedToPlaylist;
var currentSong;
var playlist = new Array();
var playlistView;
var playlistPanel;
var playlistSelectedIndex;
var coverArtImage;
var playlistMainCellTemplate = "<a onclick=\"playlistItem_onclick(this, 'play')\">$songDesc</a>";
var playlistActionCellTemplate = "<a class=\"listRemove\" onclick=\"playlistItem_onclick(this, 'remove')\"><span>__</span></a>";

function init()
{
    soundManager.url = "/atomes/music/";
    playlist = new Array();
    playlistView = document.getElementById("playlistTable");
    playlistPanel = document.getElementById("playlistPanel"); 
    coverArtImage = document.getElementById("coverArtImage");
    playButton = document.getElementById("playButton");
    pauseButton = document.getElementById("pauseButton");
    repeatAllCheckBox = document.getElementById("repeatAllCheckBox");
    currentSongArtistLabel = document.getElementById("currentSongArtistLabel");
    currentSongTitleLabel = document.getElementById("currentSongTitleLabel");
    currentSongPositionLabel = document.getElementById("currentSongPositionLabel");
    currentVolumeLabel = document.getElementById("currentVolumeLabel");
    updateCurrentVolume(getVolume());
    errorLoadingMessage = document.getElementById("fileNotFoundPanel");
    songAddedToPlaylist = document.getElementById("fileAddedToPlaylistPanel");
    updateCurrentVolume(getVolume());
}

function playSelectedSong()
{
    var currentSong = playlist[playlistSelectedIndex];
    playSong(currentSong.getUrl());
    currentSongArtistLabel.innerHTML = currentSong.artist;
    currentSongTitleLabel.innerHTML = currentSong.title;
    coverArtImage.src = "CoverArt/CoverArtFile.aspx?id="+currentSong.id;
    coverArtImage.alt = currentSong.album;
    coverArtImage.title = currentSong.album;
    coverArtImage.style.display ="inline";
    library.SongCurrentlyPlaying(currentSong.id, null);    
}

function removeSelectedSong()
{
    var i = playlistSelectedIndex;
    if(i >= 0) {
        playlistView.deleteRow(i);
        playlist.splice(i, 1);
        var nbItem = playlist.length;
        if(nbItem > 0)
            if(i < nbItem)
                 setPlaylistSelectedIndex(i);
            else setPlaylistSelectedIndex(nbItem - 1);
     }
}

function setPlaylistSelectedIndex(index)
{
    if(playlist.length > 0 && playlistSelectedIndex >= 0)
        playlistView.rows[playlistSelectedIndex].className = "";
    playlistSelectedIndex = index;
    playlistView.rows[playlistSelectedIndex].className = "selectedRow";
}

function playLastSong()
{
    setPlaylistSelectedIndex(playlist.length -1);
    playSelectedSong();
}

function playPreviousSong()
{
    if(playlistSelectedIndex > 0)
    {
        setPlaylistSelectedIndex(playlistSelectedIndex-1);
        playSelectedSong();
    }
}

function playNextSong()
{
    var nbItem = playlist.length;
    if(nbItem > 0)
    {
        setPlaylistSelectedIndex((playlistSelectedIndex + 1) % nbItem);
        playSelectedSong();
    }   
}

function enqueueSong(id, artist, title, album)
{
    playlist[playlist.length] = new Song(id, artist, title, album);
    var row = playlistView.insertRow(playlist.length - 1);
    row.className = "playlistItem";
    var actionCell = row.insertCell(0);
    actionCell.innerHTML = playlistActionCellTemplate;
    var mainCell = row.insertCell(0);
    var mainCellContent = playlistMainCellTemplate.replace('$songId', id);
    mainCellContent = mainCellContent.replace('$songDesc', artist + ' - ' + title);
    mainCell.innerHTML = mainCellContent;
    songAddedToPlaylist.style.display ="inline";
    setTimeout('songAddedToPlaylist.style.display="none"',3000); 
}



function onEnded()
{
    library.SongPlayed(currentSong.id,null);    
    onPause();
    var nbItem = playlist.length;
    if(playlistSelectedIndex < nbItem -1)
    {
        //todo : implémenter repeat one
        setPlaylistSelectedIndex(playlistSelectedIndex + 1);
        playSelectedSong();
    }
    else if(repeatAllCheckBox.checked)
    {
        setPlaylistSelectedIndex(0);
        playSelectedSong();
    }     
}

function onPlay()
{
    playButton.style.display = "none";
    pauseButton.style.display = "inline";
}

function onPause()
{
    playButton.style.display = "inline";
    pauseButton.style.display = "none";
}

function updateCurrentPosition(currentPosition, duration)
{
    currentSongPositionLabel.innerHTML = formatTime(currentPosition) + " / " + formatTime(duration);
}

function onLoad()
{
       var state =  currentSong.readyState;
       if( state == 2)
       {
              errorLoadingMessage.style.display = "inline-block";
              setTimeout('errorLoadingMessage.style.display="none"',5000); 
       }
}

function updateCurrentVolume(volume)
{
   currentVolumeLabel.innerHTML = getVolume() + "%";
}

function formatTime(timeInMilliseconds)
{
    var timeInSeconds = timeInMilliseconds / 1000;
    var minutes = Math.floor(timeInSeconds / 60);
    var seconds = Math.floor(timeInSeconds % 60);
    var minutesString = (minutes >= 10 ? "" : "0") + minutes;
    var secondsString = (seconds >= 10 ? "" : "0") + seconds;
    return minutesString + ":" + secondsString;
}

function playlist_onkeydown(event)
{
    var keyCode;
    if(window.event)//IE
        keyCode = event.keyCode;
        
    else if(event.which) //else
        keyCode = event.which;
        
    if(keyCode == 46)
        removeSelectedSong();
}

function playlistItem_onclick(item, action)
{
    setPlaylistSelectedIndex(item.parentNode.parentNode.rowIndex);
    if(action == "play")
    {
        playSelectedSong();
    }
    else if(action == "remove")
    {
        removeSelectedSong();
    }
}

function songsView_onclick(action)
{
    var songsView = document.getElementById("songsView");
    if(action == 'enqueueAll')
    {
        for(i = 1; i < songsView.rows.length; i++)//skip first header row
            songsViewRowAction(songsView.rows[i], 'enqueue');
    }
    else if(action == 'playAll')
    {
        if(songsView.rows.length > 1)
            songsViewRowAction(songsView.rows[1], 'play');
        for(i = 2; i < songsView.rows.length; i++)
            songsViewRowAction(songsView.rows[i], 'enqueue');
    }
    else if(action == 'downloadAll')
    {
        var songList = '{';
        for(i = 1; i < songsView.rows.length; i++)//skip first header row
            songList = songList + ',' + songsView.rows[i].cells[0].innerHTML;
        songList = songList + '}';
        location.href = "Download.aspx?songList="+songList;
    }
}

function songsViewRowAction(row, action)
{
    var songId = row.cells[0].innerHTML;
    var songTitle = row.cells[1].innerHTML;
    var songArtist = row.cells[2].innerHTML;
    var songAlbum = trim(row.cells[3].innerHTML);
    if(action == 'play' || action == 'enqueue')
        enqueueSong(songId, songArtist, songTitle, songAlbum);
    if(action == 'play')
        playLastSong();
}

function songsViewItem_onclick(element, action)
{
    var row = element.parentNode.parentNode;
    songsViewRowAction(row, action);
    
}

function pauseButton_onclick() {
    pause();
}

function previousButton_onclick() {
    playPreviousSong();
}

function nextButton_onclick() {
    playNextSong();
}

function volumeUpButton_onclick() {
    volumeUp();
    updateCurrentVolume();
}

function volumeDownButton_onclick() {
    volumeDown();
    updateCurrentVolume();
}

function playButton_onclick() {
    if(playlist.selectedIndex == -1 && playlist.length > 0)
        playNextSong();
    else
        resume();
}


function Song(id, artist, title, album) {
    this.id = id;
    this.artist = artist;
    this.title = title;
    this.album = album;
    this.getUrl = function()
    {
        return "/Music/Player/File/"+id;
    }
};

function trim(str, chars) {
    return ltrim(rtrim(str, chars), chars);
}

function ltrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}

function rtrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}
