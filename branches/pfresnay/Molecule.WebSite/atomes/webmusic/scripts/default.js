var playlist;
var player;
var playButton;
var pauseButton;
var repeatAllCheckBox;
var currentSongInfoLabel;
var currentSongPositionLabel;
var currentVolumeLabel;
var errorLoadingMessage;
var currentSongId;

function init()
{
    soundManager.url = ".";
    playlist = $get("playListBox");
    playButton = $get("playButton");
    pauseButton = $get("pauseButton");
    repeatAllCheckBox = $get("repeatAllCheckBox");
    currentSongInfoLabel = $get("currentSongInfoLabel");
    currentSongPositionLabel = $get("currentSongPositionLabel");
    currentVolumeLabel = $get("currentVolumeLabel");
    updateCurrentVolume(getVolume());
    errorLoadingMessage = $get("fileNotFoundPanel");
}

function playSelectedSong()
{
    var selectedOption = playlist.options.item(playlist.selectedIndex);
    currentSongId = selectedOption.value;
    playSong(getUrlForSong(currentSongId));
    currentSongInfoLabel.innerHTML = selectedOption.text;
    UseCallback('idSongCurrentlyPlaying;'+currentSongId,null);    
}
function SongEvent(arg, context)
    {

    }


function removeSelectedSong()
{
    var i = playlist.selectedIndex;
    if(i >= 0)
    {
        var selectedOption = playlist.options.item(i);
        playlist.removeChild(selectedOption);
        var nbItem = playlist.options.length;
        if(nbItem > 0)
            if(i < nbItem)
                 playlist.selectedIndex = i;
            else playlist.selectedIndex = nbItem - 1;
     }
}

function getUrlForSong(songId)
{
    return "/"+songId+".media";
}

function playLastSong()
{
    playlist.selectedIndex = playlist.options.length -1;
    playSelectedSong();
}

function playPreviousSong()
{
    if(playlist.selectedIndex > 0)
    {
        playlist.selectedIndex--;
        playSelectedSong();
    }
}

function playNextSong()
{
    var nbItem = playlist.options.length;
    if(nbItem > 0)
    {
        playlist.selectedIndex = (playlist.selectedIndex + 1) % nbItem;
        playSelectedSong();
    }   
}

function enqueueSong(id, artist, title)
{ 
    var option = document.createElement("option");
    option.text = artist + " - "+ title;
    option.value = id;
    playlist.appendChild(option);
}

function onEnded()
{
    UseCallback('idSongPlayed;'+currentSongId,null);    
    onPause();
    var nbItem = playlist.options.length;
    if(playlist.selectedIndex < nbItem -1)
    {
        //todo : implémenter repeat one
        playlist.selectedIndex += 1;
        playSelectedSong();
    }
    else if(repeatAllCheckBox.checked)
    {
        playlist.selectedIndex = 0;
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
    if(playlist.selectedIndex == -1 && playlist.options.length > 0)
        playNextSong();
    else
        resume();
}
