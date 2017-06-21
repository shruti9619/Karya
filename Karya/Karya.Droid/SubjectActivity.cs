using System;
using Android.App;
using Android.OS;
using Android.Widget;
using System.IO;
using Android.Content;
using System.Collections.Generic;
using Karya.Core;
using Newtonsoft.Json;

namespace Karya.Droid
{
    [Activity(Label = "Subjects")]
    public class SubjectActivity : Activity
    {
        GetSubjectClass getsub = new GetSubjectClass();
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Subjectlayout);
            // Create your application here
            List<Subject> slist = await getsub.ReadAllSubject();
            ListView sublv = FindViewById<ListView>(Resource.Id.sublist);
            sublv.Adapter= new SubjectScreenAdapter(this, slist);
            sublv.ItemClick += onsublistitemclick;
            Button subaddbut = FindViewById<Button>(Resource.Id.subaddbut);
            subaddbut.Click += onsubaddbutclick;

            Button subsdelbut = FindViewById<Button>(Resource.Id.delsubsbut);
            subsdelbut.Click += onsubdelbutclick;
        }

        private async void onsubdelbutclick(object sender, EventArgs e)
        {
            await getsub.DeleteAllSubjects();
        }

        private void onsubaddbutclick(object sender, EventArgs e)
        {
            //navigate to the other page
            var intent = new Intent(this, typeof(AddSubjectActivity));
            StartActivity(intent);
        }

        private void onsublistitemclick(object sender, EventArgs e)
        {
            
            //navigate to the other page
            var intent = new Intent(this, typeof(InsubjectActivity));
            var MySerializedObject = JsonConvert.SerializeObject(sender);
            intent.PutExtra("subobj", MySerializedObject);
            StartActivity(intent);
        }


    }
}