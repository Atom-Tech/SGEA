using MySql.Data.MySqlClient;
using SGEA.Forms;
using SGEA.Janelas.Orcamento;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFCustomMessageBox;
using System.Data.SQLite;

namespace SGEA.Pages
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        private int cdUsuario;
        private string login;
        private string grupo;

        public Main(int cdUsuario, string login, string grupo)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.login = login;
            this.grupo = grupo;
            Loaded += Main_Loaded;
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            if (grupo == "Funcionário")
                listaHistorico.Visibility = Visibility.Collapsed;
            AtualizarAgenda();
            AtualizarHistorico();
            VerificarProjeto();
            ObservacaoProjeto();
            Wait.Waiting();
        }

        public void AtualizarHistorico()
        {
            try
            {
                listaHistorico.SelectHistoricoHoje();
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        public void AtualizarAgenda()
        {
            try
            {
                listaAgenda.DataContext = Connect.LiteConnection("select * from vwVisitaHoje");
                Wait.Waiting();
                listaAgenda.Columns[5].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                VerificarAgenda();
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        public void VerificarProjeto()
        {
            try
            {
                listaProjetos.DataContext = Connect.LiteConnection("select * from vwProjeto");
                Wait.Waiting();
                listaProjetos.Columns[6].Visibility = Visibility.Collapsed;
                DataProjeto();
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        public void DataProjeto()
        {
            listaProjetos.ItemContainerGenerator.StatusChanged += (s, e) =>
            {
                if (listaProjetos.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    for (int i = 0; i < listaProjetos.Items.Count; i++)
                    {
                        DataGridRow row = (DataGridRow)listaProjetos.ItemContainerGenerator.ContainerFromIndex(i);
                        DataRowView r = (DataRowView)listaProjetos.Items[i];
                        string data = r[3].ToString();
                        int dia = Convert.ToInt32(data.Substring(8, 2));
                        int mes = Convert.ToInt32(data.Substring(5, 2));
                        int ano = Convert.ToInt32(data.Substring(0, 4));
                        DateTime d = new DateTime(ano, mes, dia);
                        if (d < DateTime.Today)
                        {
                            row.Background = Brushes.Red;
                            row.Foreground = Brushes.White;
                        }
                        else
                        {
                            row.Background = Brushes.Green;
                            row.Foreground = Brushes.White;
                        }
                    }
                }
            };
        }
        public void VerificarAgenda()
        {
            listaAgenda.ItemContainerGenerator.StatusChanged += (s, e) =>
            {
                if (listaAgenda.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    for (int i = 0; i < listaAgenda.Items.Count; i++)
                    {
                        DataGridRow row = (DataGridRow)listaAgenda.ItemContainerGenerator.ContainerFromIndex(i);
                        DataRowView r = (DataRowView)listaAgenda.Items[i];
                        int[] horario = Array.ConvertAll(r[2].ToString().Split(':'),Int32.Parse);
                        TimeSpan t = new TimeSpan(horario[0], horario[1], horario[2]);
                        if (t < DateTime.Now.TimeOfDay)
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
            };
        }

        public void ObservacaoProjeto()
        {
            for (int i = 0; i < listaProjetos.Items.Count; i++)
            {
                DataRowView row = (DataRowView)listaProjetos.Items[i];
                string data = row[4].ToString();
                int dia = Convert.ToInt32(data.Substring(8, 2));
                int mes = Convert.ToInt32(data.Substring(5, 2));
                int ano = Convert.ToInt32(data.Substring(0, 4));
                DateTime d = new DateTime(ano, mes, dia);
                if (row[2].ToString() == "" && d < DateTime.Today)
                {
                    MessageBoxResult r;
                    do
                    {
                        r = CustomMessageBox.ShowYesNo("Há um projeto previsto para o dia " + row[2] +
                            "\nMas ele não foi concluído", "Projeto Atrasado", "Concluir", "Adicionar Observação", MessageBoxImage.Warning);
                        switch (r)
                        {
                            case MessageBoxResult.Yes:
                                ClasseProjeto p = new ClasseProjeto(cdUsuario);
                                p.ConcluirProjeto(Convert.ToInt32(row[0]));
                                break;
                            case MessageBoxResult.No:
                                Janelas.Orcamento.Projeto.Obs o = new Janelas.Orcamento.Projeto.Obs(Convert.ToInt32(row[0]));
                                o.ShowDialog();
                                break;
                        }
                    } while (r == MessageBoxResult.None);
                }
            }
            VerificarProjeto();
        }
        
        private void listaProjetos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)listaProjetos.Items[listaProjetos.SelectedIndex];
                int cd = Convert.ToInt32(row[5]);
                View v = new View(cd);
                v.ShowDialog();
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }
    }
}
