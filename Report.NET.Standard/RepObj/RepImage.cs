using Report.NET.Standard.Base;
using System.Drawing;
using System.IO;

namespace Root.Reports
{
    /// <summary>Report Image Object.</summary>
    public class RepImage : RepObj
    {
        /// <summary>Image stream</summary>
        internal Stream stream;

        /// <summary>Image data</summary>
        internal ImageData imageData;

        /// <summary>Creates a new image object.</summary>
        /// <param name="stream">Image stream</param>
        /// <param name="rWidth">Width of the image</param>
        /// <param name="rHeight">Height of the image</param>
        public RepImage(Stream stream, UnitModel rWidth, UnitModel rHeight)
        {
            this.stream = stream;
            this.rWidth = rWidth != null ? rWidth.Point : double.NaN;
            this.rHeight = rHeight != null ? rHeight.Point : double.NaN;
        }

        /// <summary>This method will be called after the report object has been added to the container.</summary>
        internal protected override void OnAdded()
        {
            oRepObjX = report.formatter.oCreate_RepImage();

            imageData = (ImageData)report.ht_ImageData[stream];
            if (imageData == null)
            {
                imageData = new ImageData(stream);
                report.ht_ImageData.Add(stream, imageData);
            }

            //Changed By TechnoGuru - jjborie@yahoo.fr - http://www.borie.org/
            imageData.stream.Position = 0;
            using (Image image = Image.FromStream(imageData.stream))
            {
                if (double.IsNaN(rWidth))
                {
                    if (double.IsNaN(rHeight))
                    {
                        rWidth = image.Width / image.HorizontalResolution * 72;
                        rHeight = image.Height / image.VerticalResolution * 72; ;
                    }
                    else
                    {
                        rWidth = image.Width * rHeight / image.Height;
                    }
                }
                else if (double.IsNaN(rHeight))
                {
                    rHeight = image.Height * rWidth / image.Width;
                }
            }
        }
    }
}
