using MySql.Data.MySqlClient;
using SGEA.Classes;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System;
using System.Data.SQLite;

namespace SGEA.Janelas.Usuario
{
    /// <summary>
    /// Interaction logic for Alterar.xaml
    /// </summary>
    public partial class Alterar : Window
    {
        private int cd;
        private string login;
        private bool vFechar = false;

        public Alterar(string login, string email, string nome,
            string cep, string bairro, string rua, string fixo, string cel,
            string grupo, string sexo, int op, int cd)
        {
            InitializeComponent();
            this.login = login;
            campoLogin.Text = login;
            campoEmail.Text = email;
            campoNome.Text = nome;
            this.cd = cd;
            campoCep.Text = cep;
            campoBairro.Text = bairro;
            campoRua.Text = rua;
            if (sexo == "M")
                radioM.IsChecked = true;
            else if (sexo == "F")
                radioF.IsChecked = true;
            telFixo.Text = fixo;
            telCel.Text = cel;
        }

        public bool VerificarSenha(string senhaA)
        {
            bool ver = false;
            try
            {
                Criptografar c = new Criptografar();
                string senha = c.EncryptToString(senhaA);
                dataGrid.DataContext = Connect.LiteConnection("Select login, senha from tbUsuario where login = '" + login +
                    "' and senha = '" + senha + "'");
                if (dataGrid.Items.Count > 0)
                {
                    ver = true;
                }
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return ver;
        }

        public string NovoLogin()
        {
            return campoLogin.Text;
        }

        public bool Retornar()
        {
            return vFechar;
        }

        private void botaoAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (radioM.IsChecked == false && radioF.IsChecked == false)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Selecione um dos sexos");
            }
            else
            {
                string sexo = "";
                if (radioM.IsChecked == true)
                    sexo = "M";
                else if (radioF.IsChecked == true)
                    sexo = "F";
                if (campoEmail.Text.Length == 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Digite um e-mail");
                }
                else if (new EmailAddressAttribute().IsValid(campoEmail.Text))
                {
                    if (campoSenhaNova.Password == campoConfirmar.Password)
                    {
                        if (checkSenha.IsChecked == true)
                        {
                            if (Criptografar.segSenha(campoSenhaNova.Password))
                            {
                                bool ver = VerificarSenha(campoSenhaAntiga.Password);
                                ClasseUsuario u = new ClasseUsuario(cd);
                                u.AlterarUsuario(campoNome.Text, campoCep.Text, campoBairro.Text, campoRua.Text, telFixo.Text, telCel.Text, campoLogin.Text,
                                    login, campoEmail.Text, sexo, checkSenha.IsChecked, campoSenhaAntiga.Password, campoSenhaNova.Password, ver);
                                vFechar = true;
                                Close();
                            }
                            else
                            {
                                Xceed.Wpf.Toolkit.MessageBox.Show("Requer pelo menos um número, uma letra maíuscula, uma letra minuscula e no mínimo 8 caracteres");
                            }
                        }
                        else
                        {
                            bool ver = VerificarSenha(campoSenhaAntiga.Password);
                            ClasseUsuario u = new ClasseUsuario(cd);
                            u.AlterarUsuario(campoNome.Text, campoCep.Text, campoBairro.Text, campoRua.Text, telFixo.Text, telCel.Text, campoLogin.Text,
                                login, campoEmail.Text, sexo, checkSenha.IsChecked, campoSenhaAntiga.Password, campoSenhaNova.Password, ver);
                            vFechar = true;
                            Close();
                        }
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("As senhas não correspondem");
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("E-Mail inválido");
                }
            }
        }

        private void checkSenha_Checked(object sender, RoutedEventArgs e)
        {
            campoSenhaAntiga.IsEnabled = true;
            campoSenhaNova.IsEnabled = true;
            campoConfirmar.IsEnabled = true;
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

        private void checkSenha_Unchecked(object sender, RoutedEventArgs e)
        {
            campoSenhaAntiga.IsEnabled = false;
            campoSenhaNova.IsEnabled = false;
            campoConfirmar.IsEnabled = false;
        }

        private void campoCep_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string cep = campoCep.Text;
            if (campoCep.IsMaskFull && cep.IsCepValid())
            {
                campoCidade.Text = cep.getCidade();
                campoBairro.Text = cep.getBairro();
                campoRua.Text = cep.getRua();
            }
        }

        public void PegarCep()
        {
            campoCep.Text =
                campoCidade.Text.getCep(campoBairro.Text);
        }

        private void campoCidade_LostFocus(object sender, RoutedEventArgs e)
        {
            PegarCep();
        }

        private void campoBairro_LostFocus(object sender, RoutedEventArgs e)
        {
            PegarCep();
        }
    }
}
