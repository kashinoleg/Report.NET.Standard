using System;
using System.Drawing;

namespace Root.Reports
{
    /// <summary>PDF Indirect Object: page contents</summary>
    internal class PdfIndirectObject_PageContents : PdfIndirectObject, IPdfRepObjX
    {
        #region PdfIndirectObject_PageContents
        /// <summary>Page</summary>
        private Page page;

        /// <summary>Creates a page contents indirect object.</summary>
        /// <param name="pdfFormatter">PDF formatter</param>
        internal PdfIndirectObject_PageContents(PdfFormatter pdfFormatter, Page page) : base(pdfFormatter)
        {
            this.page = page;
        }

        /// <summary>Builds the indirect object.</summary>
        internal override void Write()
        {
            //PdfIndirectObject_Page pdfIndirectObject_Page = (PdfIndirectObject_Page)page.oRepObjX;

            StartObj();
            Dictionary_Start();
            Dictionary_Key("Length");
            Space();
            pdfFormatter.FlushBuffer();

            rPenWidth_Cur = -1;
            rPatternOn = -1;
            rPatternOff = -1;
            bColor_rg = false;
            bColor_RG = false;
            sFont_Cur = null;

            Environment e = new Environment();
            e.report = report;
            e.pdfIndirectObject_PageContents = this;
            e.repObj = page;
            e.matrixD = page.matrixD.Clone();
            e.bComplex = e.matrixD.bComplex;
            Write(e);

            pdfFormatter.WriteDirect(sb.Length + ">>\nstream\n");
            Token("endstream");
            EndObj();
        }
        #endregion

        #region Write Properties
        /// <summary>Current pen width</summary>
        private Double rPenWidth_Cur;

        private Double rPatternOn;
        private Double rPatternOff;

        private Boolean bColor_rg;
        private Color color_rg;

        private Boolean bColor_RG;
        private Color color_RG;

        private String sFont_Cur;

        internal class Environment
        {
            internal ReportBase report;
            internal PdfIndirectObject_PageContents pdfIndirectObject_PageContents;
            internal RepObj repObj;
            internal MatrixD matrixD;
            internal Boolean bComplex;
        }

        /// <summary>Prepares the PDF-object structure for a container.</summary>
        private void BuildPageFromContainerX(Container container)
        {
            Environment e = new Environment();
            e.report = report;
            e.pdfIndirectObject_PageContents = this;

            foreach (RepObj repObj in container)
            {
                IPdfRepObjX pdfRepObjX = (IPdfRepObjX)repObj.oRepObjX;
                e.repObj = repObj;
                e.matrixD = container.matrixD.Clone();
                e.matrixD.Multiply(repObj.matrixD);
                e.bComplex = e.matrixD.bComplex;
                pdfRepObjX.Write(e);
            }
        }

        /// <summary>Writes the brush properties to the buffer.</summary>
        /// <param name="brushProp">New brush properties</param>
        internal void Write_Brush(BrushProp brushProp)
        {
            Write_rg(brushProp.color);
        }

        /// <summary>Writes the font properties to the buffer.</summary>
        /// <param name="fontProp">New font properties</param>
        internal void Write_Font(FontProp fontProp)
        {
            Write_rg(fontProp.color);
            FontData fontData = fontProp.fontData;
            PdfIndirectObject_Font pdfIndirectObject_Font = (PdfIndirectObject_Font)fontData.oFontDataX;
            String sFont_New = pdfIndirectObject_Font.iObjectNumber + " " + RT.sPdfDim(fontProp.rSizePoint);
            if (sFont_Cur != sFont_New)
            {
                Name("F" + pdfIndirectObject_Font.iObjectNumber);
                Space();
                Number(fontProp.rSizePoint);
                Command("Tf");
                sFont_Cur = sFont_New;
            }
        }

        /// <summary>Writes the matrix definition to the buffer.</summary>
        /// <param name="m">Trasformation matrix</param>
        internal void Write_Matrix(MatrixD m)
        {
            Number(m.rSX);
            Space();
            Number(-m.rRY);
            Space();
            Number(-m.rRX);
            Space();
            Number(m.rSY);
            Space();
            Write_Point(m.rDX, m.rDY);
        }

        /// <summary>Writes the background color to the buffer.</summary>
        /// <param name="color">New background color</param>
        internal void Write_rg(Color color)
        {
            if (!bColor_rg || !color.Equals(color_rg))
            {
                if (color.R == color.G && color.G == color.B)
                {  // gray
                    Number(color.R / 255F); Command("g");
                }
                else
                {
                    Number(color.R / 255F); Number(color.G / 255F); Number(color.B / 255F); Command("rg");
                }
                bColor_rg = true;
                color_rg = color;
            }
        }

        /// <summary>Writes the absolute coordinates of the specified point to the buffer.</summary>
        /// <param name="rX">X</param>
        /// <param name="rY">Y</param>
        internal void Write_Point(Double rX, Double rY)
        {
            Number(rX);
            Space();
            Number(page.rHeight - rY);
        }

        /// <summary>Applies the geometric transformation represented by the matrix to the point and writes the absolute coordinates to the buffer.</summary>
        /// <param name="m">Trasformation matrix</param>
        /// <param name="rX">X</param>
        /// <param name="rY">Y</param>
        internal void Write_Point(MatrixD m, Double rX, Double rY)
        {
            Write_Point(m.rTransformX(rX, rY), m.rTransformY(rX, rY));
        }

        /// <summary>Writes the pen properties into the buffer.</summary>
        /// <param name="penProp">New pen properties</param>
        internal void Write_Pen(PenProp penProp)
        {
            if (!bColor_RG || !penProp.color.Equals(color_RG))
            {
                if (penProp.color.R == penProp.color.G && penProp.color.G == penProp.color.B)
                {  // gray 
                    WriteLine(RT.sPdfDim(penProp.color.R / 255.0) + " G");
                }
                else
                {
                    WriteLine(RT.sPdfDim(penProp.color.R / 255.0) + " " + RT.sPdfDim(penProp.color.G / 255.0) + " " + RT.sPdfDim(penProp.color.B / 255.0) + " RG");
                }
                bColor_RG = true;
                color_RG = penProp.color;
            }
            if (rPenWidth_Cur != penProp.rWidth)
            {
                WriteLine(RT.sPdfDim(penProp.rWidth) + " w");
                rPenWidth_Cur = penProp.rWidth;
            }
            if (rPatternOn != penProp.rPatternOn || rPatternOff != penProp.rPatternOff)
            {
                if (penProp.rPatternOff == 0)
                {
                    WriteLine("[] 0 d");
                }
                else
                {
                    if (penProp.rPatternOn == penProp.rPatternOff)
                    {
                        WriteLine("[" + RT.sPdfDim(penProp.rPatternOn) + "] 0 d");
                    }
                    else
                    {
                        WriteLine("[" + RT.sPdfDim(penProp.rPatternOn) + " " + RT.sPdfDim(penProp.rPatternOff) + "] 0 d");
                    }
                }
            }
        }

        #region IPdfRepObjX Members
        /// <summary>Writes the RepObj to the buffer.</summary>
        /// <param name="e">Environment data</param>
        public void Write(PdfIndirectObject_PageContents.Environment e)
        {
            PdfContainerX.instance.Write(e);
        }
        #endregion
    }
    #endregion
}
