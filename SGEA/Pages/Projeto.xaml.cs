using MySql.Data.MySqlClient;
using SGEA.Forms;
using SGEA.Janelas.Orcamento;
using SGEA.Janelas.Orcamento.Projeto;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
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
using System.Windows.Controls.Primitives;

namespace SGEA.Pages
{
    /// <summary>
    /// Interaction logic for Projeto.xaml
    /// </summary>
    public partial class Projeto : Page
    {
        private SGEA.Main m;
        private int cdUsuario;

        public Projeto(int cdUsuario, SGEA.Main m)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.m = m;
            Loaded += Projeto_Loaded;
        }

        private void Projeto_Loaded(object sender, RoutedEventArgs e)
        {
            Atualizar();
        }

        public void Atualizar()
        {
            AtualizarExecucao();
            AtualizarConcluido();
            AtualizarAtrasado();
        }

        public void VerificarProjeto()
        {
            listaConcluido.ItemContainerGenerator.StatusChanged += (s, e) =>
            {
                if (listaConcluido.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    for (int i = 0; i < listaConcluido.Items.Count; i++)
                    {
                        DataGridRow row = (DataGridRow)listaConcluido.ItemContainerGenerator.ContainerFromIndex(i);
                        DataRowView r = (DataRowView)listaConcluido.Items[i];
                        string tempo = r[8].ToString();
                        if (tempo.Contains("lucro"))
                        {
                            row.Background = Brushes.Green;
                            row.Foreground = Brushes.White;
                        }
                        else if (tempo.Contains("prejuízo"))
                        {
                            row.Background = Brushes.Red;
                            row.Foreground = Brushes.White;
                        }
                    }
                }
            };
        }

        private void AtualizarAtrasado()
        {
            try
            {
                listaAtrasado.DataContext = Connect.LiteConnection("select * from vwProjetoAtrasado");
                listaAtrasado.Columns[6].Visibility = Visibility.Collapsed;
                listaAtrasado.Columns[7].Visibility = Visibility.Collapsed;
                if (listaAtrasado.Items.Count > 0)
                {
                    listaAtrasado.ToolTip = "Projetos atrasados. Clique duas vezes para verificá-lo";
                }
                else
                {
                    listaAtrasado.ToolTip = "Não há projetos atrasados";
                }
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }

        }

        private void AtualizarConcluido()
        {
            try
            {
                listaConcluido.DataContext = Connect.LiteConnection("select * from vwProjetoConcluido");
                Wait.Waiting();
                listaConcluido.Columns[7].Visibility = Visibility.Collapsed;
                listaConcluido.Columns[9].Visibility = Visibility.Collapsed;
                if (listaConcluido.Items.Count > 0)
                {
                    listaConcluido.ToolTip = "Projetos concluídos. Clique duas vezes para verificá-lo";
                }
                else
                {
                    listaConcluido.ToolTip = "Não há projetos concluídos";
                }
                VerificarProjeto();
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        private void AtualizarExecucao()
        {
            try
            {
                listaExecucao.DataContext = Connect.LiteConnection("select * from vwProjetoExecucao");
                Wait.Waiting();
                listaExecucao.Columns[5].Visibility = Visibility.Collapsed;
                listaExecucao.Columns[6].Visibility = Visibility.Collapsed;
                if (listaExecucao.Items.Count > 0)
                {
                    listaExecucao.ToolTip = "Projetos em execução. Clique duas vezes para verificá-lo";
                }
                else
                {
                    listaExecucao.ToolTip = "Não há projetos em execução";
                }
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        private void listaExecucao_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = listaExecucao.SelectedIndex;
            if (index != -1)
            {
                DataRowView row = (DataRowView)listaExecucao.Items[index];
                int cd = (int)row[5];
                View o = new View(cd);
                o.ShowDialog();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void listaAtrasado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = listaAtrasado.SelectedIndex;
            if (index != -1)
            {
                DataRowView row = (DataRowView)listaAtrasado.Items[index];
                int cd = (int)row[6];
                View o = new View(cd);
                o.ShowDialog();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void listaConcluido_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = listaConcluido.SelectedIndex;
            if (index != -1)
            {
                DataRowView row = (DataRowView)listaConcluido.Items[index];
                int cd = (int)row[7];
                View o = new View(cd);
                o.ShowDialog();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void botaoConcluirE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult box = MessageBox.Show("Você vai concluir um projeto, tem certeza?", "Confirmar", MessageBoxButton.YesNo);
                if (box == MessageBoxResult.Yes)
                {
                    int index = listaExecucao.SelectedIndex;
                    DataRowView row = (DataRowView)listaExecucao.Items[index];
                    int cd = Convert.ToInt32(row[0]);
                    ClasseProjeto p = new ClasseProjeto(cdUsuario);
                    p.ConcluirProjeto(cd);
                    Atualizar();
                    m.ExibirNotificacao();
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void botaoConcluirA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult box = MessageBox.Show("Você vai concluir um projeto, tem certeza?", "Confirmar", MessageBoxButton.YesNo);
                if (box == MessageBoxResult.Yes)
                {
                    int index = listaAtrasado.SelectedIndex;
                    DataRowView row = (DataRowView)listaAtrasado.Items[index];
                    int cd = Convert.ToInt32(row[0]);
                    ClasseProjeto p = new ClasseProjeto(cdUsuario);
                    p.ConcluirProjeto(cd);
                    Atualizar();
                    m.ExibirNotificacao();
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void botaoObs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listaAtrasado.SelectedIndex;
                DataRowView row = (DataRowView)listaAtrasado.Items[index];
                int cd = Convert.ToInt32(row[0]);
                string obs = row[4].ToString();
                Obs o = new Obs(cd, obs);
                o.ShowDialog();
                AtualizarAtrasado();
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        public void Notificacao(string cd, bool vf)
        {
            int x = -1;
            Wait.Waiting();
            for (int i = 0; i < listaAtrasado.Items.Count; i++)
            {
                DataRowView row = (DataRowView)listaAtrasado.Items[i];
                if (row[0].ToString() == cd)
                {
                    x = i;
                    break;
                }
            }
            listaAtrasado.SelectedIndex = x;
            if (vf)
                botaoConcluirA.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void botaoClienteE_Click(object sender, RoutedEventArgs e)
        {
            if (listaExecucao.SelectedIndex != -1)
            {
                DataRowView row = (DataRowView)listaExecucao.Items[listaExecucao.SelectedIndex];
                m.VerificarCliente(row[6].ToString());
            }
        }

        private void botaoClienteA_Click(object sender, RoutedEventArgs e)
        {
            if (listaAtrasado.SelectedIndex != -1)
            {
                DataRowView row = (DataRowView)listaAtrasado.Items[listaAtrasado.SelectedIndex];
                m.VerificarCliente(row[7].ToString());
            }
        }

        private void botaoClienteC_Click(object sender, RoutedEventArgs e)
        {
            if (listaConcluido.SelectedIndex != -1)
            {
                DataRowView row = (DataRowView)listaConcluido.Items[listaConcluido.SelectedIndex];
                m.VerificarCliente(row[9].ToString());
            }
        }
    }
}
