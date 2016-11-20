using System.ComponentModel;
using System.Linq;
using System;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Mvvm;
using Apresentacao10.Slides;

namespace Apresentacao10.Views
{
    public sealed partial class Shell : Page
    {
        private MainPage m;

        public static Shell Instance { get; set; }
        public static HamburgerMenu Lista => Instance.lista;
        Services.SettingsServices.SettingsService _settings;

        public Shell()
        {
            Instance = this;
            InitializeComponent();
            _settings = Services.SettingsServices.SettingsService.Instance;
        }

        public void Transfer(MainPage m)
        {
            this.m = m;
        }

        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }

        public void SetNavigationService(INavigationService navigationService)
        {
            lista.NavigationService = navigationService;
            Lista.RefreshStyles(_settings.AppTheme, true);
            Lista.IsFullScreen = _settings.IsFullScreen;
            Lista.HamburgerButtonVisibility = _settings.ShowHamburgerButton ? Visibility.Visible : Visibility.Collapsed;
        }

        private void intro_Selected(object sender, RoutedEventArgs e)
        {
            m.Navigate(typeof(Intro));
        }

        private void equipe_Selected(object sender, RoutedEventArgs e)
        {
            m.Navigate(typeof(Equipe));
        }
    }
}

