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
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Molecule.IO;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Molecule.Security;

namespace WebPhoto.Services
{


    public enum PhotoFileClip { No, Square };
    public enum PhotoFileSize : int { Thumbnail = 80, Normal = 640, Large = 1024, Raw };

    /// <remarks>
    /// Does not use thumbnail management standard (http://jens.triq.net/thumbnail-spec/) for many reasons :
    /// - standard only allow 2 size : 128 & 256.
    /// - standard assume that thumbnail contains metadata like original image uri, we should not send it to naviguator.
    /// - we can clip thumbnail to fit a certain rectangle (like a square). This is not compliant with standard.
    /// </remarks>
    public class PhotoFileSpec
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(PhotoFileSpec));

        static PhotoFileSpec()
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Thumbnail path set to {0}", root_path);
            }
        }

        public static bool CoverExistsForSize(string originalFilePath, PhotoFileSize size)
        {
            return CoverExistsForSize(originalFilePath, size, PhotoFileClip.No);
        }

        public static bool CoverExistsForSize(string originalFilePath, PhotoFileSize size, PhotoFileClip clip)
        {
            return File.Exists(GetPathForSize(originalFilePath, size, clip));
        }

        public static string GetPathForSize(string originalFilePath, PhotoFileSize size)
        {
            return GetPathForSize(originalFilePath, size, PhotoFileClip.No);
        }

        public static string GetPathForSize(string originalFilePath, PhotoFileSize size, PhotoFileClip clip)
        {
            var fileid = getIdForFilePath(originalFilePath);
            var folder = size.ToString();
            var folderPath = Path.Combine(root_path, folder);
            if (clip == PhotoFileClip.Square)
                folderPath = Path.Combine(folderPath, "square");

            return Path.Combine(folderPath, String.Format("{0}.jpg", fileid));
        }

        private static string getIdForFilePath(string filePath)
        {
            Uri uri = new Uri(Path.GetFullPath(filePath), UriKind.Absolute);
            return CryptoUtil.Md5Encode(uri.ToString(), Encoding.Default);
        }

        private static string root_path = Path.Combine(XdgBaseDirectorySpec.GetUserDirectory("XDG_CACHE_HOME", ".molecule"), "thumbnails");

        public static string CachePath
        {
            get { return root_path; }
        }

        public static void ClearCache()
        {
            if(Directory.Exists(CachePath))
                Directory.Delete(CachePath, true);
        }
    }
}
