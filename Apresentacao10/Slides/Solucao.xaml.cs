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
    public sealed partial class Solucao : Page
    {
        Storyboard m, c;

        public Solucao()
        {
            this.InitializeComponent();
            m = (Storyboard)Resources["monitor"];
            c = (Storyboard)Resources["celular"];
        }

        private void monitor_Tapped(object sender, TappedRoutedEventArgs e)
        {
            m.Begin();
            c.Stop();
        }

        private void celular_Tapped(object sender, TappedRoutedEventArgs e)
        {
            m.Stop();
            c.Begin();
        }
    }
}
