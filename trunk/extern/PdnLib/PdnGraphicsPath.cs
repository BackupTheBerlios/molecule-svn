/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) Rick Brewster, Tom Jackson, and past contributors.            //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using PaintDotNet.SystemLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace PaintDotNet
{ 
    [Serializable]
    public sealed class PdnGraphicsPath
        : MarshalByRefObject,
          ICloneable,
          IDisposable,
          ISerializable
    { 
        private GraphicsPath gdiPath;
        private bool tooComplex = false;
        internal PdnRegion regionCache = null;

        public static implicit operator GraphicsPath(PdnGraphicsPath convert)
        {
            return convert.gdiPath;
        }

        internal PdnRegion GetRegionCache()
        {
            if (regionCache == null)
            {
                regionCache = new PdnRegion(this.gdiPath);
            }

            return regionCache;
        }

        private GraphicsPath GdiPath
        {
            get 
            { 
                return gdiPath; 
            }
        }

        private void Changed()
        {
            if (regionCache != null)
            {
                lock (regionCache.SyncRoot)
                {
                    regionCache.Dispose();
                    regionCache = null;
                }
            }
        }

        public PdnGraphicsPath()
        {
            Changed();
            gdiPath = new GraphicsPath();
        }

        public PdnGraphicsPath(GraphicsPath wrapMe)
        {
            Changed();
            gdiPath = wrapMe;
        }

        public PdnGraphicsPath(FillMode fillMode)
        {
            Changed();
            gdiPath = new GraphicsPath(fillMode);
        }

        public PdnGraphicsPath(Point[] pts, byte[] types)
        {
            Changed();
            gdiPath = new GraphicsPath(pts, types);
        }

        public PdnGraphicsPath(PointF[] pts, byte[] types)
        {
            Changed();
            gdiPath = new GraphicsPath(pts, types);
        }

        public PdnGraphicsPath(Point[] pts, byte[] types, FillMode fillMode)
        {
            Changed();
            gdiPath = new GraphicsPath(pts, types, fillMode);
        }

        public PdnGraphicsPath(PointF[] pts, byte[] types, FillMode fillMode)
        {
            Changed();
            gdiPath = new GraphicsPath(pts, types, fillMode);
        }

        public PdnGraphicsPath(SerializationInfo info, StreamingContext context)
        {
            int ptCount = info.GetInt32("ptCount");

            PointF[] pts;
            byte[] types;
            
            if (ptCount == 0)
            {
                pts = new PointF[0];
                types = new byte[0];
            }
            else
            {
                pts = (PointF[])info.GetValue("pts", typeof(PointF[]));
                types = (byte[])info.GetValue("types", typeof(byte[]));
            }
            
            FillMode fillMode = (FillMode)info.GetValue("fillMode", typeof(FillMode));
            Changed();

            if (ptCount == 0)
            {
                gdiPath = new GraphicsPath();
            }
            else
            {
                gdiPath = new GraphicsPath(pts, types, fillMode);
            }

            this.tooComplex = false;
            this.regionCache = null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            lock (this) // avoid race condition with Dispose()
            {
                info.AddValue("ptCount", this.gdiPath.PointCount);

                if (this.gdiPath.PointCount > 0)
                {
                    info.AddValue("pts", this.gdiPath.PathPoints);
                    info.AddValue("types", this.gdiPath.PathTypes);
                }

                info.AddValue("fillMode", this.gdiPath.FillMode);
            }
        }

        
        ~PdnGraphicsPath()
        {
            Changed();
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        { 
            if (disposing)
            {
                lock (this) // avoid race condition with GetObjectData()
                {
                    if (gdiPath != null)
                    {
                        gdiPath.Dispose();
                        gdiPath = null;
                    }

                    if (regionCache != null)
                    {
                        regionCache.Dispose();
                        regionCache = null;
                    }
                }
            }
        }

        public FillMode FillMode
        { 
            get 
            { 
                return gdiPath.FillMode; 
            }

            set 
            { 
                Changed(); 
                gdiPath.FillMode = value; 
            }
        }

        public PathData PathData
        { 
            get 
            { 
                return gdiPath.PathData; 
            }
        }

        public PointF[] PathPoints
        { 
            get 
            { 
                return gdiPath.PathPoints; 
            }
        }

        public byte[] PathTypes
        { 
            get 
            { 
                return gdiPath.PathTypes; 
            }
        }

        public int PointCount
        { 
            get 
            { 
                return gdiPath.PointCount; 
            }
        }

        public void AddArc(Rectangle rect, float startAngle, float sweepAngle)
        {
            Changed();
            gdiPath.AddArc(rect, startAngle, sweepAngle);
        }

        public void AddArc(RectangleF rectF, float startAngle, float sweepAngle)
        {
            Changed();
            gdiPath.AddArc(rectF, startAngle, sweepAngle);
        }

        public void AddArc(int x, int y, int width, int height, float startAngle, float sweepAngle)
        {
            Changed();
            gdiPath.AddArc(x, y, width, height, startAngle, sweepAngle);
        }

        public void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            Changed();
            gdiPath.AddArc(x, y, width, height, startAngle, sweepAngle);
        }

        public void AddBezier(Point pt1, Point pt2, Point pt3, Point pt4)
        {
            Changed();
            gdiPath.AddBezier(pt1, pt2, pt3, pt4);
        }

        public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
        {
            Changed();
            gdiPath.AddBezier(pt1, pt2, pt3, pt4);
        }

        public void AddBezier(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            Changed();
            gdiPath.AddBezier(x1, y1, x2, y2, x3, y3, x4, y4);
        }

        public void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            Changed();
            gdiPath.AddBezier(x1, y1, x2, y2, x3, y3, x4, y4);
        }

        public void AddBeziers(Point[] points)
        {
            Changed();
            gdiPath.AddBeziers(points);
        }

        public void AddBeziers(PointF[] points)
        {
            Changed();
            gdiPath.AddBeziers(points);
        }

        public void AddClosedCurve(Point[] points)
        {
            Changed();
            gdiPath.AddClosedCurve(points);
        }

        public void AddClosedCurve(PointF[] points)
        {
            Changed();
            gdiPath.AddClosedCurve(points);
        }

        public void AddClosedCurve(Point[] points, float tension)
        {
            Changed();
            gdiPath.AddClosedCurve(points, tension);
        }

        public void AddClosedCurve(PointF[] points, float tension)
        {
            Changed();
            gdiPath.AddClosedCurve(points, tension);
        }

        public void AddCurve(Point[] points)
        {
            Changed();
            gdiPath.AddCurve(points);
        }

        public void AddCurve(PointF[] points)
        {
            Changed();
            gdiPath.AddCurve(points);
        }

        public void AddCurve(Point[] points, float tension)
        {
            Changed();
            gdiPath.AddCurve(points, tension);
        }

        public void AddCurve(PointF[] points, float tension)
        {
            Changed();
            gdiPath.AddCurve(points, tension);
        }

        public void AddCurve(Point[] points, int offset, int numberOfSegments, float tension)
        {
            Changed();
            gdiPath.AddCurve(points, offset, numberOfSegments, tension);
        }

        public void AddCurve(PointF[] points, int offset, int numberOfSegments, float tension)
        {
            Changed();
            gdiPath.AddCurve(points, offset, numberOfSegments, tension);
        }

        public void AddEllipse(Rectangle rect)
        {
            Changed();
            gdiPath.AddEllipse(rect);
        }

        public void AddEllipse(RectangleF rectF)
        {
            Changed();
            gdiPath.AddEllipse(rectF);
        }

        public void AddEllipse(int x, int y, int width, int height)
        {
            Changed();
            gdiPath.AddEllipse(x, y, width, height);
        }

        public void AddEllipse(float x, float y, float width, float height)
        {
            Changed();
            gdiPath.AddEllipse(x, y, width, height);
        }

        public void AddLine(Point pt1, Point pt2)
        {
            Changed();
            gdiPath.AddLine(pt1, pt2);
        }

        public void AddLine(PointF pt1, PointF pt2)
        {
            Changed();
            gdiPath.AddLine(pt1, pt2);
        }

        public void AddLine(int x1, int y1, int x2, int y2)
        {
            Changed();
            gdiPath.AddLine(x1, y1, x2, y2);
        }

        public void AddLine(float x1, float y1, float x2, float y2)
        {
            Changed();
            gdiPath.AddLine(x1, y1, x2, y2);
        }

        public void AddLines(Point[] points)
        {
            Changed();
            gdiPath.AddLines(points);
        }

        public void AddLines(PointF[] points)
        {
            Changed();
            gdiPath.AddLines(points);
        }

        public void AddPath(GraphicsPath addingPath, bool connect)
        {
            if (addingPath.PointCount != 0)
            {
                Changed();
                gdiPath.AddPath(addingPath, connect);
            }
        }

        public void AddPie(Rectangle rect, float startAngle, float sweepAngle)
        {
            Changed();
            gdiPath.AddPie(rect, startAngle, sweepAngle);
        }

        public void AddPie(int x, int y, int width, int height, float startAngle, float sweepAngle)
        {
            Changed();
            gdiPath.AddPie(x, y, width, height, startAngle, sweepAngle);
        }

        public void AddPie(float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            Changed();
            gdiPath.AddPie(x, y, width, height, startAngle, sweepAngle);
        }

        public void AddPolygon(Point[] points)
        {
            Changed();
            gdiPath.AddPolygon(points);
        }

        public void AddPolygon(PointF[] points)
        {
            Changed();
            gdiPath.AddPolygon(points);
        }

        public void AddPolygons(PointF[][] polygons)
        {
            foreach (PointF[] polygon in polygons)
            {
                AddPolygon(polygon);
                CloseFigure();
            }
        }

        public void AddPolygons(Point[][] polygons)
        {
            foreach (Point[] polygon in polygons)
            {
                AddPolygon(polygon);
                CloseFigure();
            }
        }

        public void AddRectangle(Rectangle rect)
        {
            Changed();
            gdiPath.AddRectangle(rect);
        }

        public void AddRectangle(RectangleF rectF)
        {
            Changed();
            gdiPath.AddRectangle(rectF);
        }

        public void AddRectangles(Rectangle[] rects)
        {
            Changed();
            gdiPath.AddRectangles(rects);
        }

        public void AddRectangles(RectangleF[] rectsF)
        {
            Changed();
            gdiPath.AddRectangles(rectsF);
        }

        public void AddString(string s, FontFamily family, int style, float emSize, Point origin, StringFormat format)
        {
            Changed();
            gdiPath.AddString(s, family, style, emSize, origin, format);
        }

        public void AddString(string s, FontFamily family, int style, float emSize, PointF origin, StringFormat format)
        {
            Changed();
            gdiPath.AddString(s, family, style, emSize, origin, format);
        }

        public void AddString(string s, FontFamily family, int style, float emSize, Rectangle layoutRect, StringFormat format)
        {
            Changed();
            gdiPath.AddString(s, family, style, emSize, layoutRect, format);
        }

        public void AddString(string s, FontFamily family, int style, float emSize, RectangleF layoutRect, StringFormat format)
        {
            Changed();
            gdiPath.AddString(s, family, style, emSize, layoutRect, format);
        }

        public void ClearMarkers()
        {
            Changed();
            gdiPath.ClearMarkers();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public PdnGraphicsPath Clone()
        {
            PdnGraphicsPath path = new PdnGraphicsPath((GraphicsPath)gdiPath.Clone());
            path.tooComplex = this.tooComplex;
            return path;
        }

        public void CloseAllFigures()
        {
            Changed();
            gdiPath.CloseAllFigures();
        }

        public void CloseFigure()
        {
            Changed();
            gdiPath.CloseFigure();
        }

        //public void Draw(Graphics g, Pen pen)
        //{
        //    Draw(g, pen, false);
        //}

        ///// <summary>
        ///// Draws the path to the given Graphics context using the given Pen.
        ///// </summary>
        ///// <param name="g">The Graphics context to draw to.</param>
        ///// <param name="pen">The Pen to draw with.</param>
        ///// <param name="presentationIntent">
        ///// If true, gives a hint that the path is being drawn to be presented to the user.
        ///// </param>
        ///// <remarks>
        ///// If the path is "too complex," and if presentationIntent is true, then the path will
        ///// not be drawn. To force the path to be drawn, set presentationIntent to false.
        ///// </remarks>
        //public void Draw(Graphics g, Pen pen, bool presentationIntent)
        //{
        //    try
        //    {
        //        if (!tooComplex || !presentationIntent)
        //        {
        //            int start = Environment.TickCount;
        //            g.DrawPath(pen, this.gdiPath);
        //            int end = Environment.TickCount;

        //            if ((end - start) > 1000)
        //            {
        //                tooComplex = true;
        //            }
        //        }
        //    }

        //    catch (OutOfMemoryException ex)
        //    {
        //        tooComplex = true;
        //        Tracing.Ping("DrawPath exception: " + ex);
        //    }
        //}

        public void Flatten()
        {
            Changed();
            gdiPath.Flatten();
        }

        public void Flatten(Matrix matrix)
        {
            Changed();
            gdiPath.Flatten(matrix);
        }

        public void Flatten(Matrix matrix, float flatness)
        {
            Changed();
            gdiPath.Flatten(matrix, flatness);
        }

        public RectangleF GetBounds2()
        {
            if (this.PointCount == 0)
            {
                return RectangleF.Empty;
            }

            PointF[] points = this.PathPoints;

            if (points.Length == 0)
            {
                return RectangleF.Empty;
            }

            float left = points[0].X;
            float right = points[0].X;
            float top = points[0].Y;
            float bottom = points[0].Y;

            for (int i = 1; i < points.Length; ++i)
            {
                if (points[i].X < left)
                {
                    left = points[i].X;
                }

                if (points[i].Y < top)
                {
                    top = points[i].Y;
                }

                if (points[i].X > right)
                {
                    right = points[i].X;
                }

                if (points[i].Y > bottom)
                {
                    bottom = points[i].Y;
                }
            }

            return RectangleF.FromLTRB(left, top, right, bottom);
        }

        public RectangleF GetBounds()
        {
            return gdiPath.GetBounds();
        }

        public RectangleF GetBounds(Matrix matrix)
        {
            return gdiPath.GetBounds(matrix);
        }

        public RectangleF GetBounds(Matrix matrix, Pen pen)
        {
            return gdiPath.GetBounds(matrix, pen);
        }

        public PointF GetLastPoint()
        {
            return gdiPath.GetLastPoint();
        }

        public bool IsEmpty
        {
            get
            {
                return this.PointCount == 0;
            }
        }

        public bool IsOutlineVisible(Point point, Pen pen)
        {
            return gdiPath.IsOutlineVisible(point, pen);
        }

        public bool IsOutlineVisible(PointF point, Pen pen)
        {
            return gdiPath.IsOutlineVisible(point, pen);
        }

        public bool IsOutlineVisible(int x, int y, Pen pen)
        {
            return gdiPath.IsOutlineVisible(x, y, pen);
        }

        public bool IsOutlineVisible(Point point, Pen pen, Graphics g)
        {
            return gdiPath.IsOutlineVisible(point, pen, g);
        }

        public bool IsOutlineVisible(PointF point, Pen pen, Graphics g)
        {
            return gdiPath.IsOutlineVisible(point, pen, g);
        }

        public bool IsOutlineVisible(float x, float y, Pen pen)
        {
            return gdiPath.IsOutlineVisible(x, y, pen);
        }

        public bool IsOutlineVisible(int x, int y, Pen pen, Graphics g)
        {
            return gdiPath.IsOutlineVisible(x, y, pen, g);
        }

        public bool IsOutlineVisible(float x, float y, Pen pen, Graphics g)
        {
            return gdiPath.IsOutlineVisible(x, y, pen, g);
        }

        public bool IsVisible(Point point)
        {
            return gdiPath.IsVisible(point);
        }

        public bool IsVisible(PointF point)
        {
            return gdiPath.IsVisible(point);
        }

        public bool IsVisible(int x, int y)
        {
            return gdiPath.IsVisible(x, y);
        }

        public bool IsVisible(Point point, Graphics g)
        {
            return gdiPath.IsVisible(point, g);
        }

        public bool IsVisible(PointF point, Graphics g)
        {
            return gdiPath.IsVisible(point, g);
        }

        public bool IsVisible(float x, float y)
        {
            return gdiPath.IsVisible(x, y);
        }

        public bool IsVisible(int x, int y, Graphics g)
        {
            return gdiPath.IsVisible(x, y, g);
        }

        public bool IsVisible(float x, float y, Graphics g)
        {
            return gdiPath.IsVisible(x, y, g);
        }
        
        public void Reset()
        {
            Changed();
            this.tooComplex = false;
            gdiPath.Reset();
        }

        public void Reverse()
        {
            Changed();
            gdiPath.Reverse();
        }

        public void SetMarkers()
        {
            Changed();
            gdiPath.SetMarkers();
        }

        public void StartFigure()
        {
            Changed();
            gdiPath.StartFigure();
        }

        public void Transform(Matrix matrix)
        {
            Changed();
            gdiPath.Transform(matrix);
        }

        public void Warp(PointF[] destPoints, RectangleF srcRect)
        {
            Changed();
            gdiPath.Warp(destPoints, srcRect);
        }

        public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix)
        {
            Changed();
            gdiPath.Warp(destPoints, srcRect, matrix);
        }

        public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix, WarpMode warpMode)
        {
            Changed();
            gdiPath.Warp(destPoints, srcRect, matrix, warpMode);
        }

        public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix, WarpMode warpMode, float flatness)
        {
            Changed();
            gdiPath.Warp(destPoints, srcRect, matrix, warpMode, flatness);
        }

        public void Widen(Pen pen)
        {
            Changed();
            gdiPath.Widen(pen);
        }

        public void Widen(Pen pen, Matrix matrix)
        {
            Changed();
            gdiPath.Widen(pen, matrix);
        }

        public void Widen(Pen pen, Matrix matrix, float flatness)
        {
            Changed();
            gdiPath.Widen(pen, matrix, flatness);
        }
    }
}
