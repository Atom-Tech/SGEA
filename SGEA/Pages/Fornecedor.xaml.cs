using SGEA.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SGEA.Pages
{
    /// <summary>
    /// Interaction logic for Fornecedor.xaml
    /// </summary>
    public partial class Fornecedor : Page
    {
        private bool cnpjCorreto = false;
        private int cdUsuario;
        private int op = 0, id;
        private string grupo;
    
        public Fornecedor(int cdUsuario, string grupo)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.grupo = grupo;
            Loaded += Fornecedor_Loaded;
        }

        private void Fornecedor_Loaded(object sender, RoutedEventArgs e)
        {
            if (grupo == "Funcionário")
                botaoDeletar.Visibility = Visibility.Collapsed;
            Atualizar();
            AlimentarPerfis();
            AtivarCampos(false);
        }

        private void AlimentarPerfis()
        {
            try
            {
                campoPerfil.ItemFill("select cdPerfil, nmPerfil from tbPerfil");
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
            campoCNPJ.IsEnabled = v;
            campoRS.IsEnabled = v;
            telFixo.IsEnabled = v;
            telCel.IsEnabled = v;
            botaoSalvar.IsEnabled = v;
            botaoAdd.IsEnabled = false;
            botaoDel.IsEnabled = false;
            if (op == 2) botaoSalvar.Content = "Pesquisar";
            else botaoSalvar.Content = "Salvar";
        }

        private void Atualizar()
        {
            try
            {
                listaForn.DataContext = Connect.LiteConnection("select cdFornecedor 'Código', cnpj 'CNPJ', nmFornecedor 'Nome Fantasia', razaoSocial 'Razão Social', " +
        " email 'Email', cep 'CEP', bairro 'Bairro', rua 'Rua', numero 'Nº', telFixo 'Telefone Fixo', telCel 'Celular' from tbFornecedor");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        private void listaForn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AtivarCampos(false);
            if (listaForn.Items.Count > 0)
            {
                campoPerfil.IsEnabled = true;
                botaoAdd.IsEnabled = true;
                botaoDel.IsEnabled = true;
                int index = listaForn.SelectedIndex;
                DataRowView row;
                try
                {
                    row = (DataRowView)listaForn.Items[index];
                }
                catch
                {
                    row = (DataRowView)listaForn.Items[0];
                }
                id = Convert.ToInt32(row[0]);
                campoNome.Text = row[2].ToString();
                campoCep.Text = row[5].ToString();
                campoBairro.Text = row[6].ToString();
                campoRua.Text = row[7].ToString();
                campoNum.Text = row[8].ToString();
                campoEmail.Text = row[4].ToString();
                campoCNPJ.Text = row[1].ToString();
                campoRS.Text = row[3].ToString();
                telCel.Text = row[10].ToString();
                telFixo.Text = row[9].ToString();
                AtualizarPerfis();
            }
        }

        private void botaoAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (listaForn.SelectedIndex != -1)
            {
                if (campoPerfil.Items.Count > 0)
                    campoPerfil.SelectedIndex = 0;
                op = 1;
                AtivarCampos(true);
                botaoAdd.IsEnabled = true;
                botaoDel.IsEnabled = true;
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void botaoDeletar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listaForn.SelectedIndex;
                DataRowView row = (DataRowView)listaForn.Items[index];
                int cd = Convert.ToInt32(row[0]);
                ClasseFornecedor f = new ClasseFornecedor(cdUsuario);
                f.DeletarFornecedor(cd);
                Atualizar();
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void botaoSalvar_Click(object sender, RoutedEventArgs e)
        {
            bool v = false;
            if (campoEmail.Text.Length == 0 && op != 2)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Digite um e-mail");
            }
            else if (new EmailAddressAttribute().IsValid(campoEmail.Text) && op != 2)
            {
                if (cnpjCorreto)
                {
                    if (!telFixo.IsMaskFull) telFixo.Text = "(00)0000-0000";
                    if (!telCel.IsMaskFull) telCel.Text = "(00)00000-0000";
                    string cnpj = campoCNPJ.Text;
                    ClasseFornecedor p = new ClasseFornecedor(cdUsuario);
                    if (op == 0)
                    {
                        v = p.CadastrarFornecedor(campoNome.Text, campoCep.Text, campoBairro.Text, campoRua.Text, campoNum.Text, campoEmail.Text,
                            telFixo.Text, telCel.Text, cnpj, campoRS.Text);
                    }
                    else if (op == 1)
                    {
                        v = p.AlterarFornecedor(id, cnpj, campoNome.Text, campoRS.Text, campoEmail.Text,
                            campoCep.Text, campoBairro.Text, campoRua.Text, campoNum.Text, telFixo.Text, telCel.Text);
                    }
                    if (v)
                    {
                        Atualizar();
                        AtivarCampos(false);
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("CNPJ Inválido");
                }
            }
            else if (op == 2)
            {
                Dictionary<string, string> pesquisa = new Dictionary<string, string>();
                if (campoNome.Text != "")
                    pesquisa.Add("nmFornecedor", campoNome.Text);
                if (campoCNPJ.IsMaskFull)
                    pesquisa.Add("cnpj", campoCNPJ.Text);
                if (campoEmail.Text != "")
                    pesquisa.Add("email", campoEmail.Text);
                if (campoBairro.Text != "")
                    pesquisa.Add("bairro", campoBairro.Text);
                if (campoRua.Text != "")
                    pesquisa.Add("rua", campoRua.Text);
                if (campoNum.Text != "")
                    pesquisa.Add("numero", campoNum.Text);
                if (campoRS.Text != "")
                    pesquisa.Add("razaoSocial", campoRS.Text);
                if (telFixo.IsMaskFull)
                    pesquisa.Add("telFixo", telFixo.Text);
                if (telCel.IsMaskFull)
                    pesquisa.Add("telCel", telCel.Text);
                if (pesquisa.Count > 0 || campoCidade.Text != "")
                {
                    string cmdText = "select cdFornecedor 'Código', cnpj 'CNPJ', nmFornecedor 'Nome Fantasia', razaoSocial 'Razão Social', " +
        " email 'Email', cep 'CEP', bairro 'Bairro', rua 'Rua', numero 'Nº', telFixo 'Telefone Fixo', telCel 'Celular' from tbFornecedor where ";
                    if (pesquisa.Count > 0)
                    {
                        foreach (var filtro in pesquisa)
                        {
                            if (filtro.Key != "nmFornecedor")
                                cmdText += filtro.Key + " = '" + filtro.Value + "' and ";
                            else
                                cmdText += filtro.Key + " like '" + filtro.Value + "%' and ";
                        }
                        if (campoCidade.Text == "")
                            cmdText = cmdText.Substring(0, cmdText.Length - 5);
                    }
                    if (campoCidade.Text != "")
                        cmdText = campoCidade.PesquisarCidade(cmdText);
                    listaForn.DataContext = Connect.LiteConnection(cmdText);
                    AtivarCampos(false);
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("E-Mail inválido");
            }
        }

        private void botaoAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (campoPerfil.Items.Count > 0)
                {
                    if (campoPerfil.SelectedIndex != -1)
                    {
                        int cdPerfil = Convert.ToInt32(campoPerfil.SelectedValue);
                        using (var mConn = Connect.LiteString())
                        {
                            mConn.Open();
                            if (mConn.State == ConnectionState.Open)
                            {
                                string cmdText = "insert into tbPerfilForn values (null,@cd,@id)";
                                SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                                cmd.Parameters.AddWithValue("@cd", cdPerfil);
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                                AtualizarPerfis();
                            }
                        }
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Não há perfil selecionado");
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Não há perfil cadastrado");
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        public void AtualizarPerfis()
        {
            try
            {
                tabelaPerfil.DataContext = Connect.LiteConnection("select cdPerfil 'Código', cdPF, nmPerfil 'Perfil', dsPerfil 'Descrição' from tbPerfil, tbPerfilForn where cdPerfil = idPerfil and idFornecedor = '" + id + "'");
                Wait.Waiting();
                tabelaPerfil.Columns[1].Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }


        private void botaoDel_Click(object sender, RoutedEventArgs e)
        {
            if (tabelaPerfil.SelectedIndex != -1)
            {
                DataRowView row = tabelaPerfil.SelectedRow();
                int cdPF = Convert.ToInt32(row[1]);
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "delete from tbPerfilForn where cdPF = @cd";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cd", cdPF);
                        cmd.ExecuteNonQuery();
                        AtualizarPerfis();
                    }
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void botaoPesquisar_Click(object sender, RoutedEventArgs e)
        {
            op = 2;
            AtivarCampos(true);
        }

        private void botaoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            Atualizar();
        }

        private void campoCNPJ_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (campoCNPJ.IsMaskFull)
            {
                cnpjCorreto = campoCNPJ.Text.IsCnpj();
                if (cnpjCorreto)
                {
                    campoCNPJ.BorderBrush = Brushes.Green;
                }
                else
                {
                    campoCNPJ.BorderBrush = Brushes.Red;
                }
            }
            else
            {
                cnpjCorreto = false;
            }
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
            campoNome.Text = "";
            campoCep.Text = "#####-###";
            campoBairro.Text = "";
            campoRua.Text = "";
            campoEmail.Text = "";
            campoNum.Text = "";
            campoCNPJ.Text = "";
            campoRS.Text = "";
            telCel.Text = "";
            telFixo.Text = "";
            AtivarCampos(true);
        }
    }
}
