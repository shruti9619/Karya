using System;
using System.IO;
using Windows.Storage;

namespace Karya
{
    public class AppClass
    {

        public static string DB_PATH = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "Karyadb.db3"));//DataBase Name 


        public AppClass()
        {
        }

        public static void addtodebug(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }
    }
}

