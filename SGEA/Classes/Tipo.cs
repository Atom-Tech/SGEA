using MySql.Data.MySqlClient;
using SGEA.Classes;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Windows;

namespace SGEA
{
    public class Tipo
    {
        private int cdUsuario;

        public Tipo(int cdUsuario)
        {
            this.cdUsuario = cdUsuario;
        }

        /// <summary>
        /// Método para Inserir Tipo
        /// </summary>
        /// <param name="addTipo">Nome do Tipo</param>
        public bool InserirTipo(string addTipo)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (addTipo != "")
                        {
                            string cmdText = "INSERT INTO tbTipo (nmTipo) VALUES (@tipo)";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@tipo", addTipo);
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Tipo Inserido Com Sucesso!");
                            Historico.AdicionarHistorico(cdUsuario, "inseriu", "um", "tipo de produto");
                            return true;
                        }
                        else
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                        }
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter mais de um tipo com o mesmo nome");
            }            
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        /// <summary>
        /// Método para Deletar o Tipo
        /// </summary>
        /// <param name="c1">Código do Tipo</param>
        public void DeletarTipo(int c1)
        {
            using (var mConn = Connect.LiteString())
            {
                mConn.Open();
                if (mConn.State == ConnectionState.Open)
                {
                    MessageBoxResult dialogResult = MessageBox.Show("Você vai deletar um tipo de produto. Tem certeza?", "Aviso", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        Verificar(c1);
                        string cmdText = "delete from tbTipo where cdTipo = @cd";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cd", c1);
                        cmd.ExecuteNonQuery();
                        Xceed.Wpf.Toolkit.MessageBox.Show("Tipo deletado");
                        Historico.AdicionarHistorico(cdUsuario, "deletou", "o", "tipo de produto", c1);
                    }
                }
            }
        }

        /// <summary>
        /// Todos os produtos com o tipo deletado ficam "Sem Tipo"
        /// </summary>
        /// <param name="c1">Código do Tipo</param>
        public void Verificar(int c1)
        {
            using (var mConn = Connect.LiteString())
            {
                mConn.Open();
                if (mConn.State == ConnectionState.Open)
                {
                    string cmdText = "update tbProduto set tpProduto = 1 where tpProduto = @cd";
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                    cmd.Parameters.AddWithValue("@cd", c1);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
