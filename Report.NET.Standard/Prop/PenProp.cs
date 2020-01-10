using Report.NET.Standard.Base;
using System.Drawing;

namespace Root.Reports
{
    #region
    /// <summary>Defines the properties of a pen.</summary>
    /// <remarks>The pen property object defines the style of a pen.</remarks>
    /// <example>Pen property sample:
    /// <code>
    /// using Root.Reports;
    /// using System;
    /// using System.Drawing;
    ///
    /// public class PenPropSample : Report {
    ///   public static void Main() {
    ///     PdfReport&lt;PenPropSample&gt; pdfReport = new PdfReport&lt;PenPropSample&gt;();
    ///     pdfReport.View("PenPropSample.pdf");
    ///   }
    ///
    ///   protected override void Create() {
    ///     <b>PenProp penProp = new PenProp(this, 1.5, Color.Red)</b>;
    ///     new Page(this);
    ///     page_Cur.AddLT_MM(30, 30, new RepRectMM(<b>penProp</b>, 150, 60));
    ///   }
    /// }
    /// </code>
    /// </example>
    #endregion
    public class PenProp
    {
        /// <summary>Null value</summary>
        public static readonly PenProp penProp_Null = new PenProp(null, new UnitModel());

        /// <summary>Report to which this pen belongs</summary>
        internal readonly ReportBase report;

        /// <summary>Width of the pen</summary>
        private UnitModel _rWidth;

        /// <summary>Color of the pen</summary>
        private Color _color;

        /// <summary>Number of 1/72-units of the on-pattern</summary>
        private double _rPatternOn;

        /// <summary>Number of 1/72-units of the off-pattern</summary>
        private double _rPatternOff;

        /// <summary>Reference to the same but registered property object.
        /// If null, it has not yet been used and therefore it is not registered.</summary>
        private PenProp _penProp_Registered;

        /// <summary>Initializes a new pen properties object</summary>
        /// <param name="report">Report to which this pen belongs</param>
        /// <param name="rWidth">Width of the pen</param>
        /// <param name="color">Color of the pen</param>
        /// <param name="rPatternOn">Number of 1/72-units of the on-pattern</param>
        /// <param name="rPatternOff">Number of 1/72-units of the off-pattern</param>
        public PenProp(ReportBase report, UnitModel rWidth, Color color, double rPatternOn, double rPatternOff)
        {
            this.report = report;
            _rWidth = rWidth;
            _color = color;
            _rPatternOn = rPatternOn;
            _rPatternOff = rPatternOff;
        }

        /// <summary>Initializes a new pen properties object</summary>
        /// <param name="report">Report to which this pen belongs</param>
        /// <param name="rWidth">Width of the pen</param>
        public PenProp(ReportBase report, UnitModel rWidth) : this(report, rWidth, Color.Black, 0, 0)
        {
        }

        /// <summary>Initializes a new pen properties object</summary>
        /// <param name="report">Report to which this pen belongs</param>
        /// <param name="rWidth">Width of the pen</param>
        /// <param name="rPatternOn">Number of 1/72-units of the on-pattern</param>
        /// <param name="rPatternOff">Number of 1/72-units of the off-pattern</param>
        public PenProp(ReportBase report, UnitModel rWidth, double rPatternOn, double rPatternOff) : this(report, rWidth, Color.Black, rPatternOn, rPatternOff)
        {
        }

        /// <summary>Initializes a new pen properties object</summary>
        /// <param name="report">Report to which this pen belongs</param>
        /// <param name="rWidth">Width of the pen</param>
        /// <param name="color">Color of the pen</param>
        public PenProp(ReportBase report, UnitModel rWidth, Color color) : this(report, rWidth, color, 0, 0)
        {
        }

        /// <summary>Gets or sets the color of the pen</summary>
        public Color color
        {
            get { return _color; }
            set
            {
                _color = value;
                _penProp_Registered = null;
            }
        }

        /// <summary>Gets or sets the number of 1/72-units of the on-pattern</summary>
        public double rPatternOff
        {
            get { return _rPatternOff; }
            set
            {
                _rPatternOff = value;
                _penProp_Registered = null;
            }
        }

        /// <summary>Gets or sets the number of 1/72-units of the on-pattern</summary>
        public double rPatternOn
        {
            get { return _rPatternOn; }
            set
            {
                _rPatternOn = value;
                _penProp_Registered = null;
            }
        }

        /// <summary>Returns a reference to the same but registered property object.</summary>
        internal PenProp penProp_Registered
        {
            get
            {
                if (_penProp_Registered == null)
                {
                    string sKey = _rWidth.Point.ToString("F3") + ";" + _color.R + "-" + _color.G + "-" + _color.B + ";" + _rPatternOn + "-" + _rPatternOff;
                    _penProp_Registered = (PenProp)report.ht_PenProp[sKey];
                    if (_penProp_Registered == null)
                    {
                        _penProp_Registered = new PenProp(report, _rWidth, _color, _rPatternOn, _rPatternOff);
                        _penProp_Registered._penProp_Registered = _penProp_Registered;
                        report.ht_PenProp.Add(sKey, _penProp_Registered);
                    }
                }
                return _penProp_Registered;
            }
        }

        /// <summary>Gets or sets the width of the pen</summary>
        public UnitModel rWidth
        {
            get { return _rWidth; }
            set
            {
                _rWidth = value;
                _penProp_Registered = null;
            }
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="o">The object to compare with the current object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object o)
        {
            if (o == null)
            {
                return false;
            }
            PenProp pp = (PenProp)o;
            return RT.bEquals(rWidth.Point, pp.rWidth.Point, 0.1) && object.Equals(_color, pp._color) &&
              RT.bEquals(rPatternOn, pp.rPatternOn, 0.1) && RT.bEquals(rPatternOff, pp.rPatternOff, 0.1);
        }

        /// <summary>Hash function of this class.</summary>
        /// <returns>Hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            return _rWidth.GetHashCode() ^ _color.GetHashCode() ^ _rPatternOn.GetHashCode() ^ _rPatternOff.GetHashCode();
        }
    }
}
