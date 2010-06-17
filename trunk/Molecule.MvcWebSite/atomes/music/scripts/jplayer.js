
var currentSong;
var currentVolume = 70;
var volumeIncrement = 5;

$(document).ready(function() {

    // Local copy of jQuery selectors, for performance.
    var jpPlayTime = $("#jplayer_play_time");
    var jpTotalTime = $("#jplayer_total_time");

    $("#jquery_jplayer").jPlayer({
        volume: currentVolume,
        oggSupport: true,
        swfPath: "/atomes/music"
    })
	.jPlayer("onProgressChange", function(loadPercent, playedPercentRelative, playedPercentAbsolute, playedTime, totalTime) {
	    jpPlayTime.text($.jPlayer.convertTime(playedTime));
	    jpTotalTime.text($.jPlayer.convertTime(totalTime));
	})
	.jPlayer("onSoundComplete", function() {
	    onEnded();
	}); 
});

function playSong(songUrl) {

    $("#jquery_jplayer").jPlayer("clearFile");
    $("#jquery_jplayer").jPlayer("setFile", songUrl);
    $("#jquery_jplayer").jPlayer("play");

//    currentSong = soundManager.createSound({
//        id: "current",
//        url: songUrl,
//        autoPlay: true,
//        autoLoad: true,
//        onplay: function() { onPlay(); },
//        onpause: function() { onPause(); },
//        onresume: function() { onPlay(); },
//        onfinish: function() { onEnded(); },
//        onload: function() { onLoad(); },
//        volume: currentVolume,
//        whileplaying: function() { updateCurrentPosition(currentSong.position, currentSong.duration); }
//    });
    //soundManager.setVolume("current", currentVolume);
    onPlay();
}

function getVolume() {
    return currentVolume;
}

function volumeUp() {
    currentVolume += volumeIncrement;
    if (currentVolume > 100)
        currentVolume = 100;
    $("#jquery_jplayer").jPlayer("volume", currentVolume);
}

function volumeDown() {
    currentVolume -= volumeIncrement;
    if (currentVolume < 0)
        currentVolume = 0;
    $("#jquery_jplayer").jPlayer("volume", currentVolume);
}

function resume() {
    $("#jquery_jplayer").jPlayer("play");
    onPlay();
}

function pause() {
    $("#jquery_jplayer").jPlayer("pause");
    onPause();
}