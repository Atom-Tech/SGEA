using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Input;

namespace SGEA.Janelas.Orcamento.Projeto
{
    /// <summary>
    /// Interaction logic for Obs.xaml
    /// </summary>
    public partial class Obs : Window
    {
        private int cd;
        public Obs()
        {
            InitializeComponent();
        }
        public Obs(int cd)
        {
            InitializeComponent();
            this.cd = cd;
        }
        public Obs(int cd, string obs)
        {
            InitializeComponent();
            this.cd = cd;
            campoObs.Text = obs;
        }

        private void botaoAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "update tbProjeto set observacao = @obs where cdProjeto =  '" + cd + "'";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@obs", campoObs.Text);
                        cmd.ExecuteNonQuery();
                        Close();
                    }

                }
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
        
        private bool _isMouseDown;

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
