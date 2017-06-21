using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Karya.Core;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.IO;

namespace Karya.Droid
{

    [Activity(Label = "Karya", MainLauncher = true, Icon = "@drawable/logopic")]
    public class LoginActivity : Activity
    {

        public User commonuser = AuthClass.commonuser;
        private MobileServiceUser user;
        public MobileServiceClient client = AuthClass.client;



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Loginpage);
            // Create your application here
            Button signinbutton = FindViewById<Button>(Resource.Id.signinbut);
            Button signupbutton = FindViewById<Button>(Resource.Id.signupbut);

            signinbutton.Click += onsigninbutclick;
        }





        public async Task<bool> AuthenticateAsync()
        {
            var success = false;
            try
            {
                // Sign in with Microsoft login using a server-managed flow.
                user = await AuthClass.client.LoginAsync(this,
                    MobileServiceAuthenticationProvider.MicrosoftAccount);
                CreateAndShowDialog(string.Format("you are now logged in - {0}",
                    user.UserId), "Logged in!");

                success = true;
            }
            catch (Exception ex)
            {
                CreateAndShowDialog(ex.Message, "Authentication failed");
            }
            return success;
        }


        private void CreateAndShowDialog(string message, string title)
        {

            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }



        public async void onsigninbutclick(object sender, EventArgs e)
        {
            try
            {
                    Directory.CreateDirectory(AppClass.pathDbs);
                    string DB_PATH = Path.Combine(AppClass.pathDbs, "Karyadb.db3");
            }
            catch (Exception ex)
            {
                AppClass.addtodebug(ex.Message);
            }
            new AuthClass();
            var authresult = await AuthenticateAsync();
            if (authresult)
            {
                
            //navigate to the other page
                   var intent = new Intent(this, typeof(MainActivity));                                  
                   StartActivity(intent);
            }


}

    }
}