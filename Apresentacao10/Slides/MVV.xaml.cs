using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Apresentacao10.Slides
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MVV : Page
    {
        Storyboard m, vi, va;

        public MVV()
        {
            this.InitializeComponent();
            m = (Storyboard)Resources["Missao"];
            vi = (Storyboard)Resources["Visao"];
            va = (Storyboard)Resources["Valores"];
        }

        private void textBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            m.Begin();
            vi.Stop();
            va.Stop();
        }

        private void textBlock2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            m.Stop();
            vi.Begin();
            va.Stop();
        }

        private void textBlock3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            m.Stop();
            vi.Stop();
            va.Begin();
        }
    }
}
