using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Apresentacao.Slide
{
    public static class SlideFrame
    {
        private static Main slideMestre;
        public static int s { get; private set; }
        public static bool[] block = new bool[14];

        public static void getSlide(Main m)
        {
            slideMestre = m;
        }

        public static void TrocaSlide(int s)
        {
            SlideFrame.s = s;
            Main.Barra.Content[s].IsSelected = true;
        }
    }
}
