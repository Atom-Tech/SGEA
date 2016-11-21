using MySql.Data.MySqlClient;
using SGEA.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace SGEA.Pages
{
    /// <summary>
    /// Interaction logic for Usuario.xaml
    /// </summary>
    public partial class Usuario : Page
    {
        private string login, loginU;
        private int op = 3, cdUsuario;

        public Usuario(int cdUsuario, string login)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.login = login;
            Loaded += Usuario_Loaded;
        }

        private void Usuario_Loaded(object sender, RoutedEventArgs e)
        {
            Atualizar();
            AtivarCampos(false);
        }
        public void Atualizar()
        {
            try
            {
                listaFunc.DataContext = Connect.LiteConnection("select cdUsuario 'Código', login 'Login',"
                    + " nmUsuario 'Nome', sexo 'Sexo', cep 'CEP', bairro 'Bairro', rua 'Rua', numero 'Nº', email 'E-Mail', "
                    + " grupo 'Grupo', telFixo 'Telefone', telCel 'Celular' from tbUsuario "
                    + " where login != '" + login + "'");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        public void AtivarCampos(bool vf)
        {
            tipoUsuario.IsEnabled = vf;
            campoNome.IsEnabled = vf;
            campoEmail.IsEnabled = vf;
            campoCep.IsEnabled = vf;
            campoCidade.IsEnabled = vf;
            campoBairro.IsEnabled = vf;
            campoRua.IsEnabled = vf;
            campoNum.IsEnabled = vf;
            radioF.IsEnabled = vf;
            radioM.IsEnabled = vf;
            campoLogin.IsEnabled = vf;
            checkSenha.IsEnabled = vf;
            telCel.IsEnabled = vf;
            telFixo.IsEnabled = vf;
            campoSenhaAntiga.Password = "";
            campoSenhaNova.Password = "";
            campoConfirmar.Password = "";
            botaoSalvar.IsEnabled = vf;
            grupoSenha.Visibility = Visibility.Collapsed;
            if (op == 2)
            {
                checkUsuario.Visibility = Visibility.Visible;
                checkRadio.Visibility = Visibility.Visible;
                botaoSalvar.Content = "Pesquisar";
            }
            else
            {
                checkUsuario.Visibility = Visibility.Collapsed;
                checkRadio.Visibility = Visibility.Collapsed;
                botaoSalvar.Content = "Salvar";
            }
            campoSenhaAntiga.IsEnabled = false;
            campoSenhaNova.IsEnabled = false;
            campoConfirmar.IsEnabled = false;
        }

        private void botaoSalvar_Click(object sender, RoutedEventArgs e)
        {
            bool v = false;
            if (op != 2)
            {
                if (radioM.IsChecked == false && radioF.IsChecked == false)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Selecione um dos sexos");
                }
                else
                {
                    string fixo, cel;
                    if (telFixo.IsMaskFull) fixo = telFixo.Text;
                    else fixo = "(00)0000-0000";
                    if (telCel.IsMaskFull) cel = telCel.Text;
                    else cel = "(00)00000-0000";
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
                        dataGrid1.DataContext = Connect.LiteConnection("select cdUsuario from tbUsuario order by cdUsuario desc");
                        DataRowView row = (DataRowView)dataGrid1.Items[0];
                        int codigo = Convert.ToInt32(row[0]) + 1;
                        Criptografar c = new Criptografar();
                        string loginG = campoLogin.Text.First().ToString().ToUpper() + campoLogin.Text.Substring(1); ;
                        string senha = c.EncryptToString(loginG + codigo);
                        ClasseUsuario u = new ClasseUsuario(cdUsuario);
                        if (op == 0)
                        {
                            v = u.CadastrarUsuario(campoNome.Text, campoCep.Text, campoBairro.Text, campoRua.Text, campoNum.Text, campoEmail.Text, sexo, fixo,
                                cel, campoLogin.Text, senha, tipoUsuario.Text);
                            if (v)
                            {
                                Atualizar();
                                AtivarCampos(false);
                                Xceed.Wpf.Toolkit.MessageBox.Show("A senha gerada é: " + c.DecryptString(senha));
                            }
                        }
                        else if (op == 1)
                        {
                            if (campoSenhaNova.Password == campoConfirmar.Password)
                            {
                                if (Criptografar.segSenha(campoSenhaNova.Password, checkSenha.IsChecked))
                                {
                                    bool ver = VerificarSenha(campoSenhaAntiga.Password);
                                    v = u.AlterarUsuario(campoNome.Text, campoCep.Text, campoBairro.Text, campoRua.Text, campoNum.Text, fixo, cel, campoLogin.Text,
                                        loginU, campoEmail.Text, sexo, checkSenha.IsChecked, campoSenhaAntiga.Password, campoSenhaNova.Password, tipoUsuario.Text, ver);
                                    if (v)
                                    {
                                        Atualizar();
                                        AtivarCampos(false);
                                    }
                                }
                                else
                                {
                                    Xceed.Wpf.Toolkit.MessageBox.Show("Requer pelo menos um número, uma letra maíuscula, uma letra minuscula e no mínimo 8 caracteres");
                                }
                            }
                            else
                            {
                                Xceed.Wpf.Toolkit.MessageBox.Show("Senhas não correspondem");
                            }
                        }
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("E-Mail inválido");
                    }
                }
            }
            else
            {
                Dictionary<string, string> pesquisa = new Dictionary<string, string>();
                if (checkUsuario.IsChecked == true)
                    pesquisa.Add("grupo", tipoUsuario.Text);
                if (campoNome.Text != "")
                    pesquisa.Add("nmUsuario", campoNome.Text);
                if (campoEmail.Text != "")
                    pesquisa.Add("email", campoEmail.Text);
                if (campoCep.IsMaskFull)
                    pesquisa.Add("cep", campoCep.Text);
                if (campoBairro.Text != "")
                    pesquisa.Add("bairro", campoBairro.Text);
                if (campoRua.Text != "")
                    pesquisa.Add("rua", campoRua.Text);
                if (campoNum.Text != "")
                    pesquisa.Add("numero", campoNum.Text);
                if (checkRadio.IsChecked == true)
                {
                    if (radioM.IsChecked == true)
                        pesquisa.Add("sexo", "M");
                    if (radioF.IsChecked == true)
                        pesquisa.Add("sexo", "F");
                }
                if (campoLogin.Text != "")
                    pesquisa.Add("login", campoLogin.Text);
                if (telFixo.IsMaskFull)
                    pesquisa.Add("telFixo", telFixo.Text);
                if (telCel.IsMaskFull)
                    pesquisa.Add("telCel", telCel.Text);
                if (pesquisa.Count > 0 || campoCidade.Text != "")
                {
                    string cmdText = "select cdUsuario 'Código', login 'Login',"
                        + " nmUsuario 'Nome', sexo 'Sexo', cep 'CEP', bairro 'Bairro', rua 'Rua', numero 'Nº', email 'E-Mail', "
                        + " grupo 'Grupo', telFixo 'Telefone', telCel 'Celular' from tbUsuario "
                        + " where login != '" + login + "' and ";
                    if (pesquisa.Count > 0)
                    {
                        foreach (var filtro in pesquisa)
                        {
                            if (filtro.Key != "nmUsuario" && filtro.Key != "login")
                                cmdText += filtro.Key + " = '" + filtro.Value + "' and ";
                            else
                                cmdText += filtro.Key + " like '" + filtro.Value + "%' and ";
                        }
                        if (campoCidade.Text == "")
                            cmdText = cmdText.Substring(0, cmdText.Length - 5);
                    }
                    if (campoCidade.Text != "")
                        cmdText = campoCidade.PesquisarCidade(cmdText);
                    listaFunc.DataContext = Connect.LiteConnection(cmdText);
                    AtivarCampos(false);
                }
            }
        }

        public bool VerificarSenha(string senhaA)
        {
            try
            {
                Criptografar c = new Criptografar();
                DataRowView row = listaFunc.SelectedRow();
                string login = row[1].ToString();
                string senha = c.EncryptToString(senhaA);
                dataGrid.DataContext = Connect.LiteConnection("Select login, senha from tbUsuario where login = '" + login +
                    "' and senha = '" + senha + "'");
                if (dataGrid.Items.Count > 0) return true;
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        private void botaoNovo_Click(object sender, RoutedEventArgs e)
        {
            op = 0;
            AtivarCampos(true);
            tipoUsuario.Text = "Funcionário";
            campoNome.Text = "";
            campoEmail.Text = "";
            campoCep.Text = "#####-###";
            campoCidade.Text = "";
            campoBairro.Text = "";
            campoRua.Text = "";
            campoNum.Text = "";
            radioM.IsChecked = true;
            campoLogin.Text = "";
            checkSenha.IsChecked = false;
            checkSenha.IsEnabled = false;
            campoSenhaAntiga.IsEnabled = false;
            campoSenhaNova.IsEnabled = false;
            campoConfirmar.IsEnabled = false;
            telCel.Text = "";
            telFixo.Text = "";
        }

        private void listaFunc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AtivarCampos(false);
            if (listaFunc.Items.Count > 0)
            {
                int index = listaFunc.SelectedIndex;
                DataRowView row;
                try
                {
                    row = (DataRowView)listaFunc.Items[index];
                }
                catch
                {
                    row = (DataRowView)listaFunc.Items[0];
                }
                tipoUsuario.Text = row[9].ToString();
                campoNome.Text = row[2].ToString();
                campoEmail.Text = row[8].ToString();
                campoCep.Text = row[4].ToString();
                campoBairro.Text = row[5].ToString();
                campoRua.Text = row[6].ToString();
                campoNum.Text = row[7].ToString();
                if (row[3].ToString() == "M")
                    radioM.IsChecked = true;
                else
                    radioF.IsChecked = true;
                campoLogin.Text = row[1].ToString();
                loginU = row[1].ToString();
                checkSenha.IsChecked = false;
                telCel.Text = row[11].ToString();
                telFixo.Text = row[10].ToString();
            }
        }

        private void botaoAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (listaFunc.SelectedIndex != -1)
            {
                op = 1;
                AtivarCampos(true);
                grupoSenha.Visibility = Visibility.Visible;
            }
        }

        private void botaoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            Atualizar();
        }

        private void botaoPesquisar_Click(object sender, RoutedEventArgs e)
        {
            op = 2;
            AtivarCampos(true);
        }

        private void checkSenha_Unchecked(object sender, RoutedEventArgs e)
        {
            campoSenhaAntiga.Password = "";
            campoSenhaNova.Password = "";
            campoConfirmar.Password = "";
            campoSenhaAntiga.IsEnabled = false;
            campoSenhaNova.IsEnabled = false;
            campoConfirmar.IsEnabled = false;
        }

        public void PegarCep()
        {
            if (op != 2)
                campoCep.Text =
                    campoCidade.Text.getCep(campoBairro.Text);
        }


        private void campoCep_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (op != 2)
            {
                string cep = campoCep.Text;
                if (campoCep.IsMaskFull && cep.IsCepValid())
                {
                    campoCidade.Text = cep.getCidade();
                    campoBairro.Text = cep.getBairro();
                    campoRua.Text = cep.getRua();
                }
            }
        }

        private void campoBairro_LostFocus(object sender, RoutedEventArgs e)
        {
            PegarCep();
        }

        private void campoCidade_LostFocus(object sender, RoutedEventArgs e)
        {
            PegarCep();
        }

        private void botaoDeletar_Click(object sender, RoutedEventArgs e)
        {
            if (listaFunc.SelectedIndex != -1)
            {
                int index = listaFunc.SelectedIndex;
                DataRowView row = (DataRowView)listaFunc.Items[index];
                int l = Convert.ToInt32(row[0]);
                ClasseUsuario u = new ClasseUsuario(cdUsuario);
                u.DeletarUsuario(l);
                Atualizar();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void checkSenha_Checked(object sender, RoutedEventArgs e)
        {
            campoSenhaAntiga.IsEnabled = true;
            campoSenhaNova.IsEnabled = true;
            campoConfirmar.IsEnabled = true;
        }
    }
}
