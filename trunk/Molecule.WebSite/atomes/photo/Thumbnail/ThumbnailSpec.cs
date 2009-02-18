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

namespace WebPhoto.Thumbnail
{

    public enum ThumbnailSize { Normal, Large };

    public class ThumbnailSpec
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(ThumbnailSpec));

        static ThumbnailSpec()
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Thumbnail path set to {0}", root_path);
            }
        }

        public static bool CoverExistsForSize(string originalFilePath, ThumbnailSize size)
        {
            return File.Exists(GetPathForSize(originalFilePath, size));
        }

        public static string GetPath(string originalFilePath)
        {
            return GetPathForSize(originalFilePath, 0);
        }

        public static string GetPathForSize(string originalFilePath, ThumbnailSize size)
        {
            var fileid = getIdForFilePath(originalFilePath);
            var folder = size == ThumbnailSize.Normal ? "normal" : "large";
            var folderPath = Path.Combine(root_path, folder);
            return Path.Combine(folder, String.Format("{0}.png",fileid));
        }

        private static string getIdForFilePath(string filePath)
        {
            Uri uri = new Uri(Path.GetFullPath(filePath), UriKind.Absolute);
            return CryptoUtil.Md5Encode(uri.ToString(), Encoding.Default);
        }

        private static string root_path = XdgBaseDirectorySpec.GetUserDirectory("XDG_CACHE_HOME", ".thumbnails");

        public static string RootPath
        {
            get { return root_path; }
        }
    }
}
