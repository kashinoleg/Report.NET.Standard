﻿using Report.NET.Standard.Base;
using System;
using System.Diagnostics;

namespace Root.Reports
{
    #region IPdfRepObjX
    /// <summary>Interface for the extended data of a RepObj-object when a PDF formatter is used</summary>
    internal interface IPdfRepObjX
    {
        /// <summary>Writes the RepObj to the buffer.</summary>
        /// <param name="e">Environment data</param>
        void Write(PdfIndirectObject_PageContents.Environment e);
    }
    #endregion

    #region PdfContainerX
    /// <summary>Extended PDF Container Class</summary>
    internal sealed class PdfContainerX : IPdfRepObjX
    {
        /// <summary>Singleton instance of this class.</summary>
        internal static readonly PdfContainerX instance = new PdfContainerX();

        /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
        private PdfContainerX()
        {
        }

        /// <summary>Writes the RepObj to the buffer.</summary>
        /// <param name="e">Environment data</param>
        public void Write(PdfIndirectObject_PageContents.Environment e)
        {
            PdfIndirectObject_PageContents p = e.pdfIndirectObject_PageContents;
            Container container = (Container)e.repObj;

            PdfIndirectObject_PageContents.Environment e2 = new PdfIndirectObject_PageContents.Environment();
            e2.report = e.report;
            e2.pdfIndirectObject_PageContents = e.pdfIndirectObject_PageContents;
            foreach (RepObj repObj in container)
            {
                IPdfRepObjX pdfRepObjX = (IPdfRepObjX)repObj.oRepObjX;
                e2.repObj = repObj;
                e2.matrixD = e.matrixD.Clone();
                e2.matrixD.Multiply(repObj.matrixD);
                e2.bComplex = e2.matrixD.bComplex;
                pdfRepObjX.Write(e2);
            }
        }
    }
    #endregion

    #region PdfRepImageX
    /// <summary>Extended PDF Image Class</summary>
    internal sealed class PdfRepImageX : IPdfRepObjX
    {
        /// <summary>Singleton instance of this class.</summary>
        internal static readonly PdfRepImageX instance = new PdfRepImageX();

        /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
        private PdfRepImageX()
        {
        }

        /// <summary>Writes the RepObj to the buffer.</summary>
        /// <param name="e">Environment data</param>
        public void Write(PdfIndirectObject_PageContents.Environment e)
        {
            PdfIndirectObject_PageContents p = e.pdfIndirectObject_PageContents;
            RepImage repImage = (RepImage)e.repObj;
            var rOfsX = repImage.rWidth * repImage.rAlignH;
            var rOfsY = repImage.rHeight * (1 - repImage.rAlignV);
            e.matrixD.Multiply(new MatrixD(1, 0, 0, 1, -rOfsX.Point, rOfsY.Point));
            e.matrixD.Scale(repImage.rWidth.Point, repImage.rHeight.Point);
            p.Command("q");
            p.Write_Matrix(e.matrixD); p.Command("cm");
            PdfIndirectObject_ImageJpeg pdfIndirectObject_ImageJpeg = (PdfIndirectObject_ImageJpeg)repImage.imageData.oImageResourceX;
            p.Name("I" + pdfIndirectObject_ImageJpeg.iObjectNumber); p.Command("Do");
            p.Command("Q");
        }
    }
    #endregion

    #region PdfRepLineX
    /// <summary>Extended PDF Line Class</summary>
    internal sealed class PdfRepLineX : IPdfRepObjX
    {
        /// <summary>Singleton instance of this class.</summary>
        internal static readonly PdfRepLineX instance = new PdfRepLineX();

        /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
        private PdfRepLineX()
        {
        }

        /// <summary>Writes the RepObj to the buffer.</summary>
        /// <param name="e">Environment data</param>
        public void Write(PdfIndirectObject_PageContents.Environment e)
        {
            var p = e.pdfIndirectObject_PageContents;
            var repLine = (RepLine)e.repObj;
            var rOfsX = repLine.rWidth.Point * repLine.rAlignH;
            var rOfsY = repLine.rHeight.Point * repLine.rAlignV;
            e.matrixD.Multiply(1, 0, 0, 1, -rOfsX, -rOfsY);
            if (repLine.penProp.rWidth.Point > 0f)
            {
                p.Write_Pen(repLine.penProp);
                p.Write_Point(e.matrixD.rDX, e.matrixD.rDY);
                p.Command("m");
                p.Write_Point(e.matrixD, repLine.rWidth.Point, repLine.rHeight.Point);
                p.Command("l");
                p.Command("S");
            }
        }
    }
    #endregion

    #region PdfRepArcBaseX
    /// <summary>Extended PDF ArcBase Class</summary>
    internal sealed class PdfRepArcBaseX : IPdfRepObjX
    {
        /// <summary>Singleton instance of this class.</summary>
        internal static readonly PdfRepArcBaseX instance = new PdfRepArcBaseX();

        /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
        private PdfRepArcBaseX()
        {
        }

        /// <summary>Writes the RepObj to the buffer.</summary>
        /// <param name="e">Environment data</param>
        public void Write(PdfIndirectObject_PageContents.Environment e)
        {
            PdfIndirectObject_PageContents p = e.pdfIndirectObject_PageContents;
            RepArcBase repArcBase = (RepArcBase)e.repObj;
            var rOfsX = repArcBase.rWidth.Point * (-repArcBase.rAlignH + 0.5);
            var rOfsY = repArcBase.rHeight.Point * (1 - repArcBase.rAlignV - 0.5);
            e.matrixD.Multiply(new MatrixD(1, 0, 0, 1, rOfsX, rOfsY));

            String sDrawCommand = null;
            if (repArcBase._penProp != null && repArcBase._penProp.rWidth.Point != 0.0)
            {
                p.Write_Pen(repArcBase._penProp);
                if (repArcBase._brushProp != null)
                {
                    p.Write_Brush(repArcBase._brushProp);
                    sDrawCommand = "b";  // close, fill and stroke path
                }
                else
                {
                    sDrawCommand = (repArcBase is RepArc ? "S" : "s");  // stroke : close and stroke path
                }
            }
            else if (repArcBase._brushProp != null)
            {
                p.Write_Brush(repArcBase._brushProp);
                sDrawCommand = "f";  // fill path
            }
            else
            {
                return;
            }

            var rA = repArcBase.rWidth.Point / 2;
            var rA2 = rA * rA;
            var rB = repArcBase.rHeight.Point / 2;
            var rB2 = rB * rB;

            // start point: P0
            Double rAngle0 = RT.rRadianFromDegree(repArcBase._rStartAngle);
            Double rP0X, rP0Y;
            repArcBase.GetEllipseXY(rAngle0, out rP0X, out rP0Y);
            p.Command("q");
            p.Write_Matrix(e.matrixD);
            p.Command("cm");
            if (repArcBase is RepArc || repArcBase is RepCircle || repArcBase is RepEllipse)
            {
                p.Number(rP0X); p.Space(); p.Number(rP0Y);
                p.Command("m");
            }
            else
            {
                p.Number(0); p.Space(); p.Number(0);
                p.Command("m");
                p.Number(rP0X); p.Space(); p.Number(rP0Y);
                p.Command("l");
            }

            Double r = repArcBase._rSweepAngle / 180 * Math.PI;
            Int32 iNumberOfArcs = ((Int32)(r / (Math.PI / 3.0))) + 1;
            Double rSweepAngle = r / iNumberOfArcs;
            for (Int32 iArc = 0; iArc < iNumberOfArcs; iArc++)
            {
                // end point: P3
                Double rAngle3 = rAngle0 + rSweepAngle;
                Double rP3X, rP3Y;
                repArcBase.GetEllipseXY(rAngle3, out rP3X, out rP3Y);

                Double rAngle05 = rAngle0 + rSweepAngle / 2.0;
                Double rMX, rMY;
                repArcBase.GetEllipseXY(rAngle05, out rMX, out rMY);


                Double rP1X, rP2X, rP1Y, rP2Y;
                Double rDenominator = rP0X * rP3Y - rP3X * rP0Y;
                Debug.Assert(!RT.bEquals(rDenominator, 0, 0.0001), "parallel tangents never appears if the sweep angle is less than PI/2");
                if (RT.bEquals(rP0Y, 0, 0.0001))
                {
                    Debug.Assert(!RT.bEquals(rP3Y, 0, 0.0001), "P0 and P3 on x-axis: never appears if the sweep angle is less than PI/2");
                    rP1X = rP0X;
                    rP2X = 8.0 / 3.0 * rMX - 4.0 / 3.0 * rP0X - rP3X / 3.0;
                    rP1Y = 8.0 / 3.0 * rMY - rB2 / rP3Y + rB2 * rP3X * (8.0 * rMX - 4 * rP0X - rP3X) / (3.0 * rA2 * rP3Y) - rP3Y / 3.0;
                    rP2Y = rB2 / rP3Y * (1 - rP2X * rP3X / rA2);
                }
                else if (RT.bEquals(rP3Y, 0, 0.0001))
                {
                    rP1X = 8.0 / 3.0 * rMX - rP0X / 3.0 - 4.0 / 3.0 * rP3X;
                    rP2X = rP3X;
                    rP1Y = rB2 / rP0Y * (1 - rP0X * rP1X / rA2);
                    rP2Y = 8.0 / 3.0 * rMY - rP0Y / 3.0 - rB2 / rP0Y + rB2 * rP0X * (8.0 * rMX - rP0X - 4 * rP3X) / (3.0 * rA2 * rP0Y);
                }
                else
                {
                    rP1X = (3.0 * rA2 * rB2 * (rP0Y + rP3Y)
                      + rA2 * rP0Y * rP3Y * (rP0Y + rP3Y - 8 * rMY)
                      + rB2 * rP3X * rP0Y * (rP0X + rP3X - 8 * rMX))
                      / (3 * rB2 * rDenominator);
                    rP2X = 8.0 / 3.0 * rMX - (rP0X + rP3X) / 3.0 - rP1X;

                    rP1Y = rB2 / rP0Y * (1 - rP0X * rP1X / rA2);
                    rP2Y = rB2 / rP3Y * (1 - rP2X * rP3X / rA2);
                }
                Debug.Assert(RT.bEquals(rMX, rP0X / 8.0 + 3.0 / 8.0 * rP1X + 3.0 / 8.0 * rP2X + rP3X / 8.0, 0.0001));
                Debug.Assert(RT.bEquals(rMY, rP0Y / 8.0 + 3.0 / 8.0 * rP1Y + 3.0 / 8.0 * rP2Y + rP3Y / 8.0, 0.0001));

                p.Number(rP1X); p.Space(); p.Number(rP1Y); p.Space(); p.Number(rP2X); p.Space(); p.Number(rP2Y); p.Space();
                p.Number(rP3X); p.Space(); p.Number(rP3Y);
                p.Command("c");

                rAngle0 += rSweepAngle;
                rP0X = rP3X;
                rP0Y = rP3Y;
            }
            p.Command(sDrawCommand);
            p.Command("Q");
        }
    }
    #endregion

    #region PdfRepRectX
    /// <summary>Extended PDF Rectangle Class</summary>
    internal sealed class PdfRepRectX : IPdfRepObjX
    {
        /// <summary>Singleton instance of this class.</summary>
        internal static readonly PdfRepRectX instance = new PdfRepRectX();

        /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
        private PdfRepRectX()
        {
        }

        /// <summary>Writes the RepObj to the buffer.</summary>
        /// <param name="e">Environment data</param>
        public void Write(PdfIndirectObject_PageContents.Environment e)
        {
            var p = e.pdfIndirectObject_PageContents;
            var repRect = (RepRect)e.repObj;
            var rOfsX = repRect.rWidth.Point * repRect.rAlignH;
            var rOfsY = repRect.rHeight.Point * (1 - repRect.rAlignV);
            e.matrixD.Multiply(1, 0, 0, 1, -rOfsX, rOfsY);
            if (repRect.penProp != null)
            {
                if (repRect.penProp.rWidth.Point > 0f)
                {
                    p.Write_Pen(repRect.penProp);
                }
                else
                {
                    repRect.penProp = null;
                }
            }
            if (repRect.brushProp != null)
            {
                p.Write_Brush(repRect.brushProp);
            }

            if (e.bComplex)
            {
                p.Command("q");
                p.Write_Matrix(e.matrixD);
                p.Command("cm");
                p.Write_Point(0, 0); p.Space(); p.Number(repRect.rWidth.Point); p.Space(); p.Number(repRect.rHeight.Point); p.Space();
                p.Command("re");
                p.Command(repRect.penProp == null ? "f" : (repRect.brushProp == null ? "S" : "B"));
                p.Command("Q");
            }
            else
            {
                p.Write_Point(e.matrixD.rDX, e.matrixD.rDY); p.Space(); p.Number(repRect.rWidth.Point); p.Space(); p.Number(repRect.rHeight.Point);
                p.Command("re");
                p.Command(repRect.penProp == null ? "f" : (repRect.brushProp == null ? "S" : "B"));
            }
        }
    }
    #endregion

    #region PdfRepStringX
    /// <summary>Extended PDF String Class</summary>
    internal sealed class PdfRepStringX : IPdfRepObjX
    {
        /// <summary>Singleton instance of this class.</summary>
        internal static readonly PdfRepStringX instance = new PdfRepStringX();

        /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
        private PdfRepStringX()
        {
        }

        /// <summary>Writes the RepObj to the buffer.</summary>
        /// <param name="e">Environment data</param>
        public void Write(PdfIndirectObject_PageContents.Environment e)
        {
            var p = e.pdfIndirectObject_PageContents;
            var repString = (RepString)e.repObj;
            var rWidth = repString.fontProp.rGetTextWidth(repString.sText);
            var rOfsX = rWidth * repString.rAlignH;
            var rOfsY = repString.fontProp.rSize.Point * (1 - repString.rAlignV);
            e.matrixD.Multiply(new MatrixD(1, 0, 0, 1, -rOfsX, rOfsY));

            p.Command("BT");
            p.Write_Font(repString.fontProp);
            if (e.bComplex)
            {
                p.Write_Matrix(e.matrixD);
                p.Command("Tm");
            }
            else
            {
                p.Write_Point(e.matrixD.rDX, e.matrixD.rDY);
                p.Command("Td");
            }
            p.String(repString.sText);
            p.Command("Tj");
            p.Command("ET");

            if (repString.fontProp.bUnderline)
            {
                var type1FontData = (Type1FontData)repString.fontProp.fontData;
                var rScaleFactor = repString.fontProp.rSizePoint.Point;
                var pp = new PenProp(e.report, new UnitModel() { Point = rScaleFactor * type1FontData.fUnderlineThickness / 1000 }, repString.fontProp.color);
                p.Write_Pen(pp);
                var rD = rScaleFactor * type1FontData.fUnderlinePosition / 1000;
                p.Write_Point(e.matrixD, 0, -rD);
                p.Command("m");
                p.Write_Point(e.matrixD, rWidth, -rD);
                p.Command("l");
                p.Command("S");
            }
        }
    }
    #endregion
}
