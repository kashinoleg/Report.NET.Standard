using System;

namespace Root.Reports
{
    /// <summary>Report Line Object.</summary>
    public class RepLine : RepObj
    {
        /// <summary>Pen properties of the line</summary>
        public PenProp penProp;

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Creates a new line object.</summary>
        /// <param name="penProp">Pen properties of the line</param>
        /// <param name="rX">X-coordinate of the end of the line, relative to the start point</param>
        /// <param name="rY">Y-coordinate of the end of the line, relative to the start point</param>
        public RepLine(PenProp penProp, Double rX, Double rY)
        {
            this.penProp = penProp.penProp_Registered;
            this.rWidth = rX;
            this.rHeight = rY;
            oRepObjX = penProp.report.formatter.oCreate_RepLine();
        }

    }

    //****************************************************************************************************
    /// <summary>Report Line Object with millimeter values.</summary>
    public class RepLineMM : RepLine
    {
        //----------------------------------------------------------------------------------------------------x
        /// <summary>Creates a new line object with millimeter values</summary>
        /// <param name="penProp">Pen properties of the line</param>
        /// <param name="rX">X-coordinate of the end of the line, relative to the start point, in millimeter</param>
        /// <param name="rY">Y-coordinate of the end of the line, relative to the start point, in millimeter</param>
        public RepLineMM(PenProp penProp, Double rX, Double rY) : base(penProp, RT.rPointFromMM(rX), RT.rPointFromMM(rY))
        {
        }

    }
}
