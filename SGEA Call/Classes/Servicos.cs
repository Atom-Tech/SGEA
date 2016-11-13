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
    public class Servicos
    {
        [Column("nmServico")]
        public string Nome { get; set; }

        [Column("dsServico")]
        public string Descricao { get; set; }

        [Column("quant")]
        public int Quantidade { get; set; }

        [Column("precoU")]
        public double PrecoUnitario { get; set; }

        [Column("Preco Total")]
        public double PrecoTotal { get; set; }
        
    }
}