using Microsoft.VisualStudio.TestTools.UnitTesting;
using Report.NET.Standard.Reader;
using Root.Reports;
using System.Drawing;
using System.IO;

namespace Report.NET.Core.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ReportBase report = new ReportBase(new PdfFormatter());
            FontDef fd = new FontDef(report, Standard.Fonts.StandardFontEnum.Helvetica);
            FontProp fp = new FontPropMM(fd, 25);
            Page page = new Page(report);

            page.AddCB_MM(80, new RepString(fp, "Hello World!"));

            PenProp pp = new PenProp(report, 10);
            page.AddCB_MM(100, new RepLineMM(pp, 0, 30));

            report.Save("HelloWorld.pdf");
        }

        [TestMethod]
        public void TestMethod2()
        {
            ReportBase report = new ReportBase(new PdfFormatter());
            FontDef fd = new FontDef(report, Standard.Fonts.StandardFontEnum.Helvetica);
            FontProp fp = new FontPropMM(fd, 2.1);
            FontProp fp_Title = new FontPropMM(fd, 18);
            fp_Title.bBold = true;
            BrushProp bp = new BrushProp(report, Color.LightGray);
            PenProp pp = new PenProp(report, 0.2, Color.FromArgb(235, 235, 235));

            var page = new Page(report);
            double rY = 40;
            page.AddCB_MM(rY, new RepString(fp_Title, "Image Sample"));
            fp_Title.rSizeMM = 4;
            var stream = File.Open("Image/Sample.jpg", FileMode.Open, FileAccess.Read, FileShare.Read);
            //System.IO.Stream stream = GetType().Assembly.GetManifestResourceStream("Report.NetCore.Tests.Image.Sample.jpg");
            Assert.IsNotNull(stream);
            page.AddMM(20, 90, new RepImageMM(stream, 40, double.NaN));
            page.AddMM(20, 95, new RepString(fp, "W = 40mm, H = auto."));
            page.AddMM(67, 90, new RepImageMM(stream, 40, 20));
            page.AddMM(67, 95, new RepString(fp, "W = 40mm, H = 20mm"));
            page.AddMM(114, 90, new RepImageMM(stream, double.NaN, 30));
            page.AddMM(114, 95, new RepString(fp, "W = auto., H = 30mm"));
            page.AddMM(161, 90, new RepImageMM(stream, 30, 30));
            page.AddMM(161, 95, new RepString(fp, "W = 30mm, H = 30mm"));
            rY += 150;

            // adjust the size of a bounding rectangle
            RepRect dr = new RepRectMM(bp, 80, 60);
            page.AddMM(20, rY, dr);
            RepImage di = new RepImageMM(stream, 70, double.NaN);
            page.AddMM(25, rY - 5, di);
            dr.rHeightMM = di.rHeightMM + 10;

            // rotated image
            di = new RepImageMM(stream, 40, 30);
            di.RotateTransform(-15);
            page.AddMM(120, rY - 33, di);

            // rotated image with rectangle
            StaticContainer sc = new StaticContainer(RT.rPointFromMM(45), RT.rPointFromMM(35));
            page.AddMM(145, rY - 35, sc);
            sc.RotateTransform(15);
            sc.AddMM(0, 35, new RepRectMM(bp, 45, 35));
            sc.AddMM(1.25, 33.75, new RepLineMM(pp, 42.5, 0));
            sc.AddMM(1.25, 1.25, new RepLineMM(pp, 42.5, 0));
            sc.AddAlignedMM(22.5, RepObj.rAlignCenter, 17.5, RepObj.rAlignCenter, new RepImageMM(stream, 40, 30));
            rY += 30;

            // alignment sample
            page.AddMM(20, rY, new RepString(fp_Title, "Alignment"));
            rY += 18;
            int rX = 100;
            double rD = 20;
            bp.color = Color.DarkSalmon;
            page.AddMM(rX, rY + rD, new RepRectMM(bp, rD, rD));
            page.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, new RepImageMM(stream, 20, double.NaN));
            page.AddAlignedMM(rX, RepObj.rAlignRight, rY + rD, RepObj.rAlignTop, new RepImageMM(stream, 20, double.NaN));
            page.AddMM(rX + rD, rY, new RepImageMM(stream, 20, double.NaN));  // default
            page.AddAlignedMM(rX + rD, RepObj.rAlignLeft, rY + rD, RepObj.rAlignTop, new RepImageMM(stream, 20, double.NaN));
            page.AddAlignedMM(rX + rD / 2, RepObj.rAlignCenter, rY + rD / 2, RepObj.rAlignCenter, new RepImageMM(stream, 10, double.NaN));
            //*/
            report.Save("ImageSample.pdf");
        }

        [TestMethod]
        public void TestMethod3()
        {
            //arrays
            ReportBase report = new ReportBase(new PdfFormatter());
            FontDef fd = new FontDef(report, "Roboto");
            //act
            using (var stream = new FileStream("Fonts/Roboto-Regular.ttf", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fd.aFontData[(int)FontStyle.Regular] = new OpenTypeFontData(fd, FontStyle.Regular, stream, FontTypeEnum.TTF);
            }
            using (var stream = new FileStream("Fonts/Roboto-Bold.ttf", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fd.aFontData[(int)FontStyle.Bold] = new OpenTypeFontData(fd, FontStyle.Bold, stream, FontTypeEnum.TTF);
            }
            using (var stream = new FileStream("Fonts/Roboto-Italic.ttf", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fd.aFontData[(int)FontStyle.Italic] = new OpenTypeFontData(fd, FontStyle.Italic, stream, FontTypeEnum.TTF);
            }
            using (var stream = new FileStream("Fonts/Roboto-BoldItalic.ttf", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fd.aFontData[(int)(FontStyle.Bold | FontStyle.Italic)] = new OpenTypeFontData(fd, FontStyle.Bold | FontStyle.Italic, stream, FontTypeEnum.TTF);
            }
            //assert
            Assert.IsNotNull(fd.aFontData[(int)FontStyle.Regular]);
            Assert.IsNotNull(fd.aFontData[(int)FontStyle.Bold]);
            Assert.IsNotNull(fd.aFontData[(int)FontStyle.Italic]);
            Assert.IsNotNull(fd.aFontData[(int)(FontStyle.Bold | FontStyle.Italic)]);

            FontProp fp = new FontPropMM(fd, 25);
            Page page = new Page(report);

            page.AddCB_MM(80, new RepString(fp, "Hello World!"));

            PenProp pp = new PenProp(report, 10);
            page.AddCB_MM(100, new RepLineMM(pp, 0, 30));

            report.Save("HelloWorld.pdf");
        }
    }
}
