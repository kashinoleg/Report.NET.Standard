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
        private double _rHeight;

        /// <summary>Width of the report object.</summary>
        private double _rWidth;

        /// <summary>Horizontal alignment of the report object relative to [pointF_Pos].</summary>
        public double rAlignH = 0;

        /// <summary>Vertical alignment of the report object relative to [pointF_Pos].</summary>
        public double rAlignV = 1;

        /// <summary>Horizontal alignment: left</summary>
        public const double rAlignLeft = 0;
        /// <summary>Vertical alignment: top</summary>
        public const double rAlignTop = 0;
        /// <summary>Horizontal or vertical alignment: center</summary>
        public const double rAlignCenter = 0.5;
        /// <summary>Horizontal alignment: right</summary>
        public const double rAlignRight = 1;
        /// <summary>Vertical alignment: bottom</summary>
        public const double rAlignBottom = 1;

        internal Object oRepObjX;

        /// <summary>Initializes a new instance of a report object class.</summary>
        /*protected !!!*/
        public RepObj()
        {
        }

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

        /// <summary>Sets or gets the height of this report object.</summary>
        public virtual double rHeight
        {
            set { _rHeight = value; }
            get { return _rHeight; }
        }

        /// <summary>Sets or gets the height of this report object in millimeter.</summary>
        public double rHeightMM
        {
            set { rHeight = RT.rPointFromMM(value); }
            get { return RT.rMMFromPoint(rHeight); }
        }

        /// <summary>Gets the position of the left side of this report object (points, 1/72 inch).</summary>
        public double rPosLeft
        {
            get { return matrixD.rDX - rWidth * rAlignH; }
        }

        /// <summary>Gets the position of the left side of this report object (mm).</summary>
        public double rPosLeftMM
        {
            get { return RT.rMMFromPoint(rPosLeft); }
        }

        /// <summary>Gets the position of the right side of this report object (points, 1/72 inch).</summary>
        public double rPosRight
        {
            get { return matrixD.rDX + rWidth * (1.0 - rAlignH); }
        }

        /// <summary>Gets the position of the right side of this report object (mm).</summary>
        public double rPosRightMM
        {
            get { return RT.rMMFromPoint(rPosRight); }
        }

        /// <summary>Gets the position of the top side of this report object (points, 1/72 inch).</summary>
        public virtual double rPosTop
        {
            get { return matrixD.rDY - rHeight * rAlignV; }
        }

        /// <summary>Gets the position of the top side of this report object (mm).</summary>
        public double rPosTopMM
        {
            get { return RT.rMMFromPoint(rPosTop); }
        }

        /// <summary>Gets the position of the bottom of this report object (points, 1/72 inch).</summary>
        public virtual double rPosBottom
        {
            get { return matrixD.rDY + rHeight * (1.0 - rAlignV); }
        }

        /// <summary>Gets the position of the bottom side of this report object (mm).</summary>
        public double rPosBottomMM
        {
            get { return RT.rMMFromPoint(rPosBottom); }
        }

        /// <summary>Gets the report to which the report object belongs.</summary>
        internal ReportBase report
        {
            get { return page.report; }
        }

        /// <summary>Sets or gets the width of this report object.</summary>
        public virtual double rWidth
        {
            set { _rWidth = value; }
            get { return _rWidth; }
        }

        /// <summary>Sets or gets the width of this report object in millimeter.</summary>
        public double rWidthMM
        {
            set { rWidth = RT.rPointFromMM(value); }
            get { return RT.rMMFromPoint(rWidth); }
        }

        /// <summary>Gets the horizontal position of this report object relative to its container (points, 1/72 inch).</summary>
        public double rX
        {
            get { return matrixD.rDX; }
            set { matrixD.rDX = value; }
        }

        /// <summary>Gets the horizontal position of this report object relative to its container (mm).</summary>
        public double rX_MM
        {
            get { return RT.rMMFromPoint(matrixD.rDX); }
            set { matrixD.rDX = RT.rPointFromMM(value); }
        }

        /// <summary>Gets the vertical position of this report object relative to its container.</summary>
        public double rY
        {
            get { return matrixD.rDY; }
        }

        /// <summary>Gets the vertical position (millimeter) of this report object relative to its container.</summary>
        public double rY_MM
        {
            get { return RT.rMMFromPoint(matrixD.rDY); }
        }

        /// <summary>This method will be called after the report object has been added to the container.</summary>
        internal protected virtual void OnAdded()
        {
        }

        /// <summary>Removes this report object from the container.</summary>
        public void Remove()
        {
            container.Remove(this);
        }

        /// <summary>Applies the specified rotation to the transformation matrix of this report object.</summary>
        /// <param name="rAngle">Angle of rotation in degrees</param>
        public void RotateTransform(double rAngle)
        {
            matrixD.Rotate(rAngle);
        }
    }
}
