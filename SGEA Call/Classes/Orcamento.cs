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
    public class Orcamento
    {
        [PrimaryKey, AutoIncrement]
        public int cdOrcamento { get; set; }

        public string dtOrcamento { get; set; }

        public double preco { get; set; }

        [Column("Cliente")]
        public string nmCliente { get; set; }

        [Column("observacoes")]
        public string Observacoes { get; set; }

        [Column("dtValidade")]
        public string DataValidade { get; set; }

        [Column("idExecucao")]
        public string Execucao { get; set; }

        public int idCliente { get; set; }
    }
}