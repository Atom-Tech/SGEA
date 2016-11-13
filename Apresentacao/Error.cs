using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SGEA
{
    public static class Error
    {
        public static void Erro(Exception ex)
        {
            string data = DateTime.Today.ToString();
            string d = data.Substring(0, 2);
            string m = data.Substring(3, 2);
            string a = data.Substring(6, 4);
            StackTrace st = new StackTrace(ex, true);
            StackFrame frame = st.GetFrame(0);
            string caminho = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\SGEA\Logs\" +d+"-"+m+"-"+a+".txt";
            if (!File.Exists(caminho))
            {
                File.Create(caminho);
            }
            using (StreamWriter texto = new StreamWriter(caminho,true))
            {
                texto.WriteLine("\n");
                string fileName = frame.GetFileName();
                string methodName = frame.GetMethod().Name;
                int line = frame.GetFileLineNumber();
                int col = frame.GetFileColumnNumber();
                texto.WriteLine("Mensagem do Erro: " + ex.Message);
                texto.WriteLine("Janela:           " + fileName);
                texto.WriteLine("Linha:            " + line);
                texto.WriteLine("Coluna:           " + col);
                texto.WriteLine("Método:           " + methodName);
            }
            MessageBox.Show("Desculpe, mas ocorreu um erro. Para ver detalhes do erro abra "+caminho,"Erro");
        }
    }
}
