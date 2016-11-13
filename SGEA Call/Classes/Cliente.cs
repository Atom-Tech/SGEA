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
    public class Cliente
    {
        public string nmCliente { get; set; }

        [PrimaryKey, AutoIncrement]
        public int cdCliente { get; set; }

        public string sexo { get; set; }

        public string email { get; set; }

        public string cep { get; set; }

        public string bairro { get; set; }

        public string rua { get; set; }

        public string telFixo { get; set; }

        public string telCel { get; set; }
    }
}