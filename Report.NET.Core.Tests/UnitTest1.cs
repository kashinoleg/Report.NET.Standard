using Microsoft.VisualStudio.TestTools.UnitTesting;
using Root.Reports;

namespace Report.NET.Core.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ReportBase report = new ReportBase(new PdfFormatter());
            FontDef fd = new FontDef(report, "Helvetica");
            FontProp fp = new FontPropMM(fd, 25);
            Page page = new Page(report);

            page.AddCB_MM(80, new RepString(fp, "Hello World!"));

            PenProp pp = new PenProp(report, 10);
            page.AddCB_MM(100, new RepLineMM(pp, 0, 30));

            report.Save("HelloWorld.pdf");
        }
    }
}
