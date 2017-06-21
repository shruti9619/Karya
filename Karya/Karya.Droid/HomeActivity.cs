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
    [Activity(Label = "Home")]
    public class HomeActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Homelayout);
            Button clickbut = FindViewById<Button>(Resource.Id.buttonclick);
            //Button settingbut = FindViewById<Button>(Resource.Id.settingbut);
            Button eventbut = FindViewById<Button>(Resource.Id.eventbutton);
            Button timetablebut = FindViewById<Button>(Resource.Id.timetablebutton);
            Button subjectbut = FindViewById<Button>(Resource.Id.subjectbutton);
            Button vocabbut = FindViewById<Button>(Resource.Id.vocabbutton);
            Button attendbut = FindViewById<Button>(Resource.Id.attendbutton);

            clickbut.Click += onclickbutclick;
            //settingbut.Click += onsettingbutclick;
            eventbut.Click += oneventbutclick;
            timetablebut.Click += ontimetablebutclick;
            subjectbut.Click += onsubjectbutclick;
            vocabbut.Click += onvocabbutclick;
            attendbut.Click += onattendbutclick;



        }

        //navigation functions

        private void onattendbutclick(object sender, EventArgs e)
        {
            //navigate to the other page
            var intent = new Intent(this, typeof(AttendanceActivity));
            StartActivity(intent);
        }

        private void onvocabbutclick(object sender, EventArgs e)
        {
            //navigate to the other page
            var intent = new Intent(this, typeof(VocabActivity));
            StartActivity(intent);
        }

        private void onsubjectbutclick(object sender, EventArgs e)
        {
            //navigate to the other page
            var intent = new Intent(this, typeof(SubjectActivity));
            StartActivity(intent);
        }

        private void ontimetablebutclick(object sender, EventArgs e)
        {
            //navigate to the other page
            var intent = new Intent(this, typeof(TimetableActivity));
            StartActivity(intent);
        }

        private void oneventbutclick(object sender, EventArgs e)
        {
            //navigate to the other page
            var intent = new Intent(this, typeof(EventActivity));
            StartActivity(intent);
        }

        private void onsettingbutclick(object sender, EventArgs e)
        {
            //navigate to the other page
            //var intent = new Intent(this, typeof(SettingActivity));
            //StartActivity(intent);
        }

        private void onclickbutclick(object sender, EventArgs e)
        {
            //navigate to the other page
            var intent = new Intent(this, typeof(ClickActivity));
            StartActivity(intent);
        }
    }
}