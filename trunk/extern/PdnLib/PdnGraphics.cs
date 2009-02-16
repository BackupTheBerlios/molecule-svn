/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) Rick Brewster, Tom Jackson, and past contributors.            //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PaintDotNet.SystemLayer
{
    /// <summary>
    /// These methods are used because we found some bugs in GDI+ / WinForms. Some
    /// were the cause of major flickering with the transparent toolforms.
    /// Other implementations of this class, or more generic implementations, may safely 
    /// thunk straight to equivelants in System.Drawing.Graphics.
    /// </summary>
    public static class PdnGraphics
    {


        private const int screwUpMax = 100;

        /// <summary>
        /// Retrieves an array of rectangles that approximates a region, and computes the
        /// pixel area of it. This method is necessary to work around some bugs in .NET
        /// and to increase performance for the way in which we typically use this data.
        /// </summary>
        /// <param name="region">The Region to retrieve data from.</param>
        /// <param name="scans">An array of Rectangle to put the scans into.</param>
        /// <param name="area">An integer to write the computed area of the region into.</param>
        /// <remarks>
        /// Note to implementors: Simple implementations may simple call region.GetRegionScans()
        /// and process the data for the 'out' variables.</remarks>
        public static void GetRegionScans(Region region, out Rectangle[] scans, out int area)
        {
            using (Matrix matrix = new Matrix ()) {
                RectangleF [] s = region.GetRegionScans (matrix);
                scans = new Rectangle [s.Length];
                area = 0;
                for (int i = 0; i < s.Length; i++){
                    scans [i].X      = (int) s [i].X;
                    scans [i].Y      = (int) s [i].Y;
                    scans [i].Width  = (int) s [i].Width;
                    scans [i].Height = (int) s [i].Height;
                    area += scans[i].Width * scans[i].Height;
                }
            }
        }
    }
}

