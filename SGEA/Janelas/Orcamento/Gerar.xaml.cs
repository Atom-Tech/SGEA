using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.SQLite;
using SGEA.Classes;

namespace SGEA.Janelas.Orcamento
{
    /// <summary>
    /// Interaction logic for Gerar.xaml
    /// </summary>
    public partial class Gerar : Window
    {
        private int cdUsuario, idO;
        private bool op, check;
        private string data, cliente;
        private List<int> cd = new List<int>(), quant = new List<int>(), cdN = new List<int>();
        private List<string> nome = new List<string>(), tp = new List<string>()
            , desc = new List<string>(), tipo = new List<string>(), imagem = new List<string>();
        private List<double> larg = new List<double>(), alt = new List<double>(), preco = new List<double>(), precoT = new List<double>(),
            precoM = new List<double>();

        public Gerar()
        {
            InitializeComponent();
        }
        public Gerar(bool op, int idO, List<int> cd, List<string> nome, List<string> desc, List<string> tipo,
    List<string> imagem, List<double> larg, List<double> alt, List<int> quant, List<double> preco, List<double> precoT, string cliente, string data, string obs,
            List<double> precoM)
        {
            InitializeComponent();
            this.op = op;
            this.idO = idO;
            this.cd = cd;
            cdN = cd;
            this.nome = nome;
            this.desc = desc;
            this.tipo = tipo;
            this.imagem = imagem;
            this.larg = larg;
            this.alt = alt;
            this.quant = quant;
            this.preco = preco;
            this.precoT = precoT;
            this.precoM = precoM;
            this.cliente = cliente;
            this.data = data;
            campoObs.Text = obs;
            campoData.Text = data;
            ContentRendered += Gerar_ContentRendered;
        }
        public Gerar(int cdUsuario, bool op)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.op = op;
            ContentRendered += Gerar_ContentRendered;
        }

        private void Gerar_ContentRendered(object sender, EventArgs e)
        {
            DateTime data = DateTime.Now.Date;
            campoData.DisplayDateStart = data;
            campoData.Text = data.ToString();
            data = DateTime.Now.AddMonths(1);
            listaPS.SelectedIndex = 0;
            ListaTipo();
            ListaProduto();
            ListaCliente();
            if (cliente != null)
            {
                listaCliente.Text = cliente;
            }
            else
            {
                listaCliente.SelectedIndex = 0;
            }
            if (!op)
                CarregarProduto();
            try
            {
                double precoO = 0;
                for (int i = 0; i < listaOrc.Items.Count; i++)
                {
                    precoO += precoT[i];
                }
                campoPrecoO.Text = precoO.ToString();
            }
            catch
            {
                campoPrecoO.Text = "0";
            }
        }

        public void ListaTipo()
        {
            try
            {
                listaTipo.ItemFill("select cdTipo, nmTipo from tbTipo");
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }
        public void ListaCliente()
        {
            try
            {
                listaCliente.ItemFill("select cdCliente, nmCliente from tbCliente");
            }

            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        public void CarregarProduto()
        {
            for (int i = 0; i < nome.Count(); i++)
            {
                listaOrc.Items.Add(nome[i]);
            }
        }

        private void listaPS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaPS.SelectedIndex == 0)
            {
                listaTipo.IsEnabled = true;
                ListaProduto();
            }
            if (listaPS.SelectedIndex == 1)
            {
                listaTipo.IsEnabled = false;
                ListaServico();
            }
        }

        private void listaTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListaProduto();
        }
        public void ListaServico()
        {
            listaProduto.SelectServico();
        }

        public void ListaProduto()
        {
            string cdP = "6";
            try
            {
                cdP = listaTipo.SelectedValue.ToString();
            }
            catch
            {

            }
            listaProduto.SelectProduto(cdP);
        }

        private void botaoAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listaProduto.SelectedIndex;
                DataRowView row = (DataRowView)listaProduto.Items[index];
                listaOrc.Items.Add(row[1].ToString());
                int codigo = Convert.ToInt32(row[0]);
                cd.Add(codigo);
                if (listaProduto.Columns.Count > 3)
                {
                    nome.Add(row[1].ToString());
                    desc.Add(row[2].ToString());
                    if (row[3].ToString() != "Sem Imagem")
                        imagem.Add(row[3].ToString());
                    else
                        imagem.Add("");
                    tipo.Add(row[4].ToString());
                    tp.Add("Produto");
                    preco.Add(Convert.ToDouble(row[5]));
                    precoT.Add(Convert.ToDouble(row[5]));
                    precoM.Add(Convert.ToDouble(row[5]));
                }
                else
                {
                    nome.Add(row[1].ToString());
                    desc.Add(row[2].ToString());
                    tipo.Add("Serviço");
                    tp.Add("Serviço");
                    imagem.Add("Sem Imagem");
                    preco.Add(1);
                    precoT.Add(1);
                    precoM.Add(1);
                }
                quant.Add(1);
                larg.Add(1);
                alt.Add(1);
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não selecionou");
            }
        }

        public void PrecoMetro()
        {
            int x = listaOrc.SelectedIndex;
            if (tipo[x] != "Serviço")
                try
                {
                    campoPreco.Text = (Convert.ToDouble(campoLar.Text) *
                        Convert.ToDouble(campoAlt.Text) * precoM[x]).ToString();
                }
                catch
                {
                    campoPreco.Text = precoM[x].ToString();
                }
            else
                campoPreco.Text = preco[x].ToString();
        }

        private void campoQuant_TextChanged(object sender, TextChangedEventArgs e)
        {
            int x = listaOrc.SelectedIndex;
            if (campoQuant.Text != "" && listaOrc.SelectedIndex != -1)
                try { quant[x] = int.Parse(campoQuant.Text); }
                catch { campoQuant.Text = "0"; }
            CalcularPreco();
        }

        private void botaoDel_Click(object sender, RoutedEventArgs e)
        {
            if (listaOrc.SelectedIndex != -1)
            {
                int x = listaOrc.SelectedIndex;
                listaOrc.Items.RemoveAt(x);
                cd.RemoveAt(x);
                nome.RemoveAt(x);
                desc.RemoveAt(x);
                tipo.RemoveAt(x);
                imagem.RemoveAt(x);
                larg.RemoveAt(x);
                alt.RemoveAt(x);
                quant.RemoveAt(x);
                preco.RemoveAt(x);
                precoT.RemoveAt(x);
            }
        }

        private void botaoGerar_Click(object sender, RoutedEventArgs e)
        {
            if (op)
                InserirDado();
            else
                Limpar();
        }

        private void listaOrc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            check = true;
            if (listaOrc.SelectedIndex != -1)
            {
                campoQuant.IsEnabled = true;
                int x = listaOrc.SelectedIndex;
                if (tipo[x] == "Serviço")
                {
                    campoLar.IsEnabled = false;
                    campoAlt.IsEnabled = false;
                    campoPreco.IsEnabled = true;
                    groupBox2.Header = nome[x];
                    campoDesc.Text = desc[x];
                    campoTipo.Text = tipo[x];
                    campoImagem.Source = null;
                    campoLar.Value = larg[x];
                    campoAlt.Value = alt[x];
                    campoQuant.Text = quant[x].ToString();
                    campoPreco.Value = preco[x];
                }
                else
                {
                    campoLar.IsEnabled = true;
                    campoAlt.IsEnabled = true;
                    campoPreco.IsEnabled = false;
                    groupBox2.Header = nome[x];
                    campoDesc.Text = desc[x];
                    campoTipo.Text = tipo[x];
                    try
                    {
                        campoImagem.Source = new BitmapImage(new Uri(imagem[x]));
                    }
                    catch
                    {
                        campoImagem.Source = null;
                    }
                    campoLar.Value = larg[x];
                    campoAlt.Value = alt[x];
                    campoQuant.Text = quant[x].ToString();
                    campoPreco.Value = preco[x];
                }
            }
            check = false;
        }
        
        private void campoLar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int x = listaOrc.SelectedIndex;
            larg[x] = (double)campoLar.Value;
            PrecoMetro();
            CalcularPreco();
        }

        private void campoAlt_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int x = listaOrc.SelectedIndex;
            alt[x] = (double)campoAlt.Value;
            PrecoMetro();
            CalcularPreco();
        }

        private void campoPreco_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int x = listaOrc.SelectedIndex;
            if (x != -1 && tipo[x] == "Serviço" && campoPreco.Text != "")
            {
                try
                {
                    preco[x] = (double)campoPreco.Value;
                }
                catch
                {
                    campoPreco.Text = "0";
                }
            }
            CalcularPreco();
        }

        public void CalcularPreco()
        {
            if (!check)
            {
                int x = listaOrc.SelectedIndex;
                double alt = 0, larg = 0, p = 0;
                int quant = 0;
                if (campoAlt.Text != "" && campoLar.Text != "" &&
                    campoQuant.Text != "" && campoPreco.Text != "" &&
                    listaOrc.SelectedIndex != -1)
                {
                    quant = int.Parse(campoQuant.Text);
                    alt = (double)campoAlt.Value;
                    larg = (double)campoLar.Value;
                    p = (double)campoPreco.Value;
                    campoPrecoT.Text = (p * quant).ToString();
                    precoT[x] = double.Parse(campoPrecoT.Text);
                    try
                    {
                        double precoO = 0;
                        for (int i = 0; i < listaOrc.Items.Count; i++)
                        {
                            precoO += precoT[i];
                        }
                        campoPrecoO.Text = precoO.ToString();
                    }
                    catch
                    {
                        campoPrecoO.Text = "0";
                    }
                }
            }
        }

        public void Limpar()
        {

            using (var mConn = Connect.LiteString())
            {
                DataTable ds = new DataTable();
                mConn.Open();
                if (mConn.State == ConnectionState.Open)
                {
                    string cmdText = "delete from tbItemNota where idOrcamento = " + idO;
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                    cmd.ExecuteNonQuery();
                    AlterarDado();
                }
            }
        }

        public void AlterarDado()
        {
            int q = listaOrc.Items.Count;
            int x = 0;
            for (int b = 0; b < q; b++)
            {
                if (larg[b] == 0 || alt[b] == 0)
                {
                    x++;
                }
            }
            if (x == 0)
            {
                for (int i = 0; i < q; i++)
                {
                    long r = cd[i];
                    using (var mConn = Connect.LiteString())
                    {
                        DataTable ds = new DataTable();
                        mConn.Open();
                        if (mConn.State == ConnectionState.Open)
                        {
                            string cmdText = "insert into tbItemNota values (null,@id,@cdP,@cdS,@l, @a, @q, @p)";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@id", idO);
                            if (tipo[i] == "Serviço")
                            {
                                cmd.Parameters.AddWithValue("@cdS", r);
                                cmd.Parameters.AddWithValue("@cdP", null);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@cdP", r);
                                cmd.Parameters.AddWithValue("@cdS", null);
                            }
                            cmd.Parameters.AddWithValue("@l", larg[i]);
                            cmd.Parameters.AddWithValue("@a", alt[i]);
                            cmd.Parameters.AddWithValue("@q", quant[i]);
                            cmd.Parameters.AddWithValue("@p", preco[i]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    AlterarOrcamento();
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Medidas não podem ser 0");
            }
        }

        public void AlterarOrcamento()
        {
            using (var mConn = Connect.LiteString())
            {
                DataTable ds = new DataTable();
                mConn.Open();
                if (mConn.State == ConnectionState.Open)
                {
                    DateTime data = DateTime.Now.Date;
                    string cliente = listaCliente.SelectedValue.ToString();
                    string cmdText = "update tbOrcamento set dtModificacao = @dt, dtValidade = @dtV, idCliente = @id, observacoes = @ob where cdOrcamento = " + idO;
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                    cmd.Parameters.AddWithValue("@dt", data);
                    cmd.Parameters.AddWithValue("@dtV", campoData.SelectedDate);
                    cmd.Parameters.AddWithValue("@id", cliente);
                    cmd.Parameters.AddWithValue("@ob", campoObs.Text);
                    cmd.ExecuteNonQuery();
                }
                Close();
                Xceed.Wpf.Toolkit.MessageBox.Show("Orçamento alterado com sucesso");
            }
        }

        //Método Inserir Dado
        public void InserirDado()
        {
            ClasseOrcamento o = new ClasseOrcamento(cdUsuario);
            int q = listaOrc.Items.Count;
            int x = 0;
            double precoT = 0;

            using (var mConn = Connect.LiteString())
            {
                DataTable ds = new DataTable();
                mConn.Open();
                if (mConn.State == ConnectionState.Open)
                {
                    for (int i = 0; i < q; i++)
                    {
                        if (larg[i] == 0 || alt[i] == 0)
                        {
                            x++;
                        }
                        else
                        {
                            precoT += (preco[i] * quant[i]);
                        }
                    }
                    if (x == 0)
                    {
                        string cliente = listaCliente.SelectedValue.ToString();
                        DateTime data = DateTime.Now.Date;
                        string cmdText = "INSERT INTO tbOrcamento VALUES (null,@dt,@dt,@dtv,@obs,0,@login,@id)";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@dt", data);
                        cmd.Parameters.AddWithValue("@dtv", campoData.SelectedDate);
                        cmd.Parameters.AddWithValue("@obs", campoObs.Text);
                        cmd.Parameters.AddWithValue("@login", cdUsuario);
                        cmd.Parameters.AddWithValue("@id", cliente);
                        cmd.ExecuteNonQuery();
                        int index = Atualizar();
                        o.ArmazenarRelatorio(cd, nome, desc, tipo, imagem, larg, alt, quant, this.precoT, campoObs.Text, index, q);
                        Close();
                        Xceed.Wpf.Toolkit.MessageBox.Show("Orçamento gerado Com Sucesso!");
                        Historico.AdicionarHistorico(cdUsuario, "gerou", "um", "orçamento");
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Medidas não podem ser 0");
                    }
                }
            }
        }

        public int Atualizar()
        {
            int index = 0;
            dataGridView2.SelectUltimoOrcamento();
            DataRowView row = (DataRowView)dataGridView2.Items[0];
            index = Convert.ToInt32(row[0]);
            return index;
        }

        private void minimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void fechar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool _isMouseDown = false;
        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown && this.WindowState == WindowState.Maximized)
            {
                _isMouseDown = false;
                this.WindowState = WindowState.Normal;
            }
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            this.DragMove();
        }

        private void TitleBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

    }
}
