using System;
using System.Drawing;

namespace Root.Reports
{
    #region PdfIndirectObject_Font
    /// <summary>PDF Indirect Object: Font</summary>
    /// <remarks>Each font data object that is used in the PDF document must point to an object of this type (FontData.oFontDataX).</remarks>
    internal abstract class PdfIndirectObject_Font : PdfIndirectObject
    {
        /// <summary>Font data</summary>
        protected readonly FontData fontData;

        internal readonly String sKey;

        /// <summary>This variable allows a quick test, whether the font properties are registered for the current page.
        /// If <c>pdfPageData_Registered</c> contains the current page, then it has been registered before.</summary>
        internal PdfIndirectObject_Page pdfIndirectObject_Page;

        /// <summary>Creates a font indirect object.</summary>
        /// <param name="pdfFormatter">PDF formatter</param>
        /// <param name="fontProp">Font property</param>
        internal PdfIndirectObject_Font(PdfFormatter pdfFormatter, FontData fontData) : base(pdfFormatter)
        {
            this.fontData = fontData;

            sKey = fontData.fontDef.sFontName;
            if ((fontData.fontStyle & FontStyle.Bold) > 0)
            {
                sKey += ";B";
            }
            if ((fontData.fontStyle & FontStyle.Italic) > 0)
            {
                sKey += ";I";
            }
        }
    }
    #endregion

    #region PdfIndirectObject_Font_Type1
    /// <summary>PDF Indirect Object: Font Type1</summary>
    internal sealed class PdfIndirectObject_Font_Type1 : PdfIndirectObject_Font
    {
        /// <summary>Creates a font indirect object for a Type1 font.</summary>
        /// <param name="pdfFormatter">PDF formatter</param>
        /// <param name="type1FontData">Type1 font data</param>
        internal PdfIndirectObject_Font_Type1(PdfFormatter pdfFormatter, Type1FontData type1FontData)
          : base(pdfFormatter, type1FontData)
        {
        }

        /// <summary>Writes the object to the buffer.</summary>
        internal override void Write()
        {
            Type1FontData type1FontData = (Type1FontData)fontData;
            StartObj();
            Dictionary_Start();
            Dictionary_Key("Type"); Name("Font");
            Dictionary_Key("Subtype"); Name("Type1");
            Dictionary_Key("BaseFont"); Name(type1FontData.sFontName);
            if (type1FontData.sFamilyName != "ZapfDingbats" && type1FontData.sFamilyName != "Symbol")
            {
                Dictionary_Key("Encoding"); Name("WinAnsiEncoding");
            }
            Dictionary_End();
            EndObj();
        }
    }
    #endregion
}
