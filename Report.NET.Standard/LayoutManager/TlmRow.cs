﻿using System;
using System.Diagnostics;

namespace Root.Reports
{
    /// <summary>Contents of a row of the table layout manager</summary>
    /// <remarks>
    /// <para>
    /// When a new row is initialized, the properties of the row will be set with the default values which are inherited from the table layout manager (<see cref="F:Root.Reports.TlmBase.rowDef">TlmBase.rowDef</see>).
    /// After the properties of <see cref="F:Root.Reports.TlmBase.rowDef">TlmBase.rowDef</see> have been changed, all following rows will take the new properties.
    /// The properties of one single row can be changed with the handle of the row (e.g. row.rPreferredHeight = 50).
    /// The method <see cref="TlmBase.tlmRow_New()"/> returns the handle of a new row.
    /// The current row is stored in <see cref="TlmBase.tlmRow_Cur">TlmBase.tlmRow_Cur</see>. 
    /// </para>
    /// <br></br><para><img src="images/TlmBaseDefaults_Row.gif"/></para>
    /// </remarks>
    /// <example>
    ///   <code>
    /// using Root.Reports;
    /// using System;
    ///
    /// namespace ReportSamples {
    ///   public class TlmRowSample : Report {
    ///     public static void Main() {
    ///       RT.ViewPDF(new TlmRowSample(), "TlmRowSample.pdf");
    ///     }
    ///
    ///     protected override void Create() {  
    ///       FontDef fd = new FontDef(this, FontDef.StandardFont.Helvetica);
    ///       FontProp fp = new FontPropMM(fd, 1.9);
    ///       new Page(this);
    ///
    ///       using (ListLayoutManager llm = new ListLayoutManager(this)) { 
    ///         llm.rContainerHeightMM = 50;
    ///         llm.cellDef.pp_Line = new PenProp(this, 0.01);
    ///
    ///         new ListLayoutManager.ColumnMM(llm, 20);
    ///         new ListLayoutManager.ColumnMM(llm, 50);
    ///
    ///         llm.container_CreateMM(page_Cur, 20, 30);
    ///         llm.Open();
    ///
    ///         <b>TlmRow row = llm.tlmRow_New()</b>;
    ///         <b>row.aTlmCell[1].bp_Back = new BrushProp(this, System.Drawing.Color.FromArgb(255, 230, 230))</b>;
    ///         <b>row.aTlmCell[1].pp_Line = new PenPropMM(this, 0.2, System.Drawing.Color.Red)</b>;
    ///         <b>row.aTlmCell[1].iOrderLineBottom = 1</b>;
    ///         llm.Add(0, new RepString(fp, "01-007"));
    ///         llm.Add(1, new RepString(fp, "Miller"));
    ///         llm.NewRow();
    ///         llm.Add(0, new RepString(fp, "05-052"));
    ///         llm.Add(1, new RepString(fp, "Brown"));
    ///       }
    ///     }
    ///   }
    /// }
    ///   </code>
    ///   <b>Result:</b>
    ///   <para><img src="images/TlmRowSample.gif"/></para>
    /// </example>
    public sealed class TlmRow : TlmRowDef
    {
        /// <summary>Table layout manager base</summary>
        private readonly TlmBase tlmBase;

        /// <summary>false if row must not be commited (e.g. for a header row)</summary>
        internal Boolean bAutoCommit = true;

        private Int32 _iIndex = Int32.MinValue;
        /// <summary>Gets or sets the index of this row within <see cref="TlmBase.aTlmRow"/></summary>
        internal Int32 iIndex
        {
            get
            {
                return _iIndex;
            }
            set
            {
                _iIndex = value;
            }
        }

        /// <summary>Array of all cells of this row</summary>
        /// <seealso cref="TlmRow.ArrayTlmCell.this"/>
        /// <seealso cref="TlmRow.ArrayTlmCell"/>
        /// <remarks>
        /// The cells can be referenced through the iterator <see cref="TlmRow.ArrayTlmCell.this"/>.
        /// <br> </br><para><img src="images/TlmBaseDefaults_ArrayTlmCell.gif"/></para>
        /// </remarks>
        public readonly ArrayTlmCell aTlmCell;

        /// <summary>Top position of the row</summary>
        internal Double rPosTop = 0;

        /// <summary>Bottom position of the row</summary>
        internal Double rPosBottom = Double.NaN;

        /// <summary>Status of the row</summary>
        internal enum Status
        {
            /// <summary>Row is open</summary>
            Open,
            /// <summary>Row is closed</summary>
            Closed
        }

        /// <summary>Status of the row</summary>
        internal Status status = Status.Open;

        /// <summary>Creates a row definition object.</summary>
        /// <param name="tlmBase">Table layout manager of this row</param>
        /// <param name="tlmRow_Prev">The new row will be inserted after <paramref name="tlmRow_Prev"/> or at the beginning if it is <see langword="null"/>.</param>
        /// <param name="aCellCreateType">Array with the cell creation data for each column</param>
        /// <exception cref="ReportException">The row cannot be created.</exception>
        internal TlmRow(TlmBase tlmBase)
        {
            tlmBase.CheckStatus_Open("cannot create a row.");
            this.tlmBase = tlmBase;
            tlmCellEnumerator = new TlmCellEnumerator(this);
            aTlmCell = new ArrayTlmCell(tlmBase.list_TlmColumn.Count);
            PreferredHeight = tlmBase.tlmRowDef_Default.PreferredHeight;
        }

        /// <summary>Creates a row definition object.</summary>
        /// <param name="tlmBase">Table layout manager of this row</param>
        /// <param name="tlmRow_Prev">The new row will be inserted after <paramref name="tlmRow_Prev"/> or at the beginning if it is <see langword="null"/>.</param>
        /// <param name="aCellCreateType">Array with the cell creation data for each column</param>
        /// <exception cref="ReportException">The row cannot be created.</exception>
        internal TlmRow(TlmBase tlmBase, TlmRow tlmRow_Prev, TlmBase.CellCreateType[] aCellCreateType) : this(tlmBase)
        {
            if (tlmBase.list_TlmColumn.Count != aCellCreateType.Length)
            {
                throw new ReportException("The length of the cell create type array must be equal to the number of coulmns that are defined for this table layout manager.");
            }

            for (Int32 iCol = 0; iCol < tlmBase.list_TlmColumn.Count; iCol++)
            {
                TlmColumn col = tlmBase.list_TlmColumn[iCol];
                switch (aCellCreateType[iCol])
                {
                    case TlmBase.CellCreateType.New:
                        {
                            aTlmCell.SetCell(iCol, new TlmCell(col, col, this));
                            break;
                        }
                    case TlmBase.CellCreateType.MergedV:
                        {
                            if (tlmRow_Prev == null)
                            {
                                throw new ReportException("First row cannot be merged vertically.");
                            }
                            TlmCell cell_Prev = tlmRow_Prev.aTlmCell[iCol];
                            if (cell_Prev.tlmColumn_Start.iIndex != iCol)
                            {
                                throw new ReportException("Vertically merged cells must start in the same column.");
                            }
                            Debug.Assert(cell_Prev.tlmRow_End.iIndex == tlmRow_Prev.iIndex);
                            cell_Prev.tlmRow_End = this;
                            while (true)
                            {
                                aTlmCell.SetCell(iCol, cell_Prev);
                                if (iCol >= cell_Prev.tlmColumn_End.iIndex)
                                {
                                    break;
                                }
                                iCol++;
                                if (aCellCreateType[iCol] != TlmBase.CellCreateType.MergedH)
                                {
                                    throw new ReportException("Invalid cell create type of column " + iCol.ToString() + "; 'MergedH' expected");
                                }
                            }
                            break;
                        }
                    case TlmBase.CellCreateType.MergedH:
                        {
                            if (iCol == 0)
                            {
                                throw new ReportException("First column cannot be merged horizonally.");
                            }
                            TlmCell cell_Left = aTlmCell[iCol - 1];
                            if (!Object.ReferenceEquals(cell_Left.tlmRow_Start, this))
                            {
                                throw new ReportException("Horizontally merged cells must start in the same row.");
                            }
                            aTlmCell.SetCell(iCol, cell_Left);
                            Debug.Assert(cell_Left.tlmColumn_End.iIndex + 1 == iCol);
                            cell_Left.tlmColumn_End = col;
                            break;
                        }
                    case TlmBase.CellCreateType.Empty:
                        {
                            break;
                        }
                    default:
                        {
                            Debug.Fail("unknown cell create type");
                            break;
                        }
                }
            }
            tlmBase.InsertRow(tlmRow_Prev, this);

            tlmBase.OnNewRow(this);
        }

        /// <summary>Closes the row.</summary>
        internal void Close()
        {
            if (status == Status.Closed)
            {
                return;
            }
            //Debug.Assert(!Double.IsNaN(rPosBottom));
            tlmBase.OnClosingRow(this);
            foreach (TlmCell cell in aTlmCell)
            {
                cell.Close();
            }
            tlmBase.OnClosingRow(this);
            status = Status.Closed;
        }

        /// <summary>Calculates the bottom position of the row.</summary>
        /// <returns>The bottom position in points (1/72 inch).</returns>
        internal Double rCalculateBottomPos()
        {
            Double rY = 0;
            for (Int32 iCol = 0; iCol < tlmBase.list_TlmColumn.Count; iCol++)
            {
                TlmCell cell = aTlmCell[iCol];
                if (cell == null || cell.tlmColumn_Start.iIndex != iCol || cell.tlmRow_End.iIndex != iIndex)
                {
                    continue;
                }
                Double rMaxY = cell.rCalculateMaxY(false);
                rMaxY += cell.rMarginTop + cell.rMarginBottom + cell.tlmRow_Start.rPosTop;
                if (rMaxY > rY)
                {
                    rY = rMaxY;
                }
            }
            Debug.Assert(!Double.IsNaN(rY));
            return rY;
        }

        /// <summary>Gets or sets the horizontal alignment of the cells (default: left)</summary>
        /// <value>Horizontal alignment: value between 0 and 1, 0:left <see cref="RepObj.rAlignLeft"/>, 0.5:centered <see cref="RepObj.rAlignCenter"/>, 1:right <see cref="RepObj.rAlignRight"/></value>
        public Double rAlignH
        {
            set
            {
                foreach (TlmCell cell in aTlmCell)
                {
                    cell.rAlignH = value;
                }
            }
        }
        /// <summary>Gets or sets the vertical alignment of the cells (default: top)</summary>
        /// <value>Vertical alignment: value between 0 and 1, 0:left <see cref="RepObj.rAlignLeft"/>, 0.5:centered <see cref="RepObj.rAlignCenter"/>, 1:right <see cref="RepObj.rAlignRight"/></value>
        public Double rAlignV
        {
            set
            {
                foreach (TlmCell cell in aTlmCell)
                {
                    cell.rAlignV = value;
                }
            }
        }

        /// <summary>Sets the pen properties of the horizontal lines of the cell.</summary>
        public PenProp penProp_LineH
        {
            set
            {
                foreach (TlmCell tlmCell in aTlmCell)
                {
                    tlmCell.penProp_LineH = value;
                }
            }
        }

        /// <summary>Gets or sets the bottom indent of the row (points, 1/72 inch).</summary>
        public Double rIndentBottom
        {
            set
            {
                foreach (TlmCell tlmCell in aTlmCell)
                {
                    tlmCell.rIndentBottom = value;
                }
            }
        }

        /// <summary>Gets or sets the bottom indent of the row (mm).</summary>
        public Double rIndentBottom_MM
        {
            set { rIndentBottom = RT.rPointFromMM(value); }
        }

        /// <summary>Gets or sets the vertical indents of the row (points, 1/72 inch).</summary>
        public Double rIndentV
        {
            set
            {
                foreach (TlmCell tlmCell in aTlmCell)
                {
                    tlmCell.rIndentV = value;
                }
            }
        }

        /// <summary>Gets or sets the vertical indents of the row (mm).</summary>
        public Double rIndentV_MM
        {
            set { rIndentV = RT.rPointFromMM(value); }
        }

        #region ArrayTlmCell
        /// <summary>Array of all cells of the row</summary>
        /// <remarks>
        /// Eeach row has an instance of this class that holds the cell data.
        /// The cells can be referenced through the iterator <see cref="TlmRow.ArrayTlmCell.this"/>.
        /// <br> </br><para><img src="images/TlmBaseDefaults_ArrayTlmCell.gif"/></para>
        /// </remarks>
        public class ArrayTlmCell
        {
            /// <summary>Array that contains the cells of the row</summary>
            private readonly TlmCell[] aTlmCell;

            /// <summary>Creates the cell array.</summary>
            /// <param name="iSize">Size of the array</param>
            internal ArrayTlmCell(Int32 iSize)
            {
                aTlmCell = new TlmCell[iSize];
            }

            /// <overloads>
            ///   <summary>Gets the specified cell.</summary>
            /// </overloads>
            /// 
            /// <summary>Gets the cell with the specified index.</summary>
            /// <param name="iIndex">Index of the column</param>
            /// <value>Cell with the specified index</value>
            /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmRowSample"]/*'/>
            public TlmCell this[Int32 iIndex]
            {
                get { return aTlmCell[iIndex]; }
            }

            /// <summary>Gets the cell of the specified column.</summary>
            /// <param name="col">Column</param>
            /// <value>Cell of the specified column</value>
            /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmRowSample_ColumnIndexer"]/*'/>
            public TlmCell this[TlmColumn col]
            {
                get { return aTlmCell[col.iIndex]; }
            }

            /// <summary>Sets the cell with the specified index.</summary>
            /// <param name="iIndex">Index</param>
            /// <param name="cell">Cell</param>
            internal void SetCell(Int32 iIndex, TlmCell cell)
            {
                aTlmCell[iIndex] = cell;
            }

            /// <summary>Returns an enumerator that can iterate through the cell array.</summary>
            /// <returns>IEnumerator for the cell array</returns>
            public System.Collections.IEnumerator GetEnumerator()
            {
                return aTlmCell.GetEnumerator();
            }
        }
        #endregion

        #region TlmCellEnumerator
        /// <summary>Array of all cells of the row</summary>
        public class TlmCellEnumerator : System.Collections.IEnumerable
        {
            private readonly TlmRow tlmRow;

            /// <summary>Creates the cell enumerator.</summary>
            /// <param name="tlmRow">Row</param>
            internal TlmCellEnumerator(TlmRow tlmRow)
            {
                this.tlmRow = tlmRow;
            }

            /// <summary>Returns an enumerator that can iterate through the cell array.</summary>
            /// <returns>IEnumerator for the cell array</returns>
            public System.Collections.IEnumerator GetEnumerator()
            {
                return new Enumerator(tlmRow);
            }

            private class Enumerator : System.Collections.IEnumerator
            {
                private readonly TlmRow tlmRow;

                private TlmCell tlmCell = null;

                /// <summary>Creates the enumerator.</summary>
                /// <param name="tlmRow">Row</param>
                internal Enumerator(TlmRow tlmRow)
                {
                    this.tlmRow = tlmRow;
                }

                /// <summary>Gets the current element in the collection.</summary>
                public Object Current
                {
                    get
                    {
                        if (tlmCell == null)
                        {
                            throw new InvalidOperationException("The enumerator is positioned before the first element of the collection or after the last element.");
                        }
                        return tlmCell;
                    }
                }

                /// <summary>Advances the enumerator to the next element of the collection.</summary>
                /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
                public Boolean MoveNext()
                {
                    Int32 iIndex = 0;
                    if (tlmCell != null)
                    {
                        iIndex = tlmCell.tlmColumn_End.iIndex + 1;
                    }

                    do
                    {
                        if (iIndex >= tlmRow.tlmBase.list_TlmColumn.Count)
                        {
                            tlmCell = null;
                            return false;
                        }
                        tlmCell = tlmRow.aTlmCell[iIndex];
                        iIndex++;
                    } while (tlmCell == null);
                    return true;
                }

                /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
                public void Reset()
                {
                    tlmCell = null;
                }
            }
        }

        internal readonly TlmCellEnumerator tlmCellEnumerator;

        #endregion
    }
}
