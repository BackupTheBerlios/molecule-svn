using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using WebMusic.Providers;

namespace WebMusic
{
	public class CompressionHelper
	{
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(CompressionHelper));
       
        public static void CompressFiles(IEnumerable<ISong> files, string destinationPath)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug("Starting creation of zip file : " + destinationPath);
            }

            using (ZipOutputStream zipOutputStream = new ZipOutputStream(new FileStream(destinationPath, FileMode.OpenOrCreate)))
            {
                zipOutputStream.SetLevel(0);
                foreach (ISong song in files)
                {

                    FileInfo fileInfo = new FileInfo(song.MediaFilePath);
                    ZipEntry entry = new ZipEntry(song.Artist.Name + "\\" + song.Album.Name + "\\" + song.Title + fileInfo.Extension);
                    zipOutputStream.PutNextEntry(entry);
                    FileStream fs = File.OpenRead(song.MediaFilePath);

                    byte[] buff = new byte[1024];
                    int n = 0;
                    while ((n = fs.Read(buff, 0, buff.Length)) > 0)
                    {
                        zipOutputStream.Write(buff, 0, n);

                    }
                    fs.Close();
                }
                zipOutputStream.Finish();
            }
            if (log.IsDebugEnabled)
            {
                log.Debug("Zip file created : " + destinationPath);
            }
        }
	}
}
