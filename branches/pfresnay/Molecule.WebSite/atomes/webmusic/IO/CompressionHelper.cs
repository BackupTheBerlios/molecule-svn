//
// CompressionHelper.cs
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
using System.IO;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using WebMusic.Providers;

namespace WebMusic.IO
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
