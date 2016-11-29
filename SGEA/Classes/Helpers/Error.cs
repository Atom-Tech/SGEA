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
            string caminho = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SGEA\Logs\" + d + "-" + m + "-" + a + ".txt";
            if (!File.Exists(caminho))
            {
                File.Create(caminho);
            }
            string fileName = frame.GetFileName();
            string methodName = frame.GetMethod().Name;
            int line = frame.GetFileLineNumber();
            int col = frame.GetFileColumnNumber();
            using (StreamWriter texto = new StreamWriter(caminho, true))
            {
                texto.WriteLine("\n");
                texto.WriteLine("Mensagem do Erro: " + ex.Message);
                texto.WriteLine("Janela:           " + fileName);
                texto.WriteLine("Linha:            " + line);
                texto.WriteLine("Coluna:           " + col);
                texto.WriteLine("Método:           " + methodName);
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("Desculpe, mas ocorreu um erro. Para ver detalhes do erro abra " + caminho, "Erro");
        }

        public static bool IsCnpj(this string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
    }
}
