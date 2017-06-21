using System.IO;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;

namespace Karya.Droid
{
    public class AppClass
    {

        public static string pathDbs = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Database";
        

    
        public AppClass()
        {
        }

        public static void addtodebug(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }

        public static async void pushasync()
        {
            try
            {
                AppClass.addtodebug("push async to come");
                await AuthClass.client.SyncContext.PushAsync();
            }
            catch (Exception e)
            {
                AppClass.addtodebug("push prob " + e.Message);
             
            }
        }
    }
}

