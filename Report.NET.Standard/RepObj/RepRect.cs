using Report.NET.Standard.Base;

namespace Root.Reports
{
    /// <summary>Report Rectangle Object.</summary>
    public class RepRect : RepObj
    {
        /// <summary>Pen properties of the border line</summary>
        public PenProp penProp;

        /// <summary>Brush properties of the rectangle</summary>
        public BrushProp brushProp;

        /// <summary>Creates a new filled rectangle object with a border line.</summary>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the rectangle</param>
        public RepRect(UnitModel width, UnitModel height, PenProp penProp = null, BrushProp brushProp = null)
        {
            if (penProp != null)
            {
                this.penProp = penProp.penProp_Registered;
            }
            if (brushProp != null)
            {
                this.brushProp = brushProp.brushProp_Registered;
            }
            this.rWidth = width;
            this.rHeight = height;
            if (penProp == null)
            {
                oRepObjX = brushProp.report.formatter.oCreate_RepRect();
            }
            else
            {
                oRepObjX = penProp.report.formatter.oCreate_RepRect();
            }
        }

        /// <summary>Creates a new rectangle object.</summary>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        public RepRect(PenProp penProp, UnitModel width, UnitModel height) : this(width, height, penProp, null)
        {
        }

        /// <summary>Creates a new filled rectangle object without a border line.</summary>
        /// <param name="brushProp">Brush properties of the rectangle</param>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        public RepRect(BrushProp brushProp, UnitModel width, UnitModel height) : this(width, height, null, brushProp)
        {
        }
    }
}
