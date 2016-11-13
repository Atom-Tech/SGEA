using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGEA.Classes
{
    public static class Historico
    {
        public static void AdicionarHistorico(int cd, string alteracao, string artigo, string tabela, int id = 0)
        {
            try
            {
                string mensagem = char.ToUpper(alteracao[0]) + alteracao.Substring(1) +
                    " " + artigo + " " + tabela;
                if (id != 0)
                    mensagem += " nº " + id;
                using (var mConn = Connect.LiteString())
                {
                    mConn.Open();
                    if (mConn.State == ConnectionState.Open)
                    {
                        var s = Connect.LiteConnection(Selects.SelectUsuarioLogin(cd));
                        string cmdText = "insert into tbHistorico values (null,@ds,@dt,@hr,@id)";
                        SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                        cmd.Parameters.AddWithValue("@ds", mensagem);
                        cmd.Parameters.AddWithValue("@dt", DateTime.Today);
                        cmd.Parameters.AddWithValue("@hr", DateTime.Now.TimeOfDay);
                        cmd.Parameters.AddWithValue("@id", s.Rows[0].ItemArray[0].ToString());
                        cmd.ExecuteNonQuery();
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
