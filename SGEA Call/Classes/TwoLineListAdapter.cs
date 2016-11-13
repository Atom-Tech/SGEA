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

namespace SGEA_Call.Classes
{
    public class TwoLineListAdapter : ArrayAdapter<Tuple<string, string>>
    {

        Activity context;
        public TwoLineListAdapter(Activity context, IList<Tuple<string, string>> objects)
            : base(context, Android.Resource.Id.Text1, objects)
        {
            this.context = context;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = context.LayoutInflater.Inflate(Android.Resource.Layout.TwoLineListItem, null);

            var item = GetItem(position);
            var text1 = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            text1.Text = item.Item1;
            text1.SetTextColor(Android.Graphics.Color.Black);
            text1.TextSize = 18;
            var text2 = view.FindViewById<TextView>(Android.Resource.Id.Text2);
            text2.Text = item.Item2;
            text2.SetTextColor(Android.Graphics.Color.Black);
            return view;
        }
    }
}