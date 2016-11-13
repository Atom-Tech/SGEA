using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using SGEA.Janelas;
using System.IO;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SQLite;

namespace SGEA.Pages
{
    /// <summary>
    /// Interaction logic for Produto.xaml
    /// </summary>
    public partial class Produto : Page
    {
        private double preco;
        private int cdUsuario;
        private string c = "", i = "", caminhoOriginal = "", grupo;
        private int op = 3;
        private long id;

        public Produto(int cdUsuario, string grupo)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.grupo = grupo;
            Loaded += Produto_Loaded;
        }

        private void Produto_Loaded(object sender, RoutedEventArgs e)
        {
            if (grupo == "Funcionário")
                botaoDeletar.Visibility = Visibility.Collapsed;
            AlimentarPerfis();
            AtualizarProdutos();
            AtualizarTipos();
        }

        public void AtualizarProdutos()
        {
            try
            {
                listaProdutos.DataContext = Connect.LiteConnection("select * from vwProduto");
                Wait.Waiting();
                listaProdutos.Columns[5].Visibility = Visibility.Collapsed;
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }

        }

        private void botaoSalvar_Click(object sender, RoutedEventArgs e)
        {
            bool v = false;
            if (campoTipo.Items.Count > 0)
            {
                ClasseProduto p = new ClasseProduto(cdUsuario);
                if (op == 0)
                {
                    if (campoImagem.Source != null)
                    {
                        string imagem = campoImagem.Source.ToString();
                        v = p.CadastrarProduto(campoNome.Text, campoDesc.Text, campoTipo.SelectedValue.ToString(), caminhoOriginal, c, i, campoPreco.Text);
                    }
                    else
                    {
                        v = p.CadastrarProduto(campoNome.Text, campoDesc.Text, campoTipo.SelectedValue.ToString(),campoPreco.Text);
                    }
                    id = IndexProduto();
                    if (v)
                    {
                        AtualizarProdutos();
                        AtivarCampos(false);
                    }
                }
                else if (op == 1)
                {
                    if (campoImagem.Source != null)
                    {
                        string imagem = campoImagem.Source.ToString().Substring(8);
                        if (i == "" && c == "")
                        {
                            string[] x = imagem.Split('/');
                            for (int n = 0; n < x.Length - 1; n++) c += x[n] + "/";
                            i = x[x.Length - 1];
                        }
                        campoImagem.Source = null;
                        v = p.AlterarProduto(id, campoNome.Text, campoDesc.Text, campoTipo.SelectedValue.ToString(), imagem, c, i, campoPreco.Text);
                    }
                    else
                    {
                       v = p.AlterarProduto(id, campoNome.Text, campoDesc.Text, campoTipo.SelectedValue.ToString(), campoPreco.Text);
                    }
                    if (v)
                    {
                        AtualizarProdutos();
                        AtivarCampos(false);
                    }
                }
                else if (op == 2)
                {
                    Dictionary<string, string> pesquisa = new Dictionary<string, string>();
                    if (campoNome.Text != "")
                        pesquisa.Add("nmProduto", campoNome.Text);
                    if (campoDesc.Text != "")
                        pesquisa.Add("dsProduto", campoDesc.Text);
                    if (checkTipo.IsChecked == true)
                        pesquisa.Add("tpProduto", campoTipo.SelectedValue.ToString());
                    if (comboPreco.Text != "")
                        pesquisa.Add("preco", campoPreco.Text);
                    if (pesquisa.Count > 0)
                    {
                        string cmdText = "Select cdProduto 'Código', nmProduto 'Nome do Produto', dsProduto 'Descrição do Produto', case when imagem != 'Sem Imagem' then 'Sim' else 'Não' end as 'Possui Imagem?', nmTipo 'Tipo', imagem, preco 'Preço/m²' from tbProduto, tbTipo where cdTipo = tpProduto and ";
                        foreach (var filtro in pesquisa)
                        {
                            if (filtro.Key == "tpProduto")
                                cmdText += filtro.Key + " = '" + filtro.Value + "' and ";
                            else if (filtro.Key == "preco")
                                cmdText += filtro.Key + " " + comboPreco.Text + " '" + filtro.Value + "' and ";
                            else
                                cmdText += filtro.Key + " like '" + filtro.Value + "%' and ";
                        }
                        cmdText = cmdText.Substring(0, cmdText.Length - 5);
                        listaProdutos.DataContext = Connect.LiteConnection(cmdText);
                        listaProdutos.Columns[5].Visibility = Visibility.Collapsed;
                        AtivarCampos(false);
                    }
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não há tipos cadastrados");
            }
        }

        private long IndexProduto()
        {
            try
            {
                dataGrid1.DataContext = Connect.LiteConnection("select cdProduto from tbProduto order by cdProduto desc");
                DataRowView row = (DataRowView)dataGrid1.Items[0];
                return Convert.ToInt64(row[0]);
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return 0;
        }

        private void botaoNovo_Click(object sender, RoutedEventArgs e)
        {
            op = 0;
            AtivarCampos(true);
            botaoAdd.IsEnabled = false;
            botaoDel.IsEnabled = false;
            campoNome.Text = "";
            campoDesc.Text = "";
            campoPreco.Text = "0";
            campoTipo.SelectedIndex = 0;
            campoImagem.Source = null;
            tabelaPerfil.DataContext = null;
        }

        public void AtivarCampos(bool vf)
        {
            campoPerfil.IsEnabled = false;
            campoNome.IsEnabled = vf;
            campoDesc.IsEnabled = vf;
            campoTipo.IsEnabled = vf;
            campoPreco.IsEnabled = vf;
            botaoPC.IsEnabled = vf;
            botaoLimpar.IsEnabled = vf;
            botaoSalvar.IsEnabled = vf;
            if (op == 2)
            {
                comboPreco.Visibility = Visibility.Visible;
                checkTipo.Visibility = Visibility.Visible;
                botaoSalvar.Content = "Pesquisar";
            }
            else
            {
                comboPreco.Visibility = Visibility.Collapsed;
                checkTipo.Visibility = Visibility.Collapsed;
                botaoSalvar.Content = "Salvar";
            }
        }

        public void AtualizarTipos()
        {
            try
            {
                campoTipo.ItemFill("select cdTipo, nmTipo from tbTipo");
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }

        }

        private void botaoPC_Click(object sender, RoutedEventArgs e)
        {
            c = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SGEA\\Imagens\\";
            Directory.CreateDirectory(c);
            OpenFileDialog imagem = new OpenFileDialog();
            imagem.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            if (imagem.ShowDialog() == true)
            {
                i = imagem.SafeFileName;
                campoImagem.Source = new BitmapImage(new Uri(imagem.FileName));
                caminhoOriginal = imagem.FileName;
            }
        }

        private void botaoLimpar_Click(object sender, RoutedEventArgs e)
        {
            caminhoOriginal = "";
            i = "";
            c = "";
            campoImagem.Source = null;
        }

        private void listaProdutos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AtivarCampos(false);
            try
            {
                int index = listaProdutos.SelectedIndex;
                DataRowView row;
                try
                {
                    row = (DataRowView)listaProdutos.Items[index];
                }
                catch
                {
                    row = (DataRowView)listaProdutos.Items[0];
                }
                id = Convert.ToInt64(row[0]);
                string nome = row[1].ToString();
                string desc = row[2].ToString();
                string imagem = row[5].ToString();
                string tipo = row[4].ToString();
                preco = Convert.ToDouble(row[6]);
                AtualizarPerfis();
                campoTipo.Text = tipo;
                campoNome.Text = nome;
                campoDesc.Text = desc;
                campoPreco.Text = preco.ToString();
                if (campoPerfil.Items.Count > 0)
                    campoPerfil.SelectedIndex = 0;
                botaoAdd.IsEnabled = true;
                campoPerfil.IsEnabled = true;
                botaoDel.IsEnabled = true;
                if (imagem != "Sem Imagem")
                    campoImagem.Source = new BitmapImage(new Uri(imagem));
                else
                    campoImagem.Source = null;
            }
            catch (DirectoryNotFoundException)
            {
                campoImagem.Source = null;
            }
            catch (FileNotFoundException)
            {
                campoImagem.Source = null;
            }
            catch (UriFormatException)
            {
                campoImagem.Source = null;
            }
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

        public void AtualizarPerfis()
        {
            try
            {
                tabelaPerfil.DataContext = Connect.LiteConnection("select cdPerfil 'Código', cdPP, nmPerfil 'Perfil', dsPerfil 'Descrição' from tbPerfil, tbPerfilProduto where cdPerfil = idPerfil and idProduto = '" + id + "'");
                Wait.Waiting();
                tabelaPerfil.Columns[1].Visibility = Visibility.Collapsed;
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
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
                                string cmdText = "insert into tbPerfilProduto values (null,@cd,@id)";
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
                    Xceed.Wpf.Toolkit.MessageBox.Show("Não há perfis cadastrados");
                }
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

        private void botaoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            botaoNovo.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            AtualizarProdutos();
        }

        private void maisTipo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Tipos t = new Tipos(cdUsuario, grupo);
            t.ShowDialog();
            AtualizarTipos();
        }

        private void campoPreco_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                preco = Convert.ToDouble(campoPreco.Text);
            }
            catch
            {
                campoPreco.Text = "0";
                preco = 0;
            }
        }

        private void botaoDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = tabelaPerfil.SelectedIndex;
                DataRowView row = tabelaPerfil.SelectedRow();
                int cdPP = Convert.ToInt32(row[1]);
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "delete from tbPerfilProduto where cdPP = @cd";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cd", cdPP);
                        cmd.ExecuteNonQuery();
                        AtualizarPerfis();
                    }
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void botaoAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (listaProdutos.SelectedIndex != -1)
            {
                op = 1;
                AtivarCampos(true);
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
                MessageBoxResult box = MessageBox.Show("Você vai deletar um produto, tem certeza?", "Confirmar", MessageBoxButton.YesNo);
                if (box == MessageBoxResult.Yes)
                {
                    int index = listaProdutos.SelectedIndex;
                    DataRowView row = (DataRowView)listaProdutos.Items[index];
                    id = Convert.ToInt64(row[0]);
                    ClasseProduto p = new ClasseProduto(cdUsuario);
                    p.DeletarProduto(id);
                    AtualizarProdutos();
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }
    }
}
