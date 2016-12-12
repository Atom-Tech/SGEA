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

namespace SGEA_Call.Janelas
{
    [Activity(Label = "JMenu", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class JMenu : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        public override bool OnCreateOptionsMenu(IMenu menus)
        {
            menus.Clear();
            MenuInflater.Inflate(Resource.Menu.JMenu,menus);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent i = null;
            switch (item.ItemId)
            {
                case Resource.Id.cliente:
                    i = new Intent(this, typeof(Cliente));
                    break;
                default:
                    return base.OnOptionsItemSelected(item);
            }
            if (i != null)
            {
                StartActivity(i);
                OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}