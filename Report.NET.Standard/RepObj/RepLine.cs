using Report.NET.Standard.Base;

namespace Root.Reports
{
    /// <summary>Report Line Object.</summary>
    public class RepLine : RepObj
    {
        /// <summary>Pen properties of the line</summary>
        public PenProp penProp;

        /// <summary>Creates a new line object.</summary>
        /// <param name="penProp">Pen properties of the line</param>
        /// <param name="x">X-coordinate of the end of the line, relative to the start point</param>
        /// <param name="y">Y-coordinate of the end of the line, relative to the start point</param>
        public RepLine(PenProp penProp, UnitModel x, UnitModel y)
        {
            this.penProp = penProp.penProp_Registered;
            this.rWidth = x.Point;
            this.rHeight = y.Point;
            oRepObjX = penProp.report.formatter.oCreate_RepLine();
        }
    }
}
