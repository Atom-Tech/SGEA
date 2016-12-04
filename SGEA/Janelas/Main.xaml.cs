using RegawMOD.Android;
using SGEA.Janelas;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SGEA
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private Pages.Orcamento orc;
        private Pages.Visita vis;
        private Pages.Projeto proj;
        private Pages.Cliente cliente;
        private bool _isMouseDown, not = false;
        private string nmUsuario, email, nome, cep, bairro, rua, num, telFixo, telCel, grupo, sexo;
        private int cdUsuario, janela = 0, notificacoes = 0;
        private Dictionary<int, string> listaOrc = new Dictionary<int, string>();
        private Dictionary<int, string> listaVisita = new Dictionary<int, string>();
        private Dictionary<int, string> listaProjeto = new Dictionary<int, string>();

        public Main(string login, string nmUsuario, int cdUsuario, string email, string nome, string cep,
            string bairro, string rua, string num, string telFixo, string telCel, string sexo)
        {
            InitializeComponent();
            grupo = login;
            this.nmUsuario = nmUsuario;
            this.email = email;
            this.cdUsuario = cdUsuario;
            this.nome = nome;
            this.cep = cep;
            this.bairro = bairro;
            this.rua = rua;
            this.num = num;
            this.telFixo = telFixo;
            this.telCel = telCel;
            this.sexo = sexo;
            orc = new Pages.Orcamento(cdUsuario, grupo, this);
            vis = new Pages.Visita(cdUsuario, grupo, this);
            proj = new Pages.Projeto(cdUsuario, this);
            cliente = new Pages.Cliente(cdUsuario, grupo, this);
            GerarBackup(false);
            ContentRendered += Main_ContentRendered;
            Activated += Main_Activated;
        }

        private void Main_ContentRendered(object sender, EventArgs e)
        {
            MainFrame.Navigate(new Pages.Main(cdUsuario, nmUsuario, grupo));
            janela = 0;
            campoLogin.Text = nmUsuario;
            campoGrupo.Text = grupo;
            if (grupo == "Funcionário")
            {
                historico.Visibility = Visibility.Collapsed;
                menuUsuario.Visibility = Visibility.Collapsed;
                backupI.Visibility = Visibility.Collapsed;
                backupI.Visibility = Visibility.Collapsed;
            }
            ExibirNotificacao(0, true);
        }

        public void OrcamentoInvalido()
        {
            listaOrc.Clear();
            dataGrid2.SelectNotificacaoProjetos();
            if (dataGrid2.Items.Count > 0)
            {
                for (int i = 0; i < dataGrid2.Items.Count; i++)
                {
                    DataRowView row = (DataRowView)dataGrid2.Items[i];
                    listaOrc.Add(Convert.ToInt32(row[0]), row[1].ToString());
                }
            }
        }

        public void GerarBackup(bool vf, string pasta = "")
        {
            if (pasta == "")
                pasta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string caminho = pasta + "\\SGEA\\Backup\\";
            string data = DateTime.Today.ToShortDateString().Replace('/', '-');
            Directory.CreateDirectory(caminho + data);
            bool g = true;
            if (!vf)
            {
                if ((caminho + data).IsDirectoryEmpty()) g = true;
                else g = false;
            }
            if (g)
            {
                Directory.CreateDirectory(caminho + data);
                string backup = caminho + data + "\\" + data + " "
                    + DateTime.Now.TimeOfDay.ToString(@"hh\-mm") + ".db";
                File.Copy(Connect.Banco, backup, vf);
            }
        }

        private void Main_Activated(object sender, EventArgs e)
        {
            campoLogin.Text = nmUsuario;
        }

        private void voltar_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Login l = new Login();
            l.Closed += (s, args) => Close();
            l.Show();
        }

        private void mServicos_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 3)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio - Serviços";
                MainFrame.Navigate(new SGEA.Pages.Servico(cdUsuario, grupo));
                janela = 3;
                MudarCor(janela);
            }
        }

        private void MudarCor(int janela)
        {
            menuAgenda.Background = Brushes.Transparent;
            mProdutos.Background = Brushes.Transparent;
            mServicos.Background = Brushes.Transparent;
            menuOrc.Background = Brushes.Transparent;
            menuCliente.Background = Brushes.Transparent;
            menuForn.Background = Brushes.Transparent;
            mRelatorio.Background = Brushes.Transparent;
            mProjeto.Background = Brushes.Transparent;
            mPerfis.Background = Brushes.Transparent;
            menuUsuario.Background = Brushes.Transparent;
            historico.Background = Brushes.Transparent;
            Color c = Color.FromArgb(255, 190, 230, 253);
            switch (janela)
            {
                case 1:
                    menuAgenda.Background = new SolidColorBrush(c);
                    break;
                case 2:
                    mProdutos.Background = new SolidColorBrush(c);
                    break;
                case 3:
                    mServicos.Background = new SolidColorBrush(c);
                    break;
                case 4:
                    menuUsuario.Background = new SolidColorBrush(c);
                    break;
                case 5:
                    menuCliente.Background = new SolidColorBrush(c);
                    break;
                case 6:
                    menuOrc.Background = new SolidColorBrush(c);
                    break;
                case 7:
                    menuForn.Background = new SolidColorBrush(c);
                    break;
                case 8:
                    mPerfis.Background = new SolidColorBrush(c);
                    break;
                case 9:
                    mProjeto.Background = new SolidColorBrush(c);
                    break;
                case 10:
                    mRelatorio.Background = new SolidColorBrush(c);
                    break;
                case 11:
                    historico.Background = new SolidColorBrush(c);
                    break;
            }
        }

        public void VerificarCliente(string cd)
        {
            menuCliente.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            cliente.Notificacao(cd);
        }

        private void mProdutos_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 2)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio - Produtos";
                MainFrame.Navigate(new SGEA.Pages.Produto(cdUsuario, grupo));
                janela = 2;
                MudarCor(janela);
            }
        }

        private void minimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void fechar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

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

        private void menuCliente_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 5)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio - Clientes";
                MainFrame.Navigate(cliente);
                janela = 5;
                MudarCor(janela);
            }
        }

        private void menuOrc_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 6)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio - Orçamentos";
                MainFrame.Navigate(orc);
                janela = 6;
                MudarCor(janela);
            }
        }

        private void menuForn_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 7)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio - Fornecedores";
                MainFrame.Navigate(new SGEA.Pages.Fornecedor(cdUsuario, grupo));
                janela = 7;
                MudarCor(janela);
            }
        }

        private void mPerfis_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 8)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio - Perfis";
                MainFrame.Navigate(new SGEA.Pages.Perfis(cdUsuario, grupo));
                janela = 8;
                MudarCor(janela);
            }
        }

        private void mProjeto_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 9)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio - Projetos";
                MainFrame.Navigate(proj);
                janela = 9;
                MudarCor(janela);
            }
        }

        private void mRelatorio_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 10)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio - Relatórios";
                MainFrame.Navigate(new SGEA.Pages.Relatorio());
                janela = 10;
                MudarCor(janela);
            }
        }

        private void cel_Click(object sender, RoutedEventArgs e)
        {
            string c = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SGEA\\sgea.db";
            string imagens = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SGEA\\Imagens";
            if (File.Exists(c))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Certifique-se que o seu celular está com o modo depuração ligado!");
                AndroidController a = AndroidController.Instance;
                if (a.HasConnectedDevices)
                {
                    c = "\"" + c + "\"";
                    imagens = "\"" + imagens + "\"";
                    Device d = a.GetConnectedDevice(a.ConnectedDevices[0]);
                    for (int i = 0; i < a.ConnectedDevices.Count; i++)
                    {
                        d = a.GetConnectedDevice(a.ConnectedDevices[i]);
                        if (!d.SerialNumber.Contains("emulator")) break;
                    }
                    var ad = Adb.FormAdbShellCommand(d, false, "rmdir -r sdcard/SGEA");
                    Adb.ExecuteAdbCommand(ad);
                    ad = Adb.FormAdbShellCommand(d, false, "mkdir sdcard/SGEA");
                    Adb.ExecuteAdbCommand(ad);
                    ad = Adb.FormAdbShellCommand(d, false, "mkdir sdcard/SGEA/Imagens");
                    Adb.ExecuteAdbCommand(ad);
                    bool b = d.PushFile(c, "sdcard/SGEA", 100000);
                    bool f = d.PushFile(imagens, "sdcard/SGEA/Imagens/", 100000);
                    if (b && f)
                        Xceed.Wpf.Toolkit.MessageBox.Show("Banco de Dados foi transferido com sucesso");
                    else
                        Xceed.Wpf.Toolkit.MessageBox.Show("Banco de Dados não foi transferido");
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Não há um celular Android conectado por USB");
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("O banco de dados não existe. \nPor favor, entre em contato conosco");
            }
        }

        private void config_Click(object sender, RoutedEventArgs e)
        {
            VerificarConta();
            Janelas.Usuario.Alterar u = new Janelas.Usuario.Alterar(nmUsuario, email, nome, cep, bairro, rua, num, telFixo, telCel, grupo, sexo, 0, cdUsuario);
            u.ShowDialog();
            if (u.Retornar())
            {
                campoLogin.Text = u.NovoLogin();
                nmUsuario = u.NovoLogin();
                dataGrid4.DataContext =
                    Connect.LiteConnection("select * from tbUsuario where login = '" + nmUsuario + "'");
                DataRowView row = dataGrid4.SelectFirstRow();
                nome = row[1].ToString();
                email = row[4].ToString();
                sexo = row[5].ToString();
                grupo = row[6].ToString();
                cep = row[7].ToString();
                bairro = row[8].ToString();
                rua = row[9].ToString();
                num = row[10].ToString();
                telFixo = row[11].ToString();
                telCel = row[12].ToString();
            }
        }

        private void backupG_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string caminho = dialog.SelectedPath;
                GerarBackup(true, caminho);
                Xceed.Wpf.Toolkit.MessageBox.Show("Backup gerado com sucesso!");
            }
        }

        private void backupI_Click(object sender, RoutedEventArgs e)
        {
            Backup b = new Backup();
            b.ShowDialog();
            MainFrame.Navigate(new Pages.Main(cdUsuario, nmUsuario, grupo));
        }

        public void MostrarNotificacao()
        {
            if (!not)
            {
                ((Storyboard)this.Resources["Not"]).Begin(this);
                not = true;
            }
            else
            {
                ((Storyboard)this.Resources["ReNot"]).Begin(this);
                not = false;
            }
        }

        private void historico_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 11)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio - Histórico";
                MainFrame.Navigate(new SGEA.Pages.Historico());
                janela = 11;
                MudarCor(janela);
            }
        }

        private void border_Click(object sender, RoutedEventArgs e)
        {
            MostrarNotificacao();
        }

        private void fMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    if (grupo != "Funcionário")
                        menuUsuario.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    else
                        Xceed.Wpf.Toolkit.MessageBox.Show("Você não tem permissão");
                    break;
                case Key.F2:
                    menuCliente.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.F3:
                    menuAgenda.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.F4:
                    menuForn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.F5:
                    mPerfis.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.F6:
                    mProdutos.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.F7:
                    mServicos.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.F8:
                    menuOrc.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.F9:
                    mProjeto.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.System:
                    if (e.SystemKey == Key.F10)
                    {
                        mRelatorio.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        e.Handled = true;
                    }
                    break;
                case Key.F11:
                    historico.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.F12:
                    cel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
            }
        }

        private void todos_Click(object sender, RoutedEventArgs e)
        {
            ExibirNotificacao(0);
        }

        private void vis_Click(object sender, RoutedEventArgs e)
        {
            ExibirNotificacao(1);
        }

        private void orc_Click(object sender, RoutedEventArgs e)
        {
            ExibirNotificacao(2);
        }

        private void proj_Click(object sender, RoutedEventArgs e)
        {
            ExibirNotificacao(3);
        }

        private void TitleBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        private void menuAgenda_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 1)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio - Agendas";
                MainFrame.Navigate(vis);
                janela = 1;
                MudarCor(janela);
            }
        }

        private void menuUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 4)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio - Usuários";
                MainFrame.Navigate(new SGEA.Pages.Usuario(cdUsuario, nmUsuario));
                janela = 4;
                MudarCor(janela);
            }
        }

        private void fMain_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var controle = (Visual)e.OriginalSource;
            var check = retNote.GetChildren();
            var check2 = border.GetChildren();
            bool existe = retNote == controle;
            bool existe2 = border == controle;
            foreach (var c in check)
            {
                if (controle == c)
                {
                    existe = true;
                    break;
                }
            }
            foreach (var c in check2)
            {
                if (controle == c)
                {
                    existe2 = true;
                    break;
                }
            }
            if (!existe && not && !existe2) MostrarNotificacao();
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            if (janela != 0)
            {
                titulo.Content = "Sistema de Gerenciamento de Esquadria de Aluminio";
                MainFrame.Navigate(new SGEA.Pages.Main(cdUsuario, nmUsuario, grupo));
                janela = 0;
                MudarCor(0);
            }
        }

        private void suaConta_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        public void VerificarConta()
        {
            try
            {
                dataGrid.SelectDadosUsuario(nmUsuario);
                if (dataGrid.Items.Count == 1)
                {
                    DataRowView row = (DataRowView)dataGrid.Items[0];
                    nmUsuario = row[0].ToString();
                    nome = row[1].ToString();
                    cep = row[2].ToString();
                    bairro = row[3].ToString();
                    rua = row[4].ToString();
                    telFixo = row[5].ToString();
                    telCel = row[6].ToString();
                }
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        public void VisitasAntigas()
        {
            listaVisita.Clear();
            dataGrid1.SelectVisitasAntigas();
            for (int i = 0; i < dataGrid1.Items.Count; i++)
            {
                DataRowView row = (DataRowView)dataGrid1.Items[i];
                listaVisita.Add(Convert.ToInt32(row[0]), row[1].ToString());
            }
        }

        public void VerificarProjetos()
        {
            listaProjeto.Clear();
            dataGrid3.SelectProjetoAtrasado();
            for (int i = 0; i < dataGrid3.Items.Count; i++)
            {
                DataRowView row = (DataRowView)dataGrid3.Items[i];
                listaProjeto.Add(Convert.ToInt32(row[0]), row[1].ToString());
            }
        }

        public void NotificacaoOrcamento()
        {
            OrcamentoInvalido();
            foreach (var o in listaOrc)
            {
                Border b = new Border()
                {
                    Height = 70,
                    Margin = new Thickness(10, 10, 10, 20),
                    BorderBrush = Brushes.White,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10)
                };
                StackPanel painel1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                };
                var imagem = new BitmapImage(new Uri(@"/SGEA;component/img/circuloorcamento.png", UriKind.Relative));
                Image notMenu = new Image()
                {
                    Height = 32,
                    Width = 32,
                    Stretch = Stretch.Fill
                };
                notMenu.Source = imagem;
                Rectangle r1 = new Rectangle()
                {
                    Stroke = Brushes.White
                };
                Rectangle r2 = new Rectangle()
                {
                    Stroke = Brushes.White
                };
                TextBlock t1 = new TextBlock()
                {
                    Text = "Orçamento agendado para o dia " + DateTime.Parse(o.Value).ToShortDateString() + " está atrasado",
                    Margin = new Thickness(3, 3, 0, 0),
                    Padding = new Thickness(0, 0, 3, 0),
                    TextWrapping = TextWrapping.Wrap,
                    Width = 150
                };
                StackPanel painel2 = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(3, 0, 0, 0)
                };
                Button ver = new Button()
                {
                    Content = "Verificar",
                    Style = (Style)FindResource("ButtonStyle2"),
                    Name = "verOrc" + o.Key,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 2, 0, 0),
                    Template = (ControlTemplate)FindResource("ButtonTop")
                };
                Button con = new Button()
                {
                    Content = "Confirmar",
                    Style = (Style)FindResource("ButtonStyle2"),
                    Name = "conOrc" + o.Key,
                    VerticalAlignment = VerticalAlignment.Center,
                    Template = (ControlTemplate)FindResource("ButtonCenter")
                };
                Button del = new Button()
                {
                    Content = "Deletar",
                    Style = (Style)FindResource("ButtonStyle2"),
                    Name = "delOrc" + o.Key,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Template = (ControlTemplate)FindResource("ButtonBottom")
                };
                ver.Click += Ver_Click;
                del.Click += Del_Click;
                con.Click += Con_Click;
                painel2.Children.Add(ver);
                painel2.Children.Add(con);
                painel2.Children.Add(del);
                painel1.Children.Add(notMenu);
                painel1.Children.Add(r1);
                painel1.Children.Add(t1);
                painel1.Children.Add(r2);
                painel1.Children.Add(painel2);
                b.Child = painel1;
                painel.Children.Add(b);
            }

        }

        public void NotificacaoAgenda()
        {
            VisitasAntigas();
            foreach (var v in listaVisita)
            {
                Border b = new Border()
                {
                    Height = 70,
                    Margin = new Thickness(10, 10, 10, 20),
                    BorderBrush = Brushes.White,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10)
                };
                StackPanel painel1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                };
                var imagem = new BitmapImage(new Uri(@"/SGEA;component/img/circuloAgenda.png", UriKind.Relative));
                Image notMenu = new Image()
                {
                    Height = 32,
                    Width = 32,
                    Stretch = Stretch.Fill
                };
                notMenu.Source = imagem;
                Rectangle r1 = new Rectangle()
                {
                    Stroke = Brushes.White
                };
                Rectangle r2 = new Rectangle()
                {
                    Stroke = Brushes.White
                };
                TextBlock t1 = new TextBlock()
                {
                    Text = "Visita agendada para o dia " + DateTime.Parse(v.Value).ToShortDateString() + " está atrasada",
                    Margin = new Thickness(3, 3, 0, 0),
                    Padding = new Thickness(0, 0, 3, 0),
                    TextWrapping = TextWrapping.Wrap,
                    Width = 150
                };
                StackPanel painel2 = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(3, 0, 0, 0)
                };
                Button ver = new Button()
                {
                    Content = "Verificar",
                    Style = (Style)FindResource("ButtonStyle2"),
                    Name = "verVis" + v.Key,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 2, 0, 0),
                    Template = (ControlTemplate)FindResource("ButtonTop")
                };
                Button con = new Button()
                {
                    Content = "Concluir",
                    Style = (Style)FindResource("ButtonStyle2"),
                    Name = "conVis" + v.Key,
                    VerticalAlignment = VerticalAlignment.Center,
                    Template = (ControlTemplate)FindResource("ButtonCenter")
                };
                Button del = new Button()
                {
                    Content = "Deletar",
                    Style = (Style)FindResource("ButtonStyle2"),
                    Name = "delVis" + v.Key,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Template = (ControlTemplate)FindResource("ButtonBottom")
                };
                ver.Click += Ver_Click;
                del.Click += Del_Click;
                con.Click += Con_Click;
                painel2.Children.Add(ver);
                painel2.Children.Add(con);
                painel2.Children.Add(del);
                painel1.Children.Add(notMenu);
                painel1.Children.Add(r1);
                painel1.Children.Add(t1);
                painel1.Children.Add(r2);
                painel1.Children.Add(painel2);
                b.Child = painel1;
                painel.Children.Add(b);
            }

        }

        private void Con_Click(object sender, RoutedEventArgs e)
        {
            Notificacao(true, sender, true);
        }

        public void NotificacaoProjeto()
        {
            VerificarProjetos();
            foreach (var p in listaProjeto)
            {
                Border b = new Border()
                {
                    Height = 50,
                    Margin = new Thickness(10, 10, 10, 20),
                    BorderBrush = Brushes.White,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10)
                };
                StackPanel painel1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                };
                var imagem = new BitmapImage(new Uri(@"/SGEA;component/img/circuloProjeto.png", UriKind.Relative));
                Image notMenu = new Image()
                {
                    Height = 32,
                    Width = 32,
                    Stretch = Stretch.Fill
                };
                notMenu.Source = imagem;
                Rectangle r1 = new Rectangle()
                {
                    Stroke = Brushes.White
                };
                Rectangle r2 = new Rectangle()
                {
                    Stroke = Brushes.White
                };
                TextBlock t1 = new TextBlock()
                {
                    Text = "Projeto " + p.Value + " está atrasado",
                    Margin = new Thickness(3, 3, 0, 0),
                    Padding = new Thickness(0, 0, 3, 0),
                    TextWrapping = TextWrapping.Wrap,
                    Width = 150
                };
                StackPanel painel2 = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(3, 0, 0, 0)
                };
                Button ver = new Button()
                {
                    Content = "Verificar",
                    Style = (Style)FindResource("ButtonStyle2"),
                    Name = "verPro" + p.Key,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 2, 0, 0),
                    Template = (ControlTemplate)FindResource("ButtonTop")
                };
                Button del = new Button()
                {
                    Content = "Concluir",
                    Style = (Style)FindResource("ButtonStyle2"),
                    Name = "conPro" + p.Key,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Template = (ControlTemplate)FindResource("ButtonBottom")
                };
                ver.Click += Ver_Click;
                del.Click += Del_Click;
                painel2.Children.Add(ver);
                painel2.Children.Add(del);
                painel1.Children.Add(notMenu);
                painel1.Children.Add(r1);
                painel1.Children.Add(t1);
                painel1.Children.Add(r2);
                painel1.Children.Add(painel2);
                b.Child = painel1;
                painel.Children.Add(b);
            }

        }

        public void ExibirNotificacao(int note = 0, bool vf = false)
        {
            painel.Children.Clear();
            notificacoes = 0;
            switch (note)
            {
                case 0:
                    opcoes.Content = "Todos";
                    NotificacaoOrcamento();
                    NotificacaoAgenda();
                    NotificacaoProjeto();
                    break;
                case 1:
                    opcoes.Content = "Visitas";
                    NotificacaoAgenda();
                    break;
                case 2:
                    opcoes.Content = "Orçamentos";
                    NotificacaoOrcamento();
                    break;
                case 3:
                    opcoes.Content = "Projetos";
                    NotificacaoProjeto();
                    break;
            }
            foreach (object obj in painel.Children)
            {
                if (obj.GetType() == typeof(Border))
                {
                    notificacoes++;
                }
            }
            Storyboard s = (Storyboard)this.Resources["NoteBlink"];
            s.Stop();
            if (notificacoes > 0)
            {
                s.Begin();
                if (vf)
                {
                    Storyboard n = (Storyboard)this.Resources["Balloon"];
                    n.Begin();
                }
            }
            border.Content = notificacoes.ToString();
        }

        private void Ver_Click(object sender, RoutedEventArgs e)
        {
            if (grupo != "Funcionário")
                Notificacao(false, sender);
            else
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não tem permissão");
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (grupo != "Funcionário")
                Notificacao(true, sender);
            else
                Xceed.Wpf.Toolkit.MessageBox.Show("Você não tem permissão");
        }

        public void Notificacao(bool vf, object sender, bool c = false)
        {
            MostrarNotificacao();
            string nome = ((Control)sender).Name;
            janela = 0;
            switch (nome.Substring(3, 3))
            {
                case "Orc":
                    menuOrc.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    orc.Notificacao(nome.Substring(6), vf, c);
                    break;
                case "Vis":
                    menuAgenda.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    vis.Notificacao(nome.Substring(6), vf, c);
                    break;
                case "Pro":
                    mProjeto.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    proj.Notificacao(nome.Substring(6), vf);
                    break;
            }

        }

        private void Help_OnClick(object sender, RoutedEventArgs e)
        {
            Contato c = new Contato();
            c.ShowDialog();
        }
    }
}
