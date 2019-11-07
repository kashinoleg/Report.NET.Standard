namespace Root.Reports
{
    /// <summary>Report Rectangle Object.</summary>
    public class RepRect : RepObj
    {
        /// <summary>Pen properties of the border line</summary>
        public PenProp penProp;

        /// <summary>Brush properties of the rectangle</summary>
        public BrushProp brushProp;

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Creates a new filled rectangle object with a border line.</summary>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the rectangle</param>
        /// <param name="rWidth">Width of the rectangle</param>
        /// <param name="rHeight">Height of the rectangle</param>
        public RepRect(PenProp penProp, BrushProp brushProp, double rWidth, double rHeight)
        {
            if (penProp != null)
            {
                this.penProp = penProp.penProp_Registered;
            }
            if (brushProp != null)
            {
                this.brushProp = brushProp.brushProp_Registered;
            }
            this.rWidth = rWidth;
            this.rHeight = rHeight;
            if (penProp == null)
            {
                oRepObjX = brushProp.report.formatter.oCreate_RepRect();
            }
            else
            {
                oRepObjX = penProp.report.formatter.oCreate_RepRect();
            }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Creates a new rectangle object.</summary>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="rWidth">Width of the rectangle</param>
        /// <param name="rHeight">Height of the rectangle</param>
        public RepRect(PenProp penProp, double rWidth, double rHeight) : this(penProp, null, rWidth, rHeight)
        {
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Creates a new filled rectangle object without a border line.</summary>
        /// <param name="brushProp">Brush properties of the rectangle</param>
        /// <param name="rWidth">Width of the rectangle</param>
        /// <param name="rHeight">Height of the rectangle</param>
        public RepRect(BrushProp brushProp, double rWidth, double rHeight) : this(null, brushProp, rWidth, rHeight)
        {
        }

    }

    //****************************************************************************************************
    /// <summary>Report Rectangle Object with millimeter values.</summary>
    public class RepRectMM : RepRect
    {
        //----------------------------------------------------------------------------------------------------x
        /// <summary>Creates a new filled rectangle object with a border line and millimeter values.</summary>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the rectangle</param>
        /// <param name="rWidth">Width of the rectangle</param>
        /// <param name="rHeight">Height of the rectangle</param>
        public RepRectMM(PenProp penProp, BrushProp brushProp, double rWidth, double rHeight) : base(penProp, brushProp, RT.rPointFromMM(rWidth), RT.rPointFromMM(rHeight))
        {
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Creates a new rectangle object with millimeter values.</summary>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="rWidth">Width of the rectangle</param>
        /// <param name="rHeight">Height of the rectangle</param>
        public RepRectMM(PenProp penProp, double rWidth, double rHeight) : base(penProp, RT.rPointFromMM(rWidth), RT.rPointFromMM(rHeight))
        {
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Creates a new filled rectangle object without a border line and with millimeter values.</summary>
        /// <param name="brushProp">Brush properties of the rectangle</param>
        /// <param name="rWidth">Width of the rectangle</param>
        /// <param name="rHeight">Height of the rectangle</param>
        public RepRectMM(BrushProp brushProp, double rWidth, double rHeight) : base(brushProp, RT.rPointFromMM(rWidth), RT.rPointFromMM(rHeight))
        {
        }
    }
}
