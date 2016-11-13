using Apresentacao.Slide;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Apresentacao
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Main : Window
    {
        private int s = 1;
        private bool x = true;

        public Main()
        {
            InitializeComponent();
            ContentRendered += Main_ContentRendered;
        }

        private void Main_ContentRendered(object sender, EventArgs e)
        {
            slide.ShowPage(new Slide1());
            SlideFrame.getSlide(this);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (x)
            {
                if (e.Key == Key.Escape)
                {
                    var fechar = MessageBox.Show("Deseja fechar a apresentação?","Aviso",MessageBoxButton.YesNo);
                    if (fechar == MessageBoxResult.Yes)
                        App.Current.Shutdown();
                }
                if (e.Key == Key.Back)
                {
                    WindowState = WindowState.Minimized;
                }
                if (e.Key == Key.Left || e.Key == Key.Up)
                {
                    x = false;
                    try
                    {
                        if (s > 1)
                        {
                            s -= 1;
                            slide.ShowPage(SlideFrame.TrocaSlide(s));
                        }
                    }
                    catch (SlideException)
                    {
                        s += 1;
                    }
                    catch (BlockException)
                    {
                        s += 1;
                    }
                    x = true;
                }
                if (e.Key == Key.Right || e.Key == Key.Down)
                {
                    x = false;
                    try
                    {
                        s += 1;
                        slide.ShowPage(SlideFrame.TrocaSlide(s));
                    }
                    catch (SlideException)
                    {
                        s -= 1;
                    }
                    catch (BlockException)
                    {
                        s -= 1;
                    }
                    x = true;
                }
                BlockItem();
            }
        }


        private void s1_Click(object sender, RoutedEventArgs e)
        {
            slide.ShowPage(new Slide1());
            s = 1;
            BlockItem();
        }

        private void s2_Click(object sender, RoutedEventArgs e)
        {
            slide.ShowPage(new Slide2());
            s = 2;
            BlockItem();
        }

        private void s3_Click(object sender, RoutedEventArgs e)
        {
            slide.ShowPage(new Slide3());
            s = 3;
            BlockItem();
        }

        private void s4_Click(object sender, RoutedEventArgs e)
        {
            slide.ShowPage(new Slide4());
            s = 4;
            BlockItem();
        }

        private void s5_Click(object sender, RoutedEventArgs e)
        {
            slide.ShowPage(new Slide5());
            s = 5;
            BlockItem();
        }

        private void s6_Click(object sender, RoutedEventArgs e)
        {
            slide.ShowPage(new Slide6());
            s = 6;
            BlockItem();
        }

        private void s7_Click(object sender, RoutedEventArgs e)
        {
            slide.ShowPage(new Slide7());
            s = 7;
            BlockItem();
        }

        private void s8_Click(object sender, RoutedEventArgs e)
        {
            slide.ShowPage(new Slide8());
            s = 8;
            BlockItem();
        }

        private void s9_Click(object sender, RoutedEventArgs e)
        {
            slide.ShowPage(new Slide10());
            s = 9;
            BlockItem();
        }
        
        public void BlockItem()
        {
            ImageBrush fundo = new ImageBrush();
            Image imagem = new Image();
            imagem.Source = new BitmapImage(new Uri(
                "pack://application:,,,/Apresentacao;component/brushedframe4.jpg"));
            fundo.ImageSource = imagem.Source;
            Background = fundo;
            Topmost = false;
            foreach (MenuItem m in menu.Items.OfType<MenuItem>())
            {
                m.IsEnabled = true;
            }        
            switch (s)
            {
                case 1:
                    s1.IsEnabled = false;
                    break;
                case 2:
                    s2.IsEnabled = false;
                    break;
                case 3:
                    s3.IsEnabled = false;
                    break;
                case 4:
                    s4.IsEnabled = false;
                    break;
                case 5:
                    s5.IsEnabled = false;
                    break;
                case 6:
                    s6.IsEnabled = false;
                    break;
                case 7:
                    s7.IsEnabled = false;
                    break;
                case 8:
                    s8.IsEnabled = false;
                    break;
                case 9:
                    s9.IsEnabled = false;
                    break;
            }
        }

        private void s14_Click(object sender, RoutedEventArgs e)
        {
            slide.ShowPage(new Slide10());
            s = 10;
            BlockItem();
        }
    }
}
