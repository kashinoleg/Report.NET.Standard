using System;

namespace Root.Reports
{
    /// <summary>List Layout Manager</summary>
    public class ListLayoutManager : TlmBase
    {
        /// <summary>Creates a new list layout manager.</summary>
        /// <param name="report">Report object of this list layout manager</param>
        public ListLayoutManager(ReportBase report) : base(report)
        {
            //pp_Border = new PenPropMM(report, 0.1, Color.Black);
        }

        /// <summary>This method will be called after a new row has been created.</summary>
        /// <param name="row"></param>
        internal protected override void OnNewRow(TlmRow row)
        {
            if (row.iIndex != 0)
            {
                return;
            }
            foreach (TlmColumn col in list_TlmColumn)
            {
                TlmCell cell = row.aTlmCell[col.iIndex];
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

        /// <summary>This method will be called before the report objects will be written to the container.</summary>
        internal override void OnBeforeWrite()
        {
            foreach (TlmColumn col in list_TlmColumn)
            {
                TlmCell cell = tlmRow_Committed.aTlmCell[col.iIndex];
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
