using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Controls;

namespace SGEA
{
    public static class Connect
    {
        public static SQLiteConnection LiteString()
        {
            string c = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SGEA\\sgea.db";
            return new SQLiteConnection("Data Source="+c+";FOREIGN keys=true");
        }

        public static DataTable LiteConnection(string cmdText)
        {
            using (var mConn = LiteString())
            {
                DataTable ds = new DataTable();
                mConn.Open();
                if (mConn.State == ConnectionState.Open)
                {
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                    cmd.CommandTimeout = 60;
                    SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);
                    adp.Fill(ds);
                    return ds;
                }
            }
            throw new Exception("Banco de Dados não existe");
        }

        public static ComboBox ItemFill(this ComboBox caixa, string cmdText)
        {
            using (var mConn = LiteString())
            {
                DataSet ds = new DataSet();
                mConn.Open();
                if (mConn.State == ConnectionState.Open)
                {
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, mConn);
                    SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);
                    adp.Fill(ds, "dados");
                    caixa.DataContext = ds.Tables[0].DefaultView;
                    caixa.DisplayMemberPath = ds.Tables[0].Columns[1].ToString();
                    caixa.SelectedValuePath = ds.Tables[0].Columns[0].ToString();
                    if (caixa.Items.Count > 0)
                        caixa.SelectedIndex = 0;
                }
            }
            return caixa;
        }
    }
}
