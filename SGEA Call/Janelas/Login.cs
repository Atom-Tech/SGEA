using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Mono.Data.Sqlite;
using SQLite;
using System.Collections.Generic;
using SGEA_Call.Janelas;

namespace SGEA_Call
{
    [Activity(Label = "SGEA_Call", MainLauncher = true, Icon = "@drawable/icon",
        Theme = "@style/MyTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Login : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);
            Button login = FindViewById<Button>(Resource.Id.botaoEntrar);
            login.Click += Login_Click;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            TextView mensagem = FindViewById<TextView>(Resource.Id.labelMensagem);
            try
            {
                Crypt c = new Crypt();
                EditText login = FindViewById<EditText>(Resource.Id.campoLogin);
                EditText senha = FindViewById<EditText>(Resource.Id.campoSenha);
                var mConn = new SQLiteConnection(Connect.getPath());
                var select = mConn.Query<Usuario>("SELECT * FROM tbUsuario where login = '"+login.Text+"' and senha = '"+c.EncryptToString(senha.Text)+"'");
                if (select.Count > 0)
                {
                    mensagem.Visibility = ViewStates.Invisible;
                    var main = new Intent(this, typeof(Main));
                    StartActivity(main);
                    OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
                }
                else
                {
                    mensagem.Visibility = ViewStates.Visible;
                    mensagem.SetTextColor(Android.Graphics.Color.Red);
                    mensagem.Text = "Senha Incorreta";
                }
            }
            catch (NoDatabaseException)
            {
                mensagem.Visibility = ViewStates.Visible;
                mensagem.Text = "Você precisa transferir o Banco de Dados\n pelo nosso software primeiro";
            }
        }
    }
}

