using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Security.Credentials;
using System.Windows;
using Windows.UI.Popups;
using System.Net.Http;
using System.IO;
using Karya.Core;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

namespace Karya.WinPhone
{
    public class AuthClass
    {
        public static User commonuser = new User();
        private static MobileServiceUser user;
        public static MobileServiceClient client;
        private static IMobileServiceTable<User> userTable;


       

        public static MobileServiceSQLiteStore store = new MobileServiceSQLiteStore(App.DB_PATH);


        public static void InitTables()
        {
            store = new MobileServiceSQLiteStore(App.DB_PATH);
            store.DefineTable<Subject>();
            store.DefineTable<Timetable>();
            store.DefineTable<Timetableobject>();
            store.DefineTable<Event>();
            store.DefineTable<Attendance>();
            App.addtodebug("tables created");
        }

        public static async Task InitTimetableLocalStoreAsync()
        {

            if (!client.SyncContext.IsInitialized)
            {
                await client.SyncContext.InitializeAsync(store);
                App.addtodebug("tables synced");
            }

        }
        public AuthClass()
        {
            client = new MobileServiceClient("https://karyaback.azurewebsites.net");

            userTable = client.GetTable<User>();
            InitTables();
            App.addtodebug("all tables created");


        }





        public static async Task<bool> AuthenticateAsync()
        {
            string message = null;
            bool success = false;

            // This sample uses the Facebook provider.
            var provider = MobileServiceAuthenticationProvider.MicrosoftAccount;

            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            try
            {
                // Try to get an existing credential from the vault.
                credential = vault.FindAllByResource(provider.ToString()).FirstOrDefault();
            }
            catch (Exception)
            {
                // When there is no matching resource an error occurs, which we ignore.
            }

            if (credential != null)
            {
                // Create a user from the stored credentials.
                user = new MobileServiceUser(credential.UserName);
                credential.RetrievePassword();
                user.MobileServiceAuthenticationToken = credential.Password;

                // Set the user from the stored credentials.
                client.CurrentUser = user;

                // Consider adding a check to determine if the token is 
                // expired, as shown in this post: http://aka.ms/jww5vp.

                success = true;
               // message = string.Format("Cached credentials for user - {0}", user.UserId);
            }
            else
            {
                try
                {
                    // Login with the identity provider.
                    user = await client.LoginAsync(provider);

                    // Create and store the user credentials.
                    credential = new PasswordCredential(provider.ToString(),
                        user.UserId, user.MobileServiceAuthenticationToken);
                    vault.Add(credential);
                    var userInfo = await client.InvokeApiAsync("userInfo", HttpMethod.Get, null);
                    string text = string.Format("User info: {0}", userInfo);
                    //MessageDialog m = new MessageDialog(text);
                    success = true;
                }
                catch (MobileServiceInvalidOperationException)
                {
                    message = "You must log in. Login Required";
                }
                catch (InvalidOperationException)
                { message = "Requires internet connection"; }
            }

            if (message != null)
            {
                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }

            return success;
        }



        public static async Task insertuser(String name, String email)
        {


            string prefedname = null;

            User maxuser = null;

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
            await InitTimetableLocalStoreAsync();
            App.addtodebug("all synced");
        }



        public static async void AuthenticateOutAsync()
        {
            await client.LogoutAsync();
        }

    }
}

