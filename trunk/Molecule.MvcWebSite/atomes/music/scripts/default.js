/// <reference path="~/Scripts/jquery-1.3.2-vsdoc.js" />

$(document).ready(function() {

    $("#previousButton").click(previousButton_onclick);
    $("#playButton").click(playButton_onclick);
    $("#pauseButton").click(pauseButton_onclick);
    $("#nextButton").click(nextButton_onclick);
    $("#volumeDownButton").click(volumeDownButton_onclick);
    $("#volumeUpButton").click(volumeUpButton_onclick);
    $("#playlistTable").keydown(playlist_onkeydown);
    $("#playAllButton").click(function() { songsView_onclick('playAll'); });
    $("#enqueueAllButton").click(function() { songsView_onclick('enqueueAll'); });
    $("#emptyAction").click(emptyPlaylist);
    $("#volumeBar").click(volumeBar_onclick);
    $("#volumeBarValue").click(volumeBar_onclick);
    $("#loadBar").click(loadBar_onclick);
    $("#playBar").click(loadBar_onclick);
    
    //$("#downloadAllButton").click(function() { songsView_onclick('downloadAll'); });

    manualSearch = false;

    $("#search").keyup(function(event) {
        if (event.keyCode == 13) {
            $.historyLoad($("#search").attr("value"));
        }
    });

    $(".songInfo").hide();

    // Initialize history plugin.
    // The callback is called at once by present location.hash.
    $.historyInit(pageload, "Player");

    retreiveAlbumsAndArtists();

    init();
});

// PageLoad function
// This function is called when:
// 1. after calling $.historyInit();
// 2. after calling $.historyLoad();
// 3. after pushing "Go Back" button of a browser
function pageload(hash) {
    // alert("pageload: " + hash);
    // hash doesn't contain the first # character.
    if (hash) {
        // restore ajax loaded state
        if ($.browser.msie) {
            // jquery's $.load() function does't work when hash include special characters like aao.
            //hash = encodeURIComponent(hash);
        }
        $("#search").attr("value", hash);
        if(!manualSearch)
            searchRequested();
        manualSearch = false;
    } else {
    //default content
        retreiveAlbumsAndArtists();
    }
}

function searchRequested() {
    artistsWaitEffect();
    albumsWaitEffect();
    songsWaitEffect();
    var searchString = $("#search").attr("value");
    if (searchString != "") {
        library.Search($("#search").attr("value"), function(result) {
            updateAlbumList(result.Albums);
            updateArtistList(result.Artists);
            updateSongList(result.Songs);
        });
    }
    else {
        retreiveAlbumsAndArtists();
        updateSongList({});
    }
}

function retreiveAlbumsAndArtists() {
    artistsWaitEffect();
    library.Artists(updateArtistList);

    albumsWaitEffect();
    library.Albums(updateAlbumList);
}

function artistsWaitEffect() {
    $("#artistsWaiting").show();
    $("#artistList").hide();
}

function albumsWaitEffect() {
    $("#albumsWaiting").show();
    $("#albumList").hide();
}

function songsWaitEffect() {
    $("#songsView").fadeTo(0, 0.01);
}

function updateAlbumList(albums) {
    $("#albumsWaiting").hide();
    $("#albumList li").remove();
    $("#albumList").fadeIn(200);
    var albumList = $("#albumList");
    $.each(albums, function(i, item) {
        var a = $("<li><a href='#'>" + item.Name + "</a></li>");
        a.click(function(e) {
            e.preventDefault();
            songsWaitEffect();
            var search = "album:"+item.Name;
            manualSearch = true; //we will directly get items from album ID by calling SongsByAlbum.
            $.historyLoad(search);
            library.SongsByAlbum(item.Id, updateSongList);
        });
        albumList.append(a);
    });
}

var manualSearch;

function updateArtistList(artists) {
    $("#artistsWaiting").hide();
    $("#artistList").fadeIn(200);
    $("#artistList li").remove();
    
    var artistList = $("#artistList");
    $.each(artists, function(i, item) {
        var a = $("<li><a href='#'>" + item.Name + "</a></li>");
        a.click(function(e) {
            e.preventDefault();
            albumsWaitEffect();
            library.AlbumsByArtist(item.Id, updateAlbumList);
        });
        artistList.append(a);
    });

    
}

function updateSongList(songs) {
    //$("#songsView > tbody tr").remove();

    $("#songsView").fadeTo(200, 1);

    var rows = "";

    $.each(songs, function(i, song) {
        rows = rows + songListRowTemplate(song);
    });
    $("#songsView > tbody").html(rows);
    
    var songList = '';
    $.each(songs, function(i, song) {//skip first header row
        var separator = '';
        if (i > 0) separator = ',';
        songList = songList + separator + song.Id;
    });
    
    var searchValue = $("#search").val();
    if (searchValue != null) {

        if (searchValue.indexOf('album:', 0) == 0) {
            searchValue = searchValue.replace('album:', 'album/');
        }
        else if (searchValue.indexOf('artist:', 0) == 0) {
            searchValue = searchValue.replace('artist:', 'artist/');
        }
        else if (searchValue.indexOf('title:', 0) == 0) {
            searchValue = searchValue.replace('artist:', 'artist/');
        }
        var href = "/Music/Download/Files/" + searchValue;
        $("#downloadAllLink").attr("href", href);
        $("#downloadAllLink").attr("target", "_blank");
    }
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
    //coverArtImage = document.getElementById("coverArtImage");
    playButton = document.getElementById("playButton");
    pauseButton = document.getElementById("pauseButton");
    repeatAllCheckBox = document.getElementById("repeatAllCheckBox");
    currentSongArtistLabel = document.getElementById("currentSongArtistLabel");
    currentSongTitleLabel = document.getElementById("currentSongTitleLabel");
    currentSongPositionLabel = document.getElementById("currentSongPositionLabel");
    currentVolumeLabel = document.getElementById("currentVolumeLabel");
    errorLoadingMessage = document.getElementById("fileNotFoundPanel");
    songAddedToPlaylist = document.getElementById("fileAddedToPlaylistPanel");
    updateCurrentVolume();
}

function playSelectedSong()
{
    var currentSong = playlist[playlistSelectedIndex];
    playSong(currentSong.getUrl());
    $(".songInfo").hide();
    currentSongArtistLabel.innerHTML = currentSong.artist;
    currentSongTitleLabel.innerHTML = currentSong.title;
    var newsrc = covertArtUrl.replace("#id", currentSong.id);

    $("#coverArt img").remove();
    
    var img = new Image();
    $(img)
        .load(function() {
            $(this).hide();
            
            $("#coverArt")
                .append(this)
                .attr('title', currentSong.album);
            $(this).fadeIn();
        })
        .error(function() {
            //TODO
        })
        .attr('src', newsrc);

    
    //coverArtImage.alt = currentSong.album;
    //coverArtImage.title = currentSong.album;
    $(".songInfo").fadeIn(200);
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



function onLoad()
{
       var state =  currentSong.readyState;
       if( state == 2)
       {
              errorLoadingMessage.style.display = "inline-block";
              setTimeout('errorLoadingMessage.style.display="none"',5000); 
       }
}

function updateCurrentPosition(currentPosition, duration, estimateDuration) {
    $("#totalTimeLabel").text(formatTime(estimateDuration));
    $("#playTimeLabel").text(formatTime(currentPosition));
    $("#loadBar").width((100 * duration / estimateDuration) + "%");
    $("#playBar").width((100 * currentPosition / estimateDuration) + "%");
    //currentSongPositionLabel.innerHTML = formatTime(currentPosition) + " / " + formatTime(duration);
}

function updateCurrentVolume() {
    $("#volumeBarValue").width(getVolume() + "%");
   //currentVolumeLabel.innerHTML = getVolume() + "%";
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

function emptyPlaylist() {
    var nbSongs = playlist.length;
    playlist = new Array();
    while (nbSongs > 0) {
        playlistView.deleteRow(0);
        nbSongs--;
    }
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
        var songList = '';
        for(i = 1; i < songsView.rows.length; i++){//skip first header row
        	var separator = '';
        	if(i > 1) separator = ',';
            songList = songList + separator + songsView.rows[i].cells[0].innerHTML;
        }
        
        location.href = "/Music/Player/Files/" + songList;
    }
}

function songsViewRowAction(row, action)
{
    var songId = row.cells[0].innerHTML;
    var songTitle = row.cells[1].childNodes[0].innerHTML;
    var songArtist = row.cells[2].childNodes[0].innerHTML;
    var songAlbum = trim(row.cells[3].childNodes[0].innerHTML);
    if(action == 'play' || action == 'enqueue')
        enqueueSong(songId, songArtist, songTitle, songAlbum);
    if(action == 'play')
        playLastSong();
    if (action == 'download')
        location.href = "/Music/Player/File/" + songId;
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

function volumeBar_onclick(e) {
    var offset = $("#volumeBar").offset();
    var x = e.pageX - offset.left;
    var w = $("#volumeBar").width();
    var p = 100 * x / w;
    setVolume(p);
    updateCurrentVolume();
}
function loadBar_onclick(e) {
    var offset = $("#progressBar").offset();
    var x = e.pageX - offset.left;
    var w = $("#progressBar").width();
    var p = 100 * x / w;
    setPosition(getEstimateDuration() * p / 100);
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
    this.getUrl = function(){
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
