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
using Windows.System;

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

        private void mvv_Selected(object sender, RoutedEventArgs e)
        {
            m.Navigate(typeof(MVV));
        }

        private void cliente_Selected(object sender, RoutedEventArgs e)
        {
            m.Navigate(typeof(Cliente));
        }

        private void perfil_Selected(object sender, RoutedEventArgs e)
        {
            m.Navigate(typeof(Perfil));
        }

        private void problema_Selected(object sender, RoutedEventArgs e)
        {
            m.Navigate(typeof(Problema));
        }

        private void solucao_Selected(object sender, RoutedEventArgs e)
        {
            m.Navigate(typeof(Solucao));
        }

        private void site_Selected(object sender, RoutedEventArgs e)
        {
            m.Navigate(typeof(Site));
        }

        private void sgea_Selected(object sender, RoutedEventArgs e)
        {
            m.Navigate(typeof(Sgea));
        }

        private void call_Selected(object sender, RoutedEventArgs e)
        {
            m.Navigate(typeof(MVV));
        }

        private void conclusao_Selected(object sender, RoutedEventArgs e)
        {
            m.Navigate(typeof(Conclusao));
        }
    }
}

