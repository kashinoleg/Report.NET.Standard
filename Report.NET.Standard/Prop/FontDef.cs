using Report.NET.Standard.Fonts;
using System;
using System.Drawing;

namespace Root.Reports
{
    #region Class Documentation
    /// <summary>Font Definition</summary>
    /// <remarks>Each font family must be registered before it can be used. It can be registered only once.</remarks>
    /// <example>Font definition sample:
    /// <code>
    /// class HelloWorld {
    ///   public static void Main() {
    ///     Report report = new Report(new PdfFormatter());
    ///     <b>FontDef fd = new FontDef(report, FontDef.StandardFont.Helvetica)</b>;
    ///     FontProp fp = new FontPropMM(fd, 25);
    ///     Page page = new Page(report);
    ///     page.AddCenteredMM(80, new RepString(fp, "Hello World!"));
    ///     RT.ViewPDF(report, "HelloWorld.pdf");
    ///   }
    /// }
    /// </code>
    /// </example>
    #endregion
    public sealed class FontDef
    {
        #region FontDef
        /// <summary>Report to which this font definition belongs.</summary>
        /// <remarks>A font definition must be assigned to a report.</remarks>
        public readonly ReportBase report;

        /// <summary>Name of the font family</summary>
        /// <remarks>Unique name of the font family. A font can be registered only once.</remarks>
        public readonly string sFontName;

        internal Object oFontDefX = null;

        /// <summary>Array that contains the font data objects of the font definition.</summary>
        /// <remarks>This variable can be used to get the font data object of a font definition.</remarks>
        /// <example>
        /// <code>FontData fontData = fontDef.aFontData[FontDef.Style.Bold];</code>
        /// </example>
        public FontData[] aFontData = new FontData[4];

        /// <summary>Gets the font data object that is identified by the specified style.</summary>
        /// <param name="fontStyle">Style value that identifies the font data object</param>
        /// <value>Font data object that is identified by the specified style</value>
        /// <remarks>If there is no font data object with the specified style, <see langword="null"/> will be returned.</remarks>
        internal FontData this[FontStyle fontStyle] => aFontData[(int)fontStyle];
        #endregion

        #region Conctructors
        /// <summary>Creates a new font definition.</summary>
        /// <param name="report">Report to which this font belongs</param>
        /// <param name="sFontName">Name of the font family</param>
        /// <param name="fontType">Font type</param>
        public FontDef(ReportBase report, StandardFontEnum font)
        {
            this.report = report;
            switch (font)
            {
                case StandardFontEnum.Courier:
                    sFontName = "Courier";
                    aFontData[0] = new Type1FontData(this, sFontName, FontStyle.Regular, new CourierFont());
                    aFontData[1] = new Type1FontData(this, sFontName, FontStyle.Bold, new CourierBoldFont());
                    aFontData[2] = new Type1FontData(this, sFontName, FontStyle.Italic, new CourierObliqueFont());
                    aFontData[3] = new Type1FontData(this, sFontName, FontStyle.Bold | FontStyle.Italic, new CourierBoldObliqueFont());
                    break;
                case StandardFontEnum.Helvetica:
                    sFontName = "Helvetica";
                    aFontData[0] = new Type1FontData(this, sFontName, FontStyle.Regular, new HelveticaFont());
                    aFontData[1] = new Type1FontData(this, sFontName, FontStyle.Bold, new HelveticaBoldFont());
                    aFontData[2] = new Type1FontData(this, sFontName, FontStyle.Italic, new HelveticaObliqueFont());
                    aFontData[3] = new Type1FontData(this, sFontName, FontStyle.Bold | FontStyle.Italic, new HelveticaBoldObliqueFont());
                    break;
                case StandardFontEnum.Symbol:
                    sFontName = "Symbol";
                    aFontData[0] = new Type1FontData(this, sFontName, FontStyle.Regular, new SymbolFont());
                    aFontData[1] = new Type1FontData(this, sFontName, FontStyle.Bold, new SymbolFont());
                    aFontData[2] = new Type1FontData(this, sFontName, FontStyle.Italic, new SymbolFont());
                    aFontData[3] = new Type1FontData(this, sFontName, FontStyle.Bold | FontStyle.Italic, new SymbolFont());
                    break;
                case StandardFontEnum.TimesRoman:
                    sFontName = "Times Roman";
                    aFontData[0] = new Type1FontData(this, sFontName, FontStyle.Regular, new TimesRomanFont());
                    aFontData[1] = new Type1FontData(this, sFontName, FontStyle.Bold, new TimesBoldFont());
                    aFontData[2] = new Type1FontData(this, sFontName, FontStyle.Italic, new TimesItalicFont());
                    aFontData[3] = new Type1FontData(this, sFontName, FontStyle.Bold | FontStyle.Italic, new TimesBoldItalicFont());
                    break;
                case StandardFontEnum.ZapfDingbats:
                    sFontName = "ZapfDingbats";
                    aFontData[0] = new Type1FontData(this, sFontName, FontStyle.Regular, new ZapfDingbatsFont());
                    aFontData[1] = new Type1FontData(this, sFontName, FontStyle.Bold, new ZapfDingbatsFont());
                    aFontData[2] = new Type1FontData(this, sFontName, FontStyle.Italic, new ZapfDingbatsFont());
                    aFontData[3] = new Type1FontData(this, sFontName, FontStyle.Bold | FontStyle.Italic, new ZapfDingbatsFont());
                    break;
            }
            if (report.dict_FontDef.ContainsKey(sFontName))
            {
                throw new ReportException("Font '" + sFontName + "' is already registered");
            }
            report.dict_FontDef.Add(sFontName, this);
        }

        /// <summary>Creates a new font definition.</summary>
        /// <param name="report">Report to which this font belongs</param>
        /// <param name="fontName">Name of the font family</param>
        /// <param name="fontType">Font type</param>
        public FontDef(ReportBase report, String fontName)
        {
            this.report = report;
            this.sFontName = fontName;
            if (report.dict_FontDef.ContainsKey(sFontName))
            {
                throw new ReportException("Font '" + sFontName + "' is already registered");
            }
            report.dict_FontDef.Add(sFontName, this);
        }
        #endregion
    }
}
