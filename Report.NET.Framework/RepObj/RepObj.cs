using System;

namespace Root.Reports
{
    /// <summary>Base class of all report objects.</summary>
    public abstract class RepObj
    {
        /// <summary>Container to which the report object belongs. Must be null for a page.</summary>
        internal Container container;

        /// <summary>Transformation matrix.</summary>
        internal MatrixD matrixD = new MatrixD(1, 0, 0, 1, 0, 0);

        /// <summary>Height of the report object.</summary>
        private Double _rHeight;

        /// <summary>Width of the report object.</summary>
        private Double _rWidth;

        /// <summary>Horizontal alignment of the report object relative to [pointF_Pos].</summary>
        public Double rAlignH = 0;

        /// <summary>Vertical alignment of the report object relative to [pointF_Pos].</summary>
        public Double rAlignV = 1;

        /// <summary>Horizontal alignment: left</summary>
        public const Double rAlignLeft = 0;
        /// <summary>Vertical alignment: top</summary>
        public const Double rAlignTop = 0;
        /// <summary>Horizontal or vertical alignment: center</summary>
        public const Double rAlignCenter = 0.5;
        /// <summary>Horizontal alignment: right</summary>
        public const Double rAlignRight = 1;
        /// <summary>Vertical alignment: bottom</summary>
        public const Double rAlignBottom = 1;

        internal Object oRepObjX;

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Initializes a new instance of a report object class.</summary>
        /*protected !!!*/
        public RepObj()
        {
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the page to which the report object belongs.</summary>
        internal Page page
        {
            get
            {
                if (this is Page)
                {
                    return (Page)this;
                }
                Container c = container;
                while (c.container != null)
                {
                    c = c.container;
                }
                return (Page)c;
            }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Sets or gets the height of this report object.</summary>
        public virtual Double rHeight
        {
            set { _rHeight = value; }
            get { return _rHeight; }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Sets or gets the height of this report object in millimeter.</summary>
        public Double rHeightMM
        {
            set { rHeight = RT.rPointFromMM(value); }
            get { return RT.rMMFromPoint(rHeight); }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the position of the left side of this report object (points, 1/72 inch).</summary>
        public Double rPosLeft
        {
            get { return matrixD.rDX - rWidth * rAlignH; }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the position of the left side of this report object (mm).</summary>
        public Double rPosLeftMM
        {
            get { return RT.rMMFromPoint(rPosLeft); }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the position of the right side of this report object (points, 1/72 inch).</summary>
        public Double rPosRight
        {
            get { return matrixD.rDX + rWidth * (1.0 - rAlignH); }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the position of the right side of this report object (mm).</summary>
        public Double rPosRightMM
        {
            get { return RT.rMMFromPoint(rPosRight); }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the position of the top side of this report object (points, 1/72 inch).</summary>
        public virtual Double rPosTop
        {
            get { return matrixD.rDY - rHeight * rAlignV; }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the position of the top side of this report object (mm).</summary>
        public Double rPosTopMM
        {
            get { return RT.rMMFromPoint(rPosTop); }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the position of the bottom of this report object (points, 1/72 inch).</summary>
        public virtual Double rPosBottom
        {
            get { return matrixD.rDY + rHeight * (1.0 - rAlignV); }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the position of the bottom side of this report object (mm).</summary>
        public Double rPosBottomMM
        {
            get { return RT.rMMFromPoint(rPosBottom); }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the report to which the report object belongs.</summary>
        internal ReportBase report
        {
            get { return page.report; }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Sets or gets the width of this report object.</summary>
        public virtual Double rWidth
        {
            set { _rWidth = value; }
            get { return _rWidth; }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Sets or gets the width of this report object in millimeter.</summary>
        public Double rWidthMM
        {
            set { rWidth = RT.rPointFromMM(value); }
            get { return RT.rMMFromPoint(rWidth); }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the horizontal position of this report object relative to its container (points, 1/72 inch).</summary>
        public Double rX
        {
            get { return matrixD.rDX; }
            set { matrixD.rDX = value; }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the horizontal position of this report object relative to its container (mm).</summary>
        public Double rX_MM
        {
            get { return RT.rMMFromPoint(matrixD.rDX); }
            set { matrixD.rDX = RT.rPointFromMM(value); }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the vertical position of this report object relative to its container.</summary>
        public Double rY
        {
            get { return matrixD.rDY; }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Gets the vertical position (millimeter) of this report object relative to its container.</summary>
        public Double rY_MM
        {
            get { return RT.rMMFromPoint(matrixD.rDY); }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>This method will be called after the report object has been added to the container.</summary>
        internal protected virtual void OnAdded()
        {
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Removes this report object from the container.</summary>
        public void Remove()
        {
            container.Remove(this);
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Applies the specified rotation to the transformation matrix of this report object.</summary>
        /// <param name="rAngle">Angle of rotation in degrees</param>
        public void RotateTransform(Double rAngle)
        {
            matrixD.Rotate(rAngle);
        }
    }
}
