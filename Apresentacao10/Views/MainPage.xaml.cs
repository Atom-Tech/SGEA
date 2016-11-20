using System;
using Apresentacao10.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Core;
using System.IO;
using Apresentacao10.Slides;

namespace Apresentacao10.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Shell s = Shell.Instance;
            s.Transfer(this);
            NavigationCacheMode = NavigationCacheMode.Enabled;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
            //chrome.Source = new Uri
            //    ("ms-appx-web:///Assets/Site/index.php");
            Navigate(typeof(Intro));
        }

        public void Navigate(Type t)
        {
            frame.Navigate(t);
        }

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Escape:
                    CoreApplication.Exit();
                    break;
            }
        }
    }
}
