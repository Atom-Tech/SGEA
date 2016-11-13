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
using SQLite;
using Mono.Samples.LabelledSections;
using SGEA_Call.Classes;

namespace SGEA_Call.Janelas
{
    [Activity(Label = "Fornecedor", Theme = "@style/MyTheme")]
    public class Fornecedor : ListActivity
    {
        int[] codigo;
        ListItemCollection<ListItemValue> fornecedores = new ListItemCollection<ListItemValue>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Fornecedor);
            var imagemMain = FindViewById<ImageView>(Resource.Id.imageView1);
            imagemMain.Click += ImagemMain_Click;
            var mConn = new SQLiteConnection(Connect.getPath());
            var select = mConn.Query<Classes.Fornecedor>("select * from tbFornecedor order by nmFornecedor asc");
            codigo = new int[select.Count];
            for (int i = 0; i < select.Count; i++)
            {
                codigo[i] = select[i].cdFornecedor;
                ListItemValue l = new ListItemValue(select[i].nmFornecedor);
                fornecedores.Add(l);
            }
            var sortedContacts = fornecedores.GetSortedData();
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
            int m = 0;
            for (int x = 0; x < l.Adapter.Count; x++)
            {
                string s = l.Adapter.GetItem(x).ToString();
                if (s.Length == 1) m++;
                if (x == position) break;
            }
            position -= m;
            var intent = new Intent(this, typeof(DadosFornecedor));
            intent.PutExtra("Código", codigo[position]);
            StartActivity(intent);
            OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);

        }
    }
}
