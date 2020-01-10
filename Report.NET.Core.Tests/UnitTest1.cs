using Microsoft.VisualStudio.TestTools.UnitTesting;
using Report.NET.Standard.Base;
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
            var report = new ReportBase(new PdfFormatter());
            var fd = new FontDef(report, Standard.Fonts.StandardFontEnum.Helvetica);
            var fp = new FontProp(fd, new UnitModel() { MM = 25 });
            var page = new Page(report);

            page.AddCB(new UnitModel() { MM = 80 }, new RepString(fp, "Hello World!"));

            var pp = new PenProp(report, new UnitModel() { Point = 10 });
            page.AddCB(new UnitModel() { MM = 100 }, new RepLine(pp, new UnitModel() { MM = 0 }, new UnitModel() { MM = 30 }));

            report.Save("HelloWorld.pdf");
        }

        [TestMethod]
        public void TestMethod2()
        {
            var report = new ReportBase(new PdfFormatter());
            var fd = new FontDef(report, Standard.Fonts.StandardFontEnum.Helvetica);
            var fp = new FontProp(fd, new UnitModel() { MM = 2.1 });
            var fp_Title = new FontProp(fd, new UnitModel() { MM = 18 });
            fp_Title.bBold = true;
            var bp = new BrushProp(report, Color.LightGray);
            var pp = new PenProp(report, new UnitModel() { Point = 0.2 }, Color.FromArgb(235, 235, 235));

            var page = new Page(report);
            double rY = 40;
            page.AddCB(new UnitModel() { MM = rY }, new RepString(fp_Title, "Image Sample"));
            fp_Title.rSize = new UnitModel() { MM = 4 };
            var stream = File.Open("Image/Sample.jpg", FileMode.Open, FileAccess.Read, FileShare.Read);
            //System.IO.Stream stream = GetType().Assembly.GetManifestResourceStream("Report.NetCore.Tests.Image.Sample.jpg");
            Assert.IsNotNull(stream);
            page.Add(new UnitModel() { MM = 20 }, new UnitModel() { MM = 90 }, new RepImage(stream, new UnitModel() { MM = 40 }, null));
            page.Add(new UnitModel() { MM = 20 }, new UnitModel() { MM = 95 }, new RepString(fp, "W = 40mm, H = auto."));
            page.Add(new UnitModel() { MM = 67 }, new UnitModel() { MM = 90 }, new RepImage(stream, new UnitModel() { MM = 40 }, new UnitModel() { MM = 20 }));
            page.Add(new UnitModel() { MM = 67 }, new UnitModel() { MM = 95 }, new RepString(fp, "W = 40mm, H = 20mm"));
            page.Add(new UnitModel() { MM = 114 }, new UnitModel() { MM = 90 }, new RepImage(stream, null, new UnitModel() { MM = 30 }));
            page.Add(new UnitModel() { MM = 114 }, new UnitModel() { MM = 95 }, new RepString(fp, "W = auto., H = 30mm"));
            page.Add(new UnitModel() { MM = 161 }, new UnitModel() { MM = 90 }, new RepImage(stream, new UnitModel() { MM = 30 }, new UnitModel() { MM = 30 }));
            page.Add(new UnitModel() { MM = 161 }, new UnitModel() { MM = 95 }, new RepString(fp, "W = 30mm, H = 30mm"));
            rY += 150;

            // adjust the size of a bounding rectangle
            RepRect dr = new RepRect(bp, new UnitModel() { MM = 80 }, new UnitModel() { MM = 60 });
            page.Add(new UnitModel() { MM = 20 }, new UnitModel() { MM = rY }, dr);
            RepImage di = new RepImage(stream, new UnitModel() { MM = 70 }, null);
            page.Add(new UnitModel() { MM = 25 }, new UnitModel() { MM = rY - 5 }, di);
            dr.rHeight = new UnitModel() { MM = di.rHeight.MM + 10 };

            // rotated image
            di = new RepImage(stream, new UnitModel() { MM = 40 }, new UnitModel() { MM = 30 });
            di.RotateTransform(-15);
            page.Add(new UnitModel() { MM = 120 }, new UnitModel() { MM = rY - 33 }, di);

            // rotated image with rectangle
            StaticContainer sc = new StaticContainer(new UnitModel() { MM = 45 }, new UnitModel() { MM = 35 });
            page.Add(new UnitModel() { MM = 145 }, new UnitModel() { MM = rY - 35 }, sc);
            sc.RotateTransform(15);
            sc.Add(new UnitModel() { MM = 0 }, new UnitModel() { MM = 35 }, new RepRect(bp, new UnitModel() { MM = 45 }, new UnitModel() { MM = 35 }));
            sc.Add(new UnitModel() { MM = 1.25 }, new UnitModel() { MM = 33.75 }, new RepLine(pp, new UnitModel() { MM = 42.5 }, new UnitModel() { MM = 0 }));
            sc.Add(new UnitModel() { MM = 1.25 }, new UnitModel() { MM = 1.25 }, new RepLine(pp, new UnitModel() { MM = 42.5 }, new UnitModel() { MM = 0 }));
            sc.AddAligned(new UnitModel() { MM = 22.5 }, RepObj.rAlignCenter, new UnitModel() { MM = 17.5 }, RepObj.rAlignCenter, new RepImage(stream, new UnitModel() { MM = 40 }, new UnitModel() { MM = 30 }));
            rY += 30;

            // alignment sample
            page.Add(new UnitModel() { MM = 20 }, new UnitModel() { MM = rY }, new RepString(fp_Title, "Alignment"));
            rY += 18;
            int rX = 100;
            double rD = 20;
            bp.color = Color.DarkSalmon;
            page.Add(new UnitModel() { MM = rX }, new UnitModel() { MM = rY + rD }, new RepRect(bp, new UnitModel() { MM = rD }, new UnitModel() { MM = rD }));
            page.AddAligned(new UnitModel() { MM = rX }, RepObj.rAlignRight, new UnitModel() { MM = rY }, RepObj.rAlignBottom, new RepImage(stream, new UnitModel() { MM = 20 }, null));
            page.AddAligned(new UnitModel() { MM = rX }, RepObj.rAlignRight, new UnitModel() { MM = rY + rD }, RepObj.rAlignTop, new RepImage(stream, new UnitModel() { MM = 20 }, null));
            page.Add(new UnitModel() { MM = rX + rD }, new UnitModel() { MM = rY }, new RepImage(stream, new UnitModel() { MM = 20 }, null));  // default
            page.AddAligned(new UnitModel() { MM = rX + rD }, RepObj.rAlignLeft, new UnitModel() { MM = rY + rD }, RepObj.rAlignTop, new RepImage(stream, new UnitModel() { MM = 20 }, null));
            page.AddAligned(new UnitModel() { MM = rX + rD / 2 }, RepObj.rAlignCenter, new UnitModel() { MM = rY + rD / 2 }, RepObj.rAlignCenter, new RepImage(stream, new UnitModel() { MM = 10 }, null));
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

            var fp = new FontProp(fd, new UnitModel() { MM = 25 });
            var page = new Page(report);

            page.AddCB(new UnitModel() { MM = 80 }, new RepString(fp, "Hello World!"));

            var pp = new PenProp(report, new UnitModel() { MM = 1 });
            page.AddCB(new UnitModel() { MM = 100 }, new RepLine(pp, new UnitModel() { MM = 0 }, new UnitModel() { MM = 30 }));

            report.Save("HelloWorld.pdf");
        }
    }
}
