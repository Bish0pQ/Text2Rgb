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

namespace Text2Rgb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageService imgService;
        Bitmap bm;
        
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
    }
}
