using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using PaintDotNet;

namespace Molecule.Drawing
{
    /// <summary>
    /// Hack container : returned bitmap is alias for internal surface data.
    /// need to have an handle to surface data to dispose it correctly.
    /// </summary>
    public class BitmapEx : IDisposable
    {
        public Bitmap Bitmap { get; internal set; }
        internal IDisposable surface { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            if (Bitmap != null)
                Bitmap.Dispose();
            Bitmap = null;
            if (surface != null)
                surface.Dispose();
            surface = null;
        }

        #endregion
    }

    public static class BitmapExtensions
    {
        private static BitmapEx resize(Bitmap bitmap, int width, int height, Rectangle? clipRectangle)
        {
            using (var src = Surface.CopyFromBitmap(bitmap))
            {
                var dest = new Surface(width, height);
                BitmapEx res = new BitmapEx();
                res.surface = dest;
                dest.FitSurface(ResamplingAlgorithm.SuperSampling, src);
                if (clipRectangle == null)
                    res.Bitmap = dest.CreateAliasedBitmap();
                else
                    res.Bitmap = dest.CreateAliasedBitmap(clipRectangle.Value);
                return res;
            }
        }

        private static BitmapEx resize(Bitmap bitmap, int maxSize, bool clipAsSquare)
        {
            double ratio = (double)bitmap.Width / bitmap.Height;
            int destWidth;
            int destHeight;
            Rectangle? clipRectangle = null;
            bool landscape = bitmap.Width > bitmap.Height;
            if (landscape != clipAsSquare)
            {
                destWidth = maxSize;
                destHeight = (int)(destWidth / ratio);
                if (clipAsSquare)
                    clipRectangle = new Rectangle(0, (destHeight - maxSize) / 2, maxSize, maxSize);
            }
            else
            {
                destHeight = maxSize;
                destWidth = (int)(ratio * destHeight);
                if (clipAsSquare)
                    clipRectangle = new Rectangle((destWidth - maxSize) / 2, 0, maxSize, maxSize);
            }
            return resize(bitmap, destWidth, destHeight, clipRectangle);
        }

        public static BitmapEx GetResized(this Bitmap bitmap, int width, int height)
        {
            return resize(bitmap, width, height, null);
        }

        public static BitmapEx GetResized(this Bitmap bitmap, int maxSize)
        {
            return resize(bitmap, maxSize, false);
        }

        public static BitmapEx GetSquare(this Bitmap bitmap, int maxSize)
        {
            return resize(bitmap, maxSize, true);
        }

        const int orientationPropertyId = 274;

        //rotate image regarding Exif orientation metadata
        public static void AutoRotate(this Image img)
        {
            bool orientationInfoPresent = img.PropertyIdList.Contains(orientationPropertyId);

            if (orientationInfoPresent)
            {
                int orientation = BitConverter.ToInt16(img.GetPropertyItem(orientationPropertyId).Value, 0);
                
                RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;

                switch (orientation)
                {
                    case 1:
                        rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                        break;
                    case 2:
                        rotateFlipType = RotateFlipType.RotateNoneFlipY;
                        break;
                    case 3:
                        rotateFlipType = RotateFlipType.Rotate180FlipNone;
                        break;
                    case 4:
                        rotateFlipType = RotateFlipType.Rotate180FlipX;
                        break;
                    case 5:
                        rotateFlipType = RotateFlipType.Rotate90FlipY;
                        break;
                    case 6:
                        rotateFlipType = RotateFlipType.Rotate90FlipNone;
                        break;
                    case 7:
                        rotateFlipType = RotateFlipType.Rotate90FlipX;
                        break;
                    case 8:
                        rotateFlipType = RotateFlipType.Rotate270FlipNone;
                        break;
                }
                img.RotateFlip(rotateFlipType);
            }
        }
    }
}
