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
using System.Data.SqlClient;
using Mono.Data.Sqlite;
using System.IO;

namespace SGEA_Call
{
    public static class Connect
    {
        public static string getPath() => "sdcard/SGEA/sgea.db";
        
        public static SqliteConnection LiteString()
        {            
            string path = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path,"sgea.db");
            if (File.Exists(path))
            {
                SqliteConnectionStringBuilder c = new SqliteConnectionStringBuilder();
                c.DataSource = path;
                return new SqliteConnection(c.ConnectionString);
            }
            throw new NoDatabaseException();
        }
    }
}