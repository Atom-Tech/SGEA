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
    class Perfil
    {
        [PrimaryKey, AutoIncrement]
        public int cdPerfil { get; set; }

        public string nmPerfil { get; set; }

        public string dsPerfil { get; set; }

    }
}