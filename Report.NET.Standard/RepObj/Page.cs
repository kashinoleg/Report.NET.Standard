using Report.NET.Standard.Base;

namespace Root.Reports
{
    /// <summary>Page of a report</summary>
    /// <example>Page sample:
    /// <code>
    /// using Root.Reports; 
    /// using System; 
    /// 
    /// namespace ReportSamples { 
    ///   public class PageSample : Report { 
    ///     public static void Main() { 
    ///       RT.ViewPDF(new PageSample(), "PageSample.pdf"); 
    ///     } 
    /// 
    ///     protected override void Create() { 
    ///       FontDef fd = new FontDef(this, FontDef.StandardFont.Helvetica); 
    ///       FontProp fp = new FontPropMM(fd, 20); 
    ///       Page page = new Page(this); 
    ///       page.rWidthMM = 100; 
    ///       page.rHeightMM = 50; 
    ///       page.AddCenteredMM(80, new RepString(fp, "Page Sample")); 
    ///     } 
    ///   } 
    /// } 
    /// </code>
    /// </example>
    public sealed class Page : StaticContainer
    {
        /// <summary>Page Number</summary>
        public int iPageNo;

        /// <summary>Page name</summary>
        public string sName;

        /// <summary>Creates a page for the report</summary>
        /// <param name="report">Report to which this page will be add</param>
        public Page(ReportBase report) : base(new UnitModel() { MM = 210.224 }, new UnitModel() { MM = 297.302 })
        {
            report.RegisterPage(this);
            oRepObjX = report.formatter.oCreate_PageX(this);
            iPageNo = report.iPageCount;
        }

        /// <summary>Sets the landscape orientation for this page.</summary>
        public void SetLandscape()
        {
            var r = rHeight;
            rHeight = rWidth;
            rWidth = r;
        }
    }
}
