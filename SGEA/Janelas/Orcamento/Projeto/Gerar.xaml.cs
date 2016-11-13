using SGEA.Forms;
using System;
using System.Windows;
using System.Windows.Input;

namespace SGEA.Janelas.Orcamento.Projeto
{
    /// <summary>
    /// Interaction logic for Gerar.xaml
    /// </summary>
    public partial class Gerar : Window
    {
        private int cd, cdUsuario;
        public Gerar(int cdUsuario,  int cd)
        {
            InitializeComponent();
            this.cdUsuario = cdUsuario;
            this.cd = cd;
            ContentRendered += Gerar_ContentRendered;
        }

        private void Gerar_ContentRendered(object sender, EventArgs e)
        {
            campoData.Text = DateTime.Now.Date.ToString();
            campoData.DisplayDateStart = DateTime.Today;
        }

        private void botaoConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (campoNome.Text != "")
            {
                ClasseProjeto p = new ClasseProjeto(cdUsuario);
                p.GerarProjeto(cd, campoNome.Text, campoData.SelectedDate.Value.Date);
                Close();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Nome não pode ser nulo");
            }
        }
        private void minimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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
