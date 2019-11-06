using System;
using System.Drawing;

namespace Root.Reports
{
    /// <summary>Table Layout Manager</summary>
    public class TableLayoutManager : TlmBase
    {
        //====================================================================================================x
        /// <summary>Definition of the default properties of a cell of this table</summary>
        public readonly TlmCellDef tlmCellDef_Header;

        /// <summary>Definition of the default properties of a row of this table</summary>
        public readonly TlmRowDef tlmRowDef_Header;
        //----------------------------------------------------------------------------------------------------x
        /// <summary>Creates a new table layout manager.</summary>
        /// <param name="report">Report of this table layout manager</param>
        public TableLayoutManager(ReportBase report) : base(report)
        {
            tlmHeightMode = TlmHeightMode.Static;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -x
            PenProp penProp_Solid = new PenProp(report, 0.5, Color.Black);
            tlmCellDef_Default.penProp_LineV = penProp_Solid;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -x
            tlmColumnDef_Default.penProp_BorderH = penProp_Solid;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -x
            tlmCellDef_Header = new TlmCellDef();
            tlmRowDef_Header = new TlmRowDef();

            tlmCellDef_Header.rAlignH = RepObj.rAlignLeft;
            tlmCellDef_Header.rAlignV = RepObj.rAlignTop;
            tlmCellDef_Header.rAngle = 0;
            tlmCellDef_Header.tlmTextMode = TlmTextMode.MultiLine;
            tlmCellDef_Header.rLineFeed = 72.0 / 6;

            tlmCellDef_Header.rMargin = 0;
            tlmCellDef_Header.rIndentH_MM = 1;
            tlmCellDef_Header.rIndentV_MM = 2;

            tlmCellDef_Header.brushProp_Back = new BrushProp(report, Color.FromArgb(220, 220, 220));
            tlmCellDef_Header.penProp_Line = penProp_Solid;
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Creates a new table layout manager.</summary>
        /// <param name="fp_Header">Font property of the header</param>
        public TableLayoutManager(FontProp fontProp_Header) : this(fontProp_Header.fontDef.report)
        {
            this.fontProp_Header = fontProp_Header;
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary></summary>
        public CellCreateType[] aCellCreateType = null;

        /// <summary>Creates the table header.</summary>
        private void CreateHeader(Container cont)
        {
            TlmRow row = tlmRow_New((TlmRow)null, aCellCreateType);
            row.bAutoCommit = false;
            row.rPreferredHeight = tlmRowDef_Header.rPreferredHeight;

            foreach (TlmColumn col in list_TlmColumn)
            {
                TlmCell cell = row.aTlmCell[col.iIndex];

                TlmCellDef hd_Base = tlmCellDef_Header;
                TlmCellDef hd_Col = col.tlmCellDef_Header;

                cell.rAlignH = (Double.IsNaN(hd_Col.rAlignH) ? hd_Base.rAlignH : hd_Col.rAlignH);
                cell.rAlignV = (Double.IsNaN(hd_Col.rAlignV) ? hd_Base.rAlignV : hd_Col.rAlignV);
                cell.rAngle = (Double.IsNaN(hd_Col.rAngle) ? hd_Base.rAngle : hd_Col.rAngle);
                cell.tlmTextMode = (hd_Col.tlmTextMode == TlmTextMode.FallBack ? hd_Base.tlmTextMode : hd_Col.tlmTextMode);
                cell.rLineFeed = (Double.IsNaN(hd_Col.rLineFeed) ? hd_Base.rLineFeed : hd_Col.rLineFeed);

                cell.rMarginLeft = (Double.IsNaN(hd_Col.rMarginLeft) ? hd_Base.rMarginLeft : hd_Col.rMarginLeft);
                cell.rMarginRight = (Double.IsNaN(hd_Col.rMarginRight) ? hd_Base.rMarginRight : hd_Col.rMarginRight);
                cell.rMarginTop = (Double.IsNaN(hd_Col.rMarginTop) ? hd_Base.rMarginTop : hd_Col.rMarginTop);
                cell.rMarginBottom = (Double.IsNaN(hd_Col.rMarginBottom) ? hd_Base.rMarginBottom : hd_Col.rMarginBottom);

                cell.rIndentLeft = (Double.IsNaN(hd_Col.rIndentLeft) ? hd_Base.rIndentLeft : hd_Col.rIndentLeft);
                cell.rIndentRight = (Double.IsNaN(hd_Col.rIndentRight) ? hd_Base.rIndentRight : hd_Col.rIndentRight);
                cell.rIndentTop = (Double.IsNaN(hd_Col.rIndentTop) ? hd_Base.rIndentTop : hd_Col.rIndentTop);
                cell.rIndentBottom = (Double.IsNaN(hd_Col.rIndentBottom) ? hd_Base.rIndentBottom : hd_Col.rIndentBottom);

                cell.brushProp_Back = (Object.ReferenceEquals(hd_Col.brushProp_Back, BrushProp.bp_Null) ? hd_Base.brushProp_Back : hd_Col.brushProp_Back);

                cell.penProp_LineLeft = (Object.ReferenceEquals(hd_Col.penProp_LineLeft, PenProp.penProp_Null) ? hd_Base.penProp_LineLeft : hd_Col.penProp_LineLeft);
                cell.penProp_LineRight = (Object.ReferenceEquals(hd_Col.penProp_LineRight, PenProp.penProp_Null) ? hd_Base.penProp_LineRight : hd_Col.penProp_LineRight);
                cell.penProp_LineTop = (Object.ReferenceEquals(hd_Col.penProp_LineTop, PenProp.penProp_Null) ? hd_Base.penProp_LineTop : hd_Col.penProp_LineTop);
                cell.penProp_LineBottom = (Object.ReferenceEquals(hd_Col.penProp_LineBottom, PenProp.penProp_Null) ? hd_Base.penProp_LineBottom : hd_Col.penProp_LineBottom);

                cell.iOrderLineLeft = (hd_Col.iOrderLineLeft == Int32.MinValue ? hd_Base.iOrderLineLeft : hd_Col.iOrderLineLeft);
                cell.iOrderLineRight = (hd_Col.iOrderLineRight == Int32.MinValue ? hd_Base.iOrderLineRight : hd_Col.iOrderLineRight);
                cell.iOrderLineTop = (hd_Col.iOrderLineTop == Int32.MinValue ? hd_Base.iOrderLineTop : hd_Col.iOrderLineTop);
                cell.iOrderLineBottom = (hd_Col.iOrderLineBottom == Int32.MinValue ? hd_Base.iOrderLineBottom : hd_Col.iOrderLineBottom);

                if (col.sHeader != null)
                {
                    cell.Add(new RepString(col.fontProp_Header, col.sHeader));
                }
            }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Raises the NewContainer event.</summary>
        /// <param name="ea">Event argument</param>
        internal protected override void OnNewContainer(NewContainerEventArgs ea)
        {
            base.OnNewContainer(ea);
            CreateHeader(container_Cur);
        }

        //----------------------------------------------------------------------------------------------------x
        // Virtual Methods
        //----------------------------------------------------------------------------------------------------x

        //----------------------------------------------------------------------------------------------------x
        /// <summary>This method will be called before the row will be closed.</summary>
        /// <param name="row">Row that will be closed</param>
        internal protected override void OnClosingRow(TlmRow row)
        {
            if (row.iIndex != 1)
            {
                return;
            }
            for (Int32 iCol = 0; iCol < list_TlmColumn.Count; iCol++)
            {
                TlmCell cell = row.aTlmCell[iCol];
                if (cell.tlmColumn_Start.iIndex != iCol)
                {
                    continue;
                }
                TlmColumn col = list_TlmColumn[iCol];
                if (!Double.IsNaN(col.rBorderTop))
                {
                    cell.rMarginTop = col.rBorderTop;
                }
                if (!Object.ReferenceEquals(col.penProp_BorderTop, PenProp.penProp_Null))
                {
                    cell.penProp_LineTop = col.penProp_BorderTop;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------x
        /// <summary>This method will be called before the report objects will be written to the container.</summary>
        internal override void OnBeforeWrite()
        {
            for (Int32 iCol = 0; iCol < list_TlmColumn.Count; iCol++)
            {
                TlmCell cell = tlmRow_Committed.aTlmCell[iCol];
                if (cell.tlmColumn_Start.iIndex != iCol)
                {
                    continue;
                }
                TlmColumn col = list_TlmColumn[iCol];
                if (!Double.IsNaN(col.rBorderBottom))
                {
                    cell.rMarginBottom = col.rBorderBottom;
                }
                if (!Object.ReferenceEquals(col.penProp_BorderBottom, PenProp.penProp_Null))
                {
                    cell.penProp_LineBottom = col.penProp_BorderBottom;
                }
            }
        }
    }
}
