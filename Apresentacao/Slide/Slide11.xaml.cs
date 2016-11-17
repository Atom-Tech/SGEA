using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace Apresentacao.Slide
{
    /// <summary>
    /// Interaction logic for Slide11.xaml
    /// </summary>
    public partial class Slide11 : UserControl
    {
        Main slideMestre;
        string add;

        public Slide11(Main m)
        {
            InitializeComponent();
            slideMestre = m;
            add = (Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) +
                "\\AtomTech\\SGEA\\Site\\index.php");
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                horario.Content = DateTime.Now.ToString("HH:mm");
                data.Content = DateTime.Today.ToShortDateString();
            }, this.Dispatcher);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            chrome.Address = add;
        }

        private void botaoVoltar_Click(object sender, RoutedEventArgs e)
        {
            if (chrome.CanGoBack) chrome.BackCommand.Execute(null);
        }

        private void botaoAvancar_Click(object sender, RoutedEventArgs e)
        {
            if (chrome.CanGoForward) chrome.ForwardCommand.Execute(null);
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            //chrome.Source = link;
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5) chrome.ReloadCommand.Execute(null);
        }

        private void botaoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            if (refresh.IsVisible)
            {
                chrome.ReloadCommand.Execute(null);
            }
            else
            {
                chrome.StopCommand.Execute(null);
                refresh.Visibility = Visibility.Visible;
            }
        }

        private void url_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                    chrome.Address = add;
            }
            catch
            {

            }
        }

        private void botaoFechar_Click(object sender, RoutedEventArgs e)
        {
            slideMestre.slide.ShowPage(new Slide8());
        }

        private void botaoSGEA_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"C:\Program Files (x86)\AtomTech\SGEA\SGEA.exe");
        }

        private void botaoMobile_Click(object sender, RoutedEventArgs e)
        {
            SlideFrame.SlideMobile(14);
        }

        private void chrome_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (!e.IsLoading)
                {
                    if (chrome.CanGoBack)
                    {
                        leftAble.Visibility = Visibility.Visible;
                        botaoVoltar.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        leftAble.Visibility = Visibility.Collapsed;
                        botaoVoltar.Cursor = Cursors.Arrow;
                    }
                    if (chrome.CanGoForward)
                    {
                        rightAble.Visibility = Visibility.Visible;
                        botaoAvancar.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        rightAble.Visibility = Visibility.Collapsed;
                        botaoAvancar.Cursor = Cursors.Arrow;
                    }
                }
            }
            ));
        }

        private void chrome_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
                refresh.Visibility = Visibility.Visible));
        }

        private void chrome_FrameLoadStart(object sender, CefSharp.FrameLoadStartEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
                refresh.Visibility = Visibility.Collapsed));
        }
    }
}
