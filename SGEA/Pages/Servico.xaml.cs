using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
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
    /// Interaction logic for Servico.xaml
    /// </summary>
    public partial class Servico : Page
    {
        private string grupo;
        private long id;
        private int op = 3, cdUsuario;
        public Servico(int cdUsuario, string grupo)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.grupo = grupo;
            Loaded += Servico_Loaded;
        }

        private void Servico_Loaded(object sender, RoutedEventArgs e)
        {
            if (grupo == "Funcionário")
                botaoDel.Visibility = Visibility.Collapsed;
            Atualizar();
        }

        private void botaoSalvar_Click(object sender, RoutedEventArgs e)
        {
            Forms.Servicos s = new Forms.Servicos(cdUsuario);
            bool v = false;
            if (op == 0)
            {
                v = s.CadastrarServicos(campoNome.Text, campoDesc.Text);
            }
            else if (op == 1)
            {
                v = s.AlterarServicos(id, campoNome.Text, campoDesc.Text);
            }
            else if (op == 2)
            {
                Dictionary<string, string> pesquisa = new Dictionary<string, string>();
                if (campoNome.Text != "")
                    pesquisa.Add("nmServico", campoNome.Text);
                if (campoDesc.Text != "")
                    pesquisa.Add("dsServico", campoDesc.Text);
                if (pesquisa.Count > 0)
                {
                    string cmdText = "select cdServico 'Código', nmServico 'Serviço', dsServico 'Descrição' from tbServico where ";
                    foreach (var filtro in pesquisa)
                    {
                        cmdText += filtro.Key + " like '" + filtro.Value + "%' and ";
                    }
                    cmdText = cmdText.Substring(0, cmdText.Length - 5);
                    listaSer.DataContext = Connect.LiteConnection(cmdText);
                    AtivarCampos(false);
                }
            }
            if (v)
            {
                Atualizar();
                AtivarCampos(false);
            }
        }

        private void botaoNovo_Click(object sender, RoutedEventArgs e)
        {
            op = 0;
            AtivarCampos(true);
            campoNome.Text = "";
            campoDesc.Text = "";
        }

        public void AtivarCampos(bool vf)
        {
            campoNome.IsEnabled = vf;
            campoDesc.IsEnabled = vf;
            botaoSalvar.IsEnabled = vf;
            if (op == 2) botaoSalvar.Content = "Pesquisar";
            else botaoSalvar.Content = "Salvar";
        }

        private void listaSer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AtivarCampos(false);
            if (listaSer.Items.Count > 0)
            {
                int index = listaSer.SelectedIndex;
                DataRowView row;
                try
                {
                    row = (DataRowView)listaSer.Items[index];
                }
                catch
                {
                    row = (DataRowView)listaSer.Items[0];
                }
                id = Convert.ToInt64(row[0]);
                string nome = row[1].ToString();
                string desc = row[2].ToString();
                campoNome.Text = nome;
                campoDesc.Text = desc;
            }
        }

        private void botaoAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (listaSer.SelectedIndex != -1)
            {
                AtivarCampos(true);
                op = 1;
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        private void botaoDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult box = MessageBox.Show("Você vai deletar um serviço, tem certeza?", "Confirmar", MessageBoxButton.YesNo);
                if (box == MessageBoxResult.Yes)
                {
                    int index = listaSer.SelectedIndex;
                    DataRowView row = (DataRowView)listaSer.Items[index];
                    id = Convert.ToInt64(row[0]);
                    Forms.Servicos s = new Forms.Servicos(cdUsuario);
                    s.DeletarServicos(id);
                    Atualizar();
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
        }

        private void botaoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            Atualizar();
        }

        public void Atualizar()
        {
            try
            {
                listaSer.DataContext = Connect.LiteConnection("select cdServico 'Código', nmServico 'Serviço', dsServico 'Descrição' from tbServico");
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }
    }
}
