//
// Copyright (c) 2009 Pascal Fresnay (dev.molecule@free.fr) - Mickael Renault (dev.molecule@free.fr) 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using Molecule.Web;
using System.Net;
using System.Threading;
using System.Xml;
using System.IO;
using Molecule.Drawing;
using System.Drawing.Imaging;

namespace WebMusic.CoverArt
{
    public class CoverArtService
    {
        static CoverArtService()
        {
            
        }

        private const string lastFmKey = "a08cec1d1d471b5bed7ad21a1aafcced";
        private const string lastFmUrl = "http://ws.audioscrobbler.com/2.0/{0}";
        private const string coverSizeName = "large";
		private const int imageSize = 100;
		
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(CoverArtService));
        public static string FetchCoverArt(string artist, string albumTitle)
        {
			string aaid = CoverArtSpec.CreateArtistAlbumId(artist, albumTitle);
			
            if (CoverArtSpec.CoverExistsForSize(aaid,imageSize))
            {
                return CoverArtSpec.GetPathForSize(aaid, imageSize);
            }

            string lastfmArtist = HttpUtility.UrlEncode(artist);
            string lastfmAlbum = HttpUtility.UrlEncode(albumTitle);

            string parameters = String.Format("?method={0}&api_key={1}&artist={2}&album={3}", "album.getinfo", lastFmKey, lastfmArtist, lastfmAlbum);
			Console.WriteLine(String.Format(lastFmUrl, parameters));
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

            // we will just get the medium size. Perhaps a problem will raise :)
            var result = (from e in xmlDoc.Descendants("image")
                          where ((string)e.Attribute("size")).Equals(coverSizeName)
                          select (string)e).ToArray<string>();
            if (!string.IsNullOrEmpty(result[0]))
            {
                string destination = CoverArtSpec.GetPathForSize(CoverArtSpec.CreateArtistAlbumId(artist, albumTitle),imageSize);			
				
				saveCoverArt(artist, albumTitle, result[0], destination);
                return destination;
            }
            else
            {
                return string.Empty;
            }
        }

        private static void saveCoverArt(string artist, string albumTitle, string sourceUrl, string destination)
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
                Stream imgStream;
                try
                {
                    imgStream = client.OpenRead(sourceUrl);
                }
                catch (WebException ex)
                {
                    if (log.IsErrorEnabled)
                        log.Error(ex.Message, ex.InnerException);
                    return;
                }

                Bitmap bmp = new Bitmap(System.Drawing.Image.FromStream(imgStream));
                Bitmap resizedBmp = bmp.GetResized(imageSize);
                resizedBmp.Save(destination, ImageFormat.Jpeg);
            }
        }

    }
}
