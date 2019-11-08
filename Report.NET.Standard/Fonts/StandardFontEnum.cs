namespace Report.NET.Standard.Fonts
{
    #region StandardFont
    //----------------------------------------------------------------------------------------------------

    /// <summary>Predefined standard fonts</summary>
    /// <remarks>
    /// The standard fonts are supported by the viewer and must not be embedded in the PDF file.
    /// <seealso cref="Root.Reports.FontDef"/>
    /// </remarks>
    /// <example>Definition of a standard font:
    /// <code>
    ///   FontDef fd = new FontDef(report, <b>FontDef.StandardFont.Helvetica</b>);
    /// </code>
    /// </example>
    public enum StandardFontEnum
    {
        /// <summary>Standard base 14 type 1 font "Courier"</summary>
        Courier,
        /// <summary>Standard base 14 type 1 font "Helvetica"</summary>
        Helvetica,
        /// <summary>Standard base 14 type 1 font "Symbol"</summary>
        Symbol,
        /// <summary>Standard base 14 type 1 font "Times-Roman"</summary>
        TimesRoman,
        /// <summary>Standard base 14 type 1 font "ZapfDingbats"</summary>
        ZapfDingbats
    }
    #endregion
}
