using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace SGEA_Call.Classes
{
    public class Relatorio
    {
        [Column("Mes Ano")]
        public string mesano { get; set; }

        [Column("Mês e Ano")]
        public string data { get; set; }

        [Column("Preço Total")]
        public double precoTotal { get; set; }

        [Column("Ano")]
        public int ano { get; set; }
    }
}