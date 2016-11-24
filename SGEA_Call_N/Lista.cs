using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SGEA_Call_N
{
    [BroadcastReceiver(Label = "SGEA Notifica��es")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider",Resource="@xml/lista")]
    public class Lista : Android.Appwidget.AppWidgetProvider
    {

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager,
            int[] appWidgetIds)
        {
            RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.widget);
            base.OnUpdate(context, appWidgetManager, appWidgetIds);
            appWidgetManager.UpdateAppWidget(appWidgetIds,views);
        }

    }
}