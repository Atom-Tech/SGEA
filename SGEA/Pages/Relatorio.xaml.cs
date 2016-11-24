using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
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
using Microsoft.Win32;

namespace SGEA.Pages
{
    /// <summary>
    /// Interaction logic for Relatorio.xaml
    /// </summary>
    public partial class Relatorio : Page
    {
        SelectedDatesCollection datasA = null, datasB = null;
        int dias = 2;

        public Relatorio()
        {
            InitializeComponent();
            Loaded += Relatorio_Loaded;
        }

        private void Relatorio_Loaded(object sender, RoutedEventArgs e)
        {
            Atualizar();
        }

        public void Atualizar()
        {
            try
            {
                listaRel.ItemsSource = null;
                listaRel.DataContext = Connect.LiteConnection("select * from vwRelatorio");
                Wait.Waiting();
                listaRel.Columns[0].Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        private void botaoComparar_Click(object sender, RoutedEventArgs e)
        {
            List<Tuple<string, string, double>> relatorio = new List<Tuple<string, string, double>>();
            if (datasA != null)
            {
                relatorio.Add(GerarRelatorio(datasA.ToList(),"A"));
                if (botaoSelecionar.IsChecked == true)
                {
                    DateTime d = DateTime.Today.AddDays(-dias);
                    List<DateTime> lista = GetDateRange(d, DateTime.Today);
                    relatorio.Add(GerarRelatorio(lista, "A"));
                }
                else
                {
                    if (datasB != null)
                    {
                        relatorio.Add(GerarRelatorio(datasB.ToList(),"B"));
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Selecione os dias para o segundo relatório");
                    }
                }
                listaRel.ItemsSource = relatorio;
                listaRel.Columns[0].Header = "Período";
                listaRel.Columns[1].Header = "Datas";
                listaRel.Columns[2].Header = "Preço Total";
                listaRel.Items.Refresh();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Selecione os dias para o primeiro relatório");
            }
        }

        public static List<DateTime> GetDateRange(DateTime StartingDate, DateTime EndingDate)
        {
            if (StartingDate > EndingDate)
            {
                return null;
            }
            List<DateTime> rv = new List<DateTime>();
            DateTime tmpDate = StartingDate;
            do
            {
                rv.Add(tmpDate);
                tmpDate = tmpDate.AddDays(1);
            } while (tmpDate <= EndingDate);
            return rv;
        }

        public Tuple<string, string, double> GerarRelatorio(List<DateTime> dias, string periodo)
        {
            using (var mConn = Connect.LiteString())
            {
                DataSet ds = new DataSet();
                mConn.Open();
                if (mConn.State == ConnectionState.Open)
                {
                    SQLiteCommand cmd = new SQLiteCommand("select * from vwOrcamento", mConn);
                    cmd.CommandTimeout = 60;
                    SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);
                    adp.Fill(ds, "dados");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        double preco = 0;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (dias.Contains(DateTime.Parse(row[3].ToString())))
                            {
                                preco += Convert.ToDouble(row[6]);
                            }
                        }
                        return new Tuple<string, string, double>("Período "+periodo,dias.Min().Date.ToShortDateString()
                            + " - " + dias.Max().Date.ToShortDateString(), preco);
                    }
                }
            }
            throw new Exception("Banco de Dados não existe");
        }

        private void botaoExibir_Click(object sender, RoutedEventArgs e)
        {
            Atualizar();
        }

        private void calendarioA_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendarioA.SelectedDates.Count > 0)
                datasA = calendarioA.SelectedDates;
            else datasA = null;
        }

        private void campoDia_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (campoDia.Text != "")
                {
                    dias = campoDia.Text.ToInt();
                }
            }
            catch
            {
                campoDia.Text = "0";
                dias = 0;
            }
        }

        private void botaoSelecionar_Checked(object sender, RoutedEventArgs e)
        {
            campoDia.IsEnabled = true;
            campoDia.Text = "0";
            botaoSelecionar.Content = "Não há dias selecionados";
            dias = 0;
        }

        private void botaoSelecionar_Unchecked(object sender, RoutedEventArgs e)
        {
            campoDia.IsEnabled = false;
        }

        private void listaRel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaRel.SelectedIndex != -1)
            {
                int index = listaRel.SelectedIndex;
                dynamic row;
                string rel;
                try
                {
                    row = (DataRowView)listaRel.Items[index];
                    rel = row[0].ToString();
                }
                catch
                {
                    row = (Tuple<string, string, double>)listaRel.Items[index];
                    rel = row.Item2;
                }
                Tuple<string, double> a = new Tuple<string, double>("", 0);
                if (rel.Contains('-'))
                {
                    string[] d = rel.Replace(" ", "").Split('-');
                    string d1 = DateTime.Parse(d[0]).ToString("yyyy-MM-dd");
                    string d2 = DateTime.Parse(d[1]).ToString("yyyy-MM-dd");
                    listaOrc.DataContext = Connect.LiteConnection("select cdOrcamento 'Código', login 'Funcionário', nmCliente 'Cliente', date(dtOrcamento) 'Data de Criação', date(dtModificacao) 'Última Modificação'," +
                        " date(dtValidade) 'Data de Validade', sum(precoU * quant) 'Preço Total', observacoes 'Observações', case when idExecucao = 0 then 'Não' when idExecucao = 1 then 'Sim' end as 'Confirmado?'" +
                        " from tbOrcamento, tbUsuario, tbItemNota, tbCliente, tbProduto, tbServico" +
                        " where cdUsuario = idUsuario and cdOrcamento = idOrcamento and cdCliente = idCliente and(cdProduto = idProduto or idProduto is null) and (cdServico = idServico or idServico is null) and " +
                        " date(dtOrcamento) >= '"+d1+"' and date(dtOrcamento) <= '"+d2+"' group by cdOrcamento");
                }
                else
                {
                    string[] d = rel.Split(' ');
                    listaOrc.DataContext = Connect.LiteConnection("select cdOrcamento 'Código', login 'Funcionário', nmCliente 'Cliente', date(dtOrcamento) 'Data de Criação', date(dtModificacao) 'Última Modificação',"+
                        " date(dtValidade) 'Data de Validade', sum(precoU * quant) 'Preço Total', observacoes 'Observações', case when idExecucao = 0 then 'Não' when idExecucao = 1 then 'Sim' end as 'Confirmado?'"+
                        " from tbOrcamento, tbUsuario, tbItemNota, tbCliente"+
                        " where cdUsuario = idUsuario and cdOrcamento = idOrcamento and cdCliente = idCliente and "+
                        " strftime('%m',dtOrcamento) = '"+d[0]+"' and strftime('%Y',dtOrcamento) = '"+d[1]+"' group by cdOrcamento");
                }
                listaOrc.Columns[7].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void calendarioB_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is Calendar || Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void calendarioA_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is Calendar || Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void listaOrc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listaOrc.SelectedIndex != -1)
            {
                DataRowView row = (DataRowView)listaOrc.Items[listaOrc.SelectedIndex];
                int cd = row[0].ToString().ToInt();
                Janelas.Orcamento.View v = new Janelas.Orcamento.View(cd);
                v.ShowDialog();
            }
        }

        private void calendarioB_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendarioB.SelectedDates.Count > 0)
                datasB = calendarioB.SelectedDates;
            else datasB = null;
        }
    }
}
