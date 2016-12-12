using RegawMOD.Android;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SGEA.Janelas
{
    /// <summary>
    /// Interaction logic for Mobile.xaml
    /// </summary>
    public partial class Mobile : Window
    {
        int img = 0;

        public Mobile()
        {
            InitializeComponent();
            ContentRendered += Mobile_ContentRendered;
        }

        private void Mobile_ContentRendered(object sender, EventArgs e)
        {
            radio1.IsChecked = true;
        }

        private void minimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void fechar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool _isMouseDown;
        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown && this.WindowState == WindowState.Maximized)
            {
                _isMouseDown = false;
                this.WindowState = WindowState.Normal;
            }
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            this.DragMove();
        }

        private void TitleBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        public void MudarImagem(int img)
        {
            Storyboard s = (Storyboard)Resources["fadeOut"];
            s.Begin();
            this.img = img;
            switch (img)
            {
                case 0:
                    galeria.Source = new BitmapImage(new Uri(@"/SGEA;component/img/Seguranca.png", UriKind.Relative));
                    break;
                case 1:
                    galeria.Source = new BitmapImage(new Uri(@"/SGEA;component/img/FontesDesconhecidas.png", UriKind.Relative));
                    break;
                case 2:
                    galeria.Source = new BitmapImage(new Uri(@"/SGEA;component/img/Sobre.png", UriKind.Relative));
                    break;
                case 3:
                    galeria.Source = new BitmapImage(new Uri(@"/SGEA;component/img/Programador.png", UriKind.Relative));
                    break;
                case 4:
                    galeria.Source = new BitmapImage(new Uri(@"/SGEA;component/img/Depuracao.png", UriKind.Relative));
                    break;
            }
            s = (Storyboard)Resources["fadeIn"];
            s.Begin();
        }

        private void radio1_Checked(object sender, RoutedEventArgs e)
        {
            MudarImagem(0);
        }

        private void radio2_Checked(object sender, RoutedEventArgs e)
        {
            MudarImagem(1);
        }

        private void radio3_Checked(object sender, RoutedEventArgs e)
        {
            MudarImagem(2);
        }

        private void radio4_Checked(object sender, RoutedEventArgs e)
        {
            MudarImagem(3);
        }

        private void radio5_Checked(object sender, RoutedEventArgs e)
        {
            MudarImagem(4);
        }

        private void voltar_Click(object sender, RoutedEventArgs e)
        {
            switch (img)
            {
                case 0:
                    radio5.IsChecked = true;
                    break;
                case 1:
                    radio1.IsChecked = true;
                    break;
                case 2:
                    radio2.IsChecked = true;
                    break;
                case 3:
                    radio3.IsChecked = true;
                    break;
                case 4:
                    radio4.IsChecked = true;
                    break;
            }
        }

        private void avancar_Click(object sender, RoutedEventArgs e)
        {
            switch (img)
            {
                case 0:
                    radio2.IsChecked = true;
                    break;
                case 1:
                    radio3.IsChecked = true;
                    break;
                case 2:
                    radio4.IsChecked = true;
                    break;
                case 3:
                    radio5.IsChecked = true;
                    break;
                case 4:
                    radio1.IsChecked = true;
                    break;
            }
        }

        private void apk_Click(object sender, RoutedEventArgs e)
        {
            AndroidController a = AndroidController.Instance;
            string caminho = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SGEA\\SGEA.SGEA.apk";
            if (a.HasConnectedDevices)
            {
                caminho = "\"" + caminho + "\"";
                Device d = a.GetConnectedDevice(a.ConnectedDevices[0]);
                for (int i = 0; i < a.ConnectedDevices.Count; i++)
                {
                    d = a.GetConnectedDevice(a.ConnectedDevices[i]);
                    if (!d.SerialNumber.Contains("emulator")) break;
                }
                var ad = Adb.FormAdbShellCommand(d, false, "pm list packages SGEA.SGEA");
                string x = Adb.ExecuteAdbCommand(ad);
                if (x != "package:SGEA.SGEA")
                {
                    var ade = Adb.FormAdbCommand(d, "install "+ caminho);
                    Adb.ExecuteAdbCommand(ade);
                    Xceed.Wpf.Toolkit.MessageBox.Show("O aplicativo foi instalado com sucesso");
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("O aplicativo já está instalado");
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não há um celular Android conectado por USB");
            }
        }

        private void bd_Click(object sender, RoutedEventArgs e)
        {
            AndroidController a = AndroidController.Instance;
            string c = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SGEA\\sgea.db";
            string imagens = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SGEA\\Imagens";
            if (File.Exists(c))
            {
                if (a.HasConnectedDevices)
                {
                    c = "\"" + c + "\"";
                    imagens = "\"" + imagens + "\"";
                    Device d = a.GetConnectedDevice(a.ConnectedDevices[0]);
                    for (int i = 0; i < a.ConnectedDevices.Count; i++)
                    {
                        d = a.GetConnectedDevice(a.ConnectedDevices[i]);
                        if (!d.SerialNumber.Contains("emulator")) break;
                    }
                    var ad = Adb.FormAdbShellCommand(d, false, "rmdir -r sdcard/SGEA");
                    Adb.ExecuteAdbCommand(ad);
                    ad = Adb.FormAdbShellCommand(d, false, "mkdir sdcard/SGEA");
                    Adb.ExecuteAdbCommand(ad);
                    ad = Adb.FormAdbShellCommand(d, false, "mkdir sdcard/SGEA/Imagens");
                    Adb.ExecuteAdbCommand(ad);
                    bool b = d.PushFile(c, "sdcard/SGEA", 100000);
                    bool f = d.PushFile(imagens, "sdcard/SGEA/Imagens/", 100000);
                    if (b && f)
                        Xceed.Wpf.Toolkit.MessageBox.Show("Banco de Dados foi transferido com sucesso");
                    else
                        Xceed.Wpf.Toolkit.MessageBox.Show("Banco de Dados não foi transferido");
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Não há um celular Android conectado por USB");
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("O banco de dados não existe. \nPor favor, entre em contato conosco");
            }
        }
    }
}
