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
using SGEA_Call.Classes;
using Android.Views.Animations;

namespace SGEA_Call.Janelas
{
    [Activity(Label = "Produto", Theme = "@style/MyTheme")]
    public class Produto : Activity
    {
        private float x;
        private string cd;
        private List<Classes.Produto> select;
        private int i, q;

        public override bool OnTouchEvent(MotionEvent e)
        {
            TextView texto = FindViewById<TextView>(Resource.Id.quant);
            TextView nome = FindViewById<TextView>(Resource.Id.campoNome);
            TextView tipo = FindViewById<TextView>(Resource.Id.campoTipo);
            TextView desc = FindViewById<TextView>(Resource.Id.campoDesc);
            TextView medidas = FindViewById<TextView>(Resource.Id.campoMedidas);
            TextView quant = FindViewById<TextView>(Resource.Id.campoQuant);
            TextView precoU = FindViewById<TextView>(Resource.Id.campoPrecoU);
            TextView precoT = FindViewById<TextView>(Resource.Id.campoPrecoT);
            var imagem = FindViewById<ImageView>(Resource.Id.imagem);
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    x = e.RawX;
                    break;
                case MotionEventActions.Up:
                    float deltaX = e.RawX - x;
                    if (Math.Abs(deltaX) > 50)
                    {
                        if (e.RawX > x)
                        {
                            if (i > 0)
                            {
                                Animation a = AnimationUtils.LoadAnimation(ApplicationContext,
                                    Resource.Animation.centerToRight);
                                var l = FindViewById<LinearLayout>(Resource.Id.root);
                                l.StartAnimation(a);
                                a.AnimationEnd += A_AnimationEnd;
                                i--;
                            }
                        }
                        else
                        {
                            if (i < q - 1)
                            {
                                Animation a = AnimationUtils.LoadAnimation(ApplicationContext,
                                    Resource.Animation.centerToLeft);
                                var l = FindViewById<LinearLayout>(Resource.Id.root);
                                l.StartAnimation(a);
                                a.AnimationEnd += A_AnimationEnd1;
                                i++;
                            }
                        }
                        if (select.Count > 0)
                        {
                            texto.Text = (i + 1) + "/" + q;
                            imagem.SetImageURI(Android.Net.Uri.Parse(select[i].Imagem));
                            nome.Text = select[i].Nome;
                            tipo.Text = select[i].Tipo;
                            desc.Text = select[i].Descricao;
                            medidas.Text = select[i].Largura + "x" + select[i].Altura + "m²";
                            precoU.Text = "R$" + select[i].PrecoUnitario;
                            precoT.Text = "R$" + select[i].PrecoTotal;
                            quant.Text = select[i].Quantidade.ToString();
                        }
                    }
                    break;
            }
            return base.OnTouchEvent(e);
        }

        private void A_AnimationEnd1(object sender, Animation.AnimationEndEventArgs e)
        {
            var l = FindViewById<LinearLayout>(Resource.Id.root);
            Animation a = AnimationUtils.LoadAnimation(ApplicationContext,
                Resource.Animation.rightToCenter);
            l.StartAnimation(a);
        }

        private void A_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            var l = FindViewById<LinearLayout>(Resource.Id.root);
            Animation a = AnimationUtils.LoadAnimation(ApplicationContext,
                Resource.Animation.leftToCenter);
            l.StartAnimation(a);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Produtos);
            cd = Intent.GetStringExtra("Código");
            var imagemMain = FindViewById<ImageView>(Resource.Id.imageView1);
            imagemMain.Click += ImagemMain_Click;
            var mConn = new SQLiteConnection(Connect.getPath());
            select = mConn.Query<Classes.Produto>("select cdProduto, nmProduto, dsProduto, nmTipo," +
                    "imagem, largura, altura, quant, precoU, precoU*quant 'Preco Total', cdItemNota" +
                    " from tbOrcamento, tbItemNota, tbProduto, tbTipo where cdOrcamento = " + cd + " and tpProduto = cdTipo and cdOrcamento = idOrcamento and cdProduto = idProduto");
            i = 0;
            q = select.Count;
            TextView texto = FindViewById<TextView>(Resource.Id.quant);
            texto.Text = "0/0";
            if (q > 0)
            {
                texto.Text = "1/" + q;
                var imagem = FindViewById<ImageView>(Resource.Id.imagem);
                imagem.SetImageURI(Android.Net.Uri.Parse(select[0].Imagem));
                TextView nome = FindViewById<TextView>(Resource.Id.campoNome);
                TextView tipo = FindViewById<TextView>(Resource.Id.campoTipo);
                TextView desc = FindViewById<TextView>(Resource.Id.campoDesc);
                TextView medidas = FindViewById<TextView>(Resource.Id.campoMedidas);
                TextView precoU = FindViewById<TextView>(Resource.Id.campoPrecoU);
                TextView precoT = FindViewById<TextView>(Resource.Id.campoPrecoT);
                TextView quant = FindViewById<TextView>(Resource.Id.campoQuant);
                nome.Text = select[i].Nome;
                tipo.Text = select[i].Tipo;
                desc.Text = select[i].Descricao;
                medidas.Text = select[i].Largura + "x" + select[i].Altura + "m²";
                quant.Text = select[i].Quantidade.ToString();
                precoU.Text = "R$" + select[i].PrecoUnitario;
                precoT.Text = "R$" + select[i].PrecoTotal;
            }
        }

        private void ImagemMain_Click(object sender, EventArgs e)
        {
            this.Return();
        }
    }
}