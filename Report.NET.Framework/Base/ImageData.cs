using System.IO;

namespace Root.Reports
{
    /// <summary>Image Data Object</summary>
    public class ImageData
    {
        /// <summary>Image stream</summary>
        internal Stream stream;

        /// <summary>Internal structure used by the formatter</summary>
        internal object oImageResourceX;

        //----------------------------------------------------------------------------------------------------x
        /// <summary>Creates a new image data object</summary>
        /// <param name="stream">Image stream</param>
        public ImageData(Stream stream)
        {
            this.stream = stream;
        }
    }
}
