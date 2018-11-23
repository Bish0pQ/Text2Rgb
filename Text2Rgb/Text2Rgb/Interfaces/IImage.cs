using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Text2Rgb.Interfaces
{
    public interface IImage
    {
        Bitmap CreateImage(string text, int maxWidth);
        void CheckImageDirectory();
        Tuple<double, double> GetImageSize(string text, double maxWidth);

    }
}
