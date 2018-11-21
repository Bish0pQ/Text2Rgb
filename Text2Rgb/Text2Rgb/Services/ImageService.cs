using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text2Rgb.Interfaces;

namespace Text2Rgb.Services
{
    public class ImageService : IImage
    {
        /* Check if image directory exists */
        public void CheckImageDirectory()
        {
            string imageDirectory = $"{Environment.CurrentDirectory}\\Images\\";
            if (!Directory.Exists(imageDirectory))
            {
                try
                {
                    Directory.CreateDirectory(imageDirectory);
                }
                catch (Exception ex)
                {
                    //Log error
                }
            }
        }

        /* Create the image */
        public Bitmap CreateImage(Tuple<int,int> size)
        {
            throw new NotImplementedException();
        }

        /* Get the size of the image */
        public Tuple<int, int> GetImageSize(int textLength, int maxWidth)
        {
            int compressedLength = textLength / 3; // Divided by 3 because you can fit 3 color codes per pixel
            int charsLeft = textLength % maxWidth; // Get the chars that are left

            int height = compressedLength / maxWidth; // Get the height of the image

            // If the max-width is higher than the expected length needed auto-adjust
            if (compressedLength < maxWidth)
            {
                maxWidth = compressedLength;
                return Tuple.Create(maxWidth, 1);
            }

            //If height is 0 return 1 since it's always 1px large
            if (height <= 0)
            {
                return Tuple.Create(maxWidth, 1);
            }

            return Tuple.Create(maxWidth, height * maxWidth);
        }
    }
}
