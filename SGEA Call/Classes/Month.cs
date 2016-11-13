using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SGEA_Call.Janelas;

namespace SGEA_Call.Classes
{
    public static class Month
    {
        public static string ToNumber(string name)
        {
            switch (name.Substring(0,name.Length - 8))
            {
                case "Janeiro"  : return "01";
                case "Fevereiro": return "02";
                case "Março"    : return "03";
                case "Abril"    : return "04";
                case "Maio"     : return "05";
                case "Junho"    : return "06";
                case "Julho"    : return "07";
                case "Agosto"   : return "08";
                case "Setembro" : return "09";
                case "Outubro"  : return "10";
                case "Novembro" : return "11";
                case "Dezembro" : return "12";
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

        public static string Format(string data)
        {
            var d = data.Split('-');
            return d[2] + " de " + ToName(Convert.ToInt32(d[1])) + " de " + d[0];
        }

        public static List<TextView> WeekOrder(this List<TextView> s, string semana)
        {
            switch (semana)
            {
                case "Sunday":
                    s[0].Text = "D";
                    s[1].Text = "S";
                    s[2].Text = "T";
                    s[3].Text = "Q";
                    s[4].Text = "Q";
                    s[5].Text = "S";
                    s[6].Text = "S";
                    break;
                case "Monday":
                    s[0].Text = "S";
                    s[1].Text = "T";
                    s[2].Text = "Q";
                    s[3].Text = "Q";
                    s[4].Text = "S";
                    s[5].Text = "S";
                    s[6].Text = "D";
                    break;
                case "Tuesday":
                    s[0].Text = "T";
                    s[1].Text = "Q";
                    s[2].Text = "Q";
                    s[3].Text = "S";
                    s[4].Text = "S";
                    s[5].Text = "D";
                    s[6].Text = "S";
                    break;
                case "Wednesday":
                    s[0].Text = "Q";
                    s[1].Text = "Q";
                    s[2].Text = "S";
                    s[3].Text = "S";
                    s[4].Text = "D";
                    s[5].Text = "S";
                    s[6].Text = "T";
                    break;
                case "Thursday":
                    s[0].Text = "Q";
                    s[1].Text = "S";
                    s[2].Text = "S";
                    s[3].Text = "D";
                    s[4].Text = "S";
                    s[5].Text = "T";
                    s[6].Text = "Q";
                    break;
                case "Friday":
                    s[0].Text = "S";
                    s[1].Text = "S";
                    s[2].Text = "D";
                    s[3].Text = "S";
                    s[4].Text = "T";
                    s[5].Text = "Q";
                    s[6].Text = "Q";
                    break;
                case "Saturday":
                    s[0].Text = "S";
                    s[1].Text = "D";
                    s[2].Text = "S";
                    s[3].Text = "T";
                    s[4].Text = "Q";
                    s[5].Text = "Q";
                    s[6].Text = "S";
                    break;
            }
            return s;
        }

        public static void Return(this Activity c)
        {
            var main = new Intent(c, typeof(Main));
            main.AddFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(main);
            c.OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }

        public static string Completar(this string[] data, string barra)
        {
            return data[2] + barra + data[1] + barra + data[0];
        }
    }
}