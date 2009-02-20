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
using System.IO;
using System.Drawing;
using Molecule.Drawing;
using System.Drawing.Imaging;
using Molecule.IO;
using Mono.Rocks;


namespace WebPhoto.Services
{
    public class ThumbnailProvider
    {
        static ThumbnailClip clip = ThumbnailClip.Square;

        public static string GetThumbnailForImage(string imagePath, int size)
        {
            string thumbnailPath = ThumbnailSpec.GetPathForSize(imagePath, size, clip);
            if (!File.Exists(thumbnailPath))
            {
                generateThumbnail(imagePath, thumbnailPath, size, clip);
            }
            return thumbnailPath;
        }

        private static void generateThumbnail(string imagePath, string thumbnailPath, int size, ThumbnailClip clip)
        {
            var dir = new DirectoryInfo(Path.GetDirectoryName(thumbnailPath));
            dir.Create(true);
            Bitmap bmp = new Bitmap(imagePath);
            Bitmap resizedBmp = clip == ThumbnailClip.Square ? bmp.GetSquare((int)size) : bmp.GetResized((int)size);
            string thumbnailTempPath = thumbnailPath + ".tmp";
            resizedBmp.Save(thumbnailTempPath, ImageFormat.Png);
            File.Move(thumbnailTempPath, thumbnailPath);
        }
    }
}
