using Molecule.Webdav;
using WebMusic.Services;
using WebMusic.Providers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using WebMusic.Providers.Base;

namespace Molecule.MvcWebSite.atomes.music
{

    public class VirtualWebdavFolder : IVirtualWebdavFolder
    {

        #region IVirtualWebdavFolder Members

        public string RootDirectoryName
        {
            get { return "Music"; }
        }

        public System.Collections.Generic.IEnumerable<WebdavFileInfo> List(string path)
        {
            // path for the VirtualWebdavFolder are like this 
            // Artist/Album/Songs
            var pathElements = path.Split('/');
            if (pathElements.Length == 1)
            {
                return from a in  MusicLibrary.GetArtists()
                    select new WebdavFileInfo{FileName = String.Format("{0}/{1}", this.RootDirectoryName, a.Name) , 
                        IsFile= false, LastAccessTime= DateTime.Now,Size = 4096 }; 
            }
            else if (pathElements.Length == 2)
            {
                // Try to list an artist
                return from a in MusicLibrary.GetAlbumsByArtist(pathElements[1])
                       select new WebdavFileInfo
                       {
                           FileName = String.Format("{0}/{1}/{2}", this.RootDirectoryName,pathElements[1], a.Name),
                           IsFile = false,
                           LastAccessTime = DateTime.Now,
                           Size = 4096
                       }; 
            }
            else if (pathElements.Length == 3)
            {
                //guess we try to list the files
                var songs = new List<ISong>();
                var album = MusicLibrary.GetAlbumsByArtist(pathElements[1]).Where<IAlbum>(a => a.Name.Equals(pathElements[2]));
                foreach (IAlbum a in album)
                {
                    songs = songs.Concat<ISong>(a.Songs).ToList<ISong>();
                }
                List<WebdavFileInfo> webdavFileInfos = new List<WebdavFileInfo>();
                foreach (ISong s in songs)
                {
                    FileInfo songFileInfo = new FileInfo(s.MediaFilePath);
                    string[] fileSplit = s.MediaFilePath.Split('.');
                    string extension = fileSplit[fileSplit.Length -1];
                    WebdavFileInfo webFile = new WebdavFileInfo();
                    webFile.Size = songFileInfo.Length;
                    webFile.IsFile = true;
                    webFile.FileName = String.Format("{0}/{1}/{2}/{3}",
                                                    this.RootDirectoryName, pathElements[1], pathElements[2],
                                                    String.Format("{0} - {1}.{2}", s.AlbumTrack.Value, s.Title, extension));
                    webFile.LastAccessTime = songFileInfo.LastAccessTime;
                    webdavFileInfos.Add(webFile);
                }

                return webdavFileInfos;
            }

            return null;
        }

        public string GetFile(string path)
        {
            string[] pathElements = path.Split('/');

            if (pathElements.Length == 3)
            {
                List<ISong> songs = new List<ISong>();
                var album = MusicLibrary.GetAlbumsByArtist(pathElements[0]).Where<IAlbum>(a => a.Name.Equals(pathElements[1]));
                foreach (IAlbum a in album)
                {
                    songs = songs.Concat<ISong>(a.Songs).ToList<ISong>();
                }
                int startIndex = pathElements[2].IndexOf('-');
                // add the white space
                startIndex = startIndex+1;
                int endIndex = pathElements[2].LastIndexOf('.');
                string songTitle = pathElements[2].Substring(startIndex+1, endIndex - startIndex - 1);
                foreach (ISong s in songs)
                {
                    if (s.Title.Equals(songTitle))
                    {
                        return s.MediaFilePath;

                    }
                }
            }
            return null;
        }

        #endregion
    }


}