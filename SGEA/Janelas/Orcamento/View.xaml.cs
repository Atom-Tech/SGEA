using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Data.SQLite;

namespace SGEA.Janelas.Orcamento
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : Window
    {
        private int cd;

        public View()
        {
            InitializeComponent();
        }


        public View(int cd, string t)
        {
            InitializeComponent();
            this.cd = cd;
            ContentRendered += View_ContentRendered;
            titulo.Content = "Verificar " + t;
        }

        private void View_ContentRendered(object sender, EventArgs e)
        {
            Atualizar();
        }

        public void Atualizar()
        {
            try
            {
                listaOrcP.DataContext = Connect.LiteConnection("select nmProduto 'Produto', dsProduto 'Descrição', nmTipo 'Tipo'," +
                     "imagem, largura 'Largura', altura 'Altura', quant 'Quantidade', preco 'Preco m²', precoU 'Preço Unitário', precoU*quant 'Preço Total', cdItemNota" +
                     " from tbOrcamento, tbItemNota, tbProduto, tbTipo where cdOrcamento = " + cd + " and cdOrcamento = idOrcamento and cdProduto = idProduto and tpProduto = cdTipo");
                listaOrcP.Columns[3].Visibility = Visibility.Collapsed;
                listaOrcP.Columns[10].Visibility = Visibility.Collapsed;
                listaOrcS.DataContext = Connect.LiteConnection("select nmServico 'Serviço', dsServico 'Descrição', " +
      " quant 'Quantidade', precoU 'Preço Unitário', precoU*quant 'Preço Total', cdItemNota" +
      " from tbOrcamento, tbItemNota, tbServico where cdOrcamento = " + cd + " and cdOrcamento = idOrcamento and cdServico = idServico ");
                listaOrcS.Columns[5].Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
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
