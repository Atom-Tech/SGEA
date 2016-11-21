using SGEA.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SGEA
{
    public static class Connect
    {
        private static string banco = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SGEA\\sgea.db";

        public static string Banco
        {
            get
            {
                return banco;
            }
        }

        public static SQLiteConnection LiteString()
        {
            return new SQLiteConnection("Data Source=" + banco + ";FOREIGN keys=true");
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

        public static bool IsDirectoryEmpty(this string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static string ToSqlString(this DateTime data)
        {
            var d = data.ToShortDateString().Split('/');
            return d[2] + "-" + d[1] + "-" + d[0];
        }

        public static bool IsCepValid(this string cep)
        {
            var s = LiteConnection("select * from cep_unico where cep = '" + cep + "'");
            if (s.Rows.Count == 1)
                return true;
            else
            {
                s = LiteConnection("select * from sp where cep = '" + cep + "'");
                if (s.Rows.Count == 1)
                    return true;
                return false;
            }
        }

        public static string getCidade(this string cep)
        {
            var s = LiteConnection("select * from cep_unico where cep = '" + cep + "'");
            if (s.Rows.Count == 1)
                return s.Rows[0].ItemArray[1].ToString();
            else
            {
                s = LiteConnection("select * from sp where cep = '" + cep + "'");
                if (s.Rows.Count == 1)
                    return s.Rows[0].ItemArray[1].ToString();
                return "";
            }
        }

        public static string getBairro(this string cep)
        {
            var s = LiteConnection("select * from sp where cep = '" + cep + "'");
            if (s.Rows.Count == 1)
                return s.Rows[0].ItemArray[3].ToString();
            return "";
        }

        public static string getRua(this string cep)
        {
            var s = LiteConnection("select * from sp where cep = '" + cep + "'");
            if (s.Rows.Count == 1)
                return s.Rows[0].ItemArray[5].ToString() + " " +
                    s.Rows[0].ItemArray[2].ToString();
            return "";
        }

        public static string getCep(this string cidade, string bairro = "")
        {
            var s = LiteConnection("select * from cep_unico where nome = '" + cidade + "'");
            if (s.Rows.Count == 1 && s.Rows[0].ItemArray[3].ToString() != "")
                return s.Rows[0].ItemArray[3].ToString();
            else
            {
                string t = "select * from sp where cidade like '" + cidade + "%'";
                if (bairro != "") t += " and bairro like '" + bairro + "%'";
                s = LiteConnection(t);
                if (s.Rows.Count > 0)
                    return s.Rows[0].ItemArray[4].ToString();
                return "#####-###";
            }
        }

        public static DataRowView SelectedRow(this DataGrid dg)
        {
            return (DataRowView)dg.Items[dg.SelectedIndex];
        }

        public static DataRowView SelectFirstRow(this DataGrid dg)
        {
            return (DataRowView)dg.Items[0];
        }

        public static string PesquisarCidade(this TextBox campoCidade,
            string cmdText)
        {
            List<string> cep = new List<string>();
            var s = LiteConnection("select * from cep_unico where nome = '" +
                campoCidade.Text + "' order by cep");
            if (s.Rows.Count == 1 && s.Rows[0].ItemArray[3].ToString() != "")
                cep.Add(s.Rows[0].ItemArray[3].ToString());
            else
            {
                s = LiteConnection("select cep from sp where cidade = '" +
                    campoCidade.Text + "' order by cep");
                if (s.Rows.Count > 0)
                {
                    foreach (DataRow c in s.Rows)
                    {
                        cep.Add(c.ItemArray[0].ToString());
                    }
                }
            }
            cmdText += "cep between '" + cep.First() + "' and '"+cep.Last()+"'";
            return cmdText;
        }

    }
}
