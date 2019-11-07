using System;

namespace Root.Reports
{
    /// <summary>Definition of the row properties</summary>
    /// <remarks>
    /// This class defines the properties of a row for a table layout manager.
    /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmBaseDefaults_Row"]/*'/>
    /// </remarks>
    public class TlmRowDef
    {
        private Double _rPreferredHeight = Double.NaN;
        /// <summary>Gets or sets the preferred height of the row (default: not set - <see cref="System.Double.NaN"/>)</summary>
        /// <value>
        /// The preferred height of the row in points (1/72 inch).
        /// The default value of this property is <see cref="System.Double.NaN"/>, that is the text within the row will not be cut.
        /// </value>
        /// <remarks>
        /// This value sets the preferred height of the row.
        /// If the height of a cell of the row is less than this value and there is enough space left, the height of the cell will be enlarged.
        /// The preferred height can also be used to limit the length of <see cref="TlmCellDef.rAngle">vertical text</see>.
        /// <para>For the metric version see <see cref="TlmRowDefBase.rPreferredHeightMM"/>.</para>
        /// </remarks>
        /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmVerticalSample"]/*'/>
        /// <seealso cref="TlmCellDef.rAngle"/>
        public Double rPreferredHeight
        {
            get { return _rPreferredHeight; }
            set { _rPreferredHeight = value; }
        }

        /// <summary>Gets or sets the preferred height of the row (default: not set - <see cref="System.Double.NaN"/>)</summary>
        /// <value>
        /// The preferred height of the row in millimeters.
        /// The default value of this property is <see cref="System.Double.NaN"/>, that is the text within the row will not be cut.
        /// </value>
        /// <remarks>
        /// This value sets the preferred height of the row.
        /// If the height of a cell of the row is less than this value and there is enough space left, the height of the cell will be enlarged.
        /// The preferred height can also be used to limit the length of <see cref="TlmCellDef.rAngle">vertical text</see>.
        /// <para>For the inch version see <see cref="TlmRowDef.rPreferredHeight"/>.</para>
        /// </remarks>
        /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmVerticalSample"]/*'/>
        /// <seealso cref="TlmCellDef.rAngle"/>
        public Double rPreferredHeightMM
        {
            get { return RT.rMMFromPoint(rPreferredHeight); }
            set { rPreferredHeight = RT.rPointFromMM(value); }
        }
    }
}
