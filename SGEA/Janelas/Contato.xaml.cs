using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SGEA.Janelas
{
    /// <summary>
    /// Interaction logic for Contato.xaml
    /// </summary>
    public partial class Contato : Window
    {
        private bool _isMouseDown = false;

        public Contato()
        {
            InitializeComponent();
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

        private void TitleBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            this.DragMove();
        }

        private void botaoEnviar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = campoEmail.Text;
                string nome = campoNome.Text;
                string comentario = campoCom.Text;
                string assunto = campoAssunto.Text;
                if (Email.EmailValido(email))
                {
                    if (nome != "" && comentario != "" && assunto != "")
                    {
                        Email.Contato("atomteccompany@gmail.com", "Atom Technology Company",
                            comentario, assunto, nome, email);
                        Xceed.Wpf.Toolkit.MessageBox.Show("Email enviado com sucesso!");
                        Close();
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("E-Mail inválido");
                }
            }
            catch (SmtpException)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não tem internet");
            }
        }
    }
}
