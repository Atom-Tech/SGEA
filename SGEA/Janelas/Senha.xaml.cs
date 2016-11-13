using MySql.Data.MySqlClient;
using SGEA.Classes;
using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using System.Data.SQLite;

namespace SGEA.Janelas
{
    /// <summary>
    /// Interaction logic for Senha.xaml
    /// </summary>
    public partial class Senha : Window
    {
        private string login;
        private string nSenha;
        public Senha()
        {
            InitializeComponent();
            campoLogin.Focus();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Criptografar cript = new Criptografar();
            if (Email.EmailValido(campoEmail.Text))
            {
                bool f = Atualizar();
                if (f)
                {
                    try
                    {
                        Criptografar c = new Criptografar();
                        Email.EnviarEmail(campoEmail.Text, login, "Você solicitou recuperação de senha do software SGEA <br />" +
                    "Login: " + login + "<br />Senha: " + c.DecryptString(nSenha) + "<br />Se você não solicitou a recuperação " +
                    "de senha, ignore essa mensagem", "Recuperação de senha");
                        Close();
                        Xceed.Wpf.Toolkit.MessageBox.Show("Um e-mail foi enviado para você com a sua senha");
                    }
                    catch (SmtpException)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Não tem internet");
                    }
                    catch (FormatException)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Login e/ou Email incorreto");
                    }
                }
            }
        }

        public bool Atualizar()
        {
            dataGrid.DataContext = Connect.LiteConnection("select login, senha from tbUsuario where login = '" + campoLogin.Text + "' and email = '" + campoEmail.Text + "'");
            if (dataGrid.Items.Count > 0)
            {
                DataRowView row = (DataRowView)dataGrid.Items[0];
                login = row[0].ToString();
                nSenha = row[1].ToString();
                return true;
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Login e/ou Email inválido");
                return false;
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
    }
}
