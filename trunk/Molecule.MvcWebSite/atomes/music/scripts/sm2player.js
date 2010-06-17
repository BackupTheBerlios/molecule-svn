
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
      onload: function() { onLoad(); },
      volume: currentVolume,
      whileplaying: function() {
        updateCurrentPosition(currentSong.position, currentSong.duration, currentSong.durationEstimate);
      }
    });
    soundManager.setVolume("current", currentVolume);
    onPlay();
}

function getVolume()
{
    return currentVolume;
}

function volumeUp()
{
    setVolume(currentVolume + volumeIncrement);
}

function volumeDown()
{
    setVolume(currentVolume - volumeIncrement);
}

function setVolume(v) {
    currentVolume = v;
    if (currentVolume > 100)
        currentVolume = 100;
    if (currentVolume < 0)
        currentVolume = 0;
    soundManager.defaultOptions.volume = currentVolume;
    if (currentSong != null)
        soundManager.setVolume("current", currentVolume);
}

function getEstimateDuration()
{
    return currentSong.durationEstimate;
}

function setPosition(p) {
    currentSong.setPosition(p);
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