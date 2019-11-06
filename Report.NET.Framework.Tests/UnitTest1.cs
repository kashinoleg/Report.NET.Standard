using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Root.Reports;

namespace Report.NET.Framework.Tests
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

            report.Save("HelloWorld.pdf");

        }
    }
}
