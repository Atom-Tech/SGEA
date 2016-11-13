using MySql.Data.MySqlClient;
using SGEA.Janelas.Orcamento;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SGEA.Pages
{
    /// <summary>
    /// Interaction logic for Orcamento.xaml
    /// </summary>
    public partial class Orcamento : Page
    {
        private SGEA.Main m;
        private string grupo;
        private int cdUsuario, id;
        private List<int> cd = new List<int>(), quant = new List<int>();
        private List<string> nome = new List<string>(), desc = new List<string>(), tipo = new List<string>(), imagem = new List<string>();
        private List<double> larg = new List<double>(), alt = new List<double>(), preco = new List<double>(), precoT = new List<double>(),
            precoM = new List<double>();

        public Orcamento(int cdUsuario, string grupo, SGEA.Main m)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.grupo = grupo;
            this.m = m;
            Loaded += Orcamento_Loaded;
        }

        private void Orcamento_Loaded(object sender, RoutedEventArgs e)
        {
            if (grupo == "Funcionário")
                botaoDel.Visibility = Visibility.Collapsed;
            AtualizarOrcamento();
            Wait.Waiting();
            listaOrc.Columns[9].Visibility = Visibility.Collapsed;
            listaOrc.Columns[8].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void botaoPDF_Click(object sender, RoutedEventArgs e)
        {
            if (listaOrc.SelectedIndex != -1)
            {
                int index = listaOrc.SelectedIndex;
                DataRowView row = (DataRowView)listaOrc.Items[index];
                string login = row[1].ToString();
                string obs = row[8].ToString();
                for (int i = 0; i < viewOrcP.Items.Count; i++)
                {
                    row = (DataRowView)viewOrcP.Items[i];
                    nome.Add(row[0].ToString());
                    desc.Add(row[1].ToString());
                    tipo.Add(row[2].ToString());
                    imagem.Add(row[3].ToString());
                    larg.Add(Convert.ToDouble(row[4]));
                    alt.Add(Convert.ToDouble(row[5]));
                    quant.Add(Convert.ToInt32(row[6]));
                    preco.Add(Convert.ToDouble(row[8]));
                }
                for (int i = 0; i < viewOrcS.Items.Count; i++)
                {
                    row = (DataRowView)viewOrcS.Items[i];
                    nome.Add(row[0].ToString());
                    desc.Add(row[1].ToString());
                    tipo.Add("Serviço");
                    imagem.Add("Sem Imagem");
                    larg.Add(1);
                    alt.Add(1);
                    quant.Add(Convert.ToInt32(row[2]));
                    preco.Add(Convert.ToDouble(row[3]));
                }
                ClasseOrcamento o = new ClasseOrcamento(cdUsuario);
                o.ExportarRelatorio(nome, desc, tipo, imagem, larg, alt, quant, preco, obs, login, viewOrcP.Items.Count + viewOrcS.Items.Count);
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }
        
        private void botaoConfirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listaOrc.SelectedIndex;
                DataRowView row = (DataRowView)listaOrc.Items[index];
                string c = row[7].ToString();
                if (c == "Não")
                {
                    using (var mConn = Connect.LiteString())
                    {
                        DataTable ds = new DataTable();
                        mConn.Open();
                        if (mConn.State == ConnectionState.Open)
                        {
                            string cmdText = "update tbOrcamento set idExecucao = 1 where cdOrcamento = '" + id + "'";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Venda Confirmada");
                            Janelas.Orcamento.Projeto.Gerar p = new Janelas.Orcamento.Projeto.Gerar(cdUsuario, id);
                            p.ShowDialog();
                            AtualizarOrcamento();
                            m.ExibirNotificacao();
                        }
                    }
                }
                else if (c == "Sim")
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Venda já está confirmada");
                }
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        private void botaoCliente_Click(object sender, RoutedEventArgs e)
        {
            if (listaOrc.SelectedIndex != -1)
            {
                DataRowView row = (DataRowView)listaOrc.Items[listaOrc.SelectedIndex];
                string cd = row[9].ToString();
                m.VerificarCliente(cd);
            }
        }

        public void Notificacao(string cd, bool vf)
        {
            int x = -1;
            Wait.Waiting();
            for (int i = 0; i < listaOrc.Items.Count; i++)
            {
                DataRowView row = (DataRowView)listaOrc.Items[i];
                if (row[0].ToString() == cd)
                {
                    x = i;
                    break;
                }
            }
            listaOrc.SelectedIndex = x;
            if (vf)
                botaoDel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void botaoNovo_Click(object sender, RoutedEventArgs e)
        {
            Gerar g = new Gerar(cdUsuario, true);
            g.ShowDialog();
            AtualizarOrcamento();
        }

        public void AtualizarOrcamento()
        {
            try
            {
                listaOrc.DataContext = Connect.LiteConnection("select * from vwOrcamento");
                VerificarData();
                Wait.Waiting();
                listaOrc.Columns[9].Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        public void VerificarData()
        {
            listaOrc.ItemContainerGenerator.StatusChanged += (s, e) =>
            {
                if (listaOrc.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    for (int i = 0; i < listaOrc.Items.Count; i++)
                    {
                        DataGridRow row = (DataGridRow)listaOrc.ItemContainerGenerator.ContainerFromIndex(i);
                        DataRowView r = (DataRowView)listaOrc.Items[i];
                        string data = r[5].ToString();
                        int dia = Convert.ToInt32(data.Substring(8, 2));
                        int mes = Convert.ToInt32(data.Substring(5, 2));
                        int ano = Convert.ToInt32(data.Substring(0, 4));
                        DateTime d = new DateTime(ano, mes, dia);
                        if (r[7].ToString() == "Sim")
                        {
                            row.Background = Brushes.Green;
                            row.Foreground = Brushes.White;
                        }
                        else
                        {
                            if (d < DateTime.Today)
                            {
                                row.Background = Brushes.Red;
                                row.Foreground = Brushes.White;
                            }
                            else
                            {
                                row.Background = Brushes.Yellow;
                                row.Foreground = Brushes.Black;
                            }
                        }
                    }
                }
            };
        }

        private void botaoDel_Click(object sender, RoutedEventArgs e)
        {
            if (listaOrc.SelectedIndex != -1)
            {
                MessageBoxResult box = MessageBox.Show("Você vai deletar um orçamento, tem certeza?", "Confirmar", MessageBoxButton.YesNo);
                if (box == MessageBoxResult.Yes)
                {
                    int index = listaOrc.SelectedIndex;
                    DataRowView row = (DataRowView)listaOrc.Items[index];
                    int cd = Convert.ToInt32(row[0]);
                    ClasseOrcamento o = new ClasseOrcamento(cdUsuario);
                    o.DeletarOrcamento(cd);
                    AtualizarOrcamento();
                    m.ExibirNotificacao();
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void listaOrc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaOrc.Items.Count > 0)
            {
                int index = listaOrc.SelectedIndex;
                DataRowView row;
                try
                {
                    row = (DataRowView)listaOrc.Items[index];
                }
                catch
                {
                    row = (DataRowView)listaOrc.Items[0];
                }
                id = Convert.ToInt32(row[0]);
                AtualizarProdutos(id);
                AtualizarServicos(id);
            }
        }

        public void AtualizarProdutos(int cd)
        {
            try
            {
                viewOrcP.DataContext = Connect.LiteConnection("select nmProduto 'Produto', dsProduto 'Descrição', nmTipo 'Tipo'," +
                    "imagem, largura 'Largura', altura 'Altura', quant 'Quantidade', preco 'Preço m²', precoU 'Preço Unitário', precoU*quant 'Preço Total', cdItemNota" +
                    " from tbOrcamento, tbItemNota, tbProduto, tbTipo where cdOrcamento = " + cd + " and tpProduto = cdTipo and cdOrcamento = idOrcamento and cdProduto = idProduto");
                Wait.Waiting();
                viewOrcP.Columns[3].Visibility = Visibility.Collapsed;
                viewOrcP.Columns[10].Visibility = Visibility.Collapsed;
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        public void AtualizarServicos(int cd)
        {
            try
            {
                viewOrcS.DataContext = Connect.LiteConnection("select nmServico 'Serviço', dsServico 'Descrição', " +
                    " quant 'Quantidade', precoU 'Preço Unitário', precoU*quant 'Preço Total', cdItemNota" +
                    " from tbOrcamento, tbItemNota, tbServico where cdOrcamento = " + cd + " and cdOrcamento = idOrcamento and cdServico = idServico ");
                Wait.Waiting();
                viewOrcS.Columns[5].Visibility = Visibility.Collapsed;
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }
        private void botaoAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (listaOrc.SelectedIndex != -1)
            {
                int index = listaOrc.SelectedIndex;
                DataRowView row = (DataRowView)listaOrc.Items[index];
                int id = Convert.ToInt32(row[0]);
                string cliente = row[2].ToString();
                string obs = row[8].ToString();
                string data = row[5].ToString();
                cd.Clear();
                nome.Clear();
                desc.Clear();
                tipo.Clear();
                imagem.Clear();
                larg.Clear();
                alt.Clear();
                quant.Clear();
                preco.Clear();
                precoT.Clear();
                for (int i = 0; i < viewOrcP.Items.Count; i++)
                {
                    DataRowView row2 = (DataRowView)viewOrcP.Items[i];
                    cd.Add(Convert.ToInt32(row2[10]));
                    nome.Add(row2[0].ToString());
                    desc.Add(row2[1].ToString());
                    tipo.Add(row2[2].ToString());
                    imagem.Add(row2[3].ToString());
                    larg.Add(Convert.ToDouble(row2[4]));
                    alt.Add(Convert.ToDouble(row2[5]));
                    quant.Add(Convert.ToInt32(row2[6]));
                    preco.Add(Convert.ToDouble(row2[8]));
                    precoT.Add(Convert.ToDouble(row2[9]));
                    precoM.Add(Convert.ToDouble(row2[7]));
                }
                for (int i = 0; i < viewOrcS.Items.Count; i++)
                {
                    DataRowView row2 = (DataRowView)viewOrcS.Items[i];
                    cd.Add(Convert.ToInt32(row2[5]));
                    nome.Add(row2[0].ToString());
                    desc.Add(row2[1].ToString());
                    quant.Add(Convert.ToInt32(row2[2]));
                    preco.Add(Convert.ToDouble(row2[3]));
                    precoT.Add(Convert.ToDouble(row2[4]));
                    precoM.Add(1);
                    tipo.Add("Serviço");
                    imagem.Add("Sem Imagem");
                    larg.Add(1);
                    alt.Add(1);
                }
                Gerar v = new Gerar(false, id, cd, nome, desc, tipo, imagem, larg, alt, quant, preco, precoT, cliente, data, obs, precoM);
                v.ShowDialog();
                AtualizarOrcamento();
                AtualizarProdutos(id);
                m.ExibirNotificacao();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }
    }
}
