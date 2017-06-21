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

using Karya.Core;
using Newtonsoft.Json;

namespace Karya.Droid
{
    [Activity(Label = "Subject Details")]
    public class InsubjectActivity : Activity
    {
        Subject passedsub = new Subject();
        GetSubjectClass getsub = new GetSubjectClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var MyJsonString = Intent.GetStringExtra("subobj");
            var sub = JsonConvert.DeserializeObject<Subject>(MyJsonString);
            passedsub = sub;
            // Create your application here
            SetContentView(Resource.Layout.Insubjectlayout);
            ListView filelsv = FindViewById<ListView>(Resource.Id.filelistview);
            Button editbut = FindViewById<Button>(Resource.Id.editbut);
            Button delbut = FindViewById<Button>(Resource.Id.delbut);
            editbut.Click += Editbut_Click;
            delbut.Click += Delbut_Click;
        }

        private async void Delbut_Click(object sender, EventArgs e)
        {
            Subject delsub = passedsub;

            var delres = await getsub.DeleteSubject(delsub);
            if (!delres)
            { //code to handle what happens when name already exists and update cant take place
               
                Toast.MakeText(this, "Subject doesn't exist any longer. Please restart the app.", ToastLength.Long);

            }
            var intent = new Intent(this, typeof(IneventActivity));
            StartActivity(intent);
            
        }

        private void Editbut_Click(object sender, EventArgs e)
        {
            
        }
    }
}