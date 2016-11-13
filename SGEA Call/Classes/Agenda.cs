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
    class Agenda
    {
        [PrimaryKey, AutoIncrement]
        public int cdVisita { get; set; }

		[Column("Data da Visita")]
        public string dtVisita { get; set; }

		[Column("Horário")]
        public string hrVisita { get; set; }

        public string nmCliente { get; set; }

        public string localVisita { get; set; }
        
        public string dsVisita { get; set; }

        public string telFixo { get; set; }

        public string telCel { get; set; }

        public int quantidade { get; set; }
    }
}