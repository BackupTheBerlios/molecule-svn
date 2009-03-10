using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using PaintDotNet;

namespace Molecule.Drawing
{
    public static class BitmapExtensions
    {
        private static Bitmap resize(Bitmap bitmap, int width, int height, Rectangle? clipRectangle)
        {
            var src = Surface.CopyFromBitmap(bitmap);
            var dest = new Surface(width, height);
            dest.FitSurface(ResamplingAlgorithm.SuperSampling, src);
            if(clipRectangle == null)
                return dest.CreateAliasedBitmap();
            else
                return dest.CreateAliasedBitmap(clipRectangle.Value);
        }

        private static Bitmap resize(Bitmap bitmap, int maxSize, bool clipAsSquare)
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

        public static Bitmap GetResized(this Bitmap bitmap, int width, int height)
        {
            return resize(bitmap, width, height, null);
        }

        public static Bitmap GetResized(this Bitmap bitmap, int maxSize)
        {
            return resize(bitmap, maxSize, false);
        }

        public static Bitmap GetSquare(this Bitmap bitmap, int maxSize)
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
