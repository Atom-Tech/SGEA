using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Apresentacao
{
    public static class Month
    {
        public static string ToNumber(string name)
        {
            switch (name.Substring(0, name.Length - 8))
            {
                case "Janeiro": return "01";
                case "Fevereiro": return "02";
                case "Março": return "03";
                case "Abril": return "04";
                case "Maio": return "05";
                case "Junho": return "06";
                case "Julho": return "07";
                case "Agosto": return "08";
                case "Setembro": return "09";
                case "Outubro": return "10";
                case "Novembro": return "11";
                case "Dezembro": return "12";
            }
            throw new Exception("Mês não existe");
        }

        public static string ToName(int m)
        {
            switch (m)
            {
                case 1:
                    return "Janeiro";
                case 2:
                    return "Fevereiro";
                case 3:
                    return "Março";
                case 4:
                    return "Abril";
                case 5:
                    return "Maio";
                case 6:
                    return "Junho";
                case 7:
                    return "Julho";
                case 8:
                    return "Agosto";
                case 9:
                    return "Setembro";
                case 10:
                    return "Outubro";
                case 11:
                    return "Novembro";
                case 12:
                    return "Dezembro";
            }
            throw new IndexOutOfRangeException();
        }

        public static List<Label> WeekOrder(this List<Label> s, string semana)
        {
            switch (semana)
            {
                case "Sunday":
                    s[0].Content = "D";
                    s[1].Content = "S";
                    s[2].Content = "T";
                    s[3].Content = "Q";
                    s[4].Content = "Q";
                    s[5].Content = "S";
                    s[6].Content = "S";
                    break;
                case "Monday":
                    s[0].Content = "S";
                    s[1].Content = "T";
                    s[2].Content = "Q";
                    s[3].Content = "Q";
                    s[4].Content = "S";
                    s[5].Content = "S";
                    s[6].Content = "D";
                    break;
                case "Tuesday":
                    s[0].Content = "T";
                    s[1].Content = "Q";
                    s[2].Content = "Q";
                    s[3].Content = "S";
                    s[4].Content = "S";
                    s[5].Content = "D";
                    s[6].Content = "S";
                    break;
                case "Wednesday":
                    s[0].Content = "Q";
                    s[1].Content = "Q";
                    s[2].Content = "S";
                    s[3].Content = "S";
                    s[4].Content = "D";
                    s[5].Content = "S";
                    s[6].Content = "T";
                    break;
                case "Thursday":
                    s[0].Content = "Q";
                    s[1].Content = "S";
                    s[2].Content = "S";
                    s[3].Content = "D";
                    s[4].Content = "S";
                    s[5].Content = "T";
                    s[6].Content = "Q";
                    break;
                case "Friday":
                    s[0].Content = "S";
                    s[1].Content = "S";
                    s[2].Content = "D";
                    s[3].Content = "S";
                    s[4].Content = "T";
                    s[5].Content = "Q";
                    s[6].Content = "Q";
                    break;
                case "Saturday":
                    s[0].Content = "S";
                    s[1].Content = "D";
                    s[2].Content = "S";
                    s[3].Content = "T";
                    s[4].Content = "Q";
                    s[5].Content = "Q";
                    s[6].Content = "S";
                    break;
            }
            return s;
        }
    }
}
