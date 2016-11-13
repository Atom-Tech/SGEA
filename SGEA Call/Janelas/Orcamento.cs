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
using SGEA_Call.Classes;
using Mono.Samples.LabelledSections;
using SQLite;

namespace SGEA_Call.Janelas
{
    [Activity(Label = "Orcamento", Theme = "@style/MyTheme")]
    public class Orcamento : ListActivity
    {
        int[] cd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Orcamento);
            string codigo = Intent.GetStringExtra("Código");
            string mes = Month.ToNumber(codigo);
            var imagemMain = FindViewById<ImageView>(Resource.Id.imageView1);
            imagemMain.Click += ImagemMain_Click;
            int ano = Convert.ToInt32(codigo.Substring(codigo.Length - 4, 4));
            var mConn = new SQLiteConnection(Connect.getPath());
            var select = mConn.Query<Classes.Orcamento>("select cdOrcamento, login 'Funcionário', nmCliente 'Cliente', date(dtOrcamento) 'dtOrcamento', date(dtModificacao)," +
                " date(dtValidade), sum(precoU * quant) 'preco', observacoes, case when idExecucao = 0 then 'Não' when idExecucao = 1 then 'Sim' end as 'Confirmado?'" +
                " from tbOrcamento, tbUsuario, tbItemNota, tbCliente, tbProduto, tbServico" +
                " where cdUsuario = idUsuario and cdOrcamento = idOrcamento and cdCliente = idCliente and strftime('%m',dtOrcamento)='" + mes +
                "' and strftime('%Y',dtOrcamento)= '" + ano + "' and (cdProduto = idProduto or idProduto is null) and(cdServico = idServico or idServico is null) group by cdOrcamento");
            var items = new List<Tuple<string, string>>();
            cd = new int[select.Count];
            for (int i = 0; i < select.Count; i++)
            {
                cd[i] = select[i].cdOrcamento;
                items.Add(new Tuple<string, string>(Month.Format(select[i].dtOrcamento)+" para "+select[i].nmCliente, "R$"+ string.Format("{0:0.00}",select[i].preco)));
            }
            ListAdapter = new TwoLineListAdapter(this, items);
        }

        private void ImagemMain_Click(object sender, EventArgs e)
        {
            this.Return();
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var intent = new Intent(this, typeof(SelecaoOrcamento));
            intent.PutExtra("Código", cd[position].ToString());
            StartActivity(intent);
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }
    }
}