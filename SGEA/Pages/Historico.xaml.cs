using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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

namespace SGEA.Pages
{
    /// <summary>
    /// Interaction logic for Historico.xaml
    /// </summary>
    public partial class Historico : Page
    {
        public Historico()
        {
            InitializeComponent();
            AtualizarDatas();
        }

        public void AtualizarDatas()
        {
            listaDia.DataContext = Connect.LiteConnection("select * from vwHistorico");
        }

        private void listaDia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaDia.SelectedIndex != -1)
            {
                int index = listaDia.SelectedIndex;
                DataRowView row = (DataRowView)listaDia.Items[index];
                string data = row[0].ToString();
                int dia = Convert.ToInt32(data.Substring(8, 2));
                int mes = Convert.ToInt32(data.Substring(5, 2));
                int ano = Convert.ToInt32(data.Substring(0, 4));
                groupHistorico.Header = "Histórico do Dia " + dia + "/"
                    + mes + "/" + ano;
                AtualizarHistorico(data);
            }
        }

        public void AtualizarHistorico(string data)
        {
            listaHistorico.DataContext = Connect.LiteConnection("select login 'Funcionário', " +
                    " time(hrHistorico) 'Horário', dsHistorico 'Mensagem' from tbHistorico" +
                    " where date(dtHistorico) = date('" + data + "')");
            listaHistorico.Columns[2].Width = new DataGridLength(1,DataGridLengthUnitType.Star);
        }
    }
}
