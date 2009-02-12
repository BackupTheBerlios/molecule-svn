using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMPLib;
using WebMusic.Providers.Base;
using Molecule.Runtime;
using System.Timers;

[assembly: PluginContainer]

namespace WebMusic.Providers.WMP
{
    [Plugin("Windows Media Player")]
    public class MusicLibraryProvider : IMusicLibraryProvider
    {
        Dictionary<string, Artist> artistDictionary;


        [IsUsablePlugin]
        public static bool IsUsable
        {
            get
            {
                //not very clean... should not work with Windows "N Edition".
                return Environment.OSVersion.Platform == PlatformID.Win32NT;
            }
        }

        public System.Collections.Generic.IEnumerable<string> AlbumsRecentlyAdded
        {
            get
            {
                return null;
            }
        }		
        #region IMusicLibraryProvider Members

        public void Initialize()
        {
            artistDictionary = new Dictionary<string, Artist>();
            WindowsMediaPlayer wmp = new WindowsMediaPlayerClass();
            var items = wmp.mediaCollection.getByAttribute("MediaType", "audio");

            int albumId = 0;
            int artistId = 0;

            for (int i = 0; i < items.count; i++)
            {
                IWMPMedia media = items.get_Item(i);
                string albumArtistName = media.getItemInfo("AlbumArtist").Trim();
                if (albumArtistName == "")
                {
                    albumArtistName = media.getItemInfo("Artist").Trim();
                    if(albumArtistName == "")
                        albumArtistName = "Unknown";
                }
                string albumName = media.getItemInfo("Album").Trim();
                if (albumName == "")
                    albumName = "Unknown";
                string trackName = media.getItemInfo("Title").Trim();
                if (trackName == "")
                    trackName = "Unknown"+i;
                string trackLocation = media.getItemInfo("SourceUrl");
                string trackNumberString = media.getItemInfo("OriginalIndex");
                uint trackNumber = 0;
                var duration = TimeSpan.FromSeconds(media.duration);
                UInt32.TryParse(trackNumberString, out trackNumber);

                Artist artist;

                if (!artistDictionary.TryGetValue(albumArtistName, out artist))
                {
                    artist = new Artist("artist"+artistId++, albumArtistName);
                    artistDictionary.Add(albumArtistName, artist);
                }

                Album album = (Album)artist.Albums.FirstOrDefault(al => al.Name == albumName);
                if(album == null)
                    album = artist.AddAlbum("album"+albumId++, albumName);

                Song song = new Song(trackName, album, artist, trackLocation, trackNumber, i.ToString(), duration);
                album.AddSong(song);
            }
        }

        public IEnumerable<IArtist> GetArtists()
        {
            Initialize();
            return artistDictionary.Values.Cast<IArtist>();
        }

        #endregion
    }
}
