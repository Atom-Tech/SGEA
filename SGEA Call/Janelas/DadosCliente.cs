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

namespace SGEA_Call.Janelas
{
    [Activity(Label = "DadosCliente", Theme = "@style/MyTheme")]
    public class DadosCliente : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DadosCliente);
            int codigo = Intent.GetIntExtra("Código",1);
            var imagemMain = FindViewById<ImageView>(Resource.Id.imageView1);
            imagemMain.Click += ImagemMain_Click;
            var mConn = new SQLiteConnection(Connect.getPath());
            var select = mConn.Query<Classes.Cliente>("select * from tbCliente where cdCliente = "+codigo);
            TextView nome = FindViewById<TextView>(Resource.Id.campoNome);
            TextView sexo = FindViewById<TextView>(Resource.Id.campoSexo);
            TextView cep = FindViewById<TextView>(Resource.Id.campoCep);
            TextView bairro = FindViewById<TextView>(Resource.Id.campoBairro);
            TextView rua = FindViewById<TextView>(Resource.Id.campoRua);
            TextView email = FindViewById<TextView>(Resource.Id.campoEmail);
            TextView telFixo = FindViewById<TextView>(Resource.Id.telFixo);
            TextView telCel = FindViewById<TextView>(Resource.Id.telCel);
            Button botaoFixo = FindViewById<Button>(Resource.Id.botaoFixo);
            Button botaoCel = FindViewById<Button>(Resource.Id.botaoCel);
            nome.Text = select[0].nmCliente;
            if (select[0].sexo == "M") sexo.Text = "Masculino";
            if (select[0].sexo == "F") sexo.Text = "Feminino";
            if (select[0].sexo == "I") sexo.Text = "Indefinido";
            cep.Text = select[0].cep;
            bairro.Text = select[0].bairro;
            rua.Text = select[0].rua;
            email.Text = select[0].email;
            telFixo.Text = select[0].telFixo;
            telCel.Text = select[0].telCel;
            botaoFixo.Click += BotaoFixo_Click;
            botaoCel.Click += BotaoCel_Click;
            var visitas = mConn.Query<Agenda>("select date(dtVisita) 'Data da Visita', time(hrVisita) 'Horário', localVisita from tbAgenda where idCliente=" + codigo);
            TextView textoVisitas = FindViewById<TextView>(Resource.Id.textoVisitas);
            textoVisitas.Text = "";
            if (visitas.Count > 0)
            {
                for (int i = 0; i < visitas.Count; i++)
                {
                    string[] data = visitas[i].dtVisita.Split('-');
                    string dataCompleta = data.Completar("/");
                    textoVisitas.Text += dataCompleta + " às " + visitas[i].hrVisita + " em "+ visitas[i].localVisita + "\n";
                }
            }
        }

        private void ImagemMain_Click(object sender, EventArgs e)
        {
            this.Return();
        }

        private void BotaoCel_Click(object sender, EventArgs e)
        {
            TextView cel = FindViewById<TextView>(Resource.Id.telCel);
            string numero = cel.Text.Replace("(", string.Empty);
            numero = numero.Replace(")", string.Empty);
            var uri = Android.Net.Uri.Parse("tel:" + numero);
            var intent = new Intent(Intent.ActionDial, uri);
            StartActivity(intent);
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }

        private void BotaoFixo_Click(object sender, EventArgs e)
        {
            TextView fixo = FindViewById<TextView>(Resource.Id.telFixo);
            string numero = fixo.Text.Replace("(", string.Empty);
            numero = numero.Replace(")", string.Empty);
            var uri = Android.Net.Uri.Parse("tel:" + numero);
            var intent = new Intent(Intent.ActionDial, uri);
            StartActivity(intent);
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }
    }
}