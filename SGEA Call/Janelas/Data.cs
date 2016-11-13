using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SGEA_Call.Janelas
{
    public class Data : DialogFragment
    {
        bool vf = false;
        Spinner listaM, listaA;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            var view = inflater.Inflate(Resource.Layout.Data, container, false);
            listaM = view.FindViewById<Spinner>(Resource.Id.listaMes);
            listaA = view.FindViewById<Spinner>(Resource.Id.listaAno);
            var botao = view.FindViewById<Button>(Resource.Id.botaoOK);
            botao.Click += Botao_Click;
            List<string> mes = new List<string>();
            List<string> ano = new List<string>();
            for (int i = 1; i < 13; i++)
                mes.Add(i.ToString().PadLeft(2, '0'));
            for (int i = DateTime.Today.Year;
                i < DateTime.Today.AddYears(5).Year; i++)
            {
                ano.Add(i.ToString());
            }
            ArrayAdapter adapter = new ArrayAdapter(view.Context,
            Android.Resource.Layout.SimpleListItem1, mes);
            listaM.Adapter = adapter;
            listaM.SetSelection(DateTime.Today.Month - 1);
            adapter = new ArrayAdapter(view.Context,
            Android.Resource.Layout.SimpleListItem1, ano);
            listaA.Adapter = adapter;
            return view;
        }

        private void Botao_Click(object sender, EventArgs e)
        {
            vf = true;
            int ano = int.Parse(listaA.SelectedItem.ToString());
            int m = int.Parse(listaM.SelectedItem.ToString());
            Main ma = (Main)Activity;
            ma.VerificarData(m, ano);
            Dismiss();
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            if (!vf)
            {
                int ano = DateTime.Today.Year;
                int m = DateTime.Today.Month;
                Main ma = (Main)Activity;
                ma.VerificarData(m, ano);
            }
            base.OnDismiss(dialog);
        }

    }
}