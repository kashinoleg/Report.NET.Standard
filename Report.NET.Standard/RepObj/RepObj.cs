using Report.NET.Standard.Base;
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
        private UnitModel _rHeight;

        /// <summary>Width of the report object.</summary>
        private UnitModel _rWidth;

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
        public virtual UnitModel rHeight
        {
            set => _rHeight = value;
            get => _rHeight;
        }

        /// <summary>Gets the position of the left side of this report object (points, 1/72 inch).</summary>
        public UnitModel rPosLeft => new UnitModel() { Point = matrixD.rDX - rWidth.Point * rAlignH };

        /// <summary>Gets the position of the right side of this report object (points, 1/72 inch).</summary>
        public UnitModel rPosRight => new UnitModel() { Point = matrixD.rDX + rWidth.Point * (1.0 - rAlignH) };

        /// <summary>Gets the position of the top side of this report object (points, 1/72 inch).</summary>
        public virtual UnitModel rPosTop => new UnitModel() { Point = matrixD.rDY - rHeight.Point * rAlignV };

        /// <summary>Gets the position of the bottom of this report object (points, 1/72 inch).</summary>
        public virtual UnitModel rPosBottom => new UnitModel() { Point = matrixD.rDY + rHeight.Point * (1.0 - rAlignV) };

        /// <summary>Gets the report to which the report object belongs.</summary>
        internal ReportBase report => page.report;

        /// <summary>Sets or gets the width of this report object.</summary>
        public virtual UnitModel rWidth
        {
            set => _rWidth = value;
            get => _rWidth;
        }

        /// <summary>Gets the horizontal position of this report object relative to its container (points, 1/72 inch).</summary>
        public UnitModel rX
        {
            get => new UnitModel() { Point = matrixD.rDX };
            set => matrixD.rDX = value.Point;
        }

        /// <summary>Gets the vertical position of this report object relative to its container.</summary>
        public UnitModel rY => new UnitModel() { Point = matrixD.rDY };

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
