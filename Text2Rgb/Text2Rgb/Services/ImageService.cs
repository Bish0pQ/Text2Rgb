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
    public  class ImageService : IImage
    {

        public ImageService() { }

        /* Check if image directory exists */
        public  void CheckImageDirectory()
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
                    FileHelper.LogError(ex);
                }
            }
        }

        /* Create the image */
        public  Bitmap CreateImage(string text, int maxWidth)
        {
            string workingText = text;
            string currentText = string.Empty;
      
            Tuple<double, double> imgSize = GetImageSize(text, maxWidth);
            Bitmap b = new Bitmap(int.Parse(imgSize.Item1.ToString()), int.Parse(imgSize.Item2.ToString()));

            double pixels = imgSize.Item1 * imgSize.Item2;
            int y = 1;

            for (int x = 0; x < pixels; x++)
            {
                try
                {
                    currentText = workingText.Substring(0, 3);
                    workingText = workingText.Remove(3);

                    switch (currentText.Length)
                    {
                        case 3:
                            b.SetPixel(x, y, Color.FromArgb(currentText[0], currentText[1], currentText[2]));
                            break;
                        case 2:
                            b.SetPixel(x, y, Color.FromArgb(currentText[0], currentText[1], 0));
                            break;
                        case 1:
                            b.SetPixel(x, y, Color.FromArgb(currentText[1], 0, 0));
                            break;
                        default:
                            break;
                    }


                    // Check if we need to start with next Y coord
                    if (x % maxWidth == 0 && x != 0)
                    {
                        //Start next Y coord
                        y++;
                    }
                }
                catch (Exception ex)
                {
                    FileHelper.LogError(ex);
                }          
            }

            return b;
        }

    

        /* Get the size of the image */
        public Tuple<double, double> GetImageSize(string text, double maxWidth)
        {
            double height = 0;
            double textLength = text.Length;
            double compressedLength = textLength / 3; // Divided by 3 because you can fit 3 color codes per pixel
            double charsLeft = textLength % maxWidth; // Get the chars that are left

            // Calculate the total height
            height = Math.Ceiling(compressedLength / maxWidth);

            // If the max-width is higher than the expected length needed auto-adjust
            if (compressedLength < maxWidth)
            {
                maxWidth = compressedLength;
                return Tuple.Create(maxWidth, (double)1);
            }

            //If height is 0 return 1 since it's always 1px large
            if (height <= 0)
            {
                return Tuple.Create(maxWidth, (double)1);
            }

            return Tuple.Create(maxWidth, height);
        }


    }
}
