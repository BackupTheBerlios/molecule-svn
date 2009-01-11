//<object type="application/x-silverlight" data="data:application/x-silverlight," id="silverlightPlayer"
//    style="height: 23px; width: 18%;" onload="player_Loaded">
//    <param name="source" value="player.xaml" />
//    <a href="http://go2.microsoft.com/fwlink/?LinkID=114576&v=1.0">
//        <img src="http://go2.microsoft.com/fwlink/?LinkID=108181" alt="Téléchargez Microsoft Silverlight"
//            style="border-width: 0;" />
//    </a>
//</object>
//<small><code>
//    <div id="playerDebug">
//    </div>
//</code></small>

var player;
var playerDebug;

function playSong(songId)
{
    player.source = songId+".media";
}

function getPlayerState()
{
    return player.CurrentState;
}

function play()
{
    player.play();
}

function pause()
{
    player.pause();
}

function player_BufferingProgressChanged()
{
    updatePlayerDebug();
}

function player_DownloadProgressChanged()
{
    updatePlayerDebug();
}

function player_MediaFailed()
{

}


function player_Loaded()
{
    player = $get("silverlightPlayer").content.findName("player");
    playerDebug = $get("playerDebug");
}

function player_MediaOpened(sender, args)
{
    updatePlayerDebug();
}


function updatePlayerDebug()
{
    playerDebug.innerHTML = "state : "+player.CurrentState
    +" - buffering : "+ Math.round(player.BufferingProgress*100)+"%"
    +" - download : "+ Math.round(player.DownloadProgress *100)+"%";
}