using System;
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
        /// <param name="sFileName">Filename of the Image</param>
        /// <param name="rWidth">Width of the image</param>
        /// <param name="rHeight">Height of the image</param>
        public RepImage(String sFileName, Double rWidth, Double rHeight)
        {
            this.stream = new FileStream(sFileName, FileMode.Open);
            this.rWidth = rWidth;
            this.rHeight = rHeight;
        }

        /// <summary>Creates a new image object.</summary>
        /// <param name="stream">Image stream</param>
        /// <param name="rWidth">Width of the image</param>
        /// <param name="rHeight">Height of the image</param>
        public RepImage(Stream stream, Double rWidth, Double rHeight)
        {
            this.stream = stream;
            this.rWidth = rWidth;
            this.rHeight = rHeight;
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
#if !WindowsCE
            using (Image image = Image.FromStream(imageData.stream))
            {
                if (Double.IsNaN(rWidth))
                {
                    if (Double.IsNaN(rHeight))
                    {
                        rWidth = image.Width / image.HorizontalResolution * 72;
                        rHeight = image.Height / image.VerticalResolution * 72; ;
                    }
                    else
                    {
                        rWidth = image.Width * rHeight / image.Height;
                    }
                }
                else if (Double.IsNaN(rHeight))
                {
                    rHeight = image.Height * rWidth / image.Width;
                }
            }
#endif
        }
    }


    //****************************************************************************************************
    /// <summary>Creates a new image object.</summary>
    public class RepImageMM : RepImage
    {
        /// <summary>Creates a new Image object with millimeter values</summary>
        /// <param name="sFileName">Filename of the Image</param>
        /// <param name="rWidth">Width of the image in millimeter</param>
        /// <param name="rHeight">Height of the image in millimeter</param>
        public RepImageMM(String sFileName, Double rWidth, Double rHeight) : base(sFileName, RT.rPointFromMM(rWidth), RT.rPointFromMM(rHeight))
        {
        }

        /// <summary>Creates a new Image object with millimeter values</summary>
        /// <param name="stream">Image stream</param>
        /// <param name="rWidth">Width of the image in millimeter</param>
        /// <param name="rHeight">Height of the image in millimeter</param>
        public RepImageMM(Stream stream, Double rWidth, Double rHeight) : base(stream, RT.rPointFromMM(rWidth), RT.rPointFromMM(rHeight))
        {
        }
    }
}
