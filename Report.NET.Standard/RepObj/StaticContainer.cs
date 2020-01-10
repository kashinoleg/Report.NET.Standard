using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Report.NET.Standard.Base;

namespace Root.Reports
{
    /// <summary>
    /// Summary description for StaticContainer.
    /// </summary>
    public class StaticContainer : Container
    {
        /// <summary>
        /// 
        /// </summary>
        public StaticContainer(UnitModel rWidth, UnitModel rHeight)
        {
            this.rWidth = rWidth;
            this.rHeight = rHeight;
        }

        /// <summary>Adds a report object to the container.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="repObj">Report object to add to the container</param>
        public new void Add(UnitModel rX, UnitModel rY, RepObj repObj)
        {
            //Added By TechnoGuru - jjborie@yahoo.fr - http://www.borie.org/
            //Here we handle image comosed of severals images
            if (repObj is RepImage)
            {
#if !WindowsCE
                RepImage repImage = repObj as RepImage;
                using (Image image = Image.FromStream(repImage.stream))
                {
                    if (image.RawFormat.Equals(ImageFormat.Tiff))
                    {
                        Guid objGuid = (image.FrameDimensionsList[0]);
                        System.Drawing.Imaging.FrameDimension objDimension = new System.Drawing.Imaging.FrameDimension(objGuid);
                        // Numbre of image in the tiff file
                        int totFrame = image.GetFrameCount(objDimension);
                        if (totFrame > 1)
                        {
                            // Saves every frame in a seperate file.
                            for (int i = 0; i < totFrame; i++)
                            {
                                image.SelectActiveFrame(objDimension, i);
                                string tempFile = Path.GetTempFileName() + ".tif";
                                if (image.PixelFormat.Equals(PixelFormat.Format1bppIndexed))
                                {
                                    ImageCodecInfo myImageCodecInfo;
                                    myImageCodecInfo = GetEncoderInfo("image/tiff");
                                    EncoderParameters myEncoderParameters;
                                    myEncoderParameters = new EncoderParameters(1);
                                    myEncoderParameters.Param[0] = new
                                        EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionCCITT4);
                                    image.Save(tempFile, myImageCodecInfo, myEncoderParameters);
                                }
                                else
                                {
                                    image.Save(tempFile, ImageFormat.Tiff);
                                }
                                FileStream stream = new System.IO.FileStream(tempFile, FileMode.Open, FileAccess.Read);

                                if (i == 0)
                                {
                                    repImage.stream = stream;
                                    repObj = repImage as RepObj;
                                }
                                else
                                {
                                    new Page(report);
                                    var di = new RepImage(stream, null, null);
                                    report.page_Cur.Add(new UnitModel(), new UnitModel(), di);
                                }
                            }
                        }
                    }
                }
#endif
            }
            base.Add(rX, rY, repObj);
        }

        //Added By TechnoGuru - jjborie@yahoo.fr - http://www.borie.org/
        /// <summary>
        /// Get encoding info for a mime type
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();
            for (var j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                {
                    return encoders[j];
                }
            }
            return null;
        }

        /// <summary>Adds a report object to the container and sets the alignment.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rAlignH">Horizontal alignment of the report object relative to [X].</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="rAlignV">Vertical alignment of the report object relative to [Y].</param>
        /// <param name="repObj">Report object to add to the container</param>
        public new void AddAligned(UnitModel rX, double rAlignH, UnitModel rY, double rAlignV, RepObj repObj)
        {
            repObj.matrixD.rDX = rX.Point;
            repObj.rAlignH = rAlignH;
            repObj.matrixD.rDY = rY.Point;
            repObj.rAlignV = rAlignV;
            Add(repObj);
        }

        /// <summary>Adds a report object to the container, horizontally centered.</summary>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="repObj">Report object to add to the container</param>
        public void AddCB(UnitModel rY, RepObj repObj)
        {
            base.AddCB(rWidth / 2.0, rY, repObj);
        }

        /// <summary>Adds a report object to the container, right justified.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="repObj">Report object to add to the container</param>
        public void AddRight(UnitModel rX, UnitModel rY, RepObj repObj)
        {
            AddAligned(rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, repObj);
        }
    }
}
