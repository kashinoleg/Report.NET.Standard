using System;
using System.IO;

namespace Root.Reports
{
    /// <summary></summary>
    public abstract class Formatter
    {
        /// <summary>Report to which this formatter belongs</summary>
        internal ReportBase report;

        /// <summary>Stream to which the result must be written</summary>
        protected Stream _stream;

        /// <summary>Title of the document</summary>
        public string sTitle
        {
            get { return report.sTitle; }
            set { report.sTitle = value; }
        }

        /// <summary>The name of the person who created the document</summary>
        public string sAuthor
        {
            get { return report.sAuthor; }
            set { report.sAuthor = value; }
        }

        /// <summary>Determines the page layout in the PDF document</summary>
        public PageLayout pageLayout = PageLayout.SinglePage;

        /// <summary>Application that created the document</summary>
        public string sCreator;

        /// <summary>Creation date and time of  the document</summary>
        public DateTime dt_CreationDate = DateTime.Today;

        /// <summary>Creates a formatter object.</summary>
        protected Formatter()
        {
        }

        /// <summary>Creates the report.</summary>
        /// <param name="report">Report</param>
        /// <param name="stream">Output stream</param>
        public virtual void Create(ReportBase report, Stream stream)
        {
            this.report = report;
            _stream = stream;
        }

        /// <summary>Gets the output stream object.</summary>
        public Stream stream
        {
            set { _stream = value; }
            get { return _stream; }
        }

        #region Create RepObjX Objects
        internal virtual object oCreate_Container()
        {
            return null;
        }

        internal virtual object oCreate_RepArcBase()
        {
            return null;
        }

        internal virtual object oCreate_RepImage()
        {
            return null;
        }

        internal virtual object oCreate_RepLine()
        {
            return null;
        }

        internal virtual object oCreate_RepRect()
        {
            return null;
        }

        internal virtual object oCreate_RepString()
        {
            return null;
        }

        /// <summary>Creates the extended page data object.</summary>
        /// <param name="page">Page</param>
        /// <returns>Extended page data object</returns>
        internal abstract object oCreate_PageX(Page page);
        #endregion
    }

    #region PageLayout
    /// <summary>PDF Page Layout</summary>
    /// <remarks>This attribute specifies the page layout to be used when the document is opened.</remarks>
    public enum PageLayout
    {
        /// <summary>Display one page at a time.</summary>
        SinglePage,
        /// <summary>Display the pages in one column.</summary>
        OneColumn,
        /// <summary>Display the pages in two columns, with odd-numbered pages on the left.</summary>
        TwoColumnLeft,
        /// <summary>Display the pages in two columns, with odd-numbered pages on the right.</summary>
        TwoColumnRight,
        /// <summary>Display the pages two at a time, with odd-numbered pages on the left.</summary>
        TwoPageLeft,
        /// <summary>Display the pages two at a time, with odd-numbered pages on the right.</summary>
        TwoPageRight
    }
    #endregion
}
