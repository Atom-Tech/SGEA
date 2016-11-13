using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SGEA.Janelas
{
    /// <summary>
    /// Interaction logic for Backup.xaml
    /// </summary>
    public partial class Backup : Window
    {
        private string caminho, c;

        public Backup()
        {
            InitializeComponent();
            ContentRendered += Backup_ContentRendered;
        }

        private void Backup_ContentRendered(object sender, EventArgs e)
        {
            caminho = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SGEA\\Backup\\";
            local.Content = caminho;
            CarregarBackup();
        }

        public void CarregarBackup()
        {
            var l = Directory.GetDirectories(caminho);
            if (l.Length > 0)
            {
                SortedDictionary<DateTime, int> lista = new SortedDictionary<DateTime, int>();
                foreach (var x in l)
                {
                    var n = x.Replace(caminho, "").Replace('-', '/');
                    var b = Directory.GetFiles(x).Where(s =>
                        System.IO.Path.GetExtension(s) == ".db");
                    if (n.Substring(0,1) == "\\")
                        lista.Add(DateTime.Parse(
                            n.Substring(1)).Date, b.Count());
                    else
                        lista.Add(DateTime.Parse(n).Date, b.Count());
                }
                Dictionary<string, int> listaD = new Dictionary<string, int>();
                foreach (var x in lista)
                {
                    listaD.Add(x.Key.ToString().Substring(0,10), x.Value);
                }
                listaDias.ItemsSource = listaD;
                listaDias.Columns[0].Header = "Data";
                listaDias.Columns[0].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                listaDias.Columns[1].Header = "Backups";
            }
        }

        private void minimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void fechar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool _isMouseDown = false;
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

        private void listaDias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaDias.SelectedIndex != -1)
            {
                var data = (KeyValuePair<string,int>)listaDias.Items[listaDias.SelectedIndex];
                c = caminho + data.Key.Replace('/', '-');
                var l = Directory.GetFiles(c);
                List<string> lista = new List<string>();
                foreach (var x in l)
                {
                    var d = x.Replace(c + "\\", "").Replace(".db","");
                    lista.Add(d);
                }
                listaBackup.ItemsSource = lista;
                listaBackup.Columns[1].Visibility = Visibility.Collapsed;
            }
        }

        private void importar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listaBackup.SelectedIndex != -1)
                {
                    string data = listaBackup.Items[listaBackup.SelectedIndex].ToString();
                    c += "\\" + data + ".db";
                    File.Copy(c, Connect.Banco, true);
                    Xceed.Wpf.Toolkit.MessageBox.Show("Backup Importado!");
                    Close();
                }
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        private void mudarLocal_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var x = dialog.SelectedPath.Split('\\');
                if (x[x.Length - 1] == "Backup")
                {
                    caminho = dialog.SelectedPath + "\\";
                    local.Content = caminho;
                    CarregarBackup();
                }
                else
                {
                    DateTime h = new DateTime();
                    bool d = DateTime.TryParse(x[x.Length - 1], out h);
                    if (d)
                    {
                        caminho = "";
                        for (int i = 0; i < x.Length - 1; i++)
                        {
                            caminho += x[i] + "\\";
                        }
                        local.Content = caminho;
                        CarregarBackup();
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Selecione a pasta Backup");
                    }
                }
            }
        }
    }
}
