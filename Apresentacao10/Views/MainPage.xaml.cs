using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Core;
using System.IO;
using Apresentacao10.Slides;
using Windows.UI.Xaml.Media.Animation;

namespace Apresentacao10.Views
{
    public sealed partial class MainPage : Page
    {
        Shell s;
        int i = 0;

        public MainPage()
        {
            InitializeComponent();
            s = Shell.Instance;
            s.Transfer(this);
            NavigationCacheMode = NavigationCacheMode.Enabled;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
            s.SetView(0);
        }

        public void Navigate(Type t)
        {
            Storyboard storyBoard = (Storyboard)Resources["Transicao"];
            storyBoard.Begin();
            frame.Navigate(t);
        }

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Escape:
                    CoreApplication.Exit();
                    break;
                case Windows.System.VirtualKey.Left:
                    if (i > 0)
                    {
                        i--;
                        s.SetView(i);
                    }
                    break;
                case Windows.System.VirtualKey.Right:
                    if (i < 10)
                    {
                        i++;
                        s.SetView(i);
                    }
                    break;
            }
        }
    }
}
