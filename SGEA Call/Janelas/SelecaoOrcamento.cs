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
using SQLite;

namespace SGEA_Call.Janelas
{
    [Activity(Label = "SelecaoOrcamento", Theme = "@style/MyTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SelecaoOrcamento : Activity
    {
        string codigo;
        int cdCliente;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelecaoOrcamento);
            codigo = Intent.GetStringExtra("Código");
            var imagemMain = FindViewById<ImageView>(Resource.Id.imageView1);
            imagemMain.Click += ImagemMain_Click;
            var mConn = new SQLiteConnection(Connect.getPath());
            var select = mConn.Query<Classes.Orcamento>("select observacoes, idExecucao, date(dtValidade) 'dtValidade', idCliente from tbOrcamento where cdOrcamento = " + codigo);
            cdCliente = select[0].idCliente;
            TextView produtos = FindViewById<TextView>(Resource.Id.menuProdutos);
            TextView servicos = FindViewById<TextView>(Resource.Id.menuServicos);
            TextView obs = FindViewById<TextView>(Resource.Id.campoObs);
            TextView dtValidade = FindViewById<TextView>(Resource.Id.campoValidade);
            TextView idEx = FindViewById<TextView>(Resource.Id.campoConfirmado);
            var verCliente = FindViewById<Button>(Resource.Id.verCliente);
            verCliente.Click += VerCliente_Click;
            obs.Text = select[0].Observacoes;
            if (select[0].Execucao == "1") idEx.Text = "Sim";
            else idEx.Text = "Não";
            string[] data = select[0].DataValidade.Split('-');
            dtValidade.Text = data[2] + "/" + data[1] + "/" + data[0];
            produtos.Click += Produtos_Click;
            servicos.Click += Servicos_Click;
            ChecarProdutos();
        }

        private void VerCliente_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(DadosCliente));
            intent.PutExtra("Código", cdCliente);
            StartActivity(intent);
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }

        public void ChecarProdutos()
        {
            var mConn = new SQLiteConnection(Connect.getPath());
            var select = mConn.Query<Classes.Produto>("select cdProduto, nmProduto, dsProduto, nmTipo," +
                    "imagem, largura, altura, quant, precoU, precoU*quant 'Preco Total', cdItemNota" +
                    " from tbOrcamento, tbItemNota, tbProduto, tbTipo where cdOrcamento = " + codigo + " and tpProduto = cdTipo and cdOrcamento = idOrcamento and cdProduto = idProduto");
            foreach (var p in select)
            {
                if (p.Imagem != "Sem Imagem" && !p.Imagem.Contains("sdcard/SGEA/Imagens"))
                {
                    string[] link = p.Imagem.Split('\\');
                    string linkImagem = "sdcard/SGEA/Imagens/" + link[link.Length - 1];
                    var update = mConn.Execute("update tbProduto set imagem = '" + linkImagem + "' where cdProduto = " + p.Codigo);
                }
            }

        }

        private void ImagemMain_Click(object sender, EventArgs e)
        {
            this.Return();
        }

        private void Servicos_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(Servicos));
            intent.PutExtra("Código", codigo);
            StartActivity(intent);
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }

        private void Produtos_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(Produto));
            intent.PutExtra("Código", codigo);
            StartActivity(intent);
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }
    }
}