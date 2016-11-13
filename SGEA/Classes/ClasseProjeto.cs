using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Data.SQLite;
using SGEA.Classes;

namespace SGEA.Forms
{
    public class ClasseProjeto
    {
        private int cdUsuario;

        public ClasseProjeto (int cdUsuario)
        {
            this.cdUsuario = cdUsuario;
        }

        public void GerarProjeto(int id, string nome, DateTime data)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "insert into tbProjeto values (null,@nm,date('now'), @dt, null,null,0,@id)";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@nm", nome);
                        cmd.Parameters.AddWithValue("@dt", data);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        Historico.AdicionarHistorico(cdUsuario, "gerou", "um", "projeto");
                    }
                }
            }
            
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        public void ConcluirProjeto(int cd)
        {
            try
            {

                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "update tbProjeto set idExecucao = 1, dtEntrega = @dt where cdProjeto =  '" + cd + "'";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@dt", DateTime.Today);
                        cmd.ExecuteNonQuery();
                        Historico.AdicionarHistorico(cdUsuario, "concluiu", "o", "projeto",cd);
                    }
                }
            }
            
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }
    }
}
