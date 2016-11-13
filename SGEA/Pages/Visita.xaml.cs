using MySql.Data.MySqlClient;
using SGEA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
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
using System.Data.SQLite;

namespace SGEA.Pages
{
    /// <summary>
    /// Interaction logic for Visita.xaml
    /// </summary>
    public partial class Visita : Page
    {
        private int cdUsuario;
        private string grupo;
        private SGEA.Main m;

        private int op = 3, cd;
        public Visita(int cd, string grupo, SGEA.Main m)
        {
            InitializeComponent();
            this.grupo = grupo;
            this.m = m;
            cdUsuario = cd;
            Loaded += Visita_Loaded;
        }

        private void Visita_Loaded(object sender, RoutedEventArgs e)
        {
            if (grupo == "Funcionário")
                botaoDeletar.Visibility = Visibility.Collapsed;
            campoData.DisplayDateStart = DateTime.Now.Date;
            campoData.SelectedDate = DateTime.Now.Date;
            Atualizar();
            try
            {
                campoCliente.ItemFill("select cdCliente, nmCliente from tbCliente");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }

        }

        public void Atualizar()
        {
            try
            {
                listaVisita.DataContext = Connect.LiteConnection("select * from vwVisita");
                VerificarData();
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }
        public void VerificarData()
        {

            listaVisita.ItemContainerGenerator.StatusChanged += (s, e) =>
            {
                if (listaVisita.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    if (listaVisita.Items.Count > 0)
                    {
                        for (int i = 0; i < listaVisita.Items.Count; i++)
                        {
                            DataGridRow row = (DataGridRow)listaVisita.ItemContainerGenerator.ContainerFromIndex(i);
                            DataRowView r = (DataRowView)listaVisita.Items[i];
                            string data = r[1].ToString();
                            int ano = Convert.ToInt32(data.Substring(0, 4));
                            int mes = Convert.ToInt32(data.Substring(5, 2));
                            int dia = Convert.ToInt32(data.Substring(8, 2));
                            DateTime d = new DateTime(ano, mes, dia);
                            if (r[6].ToString() == "Sim")
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
                }
            };
        }

        private void botaoNovo_Click(object sender, RoutedEventArgs e)
        {
            op = 0;
            AtivarCampos(true);
            campoData.SelectedDate = DateTime.Now;
            campoHora.Value = DateTime.Now;
            if (campoCliente.Items.Count > 0)
                campoCliente.SelectedIndex = 0;
            campoLocal.Text = "";
            campoDesc.Text = "";
            campoObs.Text = "";
        }

        private void botaoDeletar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult box = MessageBox.Show("Você vai deletar uma visita, tem certeza?", "Confirmar", MessageBoxButton.YesNo);
                if (box == MessageBoxResult.Yes)
                {
                    int index = listaVisita.SelectedIndex;
                    DataRowView row = (DataRowView)listaVisita.Items[index];
                    int cd = Convert.ToInt32(row[0]);
                    ClasseAgenda a = new ClasseAgenda(cdUsuario);
                    a.DeletarVisita(cd);
                    Atualizar();
                    m.ExibirNotificacao();
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não há itens selecionados");
            }
        }

        private void botaoAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (listaVisita.SelectedIndex != -1)
            {
                AtivarCampos(true);
                op = 1;
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não há itens selecionados");
            }
        }

        private void listaVisita_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            op = 3;
            AtivarCampos(false);
            if (listaVisita.Items.Count > 0)
            {
                int index = listaVisita.SelectedIndex;
                DataRowView row;
                try
                {
                    row = (DataRowView)listaVisita.Items[index];
                }
                catch
                {
                    row = (DataRowView)listaVisita.Items[0];
                }
                cd = Convert.ToInt32(row[0]);
                campoData.Text = row[1].ToString();
                string hr = row[2].ToString();
                int h = Convert.ToInt32(hr.Substring(0, 2));
                int m = Convert.ToInt32(hr.Substring(3, 2));
                int s = Convert.ToInt32(hr.Substring(6, 2));
                campoHora.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, h, m, s);
                campoCliente.Text = row[3].ToString();
                campoLocal.Text = row[4].ToString();
                campoDesc.Text = row[5].ToString();
                campoObs.Text = row[7].ToString();
            }
        }

        private void botaoAgendar_Click(object sender, RoutedEventArgs e)
        {
            bool v = false;
            switch (op)
            {
                case 0:
                    try
                    {
                        string cliente = campoCliente.SelectedValue.ToString();
                        ClasseAgenda a = new ClasseAgenda(cdUsuario);
                        v = a.AgendarVisita(campoData.SelectedDate.Value.Date, campoHora.Value.Value.TimeOfDay,
                            campoLocal.Text, campoDesc.Text, cliente,campoObs.Text);
                    }
                    catch
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Não há Cliente cadastrado");
                    }
                    if (v)
                    {
                        Atualizar();
                        AtivarCampos(false);
                    }
                    break;
                case 1:
                    try
                    {
                        string cliente = campoCliente.SelectedValue.ToString();
                        ClasseAgenda a = new ClasseAgenda(cdUsuario);
                        v = a.AlterarVisita(cd, campoData.SelectedDate.Value.Date, campoHora.Value.Value.TimeOfDay,
                            cliente, campoLocal.Text, campoDesc.Text, campoObs.Text);
                        m.ExibirNotificacao();
                    }
                    catch (Exception ex)
                    {
                        Error.Erro(ex);
                    }
                    if (v)
                    {
                        Atualizar();
                        AtivarCampos(false);
                    }
                    break;
                case 2:
                    Dictionary<string, string> pesquisa = new Dictionary<string, string>();
                    if (comboData.Text != "")
                        pesquisa.Add("date(dtVisita)", campoData.SelectedDate.Value.ToSqlString());
                    if (comboHora.Text != "")
                        pesquisa.Add("time(hrVisita)", campoHora.Value.Value.TimeOfDay.ToString());
                    if (campoLocal.Text != "")
                        pesquisa.Add("localVisita", campoLocal.Text);
                    if (campoDesc.Text != "")
                        pesquisa.Add("dsVisita", campoDesc.Text);
                    if (campoObs.Text != "")
                        pesquisa.Add("observacao", campoObs.Text);
                    if (checkCliente.IsChecked == true)
                        pesquisa.Add("nmCliente", campoCliente.Text);
                    if (pesquisa.Count > 0)
                    {
                        string cmdText = "Select cdVisita 'Código', date(dtVisita) 'Data', time(hrVisita) 'Horário', nmCliente 'Cliente', " +
                            "localVisita 'Local', dsVisita 'Descrição', case when idExecucao=0 then 'Não' when idExecucao=1 then 'Sim' end as 'Confirmado?', observacao 'Observações'" +
                            "from tbAgenda, tbCliente " +
                            "where cdCliente = idCliente and ";
                        foreach (var filtro in pesquisa)
                        {
                            if (filtro.Key == "localVisita" || filtro.Key == "dsVisita" ||
                                filtro.Key == "observacao" || filtro.Key == "nmCliente")
                                cmdText += filtro.Key + " like '" + filtro.Value + "%' and ";
                            else if (filtro.Key == "date(dtVisita)")
                                cmdText += filtro.Key + " " + comboData.Text + " '" + filtro.Value + "' and ";
                            else
                                cmdText += filtro.Key + " " + comboHora.Text + " '" + filtro.Value + "' and ";
                        }
                        cmdText = cmdText.Substring(0, cmdText.Length - 5) + " order by date(dtVisita, 'localtime') asc, time(hrVisita, 'localtime') asc";
                        listaVisita.DataContext = Connect.LiteConnection(cmdText);
                        VerificarData();
                        AtivarCampos(false);
                    }
                    break;
                default:
                    Xceed.Wpf.Toolkit.MessageBox.Show("Operação Inválida");
                    break;
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

        public void Notificacao(string cd, bool vf, bool c)
        {
            int x = -1;
            Wait.Waiting();
            for (int i = 0; i < listaVisita.Items.Count; i++)
            {
                DataRowView row = (DataRowView)listaVisita.Items[i];
                if (row[0].ToString() == cd)
                {
                    x = i;
                    break;
                }
            }
            listaVisita.SelectedIndex = x;
            if (vf && !c)
                botaoDeletar.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (vf && c)
                botaoConfirmar.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void botaoConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (listaVisita.SelectedIndex != -1)
            {
                DataRowView row = (DataRowView)listaVisita.Items[listaVisita.SelectedIndex];
                if (row[6].ToString() == "Não")
                {
                    MessageBoxResult box = MessageBox.Show("Você vai concluir uma visita, tem certeza?", "Concluir", MessageBoxButton.YesNo);
                    if (box == MessageBoxResult.Yes)
                    {
                        ClasseAgenda a = new ClasseAgenda(cdUsuario);
                        a.ConcluirVisita(Convert.ToInt32(row[0]));
                        Atualizar();
                        m.ExibirNotificacao();
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("A visita já foi concluída");
                }
            }
        }

        public void AtivarCampos(bool vf)
        {
            campoData.IsEnabled = vf;
            campoHora.IsEnabled = vf;
            campoCliente.IsEnabled = vf;
            campoLocal.IsEnabled = vf;
            campoDesc.IsEnabled = vf;
            campoObs.IsEnabled = vf;
            botaoAgendar.IsEnabled = vf;
            if (op == 2)
            {
                comboHora.Visibility = Visibility.Visible;
                comboData.Visibility = Visibility.Visible;
                checkCliente.Visibility = Visibility.Visible;
                botaoAgendar.Content = "Pesquisar";
            }
            else
            {
                comboHora.Visibility = Visibility.Collapsed;
                comboData.Visibility = Visibility.Collapsed;
                checkCliente.Visibility = Visibility.Collapsed;
                botaoAgendar.Content = "Salvar";
            }
        }
    }
}
