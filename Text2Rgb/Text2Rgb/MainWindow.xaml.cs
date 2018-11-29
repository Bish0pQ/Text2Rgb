using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Text2Rgb.Services;
using System.IO;
using Microsoft.Win32;

namespace Text2Rgb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageService imgService;
        Bitmap bm;
        Bitmap loadedBm;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void BtnCreateImage_Click(object sender, RoutedEventArgs e)
        {
            //Check if max-width is parseble
            int maxWidth;

            if (!int.TryParse(txtMaxWidth.Text, out maxWidth))
            {
                MessageBox.Show("Please only type in whole numbers in the max width field.", "Error: you can only use whole numbers in the max-width field", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMaxWidth.Clear();
            }
            else if(string.IsNullOrEmpty(txtInput.Text))
            {
                MessageBox.Show("The input in the inputfield can not be empty since there would be nothing to convert to RGB. Please typ in some text in the big textbox.", "Error: no input detected!");
            }
            else
            {
                try
                {
                    bm = imgService.CreateImage(txtInput.Text, maxWidth);
                    btnSaveImage.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    //Log exception
                    FileHelper.LogError(ex);
                }
                
            }
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnSaveImage.IsEnabled = false;
            imgService = new ImageService();
            imgService.CheckImageDirectory();
        }

        private void BtnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            /* Save the image locally */
            try
            {
                string savePath = $"{Environment.CurrentDirectory}\\Images\\example.jpg";
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }

                bm.Save(savePath);
            }
            catch (Exception ex)
            {
                FileHelper.LogError(ex);
            }
        }

        private void BtnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            string path = string.Empty;

            /* Show openfiledialog and set bitmap */
            OpenFileDialog ofd = new OpenFileDialog();

            //Settings
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF) | *.BMP;*.JPG;*.GIF";
            ofd.FilterIndex = 0;
            ofd.Title = "Text2Rgb | Select an image";

            if (ofd.ShowDialog() == true)
            {
                path = ofd.FileName;
            }

            if (File.Exists(path))
            {
                try
                {
                    loadedBm = new Bitmap(path);
                    MessageBox.Show("Image has succesfully been loaded!", "Success: image has been loaded!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    FileHelper.LogError(ex);
                    MessageBox.Show("Unable to open the image, please make sure the image file is not corrupted and try again.", "Error: could not load image!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
               
            }
        }

        private void BtnDecodeImage_Click(object sender, RoutedEventArgs e)
        {
            // Check if an image is loaded
            if (loadedBm == null)
            {
                MessageBox.Show("It seems you have not loaded an image yet! Please make sure you load an image prior to decoding!", "Error: no image has been loaded", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
