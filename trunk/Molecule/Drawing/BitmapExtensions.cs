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
        public static Bitmap GetResized(this Bitmap bitmap, int width, int height)
        {
            var src = Surface.CopyFromBitmap(bitmap);
            var dest = new Surface(width, height);
            dest.FitSurface(ResamplingAlgorithm.SuperSampling, src);
            return dest.CreateAliasedBitmap();
        }

        public static Bitmap GetResized(this Bitmap bitmap, int maxSize)
        {
            double ratio = bitmap.Width / bitmap.Height;
            int destWidth;
            int destHeight;
            if (bitmap.Width > bitmap.Height)
            {
                destWidth = maxSize;
                destHeight = destWidth / ratio;
            }
            else
            {
                destHeight = maxSize;
                destWidth = ratio * destHeight;
            }
            return bitmap.GetResized(destWidth, destHeight);
        }
    }
}
