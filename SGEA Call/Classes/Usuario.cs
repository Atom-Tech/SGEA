using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SGEA_Call
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int cd { get; set; }

        public string login { get; set; }

        public string senha { get; set; }
    }
}