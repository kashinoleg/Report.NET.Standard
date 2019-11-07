using System;
using System.Drawing;

namespace Root.Reports
{
    /// <summary>Structure that defines the properties of a brush.</summary>
    public class BrushProp
    {
        /// <summary>Background fallback value</summary>
        public static readonly BrushProp bp_Null = new BrushProp(null, System.Drawing.Color.White);

        /// <summary>Report to which this brush belongs</summary>
        internal readonly ReportBase report;

        /// <summary>Color of the brush</summary>
        private Color _color;

        /// <summary>Reference to the same but registered property object. 
        /// If null, it has not yet been used and therefore it is not registered.</summary>
        private BrushProp _brushProp_Registered;

        /// <summary>Initializes a new brush properties object.</summary>
        /// <param name="report">Report to which this brush belongs</param>
        /// <param name="color">Color of the brush</param>
        public BrushProp(ReportBase report, Color color)
        {
            this.report = report;
            _color = color;
        }

        /// <summary>Gets a reference to the same but registered brush property object.</summary>
        internal BrushProp brushProp_Registered
        {
            get
            {
                if (_brushProp_Registered == null)
                {
                    String sKey = _color.R + "-" + _color.G + "-" + _color.B + "-" + _color.A;
                    _brushProp_Registered = (BrushProp)report.ht_BrushProp[sKey];
                    if (_brushProp_Registered == null)
                    {
                        _brushProp_Registered = new BrushProp(report, _color);
                        _brushProp_Registered._brushProp_Registered = _brushProp_Registered;
                        report.ht_BrushProp.Add(sKey, _brushProp_Registered);
                    }
                }
                return _brushProp_Registered;
            }
        }

        /// <summary>Gets or sets the color of the brush</summary>
        public Color color
        {
            get { return _color; }
            set
            {
                System.Diagnostics.Debug.Assert(_brushProp_Registered != this);
                _color = value;
                _brushProp_Registered = null;
            }
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="o">The object to compare with the current object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object o)
        {
            if (o == null)
            {
                return false;
            }
            BrushProp bp = (BrushProp)o;
            return Equals(_color, bp._color);
        }

        /// <summary>Hash function of this class.</summary>
        /// <returns>Hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            return _color.GetHashCode();
        }
    }
}
