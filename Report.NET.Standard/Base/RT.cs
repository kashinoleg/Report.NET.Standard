using System;
using System.Globalization;
using System.Resources;
using System.Text;

namespace Root.Reports
{
    /// <summary>Report Tools Class</summary>
    /// <remarks>This class provides general tools for the Report.NET library.</remarks>
    static public class RT
    {
        #region Static
        /// <summary>Resource manager</summary>
        private static ResourceManager rm = new ResourceManager(typeof(RT));

        /// <summary>Paper size A0 width in millimeters</summary>
        /// <remarks>Width of a A0 page in millimeters: width = 2 ^ -0.25 m</remarks>
        public const Double rA0_WidthMM = 840.89641525371454303112547623321;

        /// <summary>Paper size A0 height in millimeters</summary>
        /// <remarks>Height of a A0 page in millimeters: height = 2 ^ 0.25 m</remarks>
        public const Double rA0_HeightMM = 1189.2071150027210667174999705605;

        /// <summary>Paper size A1 width in millimeters</summary>
        /// <remarks>Width of a A1 page in millimeters.</remarks>
        public const Double rA1_WidthMM = rA0_HeightMM / 2;

        /// <summary>Paper size A1 height in millimeters</summary>
        /// <remarks>Height of a A1 page in millimeters.</remarks>
        public const Double rA1_HeightMM = rA0_WidthMM;

        /// <summary>Paper size A2 width in millimeters</summary>
        /// <remarks>Width of a A2 page in millimeters.</remarks>
        public const Double rA2_WidthMM = rA1_HeightMM / 2;

        /// <summary>Paper size A2 height in millimeters</summary>
        /// <remarks>Height of a A2 page in millimeters.</remarks>
        public const Double rA2_HeightMM = rA1_WidthMM;

        /// <summary>Paper size A3 width in millimeters</summary>
        /// <remarks>Width of a A3 page in millimeters.</remarks>
        public const Double rA3_WidthMM = rA2_HeightMM / 2;

        /// <summary>Paper size A3 height in millimeters</summary>
        /// <remarks>Height of a A3 page in millimeters.</remarks>
        public const Double rA3_HeightMM = rA2_WidthMM;

        /// <summary>Paper size A4 width in millimeters</summary>
        /// <remarks>Width of a A4 page in millimeters.</remarks>
        public const Double rA4_WidthMM = rA3_HeightMM / 2;

        /// <summary>Paper size A4 height in millimeters</summary>
        /// <remarks>Height of a A4 page in millimeters.</remarks>
        public const Double rA4_HeightMM = rA3_WidthMM;

        /// <summary>Paper size A5 width in millimeters</summary>
        /// <remarks>Width of a A5 page in millimeters.</remarks>
        public const Double rA5_WidthMM = rA4_HeightMM / 2;

        /// <summary>Paper size A5 height in millimeters</summary>
        /// <remarks>Height of a A5 page in millimeters.</remarks>
        public const Double rA5_HeightMM = rA4_WidthMM;

        /// <summary>Paper size A6 width in millimeters</summary>
        /// <remarks>Width of a A6 page in millimeters.</remarks>
        public const Double rA6_WidthMM = rA5_HeightMM / 2;

        /// <summary>Paper size A6 height in millimeters</summary>
        /// <remarks>Height of a A6 page in millimeters.</remarks>
        public const Double rA6_HeightMM = rA5_WidthMM;

        /// <summary>Sets the number format for PDF values.</summary>
        static RT()
        {
            cultureInfo_PDF.NumberFormat.NumberDecimalSeparator = ".";
        }

        /// <summary>Determines whether the specified numbers are considered equal.</summary>
        /// <param name="r1">First number to compare</param>
        /// <param name="r2">Second number to compare</param>
        /// <param name="rTolerance">Tolerance</param>
        /// <returns>
        /// <see langword="true"/> if r1 == r2 or if both numbers are <see cref="System.Double.NaN"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// This method compares to double values.
        /// They are considered equal if the differenc between the to values is less than the tolerance value.
        /// </remarks>
        public static Boolean bEquals(Double r1, Double r2, Double rTolerance)
        {
            if (Double.IsNaN(r1))
            {
                return Double.IsNaN(r2);
            }
            if (Double.IsNaN(r2))
            {
                return false;
            }
            return (Math.Abs(r1 - r2) < rTolerance);
        }
        #endregion

        #region Conversion
        /// <summary>Conversion factor: millimeter to point</summary>
        private const Double rMMToPoint = 1.0 / 25.4 * 72.0;

        /// <summary>Conversion factor: point to millimeter</summary>
        private const Double rPointToMM = 1.0 / 72.0 * 25.4;

        /// <summary>Converts millimeters to points (1/72 inch).</summary>
        /// <param name="rMM">Value in millimeters</param>
        /// <returns>value in points (1/72 inch)</returns>
        /// <remarks>This method converts a millimeter value to points.</remarks>
        public static Double rPointFromMM(Double rMM)
        {
            return rMM * rMMToPoint;
        }

        /// <summary>Converts points (1/72 inch) to millimeters.</summary>
        /// <param name="rPoint">Value in points (1/72 inch)</param>
        /// <returns>value in millimeters</returns>
        /// <remarks>This method converts a point value to millimeters.</remarks>
        public static Double rMMFromPoint(Double rPoint)
        {
            return rPoint * rPointToMM;
        }

        /// <summary>Converts degrees to radians.</summary>
        /// <param name="rDegree">Value in degrees</param>
        /// <returns>value in radians</returns>
        internal static Double rRadianFromDegree(Double rDegree)
        {
            Double r = Math.Floor(rDegree / 360.0) * 360.0;  // normalize angle
            rDegree = rDegree - r;
            return rDegree / 180.0 * Math.PI;
        }
        #endregion

        #region PDF
        /// <summary>Culture info for formatting PDF values</summary>
        private static CultureInfo cultureInfo_PDF = new System.Globalization.CultureInfo("en-US");

        /// <summary>Number format string for PDF dimensions</summary>
        private const String sPdfNumberFormat = "0.###";

        /// <summary>Converts a dimension value to the PDF value format.</summary>
        /// <param name="rDim">Dimension value</param>
        /// <returns>Dimension value in the PDF value format</returns>
        internal static String sPdfDim(Double rDim)
        {
            return rDim.ToString(sPdfNumberFormat, cultureInfo_PDF);
        }

        /// <summary>StringBuilder object for use in "sPdfString"</summary>
        private static StringBuilder sb = new StringBuilder(200);

        /// <summary>Converts a string to the PDF text format.</summary>
        /// <param name="sText">String to convert to the PDF text format</param>
        /// <returns>String in the PDF text format</returns>
        internal static String sPdfString(String sText)
        {
            lock (sb)
            {
                sb.Length = 0;
                sb.Append('(');
                for (Int32 i = 0; i < sText.Length; i++)
                {
                    Char c = sText[i];
                    if ((Int32)c == 8364)
                    {
                        sb.Append("\\200");
                        continue;
                    }
                    if (c == '(' || c == ')' || c == '\\')
                    {
                        sb.Append('\\');
                    }
                    sb.Append(c);
                }
                sb.Append(')');
                return sb.ToString();
            }
        }
        #endregion
    }
}
