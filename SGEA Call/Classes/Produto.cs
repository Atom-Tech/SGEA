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
    public class Produto
    {
        [PrimaryKey, Column("cdProduto")]
        public int Codigo { get; set; }

        [Column("nmProduto")]
        public string Nome { get; set; }

        [Column("dsProduto")]
        public string Descricao { get; set; }

        [Column("nmTipo")]
        public string Tipo { get; set; }

        [Column("imagem")]
        public string Imagem { get; set; }

        [Column("largura")]
        public double Largura { get; set; }

        [Column("altura")]
        public double Altura { get; set; }

        [Column("quant")]
        public int Quantidade { get; set; }

        [Column("precoU")]
        public double PrecoUnitario { get; set; }
        
        [Column("Preco Total")]
        public double PrecoTotal { get; set; } 
        
    }
}