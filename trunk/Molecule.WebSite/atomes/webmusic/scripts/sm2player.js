soundManager.url = '/';
var currentSong;
var currentVolume = 70;
var volumeIncrement = 5;


function playSong(songUrl)
{
    if(currentSong != null)
    {
        soundManager.destroySound(currentSong.sID);
    }
        
    currentSong = soundManager.createSound({
      id: "current",
      url: songUrl,
      autoPlay: true,
      autoLoad: true,
      onplay: function() {onPlay();},
      onpause: function() {onPause();},
      onresume: function() {onPlay();},
      onfinish: function() {onEnded();},
      onload: function(){onLoad();},
      whileplaying: function() {updateCurrentPosition(currentSong.position, currentSong.duration);}
    });
    soundManager.setVolume("current", currentVolume);
    onPlay();
    //soundManager.play("current");
    //player.source = songId+".media";
}

function getVolume()
{
    return currentVolume;
}



function volumeUp()
{
    currentVolume += volumeIncrement;
    if(currentVolume > 100)
    currentVolume = 100;
    if(currentSong != null)
        soundManager.setVolume("current", currentVolume);
}

function volumeDown()
{
    currentVolume -= volumeIncrement;
    if(currentVolume < 0)
    currentVolume = 0;
    if(currentSong != null)
        soundManager.setVolume("current", currentVolume);
}

function resume()
{
    if(currentSong != null)
        soundManager.resume("current");
}

function pause()
{
    if(currentSong != null)
        soundManager.pause("current");
}