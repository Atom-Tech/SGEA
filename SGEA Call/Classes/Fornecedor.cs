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
    public class Fornecedor
    {
        [PrimaryKey, AutoIncrement]
        public int cdFornecedor { get; set; }

        public string cnpj { get; set; }

        public string nmFornecedor { get; set; }

        public string razaoSocial { get; set; }

        public string email { get; set; }

        public string cep { get; set; }

        public string bairro { get; set; }

        public string rua { get; set; }

        public string telFixo { get; set; }

        public string telCel { get; set; }
    }
}