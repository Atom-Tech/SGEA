using MySql.Data.MySqlClient;
using SGEA;
using SGEA.Classes;
using SGEA.Janelas;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data.SQLite;

namespace SGEA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            ContentRendered += MainWindow_ContentRendered;
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            campoLogin.Focus();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Criptografar c = new Criptografar();
                string senha = c.EncryptToString(campoSenha.Password);
                dataGrid.SelectInformacoesUsuario(campoLogin.Text,senha);
                if (dataGrid.Items.Count > 0)
                {
                    DataRowView row = (DataRowView)dataGrid.Items[0];
                    string login = row[1].ToString();
                    int b = string.Compare(campoLogin.Text, login, false);
                    if (b == 0)
                    {
                        string id = row[4].ToString();
                        string email = row[3].ToString();
                        int cd = Convert.ToInt32(row[0]);
                        string nome = row[5].ToString();
                        string cep = row[6].ToString();
                        string bairro = row[7].ToString();
                        string rua = row[8].ToString();
                        string num = row[12].ToString();
                        string telFixo = row[9].ToString();
                        string telCel = row[10].ToString();
                        string sexo = row[11].ToString();
                        Hide();
                        Xceed.Wpf.Toolkit.MessageBox.Show("Bem Vindo, " + login + "!","Bem Vindo");
                        Main m = new Main(id, login, cd, email, nome, cep, bairro, rua, num, telFixo, telCel, sexo);
                        m.Closed += (s, args) => Close();
                        m.Show();
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Login e/ou Senha inválido(a)");
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Login e/ou Senha inválido(a)");
                }
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
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

        private void lbEsqueci_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Senha s = new Senha();
            s.ShowDialog();
        }
    }
}
