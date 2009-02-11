
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
var playlistSelectedIndex;
var coverArtImage;
var playlistMainCellTemplate = "<a onclick=\"playlistItem_onclick(this, 'play')\">$songDesc</a>";
var playlistActionCellTemplate = "<img alt=\"\" src=\"images/list-remove.png\" onclick=\"playlistItem_onclick(this, 'remove')\" />";

function init()
{
    soundManager.url = ".";
    playlist = new Array();
    playlistView = $get("playlistTable");    
    coverArtImage = $get("coverArtImage");
    playButton = $get("playButton");
    pauseButton = $get("pauseButton");
    repeatAllCheckBox = $get("repeatAllCheckBox");
    currentSongArtistLabel = $get("currentSongArtistLabel");
    currentSongTitleLabel = $get("currentSongTitleLabel");
    currentSongPositionLabel = $get("currentSongPositionLabel");
    currentVolumeLabel = $get("currentVolumeLabel");
    updateCurrentVolume(getVolume());
    errorLoadingMessage = $get("fileNotFoundPanel");
    songAddedToPlaylist = $get("fileAddedToPlaylistPanel");
}

function playSelectedSong()
{
    var currentSong = playlist[playlistSelectedIndex];
    playSong(currentSong.getUrl());
    currentSongArtistLabel.innerHTML = currentSong.artist;
    currentSongTitleLabel.innerHTML = currentSong.title;
    coverArtImage.src = "~/atomes/webmusic/CoverArt/"+currentSong.id+".jpg";
    coverArtImage.style.display ="inline";
    UseCallback('idSongCurrentlyPlaying;'+currentSong.id,null);    
}
function SongEvent(arg, context)
    {

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

function enqueueSong(id, artist, title)
{
    playlist[playlist.length] = new Song(id, artist, title);
    var row = playlistView.insertRow(playlist.length - 1);
    row.className = "playlistItem";
    var actionCell = row.insertCell(0);
    actionCell.innerHTML = playlistActionCellTemplate;
    var mainCell = row.insertCell(0);
    var mainCellContent = playlistMainCellTemplate.replace('$songId', id);
    mainCellContent = mainCellContent.replace('$songDesc', artist + ' - ' + title);
    mainCell.innerHTML = mainCellContent;

    songAddedToPlaylist.style.display ="inline-block";
    setTimeout('songAddedToPlaylist.style.display="none"',5000); 
}



function onEnded()
{
    UseCallback('idSongPlayed;'+currentSong.id,null);    
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
       }
       else
       {
            errorLoadingMessage.style.display = "none";
       }
}

function updateCurrentVolume(volume)
{
   // currentVolumeLabel.innerHTML = getVolume() + "%";
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
    else if(action = "remove")
    {
        removeSelectedSong();
    }
}

function songsView_onclick(action)
{
    var songsView = $get("songsView");
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
        if(songsView.rows.length > 1)
            songList = songList + songsView.rows[1].cells[0].innerHTML;
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
    if(action == 'play' || action == 'enqueue')
        enqueueSong(songId, songArtist, songTitle);
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


function Song(id, artist, title) {
    this.id = id;
    this.artist = artist;
    this.title = title;
    this.getUrl = function()
    {
        return "/"+id+".media";
    }
};