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
using Mono.Samples.LabelledSections;
using SGEA_Call.Classes;

namespace SGEA_Call.Janelas
{
    [Activity(Label = "Relatorio", Theme = "@style/MyTheme")]
    public class Relatorio : ListActivity
    {
        string[] codigo;
        ListItemCollection<ListItemValue> relatorio = new ListItemCollection<ListItemValue>();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Relatorio);
            var mConn = new SQLiteConnection(Connect.getPath());
            var select = mConn.Query<Classes.Relatorio>("select * from vwRelatorio");
            codigo = new string[select.Count];
            var imagemMain = FindViewById<ImageView>(Resource.Id.imageView1);
            imagemMain.Click += ImagemMain_Click;
            for (int i = 0; i < select.Count; i++)
            {
                codigo[i] = select[i].mesano;
                ListItemValue l = new ListItemValue(select[i].data);
                relatorio.Add(l);
            }
            var sortedContacts = relatorio.GetSortedYear();
            var adapter = CreateAdapter(sortedContacts);
            ListAdapter = adapter;
        }

        private void ImagemMain_Click(object sender, EventArgs e)
        {
            this.Return();
        }

        SeparatedListAdapter CreateAdapter<T>(Dictionary<string, List<T>> sortedObjects)
    where T : IHasLabel, IComparable<T>
        {
            var adapter = new SeparatedListAdapter(this);
            foreach (var e in sortedObjects.OrderBy(de => de.Key))
            {
                var label = e.Key;
                var section = e.Value;
                adapter.AddSection(label, new ArrayAdapter<T>(this, Resource.Layout.ListItem, section));
            }
            return adapter;
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var intent = new Intent(this, typeof(Orcamento));
            string s = l.Adapter.GetItem(position).ToString();
            intent.PutExtra("Código", l.Adapter.GetItem(position).ToString());
            StartActivity(intent);
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
        }

    }
}