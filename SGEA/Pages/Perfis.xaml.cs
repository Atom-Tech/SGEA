using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
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
    /// Interaction logic for Perfis.xaml
    /// </summary>
    public partial class Perfis : Page
    {
        private string grupo;
        private int id, op = 0, cdUsuario;

        public Perfis(int cdUsuario, string grupo)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.grupo = grupo;
            Loaded += Perfis_Loaded;
        }

        private void Perfis_Loaded(object sender, RoutedEventArgs e)
        {
            if (grupo == "Funcionário")
                botaoDeletar.Visibility = Visibility.Collapsed;
            AtivarCampos(false);
            AtualizarPerfis();
            AlimentarProdutos();
            AlimentarFornecedores();
        }

        private void AtivarCampos(bool v)
        {
            campoNome.IsEnabled = v;
            campoDesc.IsEnabled = v;
            botaoSalvar.IsEnabled = v;
            botaoAddP.IsEnabled = false;
            botaoAddF.IsEnabled = false;
            botaoDelF.IsEnabled = false;
            botaoDelP.IsEnabled = false;
            if (op == 2) botaoSalvar.Content = "Pesquisar";
            else botaoSalvar.Content = "Salvar";
        }

        private void AlimentarFornecedores()
        {
            try
            {
                campoForn.ItemFill("select cdFornecedor, nmFornecedor from tbFornecedor");
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        private void AlimentarProdutos()
        {
            try
            {
                campoProduto.ItemFill("select cdProduto, nmProduto from tbProduto");
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        private void listaPerfil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AtivarCampos(false);
            if (listaPerfil.Items.Count > 0)
            {
                botaoAddF.IsEnabled = true;
                botaoDelF.IsEnabled = true;
                botaoAddP.IsEnabled = true;
                botaoDelP.IsEnabled = true;
                int index = listaPerfil.SelectedIndex;
                DataRowView row;
                try
                {
                    row = (DataRowView)listaPerfil.Items[index];
                }
                catch
                {
                    row = (DataRowView)listaPerfil.Items[0];
                }
                id = Convert.ToInt32(row[0]);
                campoNome.Text = row[1].ToString();
                campoDesc.Text = row[2].ToString();
                AtualizarProdutos();
                AtualizarFornecedores();
            }
        }

        private void AtualizarFornecedores()
        {
            try
            {
                listaForn.DataContext = Connect.LiteConnection("select cdPF, nmFornecedor 'Fornecedor', cnpj 'CNPJ', razaoSocial 'Razão Social' from tbFornecedor, tbPerfilForn where idFornecedor = cdFornecedor and idPerfil = " + id);
                Wait.Waiting();
                listaForn.Columns[0].Visibility = Visibility.Collapsed;
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        private void AtualizarProdutos()
        {
            try
            {
                listaProdutos.DataContext = Connect.LiteConnection("select cdPP, nmProduto 'Produto', dsProduto 'Descrição', nmTipo 'Tipo' from tbProduto, tbTipo, tbPerfilProduto where tpProduto = cdTipo and idProduto = cdProduto and idPerfil = " + id);
                Wait.Waiting();
                listaProdutos.Columns[0].Visibility = Visibility.Collapsed;
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        private void botaoNovo_Click(object sender, RoutedEventArgs e)
        {
            op = 0;
            AtivarCampos(true);
            campoNome.Text = "";
            campoDesc.Text = "";
            listaProdutos.DataContext = null;
            listaForn.DataContext = null;
        }

        private void botaoAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (listaPerfil.SelectedIndex != -1)
            {
                op = 1;
                AtivarCampos(true);
                botaoAddP.IsEnabled = true;
                botaoAddF.IsEnabled = true;
                botaoDelF.IsEnabled = true;
                botaoDelP.IsEnabled = true;
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
                MessageBoxResult box = MessageBox.Show("Você vai deletar um perfil, tem certeza?", "Confirmar", MessageBoxButton.YesNo);
                if (box == MessageBoxResult.Yes)
                {
                    int index = listaPerfil.SelectedIndex;
                    DataRowView row = (DataRowView)listaPerfil.Items[index];
                    int cd = Convert.ToInt32(row[0]);
                    Classes.ClassePerfil c = new Classes.ClassePerfil(cdUsuario);
                    c.DeletarPerfil(cd);
                    AtualizarPerfis();
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void botaoSalvar_Click(object sender, RoutedEventArgs e)
        {
            bool v = false;
            Classes.ClassePerfil p = new Classes.ClassePerfil(cdUsuario);
            switch (op)
            {
                case 0:
                    v = p.AdicionarPerfil(campoNome.Text, campoDesc.Text);
                    if (v)
                    {
                        AtualizarPerfis();
                        AtivarCampos(false);
                    }
                    break;
                case 1:
                    v = p.AlterarPerfil(id, campoNome.Text, campoDesc.Text);
                    if (v)
                    {
                        AtualizarPerfis();
                        AtivarCampos(false);
                    }
                    break;
                case 2:
                    Dictionary<string, string> pesquisa = new Dictionary<string, string>();
                    if (campoNome.Text != "")
                        pesquisa.Add("nmPerfil", campoNome.Text);
                    if (campoDesc.Text != "")
                        pesquisa.Add("dsPerfil", campoDesc.Text);
                    if (pesquisa.Count > 0)
                    {
                        string cmdText = "select cdPerfil 'Código', nmPerfil 'Nome', dsPerfil 'Descrição' from tbPerfil where ";
                        foreach (var filtro in pesquisa)
                        {
                            cmdText += filtro.Key + " like '" + filtro.Value + "%' and ";
                        }
                        cmdText = cmdText.Substring(0, cmdText.Length - 5);
                        listaPerfil.DataContext = Connect.LiteConnection(cmdText);
                        AtivarCampos(false);
                    }
                    break;
            }
        }

        private void botaoAddP_Click(object sender, RoutedEventArgs e)
        {
            if (campoProduto.Items.Count > 0)
            {
                if (campoProduto.SelectedIndex == -1)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou Produto");
                }
                else
                {
                    string cdProduto = campoProduto.SelectedValue.ToString();
                    Classes.ClassePerfil p = new Classes.ClassePerfil(cdUsuario);
                    p.AdicionarPerfilProduto(id, cdProduto);
                    AtualizarProdutos();
                }
            }
        }

        private void botaoAddF_Click(object sender, RoutedEventArgs e)
        {
            if (campoForn.Items.Count > 0)
            {
                if (campoForn.SelectedIndex == -1)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou Fornecedor");
                }
                else
                {
                    string cdForn = campoForn.SelectedValue.ToString();
                    Classes.ClassePerfil p = new Classes.ClassePerfil(cdUsuario);
                    p.AdicionarPerfilForn(id, cdForn);
                    AtualizarFornecedores();
                }
            }
        }

        private void botaoDelP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listaProdutos.SelectedIndex;
                DataRowView row = (DataRowView)listaProdutos.Items[index];
                int cd = Convert.ToInt32(row[0]);
                Classes.ClassePerfil p = new Classes.ClassePerfil(cdUsuario);
                p.DeletarPerfilProduto(cd);
                AtualizarProdutos();
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void botaoPesquisar_Click(object sender, RoutedEventArgs e)
        {
            op = 2;
            AtivarCampos(true);
        }

        private void botaoDelF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listaForn.SelectedIndex;
                DataRowView row = (DataRowView)listaForn.Items[index];
                int cd = Convert.ToInt32(row[0]);
                Classes.ClassePerfil p = new Classes.ClassePerfil(cdUsuario);
                p.DeletarPerfilForn(cd);
                AtualizarFornecedores();
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void AtualizarPerfis()
        {
            try
            {
                listaPerfil.DataContext = Connect.LiteConnection("select cdPerfil 'Código', nmPerfil 'Nome', dsPerfil 'Descrição' from tbPerfil");
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }
    }
}
