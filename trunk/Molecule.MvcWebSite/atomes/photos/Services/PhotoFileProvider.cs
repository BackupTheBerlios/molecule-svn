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
using Molecule.Web;
using Molecule.Configuration;


namespace WebPhoto.Services
{
    public class PhotoFileProvider
    {
        public static PhotoFileClip ThumbnailClip = PhotoFileClip.Square;

        static ImageCodecInfo jgpEncoder = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);

        const string confQualityLevelKey = "QualityLevel";
        const string confNamespace = "WebPhoto.PhotoFileProvider";
        const int defaultQualityLevel = 65;

        int qualityLevel;

        private PhotoFileProvider()
        {
            qualityLevel = ConfigurationClient.Get<int>(confNamespace, confQualityLevelKey, defaultQualityLevel);
        }

        static PhotoFileProvider instance { get { return Singleton<PhotoFileProvider>.Instance; } }

        public static string GetResizedPhoto(string imagePath, PhotoFileSize size)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException(imagePath);
            PhotoFileClip clip = size == PhotoFileSize.Thumbnail ? ThumbnailClip : PhotoFileClip.No;
            string thumbnailPath = PhotoFileSpec.GetPathForSize(imagePath, size, clip);
            if (!File.Exists(thumbnailPath))
            {
                generatePhotoFile(imagePath, thumbnailPath, size, clip);
            }
            return thumbnailPath;
        }

        public static int QualityLevel
        {
            get { return instance.qualityLevel; }
            set {instance.qualityLevel = value; ConfigurationClient.Set(confNamespace, confQualityLevelKey, value); }
        }

        private static void generatePhotoFile(string imagePath, string resizedPath, PhotoFileSize size,
            PhotoFileClip clip)
        {
            var dir = new DirectoryInfo(Path.GetDirectoryName(resizedPath));
            dir.Create(true);
            using (Bitmap bmp = new Bitmap(imagePath))
            {
                bmp.AutoRotate();
                using (BitmapEx resizedBmp = clip == PhotoFileClip.Square ? bmp.GetSquare((int)size) : bmp.GetResized((int)size))
                {
                    string thumbnailTempPath = resizedPath + ".tmp";
                    var encodeParams = new EncoderParameters(1);
                    encodeParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, instance.qualityLevel);
                    resizedBmp.Bitmap.Save(thumbnailTempPath, jgpEncoder, encodeParams);
                    File.Move(thumbnailTempPath, resizedPath);
                }
            }
        }
    }
}
