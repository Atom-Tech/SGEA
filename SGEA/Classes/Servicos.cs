using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System;
using System.Data.SQLite;
using SGEA.Classes;

namespace SGEA.Forms
{
    public class Servicos
    {
        private int cdUsuario;

        public Servicos(int cdUsuario)
        {
            this.cdUsuario = cdUsuario;
        }

        /// <summary>
        /// Método para Cadastrar Serviço
        /// </summary>
        /// <param name="nome">Nome</param>
        /// <param name="desc">Descrição</param>
        public bool CadastrarServicos(string nome, string desc)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (nome != "")
                        {
                            string cmdText = "INSERT INTO tbServico VALUES (null,@nome,@desc)";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@nome", nome); //Insere nome
                            cmd.Parameters.AddWithValue("@desc", desc); //Insere nome
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Serviço Inserido Com Sucesso!");
                            Historico.AdicionarHistorico(cdUsuario, "inseriu", "um", "serviço");
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
                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter mais de um serviço com o mesmo nome");
            }            
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        public bool AlterarServicos(long id, string nome, string desc)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (nome != "")
                        {
                            string cmdText = "update tbServico set nmServico = @nome, dsServico = @desc where cdServico = '"+id+"'";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@nome", nome); //Insere nome
                            cmd.Parameters.AddWithValue("@desc", desc); //Insere desc
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Serviço Alterado Com Sucesso!");
                            Historico.AdicionarHistorico(cdUsuario, "alterou", "o", "serviço", Convert.ToInt32(id));
                            return true;
                        }
                        else
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Nome não pode ser nulo");
                        }
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("UNIQUE"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter mais de um serviço com o mesmo nome");
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        /// <summary>
        /// Método para Deletar Serviço
        /// </summary>
        /// <param name="cd">Código</param>
        public void DeletarServicos(long cd)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "delete from tbServico where cdServico = '" + cd + "'";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.ExecuteNonQuery();
                        Xceed.Wpf.Toolkit.MessageBox.Show("Serviço Deletado Com Sucesso!");
                        Historico.AdicionarHistorico(cdUsuario, "deletou", "o", "serviço", Convert.ToInt32(cd));
                    }
                }
            }
            catch 
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Há orçamento cadastrado com esse serviço");
            }

        }
    }
}
