using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Text2Rgb.Interfaces;

namespace Text2Rgb.Services
{
    public class ImageService : IImage
    {

        public ImageService() { }

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
                    FileHelper.LogError(ex);
                }
            }
        }

        /* Create the image */
        public Bitmap CreateImage(string text, int maxWidth)
        {
            string workingText = text;
            string currentText = string.Empty;

            Tuple<double, double> imgSize = GetImageSize(text, maxWidth);
            Bitmap b = new Bitmap(int.Parse(imgSize.Item1.ToString()), int.Parse(imgSize.Item2.ToString()));

            double pixels = imgSize.Item1 * imgSize.Item2;
            int y = 0;
            int x = 0;

            Task convertToRgb = new Task(() => {
                for (int i = 0; i < pixels; i++)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(workingText))
                        {
                            if (workingText.Length < 3 && workingText.Length > 0)
                            {
                                currentText = workingText.Substring(workingText.Length);
                                workingText = workingText.Remove(0, workingText.Length);
                            }
                            else if (workingText.Length > 3)
                            {
                                currentText = workingText.Substring(0, 3);
                                workingText = workingText.Remove(0, 3);
                            }
                        }
                        else
                        {
                            b.SetPixel(x, y, Color.FromArgb(0, 0, 0)); //Set a white pixel
                            x++;
                            // Check if we need to start with next Y coord
                            if ((x + 1) % maxWidth == 0 && x != 0)
                            {
                                //Start next Y coord if it does not exceed maximum height

                                y++;
                                x = 0;
                            }
                            continue;
                        }

                        //Check if workingText is less than 3 chars
                        if (workingText.Length == 2)
                        {
                            currentText = workingText.Substring(0, 2);
                            workingText = workingText.Remove(0, 2);
                        }
                        else if (workingText.Length == 1)
                        {
                            currentText = workingText.Substring(0, 1);
                            workingText = workingText.Remove(0, 1);
                        }
                        else
                        {
                            currentText = workingText.Substring(0, 3);
                            workingText = workingText.Remove(0, 3);
                        }

                        //Create the pixel
                        b.SetPixel(x, y, GetColorFromString(currentText));
                        x++;

                        // Check if we need to start with next Y coord
                        if ((x + 1) % maxWidth == 0 && x != 0)
                        {
                            //Start next Y coord
                            y++;
                            x = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        FileHelper.LogError(ex, $"Logged at coordinate ({x},{y}) Exception occurred with drawing pixel {i}");
                    }
                }
            });

            convertToRgb.Start();
            convertToRgb.Wait();
            MessageBox.Show("The image has succesfully been created!", "Image is created!", MessageBoxButton.OK, MessageBoxImage.Information);

            return b;
        }

        public string DecodeImage(Bitmap image)
        {
            string decodedText = string.Empty;

            // Image does not have a height of width
            if (image.Height <= 0 || image.Width <= 0)
            {
                return null;
            }

            try
            {
                // Loop through height
                for (int y = 0; y < image.Height; y++)
                {
                    // Loop through width
                    for (int x = 0; x < image.Width; x++)
                    {
                        Color c = image.GetPixel(x, y);
                        string s = $"{(char)c.R}{(char)c.G}{(char)c.B}";
                        decodedText += s;
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.LogError(ex);
            }

            return decodedText;

        }

     

        /* Get the color code */
        private Color GetColorFromString(string charList)
        {
            int stringLength = charList.Length;
            int[] _codeList = new int[stringLength];
            byte[] _buffer;

            //Get ASCII codes
            for (int i = 0; i < stringLength; i++)
            {
                _buffer = Encoding.ASCII.GetBytes(charList[i].ToString());
                _codeList[i] = _buffer[0];
                _buffer = null;
            }

            switch (stringLength)
            {
                case 1:
                    return Color.FromArgb(_codeList[0], 0, 0);
                case 2:
                    return Color.FromArgb(_codeList[0], _codeList[1], 0);
                case 3:
                    return Color.FromArgb(_codeList[0], _codeList[1], _codeList[2]);
                default:
                    return Color.FromArgb(0, 0, 0);
            }
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
