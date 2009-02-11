using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Molecule.Web;
using System.Net;
using System.Threading;
using System.Xml;
using System.IO;

namespace WebMusic.CoverArt
{
    public class CoverArtService
    {

        private const string lastFmKey = "a08cec1d1d471b5bed7ad21a1aafcced";
        private const string lastFmUrl = "http://ws.audioscrobbler.com/2.0/{0}";

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(CoverArtService));
        private CoverArtService()
        {
        }

        public static CoverArtService Instance
        {
            get
            {
                return Singleton<CoverArtService>.Instance;
            }
        }

        public string FetchCoverArt(string artist, string albumTitle)
        {

            if (CoverArtSpec.CoverExists(artist, albumTitle))
            {
                return CoverArtSpec.GetPath(CoverArtSpec.CreateArtistAlbumId(artist, albumTitle));
            }

            string lastfmArtist = HttpUtility.UrlEncode(artist);
            string lastfmAlbum = HttpUtility.UrlEncode(albumTitle);

            string parameters = String.Format("?method={0}&api_key={1}&artist={2}&album={3}", "album.getinfo", lastFmKey, lastfmArtist, lastfmAlbum);
            HttpWebRequest request = HttpWebRequest.Create(String.Format(lastFmUrl, parameters)) as HttpWebRequest;
            WebResponse response = null;
            try
            {
                 response = request.GetResponse();
            }
            catch (WebException ex)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error(ex.Message, ex.InnerException);
                }
                return string.Empty;
            }

            XmlReaderSettings settings = new XmlReaderSettings ();
 	        settings.CheckCharacters = false;
            System.Xml.Linq.XDocument xmlDoc = XDocument.Load(XmlReader.Create(response.GetResponseStream(),settings));

            // we will just get the extra large. Perhaps a problem will raise :)
            var result = (from e in xmlDoc.Descendants("image")
                          where ((string)e.Attribute("size")).Equals("extralarge")
                          select (string)e).ToArray<string>();
            if (!string.IsNullOrEmpty(result[0]))
            {
                string destination = CoverArtSpec.GetPath(CoverArtSpec.CreateArtistAlbumId(artist, albumTitle));
                this.saveCoverArt(artist, albumTitle, result[0], destination);
                return destination;
            }
            else
            {
                return string.Empty;
            }
        }

        private void saveCoverArt(string artist, string albumTitle, string sourceUrl, string destination)
        {
            if (sourceUrl != null)
            {
                string destinationDirectory = Directory.GetParent(destination).FullName;
                // if the destination directory doesn't exist
                if( !Directory.Exists( destinationDirectory ))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
                WebClient client = new WebClient();
                using (BinaryWriter bw = new BinaryWriter(File.Open(destination, FileMode.Create)))
                {
                    byte[] data = client.DownloadData(sourceUrl);
                    bw.Write(data, 0, data.Length);
                }
            }
        }

    }
}
