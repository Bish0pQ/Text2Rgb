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
        Bitmap CreateImage(Tuple<int,int> size);
        void CheckImageDirectory();
        Tuple<int, int> GetImageSize(string text, int maxWidth);
    }
}
