using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
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
using SGEA.Classes;

namespace SGEA.Pages
{
    /// <summary>
    /// Interaction logic for Cliente.xaml
    /// </summary>
    public partial class Cliente : Page
    {
        private int cdUsuario;
        private int op = 3, id;
        private string grupo;
        private SGEA.Main m;

        public Cliente(int cdUsuario, string grupo, SGEA.Main m)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.grupo = grupo;
            this.m = m;
            Loaded += Cliente_Loaded;
        }

        private void Cliente_Loaded(object sender, RoutedEventArgs e)
        {
            if (grupo == "Funcionário")
                botaoDeletar.Visibility = Visibility.Collapsed;
            AtivarCampos(false);
            Atualizar();
        }

        private void Atualizar()
        {
            try
            {
                listaCliente.DataContext = Connect.LiteConnection("select cdCliente 'Código', nmCliente 'Nome', sexo 'Sexo' , email 'Email', " +
                    " cep 'CEP', bairro 'Bairro', rua 'Rua', numero 'Nº', telFixo 'Telefone Fixo', telCel 'Celular' from tbCliente");
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        private void AtivarCampos(bool v)
        {
            campoNome.IsEnabled = v;
            campoCep.IsEnabled = v;
            campoCidade.IsEnabled = v;
            campoBairro.IsEnabled = v;
            campoRua.IsEnabled = v;
            campoNum.IsEnabled = v;
            campoEmail.IsEnabled = v;
            campoSexo.IsEnabled = v;
            telCel.IsEnabled = v;
            telFixo.IsEnabled = v;
            botaoSalvar.IsEnabled = v;
            if (op == 2)
            {
                checkSexo.Visibility = Visibility.Visible;
                botaoSalvar.Content = "Pesquisar";
            }
            else
            {
                checkSexo.Visibility = Visibility.Collapsed;
                botaoSalvar.Content = "Salvar";
            }
        }

        public void Notificacao(string cd)
        {
            int x = -1;
            Wait.Waiting();
            for (int i = 0; i < listaCliente.Items.Count; i++)
            {
                DataRowView row = (DataRowView)listaCliente.Items[i];
                if (row[0].ToString() == cd)
                {
                    x = i;
                    break;
                }
            }
            listaCliente.SelectedIndex = x;
        }

        private void botaoSalvar_Click(object sender, RoutedEventArgs e)
        {
            bool v = false;
            string sexo = "";
            if (campoSexo.Text == "Masculino")
                sexo = "M";
            else if (campoSexo.Text == "Feminino")
                sexo = "F";
            else if (campoSexo.Text == "Indefinido")
                sexo = "I";
            if ((new EmailAddressAttribute().IsValid(campoEmail.Text) || campoEmail.Text == "") && op != 2)
            {
                if (!telFixo.IsMaskFull)
                    telFixo.Text = "(00)0000-0000";
                if (!telCel.IsMaskFull)
                    telCel.Text = "(00)00000-0000";
                ClasseCliente p = new ClasseCliente(cdUsuario);
                if (campoNome.Text != "" && campoEmail.Text != "")
                {
                    if (op == 0)
                    {
                        v = p.CadastrarCliente(campoNome.Text, campoCep.Text, campoBairro.Text, campoRua.Text, campoNum.Text, campoEmail.Text,
                    sexo, telFixo.Text, telCel.Text);
                    }
                    else if (op == 1)
                    {
                        v = p.AlterarCliente(id, campoNome.Text, campoEmail.Text, sexo, campoCep.Text,
                            campoBairro.Text, campoRua.Text, campoNum.Text, telFixo.Text, telCel.Text);
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                    }
                    if (v)
                    {
                        Atualizar();
                        AtivarCampos(false);
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                }
            }
            else if (op == 2)
            {
                Dictionary<string, string> pesquisa = new Dictionary<string, string>();
                if (campoNome.Text != "")
                    pesquisa.Add("nmCliente", campoNome.Text);
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
                if (telFixo.IsMaskFull)
                    pesquisa.Add("telFixo", telFixo.Text);
                if (telCel.IsMaskFull)
                    pesquisa.Add("telCel", telFixo.Text);
                if (checkSexo.IsChecked == true)
                {
                    switch (campoSexo.Text)
                    {
                        case "Masculino":
                            pesquisa.Add("sexo", "M");
                            break;
                        case "Feminino":
                            pesquisa.Add("sexo", "F");
                            break;
                        case "Indefinido":
                            pesquisa.Add("sexo", "I");
                            break;
                    }
                }
                if (pesquisa.Count > 0 || campoCidade.Text != "")
                {
                    string cmdText = "select cdCliente 'Código', nmCliente 'Nome', sexo 'Sexo' , email 'Email', " +
                        " cep 'CEP', bairro 'Bairro', rua 'Rua', numero 'Nº', telFixo 'Telefone Fixo', telCel 'Celular' from tbCliente where ";
                    if (pesquisa.Count > 0)
                    {
                        foreach (var filtro in pesquisa)
                        {
                            if (filtro.Key == "nmCliente")
                                cmdText += filtro.Key + " like '" + filtro.Value + "%' and ";
                            else
                                cmdText += filtro.Key + " = '" + filtro.Value + "' and ";
                        }
                        if (campoCidade.Text == "")
                            cmdText = cmdText.Substring(0, cmdText.Length - 5);
                    }
                    if (campoCidade.Text != "")
                        cmdText = campoCidade.PesquisarCidade(cmdText);
                    listaCliente.DataContext = Connect.LiteConnection(cmdText);
                    AtivarCampos(false);
                }
            }
            else
                Xceed.Wpf.Toolkit.MessageBox.Show("E-Mail inválido");
        }

        private void botaoDeletar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listaCliente.SelectedIndex;
                DataRowView row = (DataRowView)listaCliente.Items[index];
                id = Convert.ToInt32(row[0]);
                Classes.ClasseCliente c = new Classes.ClasseCliente(cdUsuario);
                c.DeletarCliente(id);
                Atualizar();
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void botaoAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (listaCliente.SelectedIndex != -1)
            {
                op = 1;
                AtivarCampos(true);
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void listaCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AtivarCampos(false);
            if (listaCliente.Items.Count > 0)
            {
                int index = listaCliente.SelectedIndex;
                DataRowView row;
                try
                {
                    row = (DataRowView)listaCliente.Items[index];
                }
                catch
                {
                    row = (DataRowView)listaCliente.Items[0];
                }
                id = Convert.ToInt32(row[0]);
                campoNome.Text = row[1].ToString();
                string sexo = row[2].ToString(); ;
                if (sexo == "M")
                    campoSexo.Text = "Masculino";
                else if (sexo == "F")
                    campoSexo.Text = "Feminino";
                else if (sexo == "I")
                    campoSexo.Text = "Indefinido";
                campoEmail.Text = row[3].ToString();
                campoCep.Text = row[4].ToString();
                campoBairro.Text = row[5].ToString();
                campoRua.Text = row[6].ToString();
                campoNum.Text = row[7].ToString();
                telFixo.Text = row[8].ToString();
                telCel.Text = row[9].ToString();
            }
        }

        private void botaoPesquisar_Click(object sender, RoutedEventArgs e)
        {
            op = 2;
            AtivarCampos(true);
            campoNome.Text = "";
            campoCep.Text = "#####-###";
            campoCidade.Text = "";
            campoBairro.Text = "";
            campoRua.Text = "";
            campoNum.Text = "";
            campoEmail.Text = "";
            campoSexo.Text = "Masculino";
            telFixo.Text = "";
            telCel.Text = "";
        }

        private void botaoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            botaoNovo.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Atualizar();
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

        public void PegarCep()
        {
            if (op != 2)
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

        private void botaoNovo_Click(object sender, RoutedEventArgs e)
        {
            op = 0;
            AtivarCampos(true);
            campoNome.Text = "";
            campoCep.Text = "#####-###";
            campoCidade.Text = "";
            campoBairro.Text = "";
            campoRua.Text = "";
            campoEmail.Text = "";
            campoNum.Text = "";
            campoSexo.Text = "Masculino";
            telFixo.Text = "";
            telCel.Text = "";
        }
    }
}
