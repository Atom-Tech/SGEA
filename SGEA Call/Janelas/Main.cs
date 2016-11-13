using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using SGEA_Call.Classes;
using Mono.Data.Sqlite;

namespace SGEA_Call.Janelas
{
    [Activity(Label = "Main", Theme = "@style/MyTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Main : Activity
    {
        private int i;
        private string dia;
        public static int m, ano;
        List<TextView> texto = new List<TextView>();
        List<TextView> s = new List<TextView>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            var cal = FindViewById<ImageView>(Resource.Id.botaoCalendario);
            cal.Click += Cal_Click;
            Button menu = FindViewById<Button>(Resource.Id.botaoMenu);
            menu.Click += (b, arg) =>
            {
                PopupMenu om = new PopupMenu(this, menu);
                om.Inflate(Resource.Menu.JMenu);
                om.MenuItemClick += (b1, arg1) =>
                {
                    if (arg1.Item.TitleFormatted.ToString() == "Fornecedores")
                    {
                        Intent intent = new Intent(this, typeof(Fornecedor));
                        StartActivity(intent);
                        OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
                    }
                    if (arg1.Item.TitleFormatted.ToString() == "Clientes")
                    {
                        Intent intent = new Intent(this, typeof(Cliente));
                        StartActivity(intent);
                        OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
                    }
                    if (arg1.Item.TitleFormatted.ToString() == "Relatório")
                    {
                        Intent intent = new Intent(this, typeof(Relatorio));
                        StartActivity(intent);
                        OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
                    }
                };
                om.Show();
            };
            m = DateTime.Today.Month;
            ano = DateTime.Today.Year;
            VerificarData(m, ano);
            var mConn = new SQLiteConnection(Connect.getPath());
            var select = mConn.Query<Agenda>("select * from vwVisitaHoje");
            if (select.Count > 0)
            {

                Notification.Builder note = new Notification.Builder(this)
                    .SetContentTitle("SGEA")
                    .SetContentText("Há visitas agendadas para hoje")
                    .SetDefaults(NotificationDefaults.Vibrate)
                    .SetSmallIcon(Resource.Drawable.icon);
                Notification n = note.Build();

                NotificationManager no = GetSystemService(Context.NotificationService) as NotificationManager;

                no.Notify(0, n);
            }
            for (int x = 0; x < texto.Count; x++)
            {
                var t = texto[x];
                t.Tag = x;
                t.Click += T_Click;
            }
        }

        private void Cal_Click(object sender, EventArgs e)
        {
            var t = FragmentManager.BeginTransaction();
            var d = new Data();
            d.Show(t, "dialog_fragment");
        }
        
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.JMenu, menu);
            return true;
        }

        public void VerificarData(int m, int ano)
        {
            Main.m = m;
            Main.ano = ano;
            string mes = Month.ToName(m);
            TextView mesano = FindViewById<TextView>(Resource.Id.mes);
            mesano.Text = mes + " de " + ano;
            DateTime dia1 = new DateTime(ano, m, 1);
            string semana = dia1.DayOfWeek.ToString();
            s.Clear();
            s.Add(FindViewById<TextView>(Resource.Id.s1));
            s.Add(FindViewById<TextView>(Resource.Id.s2));
            s.Add(FindViewById<TextView>(Resource.Id.s3));
            s.Add(FindViewById<TextView>(Resource.Id.s4));
            s.Add(FindViewById<TextView>(Resource.Id.s5));
            s.Add(FindViewById<TextView>(Resource.Id.s6));
            s.Add(FindViewById<TextView>(Resource.Id.s7));
            s = s.WeekOrder(semana);
            texto.Clear();
            texto.Add(FindViewById<TextView>(Resource.Id.d1));
            texto.Add(FindViewById<TextView>(Resource.Id.d2));
            texto.Add(FindViewById<TextView>(Resource.Id.d3));
            texto.Add(FindViewById<TextView>(Resource.Id.d4));
            texto.Add(FindViewById<TextView>(Resource.Id.d5));
            texto.Add(FindViewById<TextView>(Resource.Id.d6));
            texto.Add(FindViewById<TextView>(Resource.Id.d7));
            texto.Add(FindViewById<TextView>(Resource.Id.d8));
            texto.Add(FindViewById<TextView>(Resource.Id.d9));
            texto.Add(FindViewById<TextView>(Resource.Id.d10));
            texto.Add(FindViewById<TextView>(Resource.Id.d11));
            texto.Add(FindViewById<TextView>(Resource.Id.d12));
            texto.Add(FindViewById<TextView>(Resource.Id.d13));
            texto.Add(FindViewById<TextView>(Resource.Id.d14));
            texto.Add(FindViewById<TextView>(Resource.Id.d15));
            texto.Add(FindViewById<TextView>(Resource.Id.d16));
            texto.Add(FindViewById<TextView>(Resource.Id.d17));
            texto.Add(FindViewById<TextView>(Resource.Id.d18));
            texto.Add(FindViewById<TextView>(Resource.Id.d19));
            texto.Add(FindViewById<TextView>(Resource.Id.d20));
            texto.Add(FindViewById<TextView>(Resource.Id.d21));
            texto.Add(FindViewById<TextView>(Resource.Id.d22));
            texto.Add(FindViewById<TextView>(Resource.Id.d23));
            texto.Add(FindViewById<TextView>(Resource.Id.d24));
            texto.Add(FindViewById<TextView>(Resource.Id.d25));
            texto.Add(FindViewById<TextView>(Resource.Id.d26));
            texto.Add(FindViewById<TextView>(Resource.Id.d27));
            texto.Add(FindViewById<TextView>(Resource.Id.d28));
            texto.Add(FindViewById<TextView>(Resource.Id.d29));
            texto.Add(FindViewById<TextView>(Resource.Id.d30));
            texto.Add(FindViewById<TextView>(Resource.Id.d31));
            foreach (var t in texto)
                t.Visibility = ViewStates.Visible;
            for (int x = DateTime.DaysInMonth(ano, m); x < 31; x++)
                texto[x].Visibility = ViewStates.Invisible;
            var mConn = new SQLiteConnection(Connect.getPath());
            var select = mConn.Query<Agenda>("select cdVisita 'Código', date(dtVisita) 'Data da Visita', time(hrVisita) 'Horário', nmCliente 'Cliente', " +
         " localVisita 'Local da Visita', dsVisita 'Descrição da Visita'" +
         " from tbAgenda, tbCliente" +
         " where idCliente = cdCliente and strftime('%m', dtVisita) = '" + m.ToString().PadLeft(2, '0')
         + "' and strftime('%Y', dtVisita) = '" + ano + "'");
            foreach (TextView t in texto)
            {
                t.SetBackgroundColor(Android.Graphics.Color.Rgb(158, 168, 230));
                if (t.Text == DateTime.Today.Day.ToString()
                    && m == DateTime.Today.Month && ano == DateTime.Today.Year)
                    t.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);
                else
                    t.SetTypeface(null, Android.Graphics.TypefaceStyle.Normal);
                for (int q = 0; q < select.Count; q++)
                {
                    string diaM = t.Text.PadLeft(2, '0');
                    if (select[q].dtVisita.Substring(8, 2) == diaM &&
                        select[q].dtVisita.Substring(5,2) == m.ToString())
                        t.SetBackgroundColor(Android.Graphics.Color.Rgb(175, 185, 250));
                }
            }
        }

        private void T_Click(object sender, EventArgs e)
        {
            i = 0;
            var t = sender as TextView;
            dia = t.Text.PadLeft(2, '0');
            TextView data = FindViewById<TextView>(Resource.Id.data);
            TextView hora = FindViewById<TextView>(Resource.Id.horario);
            TextView local = FindViewById<TextView>(Resource.Id.local);
            TextView desc = FindViewById<TextView>(Resource.Id.descricao);
            TextView cliente = FindViewById<TextView>(Resource.Id.cliente);
            TextView telFixo = FindViewById<TextView>(Resource.Id.telFixo);
            TextView telCel = FindViewById<TextView>(Resource.Id.telCel);
            data.Text = t.Text.ToString().PadLeft(2, '0') + "/" + m.ToString().PadLeft(2, '0') +
                "/" + ano;
            string d = DateTime.Today.Year.ToString() + "-" + m.ToString().PadLeft(2, '0')
                + "-" + dia;
            var mConn = new SQLiteConnection(Connect.getPath());
            var select = mConn.Query<Agenda>("select time(hrVisita) 'Horário', localVisita, dsVisita, nmCliente, telFixo, telCel from tbAgenda, tbCliente where cdCliente = idCliente and date(dtVisita) = '" + d + "'");
            Button visitaA = FindViewById<Button>(Resource.Id.botaoVisitaA);
            Button visitaP = FindViewById<Button>(Resource.Id.botaoVisitaP);
            Button ligarFixo = FindViewById<Button>(Resource.Id.botaoFixo);
            Button ligarCel = FindViewById<Button>(Resource.Id.botaoCel);
            ligarFixo.Visibility = ViewStates.Invisible;
            ligarCel.Visibility = ViewStates.Invisible;
            visitaA.Visibility = ViewStates.Invisible;
            visitaP.Visibility = ViewStates.Invisible;
            if (select.Count > 0)
            {
                ligarFixo.Visibility = ViewStates.Visible;
                ligarCel.Visibility = ViewStates.Visible;
                ligarFixo.Click += LigarFixo_Click;
                ligarCel.Click += LigarCel_Click;
                if (select.Count > 1)
                {
                    visitaA.Visibility = ViewStates.Visible;
                    visitaP.Visibility = ViewStates.Visible;
                    visitaA.Click += VisitaA_Click;
                    visitaP.Click += VisitaP_Click;
                }
                hora.Text = select[0].hrVisita;
                local.Text = select[0].localVisita;
                desc.Text = select[0].dsVisita;
                cliente.Text = select[0].nmCliente;
                telFixo.Text = select[0].telFixo;
                telCel.Text = select[0].telCel;
            }
            else
            {
                hora.Text = "00:00:00";
                local.Text = "Selecione um dia";
                desc.Text = "Selecione um dia";
                cliente.Text = "Selecione um dia";
                telFixo.Text = "(00)0000-0000";
                telCel.Text = "(00)00000-0000";
            }
        }

        private void LigarCel_Click(object sender, EventArgs e)
        {
            TextView cel = FindViewById<TextView>(Resource.Id.telCel);
            string numero = cel.Text.Replace("(", string.Empty);
            numero = numero.Replace(")", string.Empty);
            var uri = Android.Net.Uri.Parse("tel:" + numero);
            var intent = new Intent(Intent.ActionDial, uri);
            StartActivity(intent);
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }

        private void LigarFixo_Click(object sender, EventArgs e)
        {
            TextView fixo = FindViewById<TextView>(Resource.Id.telFixo);
            string numero = fixo.Text.Replace("(", string.Empty);
            numero = numero.Replace(")", string.Empty);
            var uri = Android.Net.Uri.Parse("tel:" + numero);
            var intent = new Intent(Intent.ActionDial, uri);
            StartActivity(intent);
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }

        private void VisitaP_Click(object sender, EventArgs e)
        {
            string m = DateTime.Today.Month.ToString();
            m = m.PadLeft(2, '0');
            string d = DateTime.Today.Year.ToString() + "-" + m + "-" + dia;
            var mConn = new SQLiteConnection(Connect.getPath());
            var select = mConn.Query<Agenda>("select count(*) 'quantidade' from tbAgenda where dtVisita = '" + d + "' group by dtVisita");
            if (i < select[0].quantidade - 1)
            {
                i++;
                TextView hora = FindViewById<TextView>(Resource.Id.horario);
                TextView local = FindViewById<TextView>(Resource.Id.local);
                TextView desc = FindViewById<TextView>(Resource.Id.descricao);
                TextView cliente = FindViewById<TextView>(Resource.Id.cliente);
                TextView telFixo = FindViewById<TextView>(Resource.Id.telFixo);
                TextView telCel = FindViewById<TextView>(Resource.Id.telCel);
                select = mConn.Query<Agenda>("select time(hrVisita) 'Horário', localVisita, dsVisita, nmCliente, telFixo, telCel from tbAgenda, tbCliente where cdCliente = idCliente and dtVisita = '" + d + "'");
                hora.Text = select[i].hrVisita;
                local.Text = select[i].localVisita;
                desc.Text = select[i].dsVisita;
                cliente.Text = select[i].nmCliente;
                telFixo.Text = select[i].telFixo;
                telCel.Text = select[i].telCel;
            }
        }

        private void VisitaA_Click(object sender, EventArgs e)
        {
            if (i > 0)
            {
                i--;
                string m = DateTime.Today.Month.ToString();
                if (m.Length == 1)
                    m = "0" + m;
                string d = DateTime.Today.Year.ToString() + "-" + m + "-" + dia;
                var mConn = new SQLiteConnection(Connect.getPath());
                TextView hora = FindViewById<TextView>(Resource.Id.horario);
                TextView local = FindViewById<TextView>(Resource.Id.local);
                TextView desc = FindViewById<TextView>(Resource.Id.descricao);
                TextView cliente = FindViewById<TextView>(Resource.Id.cliente);
                TextView telFixo = FindViewById<TextView>(Resource.Id.telFixo);
                TextView telCel = FindViewById<TextView>(Resource.Id.telCel);
                var select = mConn.Query<Agenda>("select hrVisita 'Horário', localVisita, dsVisita, nmCliente, telFixo, telCel from tbAgenda, tbCliente where cdCliente = idCliente and dtVisita = '" + d + "'");
                hora.Text = select[i].hrVisita;
                local.Text = select[i].localVisita;
                desc.Text = select[i].dsVisita;
                cliente.Text = select[i].nmCliente;
                telFixo.Text = select[i].telFixo;
                telCel.Text = select[i].telCel;
            }
        }

    }
}