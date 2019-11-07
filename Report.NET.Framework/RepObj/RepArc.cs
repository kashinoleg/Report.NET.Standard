using System;

namespace Root.Reports
{
    #region RepArcBase
    /// <summary>Base Class of the Arc, Circle, Ellipse and Pie Objects</summary>
    /// <remarks>
    /// This class is the base class of the <see cref="RepArc"/>, <see cref="RepCircle"/>, <see cref="RepEllipse"/> and <see cref="RepPie"/> classes.
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepArcBase"]/*'/>
    public abstract class RepArcBase : RepObj
    {
        /// <summary>Pen properties of the border line</summary>
        internal PenProp _penProp;

        /// <summary>Brush properties of the pie or circle</summary>
        internal BrushProp _brushProp;

        /// <summary>Angle in degrees measured clockwise from the x-axis to the first side of the pie section</summary>
        internal double _rStartAngle;

        /// <summary>Angle in degrees measured clockwise from the startAngle parameter to the second side of the pie section</summary>
        internal double _rSweepAngle;

        /// <summary>Initializes all parameters for an arc, circle, ellipse or pie.</summary>
        /// <remarks>
        /// This constructor must be called by all derived classes.
        /// The pen properties <paramref name="penProp"/> can be set to draw a border line.
        /// If the brush properties <paramref name="brushProp"/> are set, the interior of the shape will be filled.
        /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidth"/> and
        /// <paramref name="rHeight"/> parameters.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> can be used to define a portion of an ellipse.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
        /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start point of the arc</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the arc</param>
        public RepArcBase(PenProp penProp, BrushProp brushProp, double rWidth, double rHeight, double rStartAngle, double rSweepAngle)
        {
            if (penProp != null)
            {
                this._penProp = penProp.penProp_Registered;
            }
            if (brushProp != null)
            {
                this._brushProp = brushProp.brushProp_Registered;
            }
            this.rWidth = rWidth;
            this.rHeight = rHeight;
            this._rStartAngle = rStartAngle;
            this._rSweepAngle = rSweepAngle;

            oRepObjX = report.formatter.oCreate_RepArcBase();
        }

        /// <summary>Gets the report to which the report object belongs.</summary>
        private new ReportBase report
        {
            get
            {
                if (_penProp != null)
                {
                    return _penProp.report;
                }
                if (_brushProp != null)
                {
                    return _brushProp.report;
                }
                return base.report;
            }
        }

        /// <summary>Calculates the x- and y-coordinates of the ellipse for the specified angle.</summary>
        /// <param name="rAngle">Angle in radians measured clockwise from the x-axis</param>
        /// <param name="rX">x-coordinate in points (1/72 inch)</param>
        /// <param name="rY">y-coordinate in points (1/72 inch)</param>
        internal void GetEllipseXY(double rAngle, out double rX, out double rY)
        {
            const double rPi1_2 = Math.PI / 2.0;
            const double rPi3_2 = Math.PI / 2.0 * 3.0;
            rAngle -= Math.Floor(rAngle / 2.0 / Math.PI) * 2.0 * Math.PI;
            double rA = rWidth / 2.0;
            double rB = rHeight / 2.0;

            if (RT.bEquals(rAngle, rPi1_2, 0.0001))
            {
                rX = 0;
                rY = -rB;
                return;
            }
            if (RT.bEquals(rAngle, rPi3_2, 0.0001))
            {
                rX = 0;
                rY = rB;
                return;
            }

            // tan(@) = y/x  ==> y = x tan(@)                   @ != 0
            // x^2/a^2 + y^2/b^2 = 1
            // ==> 1. x^2/a^2 + x^2 tan(@)^2 / b^2 = 1
            //     2. x^2 (1/a^2 + tan(@)^2 / b^2) = 1
            //     3. x^2 = 1 / (1/a^2 + tan(@)^2 / b^2)
            //     4. x = SQRT(1 / (1/a^2 + tan(@)^2 / b^2))
            double r = Math.Tan(-rAngle);
            r = 1.0 / rA / rA + r * r / rB / rB;
            rX = Math.Sqrt(1 / r);
            if (rAngle > rPi1_2 && rAngle < rPi3_2)
            {
                rX = -rX;
            }

            // y = x tan(@)
            rY = rX * Math.Tan(-rAngle);
        }
    }
    #endregion

    #region RepArc
    /// <summary>Report Arc Object</summary>
    /// <remarks>
    /// This object draws an arc representing a portion of an ellipse or circle.
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepArc"]/*'/>
    public class RepArc : RepArcBase
    {
        /// <overloads>
        /// <summary>Creates an arc representing a portion of an ellipse or circle.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The arc represents a portion of an ellipse or circle.
        /// </remarks>
        /// </overloads>
        /// 
        /// <summary>Creates an arc representing a portion of an ellipse specified by the bounding rectangle in points (1/72 inch).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The arc represents a portion of an ellipse that is defined by the bounding rectangle described by the
        /// <paramref name="rWidth"/> and <paramref name="rHeight"/> parameters.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and end point of the arc.
        /// </remarks>
        /// <param name="penProp">Pen properties of the arc</param>
        /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the arc comes</param>
        /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the arc comes</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the arc</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the arc</param>
        public RepArc(PenProp penProp, double rWidth, double rHeight, double rStartAngle, double rSweepAngle)
          : base(penProp, null, rWidth, rHeight, rStartAngle, rSweepAngle)
        {
        }

        /// <summary>Creates an arc representing a portion of a circle specified by the radius in points (1/72 inch).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The arc represents a portion of a circle that is defined by the parameter <paramref name="rRadius"/>.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and end point of the arc.
        /// </remarks>
        /// <param name="penProp">Pen properties of the arc</param>
        /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the arc</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the arc</param>
        public RepArc(PenProp penProp, double rRadius, double rStartAngle, double rSweepAngle)
          : this(penProp, rRadius * 2.0, rRadius * 2.0, rStartAngle, rSweepAngle)
        {
        }
    }
    #endregion

    #region RepArcMM
    /// <summary>Report Arc Object (metric version)</summary>
    /// <remarks>
    /// This object draws an arc representing a portion of an ellipse or circle.
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepArc"]/*'/>
    public class RepArcMM : RepArc
    {
        /// <overloads>
        /// <summary>Creates an arc representing a portion of an ellipse or circle.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The arc represents a portion of an ellipse or circle.
        /// </remarks>
        /// </overloads>
        /// 
        /// <summary>Creates an arc representing a portion of an ellipse specified by the bounding rectangle in millimeters.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The arc represents a portion of an ellipse that is defined by the bounding rectangle described by the
        /// <paramref name="rWidthMM"/> and <paramref name="rHeightMM"/> parameters.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and end point of the arc.
        /// </remarks>
        /// <param name="penProp">Pen properties of the arc</param>
        /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse from which the arc comes</param>
        /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse from which the arc comes</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the arc</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the arc</param>
        public RepArcMM(PenProp penProp, double rWidthMM, double rHeightMM, double rStartAngle, double rSweepAngle)
          : base(penProp, RT.rPointFromMM(rWidthMM), RT.rPointFromMM(rHeightMM), rStartAngle, rSweepAngle)
        {
        }

        /// <summary>Creates an arc representing a portion of a circle specified by the radius in millimeters.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The arc represents a portion of a circle that is defined by the parameter <paramref name="rRadiusMM"/>.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and end point of the arc.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the arc</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the arc</param>
        public RepArcMM(PenProp penProp, double rRadiusMM, double rStartAngle, double rSweepAngle)
          : this(penProp, rRadiusMM * 2.0, rRadiusMM * 2.0, rStartAngle, rSweepAngle)
        {
        }
    }
    #endregion

    #region RepCircle
    /// <summary>Report Circle Object</summary>
    /// <remarks>
    /// This object draws a circle that may have a border line and/or may be filled.
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepCircle"]/*'/>
    public class RepCircle : RepArcBase
    {
        /// <overloads>
        /// <summary>Creates a circle.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The circle is defined by the parameter <paramref name="rRadius"/>.
        /// </remarks>
        /// </overloads>
        /// 
        /// <summary>Creates a filled circle with a border line defined by the radius in points (1/72 inch).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The circle is defined by the parameter <paramref name="rRadius"/>.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
        public RepCircle(PenProp penProp, BrushProp brushProp, double rRadius) : base(penProp, brushProp, rRadius * 2.0, rRadius * 2.0, 0.0, 360.0)
        {
        }

        /// <summary>Creates a circle with a border line defined by the radius in points (1/72 inch).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The circle is defined by the parameter <paramref name="rRadius"/>.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
        public RepCircle(PenProp penProp, double rRadius) : this(penProp, null, rRadius)
        {
        }

        /// <summary>Creates a filled circle defined by the radius in points (1/72 inch).</summary>
        /// <remarks>
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The circle is defined by the parameter <paramref name="rRadius"/>.
        /// </remarks>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
        public RepCircle(BrushProp brushProp, double rRadius) : this(null, brushProp, rRadius)
        {
        }
    }
    #endregion

    #region RepCircleMM
    /// <summary>Report Circle Object (metric version)</summary>
    /// <remarks>
    /// This object draws a circle that may have a border line and/or may be filled.
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepCircle"]/*'/>
    public class RepCircleMM : RepCircle
    {
        /// <overloads>
        /// <summary>Creates a circle (metric version).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The circle is defined by the parameter <paramref name="rRadiusMM"/>.
        /// </remarks>
        /// </overloads>
        /// 
        /// <summary>Creates a filled circle with a border line defined by the radius in millimeters.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The circle is defined by the parameter <paramref name="rRadiusMM"/>.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
        public RepCircleMM(PenProp penProp, BrushProp brushProp, double rRadiusMM) : base(penProp, brushProp, RT.rPointFromMM(rRadiusMM))
        {
        }

        /// <summary>Creates a circle with a border line defined by the radius in millimeters.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The circle is defined by the parameter <paramref name="rRadiusMM"/>.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
        public RepCircleMM(PenProp penProp, double rRadiusMM) : this(penProp, null, rRadiusMM)
        {
        }

        /// <summary>Creates a filled circle defined by the radius in millimeters.</summary>
        /// <remarks>
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The circle is defined by the parameter <paramref name="rRadiusMM"/>.
        /// </remarks>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
        public RepCircleMM(BrushProp brushProp, double rRadiusMM) : this(null, brushProp, rRadiusMM)
        {
        }
    }
    #endregion

    #region RepEllipse
    /// <summary>Report Ellipse Object</summary>
    /// <remarks>
    /// This object draws an ellipse that may have a border line and/or may be filled.
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepEllipse"]/*'/>
    public class RepEllipse : RepArcBase
    {
        /// <overloads>
        /// <summary>Creates an ellipse.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidth"/> and
        /// <paramref name="rHeight"/> parameters.
        /// </remarks>
        /// </overloads>
        /// 
        /// <summary>Creates a filled ellipse with a border line specified by the bounding rectangle in points (1/72 inch).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidth"/> and
        /// <paramref name="rHeight"/> parameters.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
        /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
        public RepEllipse(PenProp penProp, BrushProp brushProp, double rWidth, double rHeight)
          : base(penProp, brushProp, rWidth, rHeight, 0.0, 360.0)
        {
        }

        /// <summary>Creates a filled ellipse with a border line specified by the bounding rectangle in points (1/72 inch).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidth"/> and
        /// <paramref name="rHeight"/> parameters.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
        /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
        public RepEllipse(PenProp penProp, double rWidth, double rHeight)
          : this(penProp, null, rWidth, rHeight)
        {
        }

        /// <summary>Creates a filled ellipse specified by the bounding rectangle in points (1/72 inch).</summary>
        /// <remarks>
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidth"/> and
        /// <paramref name="rHeight"/> parameters.
        /// </remarks>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
        /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
        public RepEllipse(BrushProp brushProp, double rWidth, double rHeight)
          : this(null, brushProp, rWidth, rHeight)
        {
        }
    }
    #endregion

    #region RepEllipseMM
    /// <summary>Report Ellipse Object (metric version)</summary>
    /// <remarks>
    /// This object draws an ellipse that may have a border line and/or may be filled.
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepEllipse"]/*'/>
    public class RepEllipseMM : RepEllipse
    {
        /// <overloads>
        /// <summary>Creates an ellipse (metric version).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidthMM"/> and
        /// <paramref name="rHeightMM"/> parameters.
        /// </remarks>
        /// </overloads>
        ///
        /// <summary>Creates a filled ellipse with a border line specified by the bounding rectangle in millimeters.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidthMM"/> and
        /// <paramref name="rHeightMM"/> parameters.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse</param>
        /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse</param>
        public RepEllipseMM(PenProp penProp, BrushProp brushProp, double rWidthMM, double rHeightMM)
          : base(penProp, brushProp, RT.rPointFromMM(rWidthMM), RT.rPointFromMM(rHeightMM))
        {
        }

        /// <summary>Creates an ellipse with a border line specified by the bounding rectangle in millimeters.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidthMM"/> and
        /// <paramref name="rHeightMM"/> parameters.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse</param>
        /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse</param>
        public RepEllipseMM(PenProp penProp, double rWidthMM, double rHeightMM)
          : this(penProp, null, rWidthMM, rHeightMM)
        {
        }

        /// <summary>Creates a filled ellipse specified by the bounding rectangle in millimeters.</summary>
        /// <remarks>
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidthMM"/> and
        /// <paramref name="rHeightMM"/> parameters.
        /// </remarks>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse</param>
        /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse</param>
        public RepEllipseMM(BrushProp brushProp, double rWidthMM, double rHeightMM)
          : this(null, brushProp, rWidthMM, rHeightMM)
        {
        }
    }
    #endregion

    #region RepPie
    /// <summary>Report Pie Object</summary>
    /// <remarks>
    /// This object draws a pie shape defined by an ellipse or a circle and two radial lines.
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepPie"]/*'/>
    public class RepPie : RepArcBase
    {
        /// <overloads>
        /// <summary>Creates a pie shape defined by an ellipse or a circle and two radial lines.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The pie shape represents a portion of an ellipse or circle.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// </overloads>
        /// 
        /// <summary>Creates a filled pie section with a border line defined by an ellipse specified by the bounding
        /// rectangle in points (1/72 inch).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
        /// <paramref name="rWidth"/> and <paramref name="rHeight"/> parameters.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the pie section</param>
        public RepPie(PenProp penProp, BrushProp brushProp, double rWidth, double rHeight, double rStartAngle, double rSweepAngle)
          : base(penProp, brushProp, rWidth, rHeight, rStartAngle, rSweepAngle)
        { }

        /// <summary>Creates a pie section with a border line defined by an ellipse specified by the bounding
        /// rectangle in points (1/72 inch).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
        /// <paramref name="rWidth"/> and <paramref name="rHeight"/> parameters.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the pie section</param>
        public RepPie(PenProp penProp, double rWidth, double rHeight, double rStartAngle, double rSweepAngle)
          : this(penProp, null, rWidth, rHeight, rStartAngle, rSweepAngle)
        {
        }

        /// <summary>Creates a filled pie section defined by an ellipse specified by the bounding rectangle in points (1/72 inch).</summary>
        /// <remarks>
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
        /// <paramref name="rWidth"/> and <paramref name="rHeight"/> parameters.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the pie section</param>
        public RepPie(BrushProp brushProp, double rWidth, double rHeight, double rStartAngle, double rSweepAngle)
          : this(null, brushProp, rWidth, rHeight, rStartAngle, rSweepAngle)
        {
        }

        /// <summary>Creates a filled pie section with a border line defined by a circle specified by the radius in points (1/72 inch).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The pie shape represents a portion of a circle that is defined by the parameter <paramref name="rRadius"/>.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the rStartAngle parameter to the second side of the pie section</param>
        public RepPie(PenProp penProp, BrushProp brushProp, double rRadius, double rStartAngle, double rSweepAngle)
          : this(penProp, brushProp, rRadius * 2.0, rRadius * 2.0, rStartAngle, rSweepAngle)
        {
        }

        /// <summary>Creates a pie section with a border line defined by a circle specified by the radius in points (1/72 inch).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The pie shape represents a portion of a circle that is defined by the parameter <paramref name="rRadius"/>.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the rStartAngle parameter to the second side of the pie section</param>
        public RepPie(PenProp penProp, double rRadius, double rStartAngle, double rSweepAngle)
          : this(penProp, null, rRadius * 2.0, rRadius * 2.0, rStartAngle, rSweepAngle)
        {
        }

        /// <summary>Creates a filled pie section defined by a circle specified by the radius in points (1/72 inch).</summary>
        /// <remarks>
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The pie shape represents a portion of a circle that is defined by the parameter <paramref name="rRadius"/>.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the rStartAngle parameter to the second side of the pie section</param>
        public RepPie(BrushProp brushProp, double rRadius, double rStartAngle, double rSweepAngle)
          : this(null, brushProp, rRadius * 2.0, rRadius * 2.0, rStartAngle, rSweepAngle)
        {
        }
    }
    #endregion

    #region RepPieMM
    /// <summary>Report Pie Object (metric version)</summary>
    public class RepPieMM : RepPie
    {
        //------------------------------------------------------------------------------------------04.03.2004
        /// <overloads>
        /// <summary>Creates a pie shape defined by an ellipse or a circle and two radial lines (metric version).</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The pie shape represents a portion of an ellipse or circle.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// </overloads>
        /// 
        /// <summary>Creates a filled pie section with a border line defined by an ellipse specified by the bounding
        /// rectangle in millimeters.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
        /// <paramref name="rWidthMM"/> and <paramref name="rHeightMM"/> parameters.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the pie section</param>
        public RepPieMM(PenProp penProp, BrushProp brushProp, double rWidthMM, double rHeightMM, double rStartAngle, double rSweepAngle)
          : base(penProp, brushProp, RT.rPointFromMM(rWidthMM), RT.rPointFromMM(rHeightMM), rStartAngle, rSweepAngle)
        { }

        /// <summary>Creates a pie section with a border line defined by an ellipse specified by the bounding rectangle in millimeters.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
        /// <paramref name="rWidthMM"/> and <paramref name="rHeightMM"/> parameters.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the pie section</param>
        public RepPieMM(PenProp penProp, double rWidthMM, double rHeightMM, double rStartAngle, double rSweepAngle)
          : this(penProp, null, rWidthMM, rHeightMM, rStartAngle, rSweepAngle)
        {
        }

        /// <summary>Creates a filled pie section defined by an ellipse specified by the bounding rectangle in millimeters.</summary>
        /// <remarks>
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
        /// <paramref name="rWidthMM"/> and <paramref name="rHeightMM"/> parameters.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the pie section</param>
        public RepPieMM(BrushProp brushProp, double rWidthMM, double rHeightMM, double rStartAngle, double rSweepAngle)
          : this(null, brushProp, rWidthMM, rHeightMM, rStartAngle, rSweepAngle)
        {
        }

        /// <summary>Creates a filled pie section with a border line defined by a circle specified by the radius in millimeters.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The pie shape represents a portion of an ellipse that is defined by the parameter <paramref name="rRadiusMM"/>.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the pie section</param>
        public RepPieMM(PenProp penProp, BrushProp brushProp, double rRadiusMM, double rStartAngle, double rSweepAngle)
          : this(penProp, brushProp, rRadiusMM * 2.0, rRadiusMM * 2.0, rStartAngle, rSweepAngle)
        {
        }

        /// <summary>Creates a pie section with a border line defined by a circle specified by the radius in millimeters.</summary>
        /// <remarks>
        /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
        /// The pie shape represents a portion of an ellipse that is defined by the parameter <paramref name="rRadiusMM"/>.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="penProp">Pen properties of the border line</param>
        /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the pie section</param>
        public RepPieMM(PenProp penProp, double rRadiusMM, double rStartAngle, double rSweepAngle)
          : this(penProp, null, rRadiusMM * 2.0, rRadiusMM * 2.0, rStartAngle, rSweepAngle)
        {
        }

        /// <summary>Creates a filled pie section defined by a circle specified by the radius in millimeters.</summary>
        /// <remarks>
        /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
        /// The pie shape represents a portion of an ellipse that is defined by the parameter <paramref name="rRadiusMM"/>.
        /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
        /// end point of the radial lines.
        /// </remarks>
        /// <param name="brushProp">Brush properties of the fill</param>
        /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
        /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
        /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
        /// the end point of the pie section</param>
        public RepPieMM(BrushProp brushProp, double rRadiusMM, double rStartAngle, double rSweepAngle)
          : this(null, brushProp, rRadiusMM * 2.0, rRadiusMM * 2.0, rStartAngle, rSweepAngle)
        {
        }
    }
    #endregion
}
