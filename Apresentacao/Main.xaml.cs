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
        private int s = 0;

        public static HamburgerMenu.HamburgerMenu Barra
            { get; private set; }

        public Main()
        {
            InitializeComponent();
            Barra = barra;
            ContentRendered += Main_ContentRendered;
        }

        private void Main_ContentRendered(object sender, EventArgs e)
        {
            slide.Navigate(new Slide1());
            SlideFrame.getSlide(this);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                var fechar = MessageBox.Show("Deseja fechar a apresentação?", "Aviso", MessageBoxButton.YesNo);
                if (fechar == MessageBoxResult.Yes)
                    App.Current.Shutdown();
            }
            if (e.Key == Key.Back)
            {
                WindowState = WindowState.Minimized;
            }
            if (e.Key == Key.Left)
            {
                if (s > 0)
                {
                    s -= 1;
                    SlideFrame.TrocaSlide(s);
                }
            }
            if (e.Key == Key.Right)
            {
                if (s < 8)
                {
                    s += 1;
                   SlideFrame.TrocaSlide(s);
                }
            }
        }

        private void menuSite_Selected(object sender, RoutedEventArgs e)
        {
            s = 7;
            slide.Navigate(new Slide11());
        }

        private void intro_Selected(object sender, RoutedEventArgs e)
        {
            s = 0;
            slide.Navigate(new Slide1());
        }

        private void equipe_Selected(object sender, RoutedEventArgs e)
        {
            s = 1;
            slide.Navigate(new Slide2());
        }

        private void mvv_Selected(object sender, RoutedEventArgs e)
        {
            s = 2;
            slide.Navigate(new Slide3());
        }

        private void cliente_Selected(object sender, RoutedEventArgs e)
        {
            s = 3;
            slide.Navigate(new Slide4());
        }

        private void trabalha_Selected(object sender, RoutedEventArgs e)
        {
            s = 4;
            slide.Navigate(new Slide5());
        }

        private void problema_Selected(object sender, RoutedEventArgs e)
        {
            s = 5;
            slide.Navigate(new Slide6());
        }

        private void solucao_Selected(object sender, RoutedEventArgs e)
        {
            s = 6;
            slide.Navigate(new Slide7());
        }

        private void conclusao_Selected(object sender, RoutedEventArgs e)
        {
            s = 8;
            slide.Navigate(new Slide10());
        }
    }
}
