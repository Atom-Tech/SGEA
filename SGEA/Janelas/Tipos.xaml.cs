using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SQLite;

namespace SGEA.Janelas
{
    /// <summary>
    /// Interaction logic for Tipos.xaml
    /// </summary>
    public partial class Tipos : Window
    {
        private int cdUsuario;
        private string grupo;

        public Tipos(int cdUsuario, string grupo)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.grupo = grupo;
            ContentRendered += Tipos_ContentRendered;
        }

        private void Tipos_ContentRendered(object sender, EventArgs e)
        {
            if (grupo == "Funcionário")
                botaoDel.Visibility = Visibility.Collapsed;
            Atualizar();
        }

        public void Atualizar()
        {
            listaTipo.DataContext = Connect.LiteConnection("select * from vwTipo");
        }

        private void botaoAdd_Click(object sender, RoutedEventArgs e)
        {
            if (campoNome.Text != "Serviço")
            {
                Tipo t = new Tipo(cdUsuario);
                //Método Inserir Tipo
                t.InserirTipo(campoNome.Text);
                Atualizar();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Tipo inválido");
            }
        }

        private void botaoDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listaTipo.SelectedIndex;
                DataRowView row = (DataRowView)listaTipo.Items[index];
                int c1 = Convert.ToInt32(row[0]);
                Tipo t = new Tipo(cdUsuario);
                //Método Deletar Tipo
                t.DeletarTipo(c1);
                Atualizar();
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void listaTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int index = listaTipo.SelectedIndex;
                DataRowView row = (DataRowView)listaTipo.Items[index];
                int c1 = Convert.ToInt32(row[0]);
                if (c1 == 1)
                {
                    botaoDel.IsEnabled = false;
                }
                else
                {
                    botaoDel.IsEnabled = true;
                }
            }
            catch
            {

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
    }
}
