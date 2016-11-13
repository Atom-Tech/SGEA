using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfPageTransitions;

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

        public static UserControl TrocaSlide(int s)
        {
            WindowHelper.bringToFront("Apresentação");
            ImageBrush fundo = new ImageBrush();
            System.Windows.Controls.Image imagem =
                new System.Windows.Controls.Image();
            imagem.Source = new BitmapImage(new Uri(
                "pack://application:,,,/Apresentacao;component/brushedframe4.jpg"));
            fundo.ImageSource = imagem.Source;
            slideMestre.Background = fundo;
            slideMestre.Topmost = false;
            bool b = false;
            foreach (var x in block)
            {
                if (x) b = true;
            }
            if (!b)
            {
                SlideFrame.s = s;
                switch (s)
                {
                    case 1:
                        return new Slide1();
                    case 2:
                        return new Slide2();
                    case 3:
                        return new Slide3();
                    case 4:
                        return new Slide4();
                    case 5:
                        return new Slide5();
                    case 6:
                        return new Slide6();
                    case 7:
                        return new Slide7();
                    case 8:
                        return new Slide8();
                    case 9:
                        return new Slide10();
                    default:
                        throw new SlideException();
                }
            }
            throw new BlockException();
        }

        public static void SlideMobile(int s)
        {
            if (s == 14)
            {
                slideMestre.Background = System.Windows.Media.Brushes.Transparent;
                slideMestre.Topmost = true;
                slideMestre.slide.ShowPage(new Slide9(slideMestre));
                WindowHelper.bringToFront("Vysor");
            }
            else
            {
                WindowHelper.bringToFront("Apresentação");
                ImageBrush fundo = new ImageBrush();
                System.Windows.Controls.Image imagem =
                    new System.Windows.Controls.Image();
                imagem.Source = new BitmapImage(new Uri(
                    "pack://application:,,,/Apresentacao;component/brushedframe4.jpg"));
                fundo.ImageSource = imagem.Source;
                slideMestre.Background = fundo;
                slideMestre.Topmost = false;
                slideMestre.slide.ShowPage(new Slide11(slideMestre));
            }
        }
    }
}
