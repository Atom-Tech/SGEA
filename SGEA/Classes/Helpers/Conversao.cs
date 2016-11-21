using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SGEA
{
    public static class Conversao
    {
        public static int ToInt(this string s) => Convert.ToInt32(s);

        public static double ToDouble(this string s) => Convert.ToDouble(s);

        public static byte[] ImageToByte(BitmapImage img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
