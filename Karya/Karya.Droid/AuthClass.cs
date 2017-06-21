using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Net.Http;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

using Android.App;
using Android.Views;
using Android;
using Android.Widget;
using Android.OS;
using Karya.Core;
using System.IO;

namespace Karya.Droid
{
    public class AuthClass
    {
        public static User commonuser = new User();
        public static MobileServiceUser user;
        public static MobileServiceClient client;
        private static IMobileServiceTable<User> userTable;

       
        
        private IMobileServiceTable<User> userTable1=userTable;

        

        public static MobileServiceSQLiteStore store = new MobileServiceSQLiteStore(AppClass.pathDbs);


        public static void InitTables()
        {
            store = new MobileServiceSQLiteStore(AppClass.pathDbs);
            store.DefineTable<Subject>();
            store.DefineTable<Timetable>();
            store.DefineTable<Timetableobject>();
            store.DefineTable<Event>();
            store.DefineTable<Attendance>();
            AppClass.addtodebug("tables created");
        }

        public static async Task InitTimetableLocalStoreAsync()
        {

            if (!client.SyncContext.IsInitialized)
            {
                await client.SyncContext.InitializeAsync(store);
                AppClass.addtodebug("tables synced");
            }

        }
        public AuthClass()
        {
            client = new MobileServiceClient("https://karyaback.azurewebsites.net");

            userTable = client.GetTable<User>();
            InitTables();
            AppClass.addtodebug("all tables created");


        }


        private void CreateAndShowDialog(Exception exception, String title)
        {
            //CreateAndShowDialog(exception.Message, title);
        }

        






        // Define a authenticated user.
       // private MobileServiceUser user;
        




        public static async Task insertuser(String name, String email)
        {


            string prefedname = null;

            User maxuser = null;
            try
            {
                IMobileServiceTableQuery<string> ss = userTable.Where(x => x.Username == name.ToString() || x.Email == email.ToString()).Select(x => x.Username);
                List<string> sslist = await ss.ToListAsync();
                if (sslist.Any())
                    prefedname = sslist.FirstOrDefault();

                //  prefedname = userTable.Where(x => x.Username == "ritu" || x.Email == email.ToString()).Select(x => x.Username).ToString();
                System.Diagnostics.Debug.WriteLine(prefedname);

                //check if table is empty first
                IMobileServiceTableQuery<User> query = userTable.OrderByDescending(user => user.Userid);
                List<User> maxuserlist = await query.ToListAsync();
                if (maxuserlist.Any())
                {
                    maxuser = maxuserlist[0];
                    System.Diagnostics.Debug.WriteLine(maxuser.Userid);
                }



                if (prefedname == null)
                {
                    System.Diagnostics.Debug.WriteLine("name not found");
                    User newuser = new User();
                    int uid = maxuser.Userid;
                    newuser.Userid = uid + 1;
                    newuser.Email = email;
                    newuser.Username = name;
                    newuser.id = (uid + 1).ToString();
                    newuser.updatedAt = newuser.createdAt = DateTime.Today;
                    commonuser = newuser;
                    await userTable.InsertAsync(newuser);
                }
                //when user exists and we have to pass its userid to the getsubject page
                else
                {

                    System.Diagnostics.Debug.WriteLine("name found");

                    IMobileServiceTableQuery<int> s = userTable.Where(x => x.Username == name.ToString()).Select(x => x.Userid);

                    List<int> slist = await s.ToListAsync();
                    if (slist.Any())
                    {
                        commonuser.Userid = slist.FirstOrDefault();

                    }
                }

            }
            catch (Exception exc)
            { AppClass.addtodebug(exc.Message); }
            await InitTimetableLocalStoreAsync();
            AppClass.addtodebug("all synced");
        }



        public static async void AuthenticateOutAsync()
        {
            await client.LogoutAsync();
        }

    }
}

