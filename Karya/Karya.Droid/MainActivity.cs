using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Karya.Core;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace Karya.Droid
{
	[Activity (Label = "Karya",  Icon = "@drawable/logopic")]
	public class MainActivity : Activity
	{

       
        public User commonuser = AuthClass.commonuser;
        private MobileServiceUser user = AuthClass.user;
        public MobileServiceClient client = AuthClass.client;
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);


            CurrentPlatform.Init();

            // Get our button from the layout resource,
            // and attach an event to it
            Button gobut = FindViewById<Button>(Resource.Id.gobut);
            gobut.Click += ongobutclick;


        }

        private async void ongobutclick(object sender, EventArgs e)
        {
            //again u hv to validate the fields

            EditText usernametext = FindViewById<EditText>(Resource.Id.usernametext);
            EditText emailtext = FindViewById<EditText>(Resource.Id.emailtext);


            await AuthClass.insertuser(usernametext.Text, emailtext.Text);
            //navigate to the home page
            var intent = new Intent(this, typeof(HomeActivity));
            StartActivity(intent);
        }


        private void CreateAndShowDialog(string message, string title)
        {

            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

       



    }
}


