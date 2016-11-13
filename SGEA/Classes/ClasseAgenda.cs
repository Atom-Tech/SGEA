using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using SGEA;
using System;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Data.SQLite;
using SGEA.Classes;

namespace SGEA
{
    public class ClasseAgenda
    {
        private int cd;

        public ClasseAgenda(int cd)
        {
            this.cd = cd;
        }

        /// <summary>
        /// Método para Agendar Visita
        /// </summary>
        /// <param name="data">Data da Visita</param>
        /// <param name="hora">Horário da Visita</param>
        /// <param name="local">Local da Visita</param>
        /// <param name="desc">Descrição da Visita</param>
        /// <param name="cliente">Nome do Cliente</param>
        public bool AgendarVisita(DateTime data, TimeSpan hora, string local, string desc, string cliente, string obs)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        if (cliente != "" && local != "")
                        {
                            string cmdText = "INSERT INTO tbAgenda VALUES (null,@dt, @hr,@local,@ds,@nm,0,@obs)";
                            SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                            cmd.Parameters.AddWithValue("@dt", data);
                            cmd.Parameters.AddWithValue("@hr", hora);
                            cmd.Parameters.AddWithValue("@local", local);
                            cmd.Parameters.AddWithValue("@ds", desc);
                            cmd.Parameters.AddWithValue("@nm", cliente);
                            cmd.Parameters.AddWithValue("@obs", obs);
                            cmd.ExecuteNonQuery();
                            Xceed.Wpf.Toolkit.MessageBox.Show("Visita Agendada Com Sucesso!");
                            Historico.AdicionarHistorico(cd, "agendou", "uma", "visita");
                            return true;
                        }
                        else
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
            return false;
        }

        /// <summary>
        /// Método para Alterar Visita
        /// </summary>
        /// <param name="cd">Código da Visita</param>
        /// <param name="data">Data da Visita</param>
        /// <param name="hora">Horário da Visita</param>
        /// <param name="cliente">Nome do Cliente</param>
        /// <param name="local">Local da Visita</param>
        /// <param name="desc">Descrição da Visita</param>
        public bool AlterarVisita(int cd, DateTime data, TimeSpan hora, string cliente, string local, string desc, string obs)
        {
            using (var mConn = Connect.LiteString())
            {
                mConn.Open();
                if (mConn.State == ConnectionState.Open)
                {
                    if (cliente != "" && local != "")
                    {
                        string cmdText = "update tbAgenda set dtVisita = @dt, hrVisita = @hr, localVisita = @local, dsVisita = @ds, idCliente = @nm, observacao = @obs where cdVisita = @cd";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@nm", cliente);
                        cmd.Parameters.AddWithValue("@cd", cd);
                        cmd.Parameters.AddWithValue("@ds", desc);
                        cmd.Parameters.AddWithValue("@hr", hora);
                        cmd.Parameters.AddWithValue("@dt", data);
                        cmd.Parameters.AddWithValue("@local", local);
                        cmd.Parameters.AddWithValue("@obs", obs);
                        cmd.ExecuteNonQuery();
                        Xceed.Wpf.Toolkit.MessageBox.Show("Visita Alterada Com Sucesso!");
                        Historico.AdicionarHistorico(this.cd, "alterou", "a", "visita", cd);
                        return true;
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Não pode ter campos vazios");
                    }
                }
            }
            return false;
        }
        
        public void ConcluirVisita(int index)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "update tbAgenda set idExecucao = 1 where cdVisita = @cd";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cd", index);
                        cmd.ExecuteNonQuery();
                        Xceed.Wpf.Toolkit.MessageBox.Show("Visita concluída com sucesso");
                        Historico.AdicionarHistorico(cd, "concluiu", "a", "visita", index);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.Erro(ex);
            }
        }

        /// <summary>
        /// Método para Deletar a Visita
        /// </summary>
        /// <param name="index">Código da Visita</param>
        public void DeletarVisita(int index)
        {
            try
            {
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        string cmdText = "delete from tbAgenda where cdVisita = @cd";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@cd", index);
                        cmd.ExecuteNonQuery();
                        Xceed.Wpf.Toolkit.MessageBox.Show("Visita deletada com sucesso");
                        Historico.AdicionarHistorico(cd, "deletou", "a", "visita", index);
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
