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

namespace Karya.Droid
{
    [Activity(Label = "Add Subject")]
    public class AddSubjectActivity : Activity
    {
        GetSubjectClass getsub = new GetSubjectClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.Addsubjectlayout);
            Button addsubbut = FindViewById<Button>(Resource.Id.subadd1but);
            Button cancelbut = FindViewById<Button>(Resource.Id.cancelbut);
          

            addsubbut.Click += onaddsubbutclick;
            cancelbut.Click += oncancelbutclick;

        }

        private void oncancelbutclick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private async void onaddsubbutclick(object sender, EventArgs e)
        {
           
            EditText subname = FindViewById<EditText>(Resource.Id.subnametext);
            EditText teachername = FindViewById<EditText>(Resource.Id.teachernametext);
            bool insertres=await getsub.Insert(teachername.Text, subname.Text);
            if (insertres)
            {
                var intent = new Intent(this, typeof(SubjectActivity));
                StartActivity(intent);
            }

            else
            {
                CreateAndShowDialog("Subject could not be created", "Failed");
                var intent = new Intent(this, typeof(SubjectActivity));
                StartActivity(intent);
            }
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