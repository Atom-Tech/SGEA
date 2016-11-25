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
    [Activity(Label = "Servicos", Theme = "@style/MyTheme")]
    public class Servicos : Activity
    {
        private float x;
        private List<Classes.Servicos> select;
        private int i, q;

        public override bool OnTouchEvent(MotionEvent e)
        {
            TextView texto = FindViewById<TextView>(Resource.Id.quant);
            TextView nome = FindViewById<TextView>(Resource.Id.campoNome);
            TextView desc = FindViewById<TextView>(Resource.Id.campoDesc);
            TextView quant = FindViewById<TextView>(Resource.Id.campoQuant);
            TextView precoT = FindViewById<TextView>(Resource.Id.campoPrecoT);
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
                            nome.Text = select[i].Nome;
                            desc.Text = select[i].Descricao;
                            precoT.Text = "R$" + select[i].PrecoTotal;
                            quant.Text = select[i].Quantidade.ToString();
                        }
                    }
                    break;
            }
            return base.OnTouchEvent(e);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Servicos);
            string cd = Intent.GetStringExtra("Código");
            var imagemMain = FindViewById<ImageView>(Resource.Id.imageView1);
            imagemMain.Click += ImagemMain_Click;
            var mConn = new SQLiteConnection(Connect.getPath());
            select = mConn.Query<Classes.Servicos>("select nmServico, dsServico, " +
                " quant, precoU, precoU*quant 'Preco Total', cdItemNota" +
                " from tbOrcamento, tbItemNota, tbServico where cdOrcamento = " + cd + " and cdOrcamento = idOrcamento and cdServico = idServico");
            i = 0;
            q = select.Count;
            TextView texto = FindViewById<TextView>(Resource.Id.quant);
            texto.Text = "0/0";
            if (q > 0)
            {
                texto.Text = "1/" + q;
                TextView nome = FindViewById<TextView>(Resource.Id.campoNome);
                TextView desc = FindViewById<TextView>(Resource.Id.campoDesc);
                TextView precoT = FindViewById<TextView>(Resource.Id.campoPrecoT);
                TextView quant = FindViewById<TextView>(Resource.Id.campoQuant);
                nome.Text = select[i].Nome;
                desc.Text = select[i].Descricao;
                quant.Text = select[i].Quantidade.ToString();
                precoT.Text = "R$" + select[i].PrecoTotal;
            }
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

        private void ImagemMain_Click(object sender, EventArgs e)
        {
            this.Return();
        }
    }
}