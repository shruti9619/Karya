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
    [Activity(Label = "Attendance")]
    public class AttendanceActivity : Activity
    {
        GetAttendanceClass getatt = new GetAttendanceClass();
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Attendancelayout);
            DatePicker datepic = FindViewById<DatePicker>(Resource.Id.datepick);
            ListView attlistview = FindViewById<ListView>(Resource.Id.attlistview);
            List<Attendance> attlist = await getatt.Getattschedule(await getatt.Getdayschedule(DateTime.Today.DayOfWeek));
            if(attlist.Any())
            attlistview.Adapter = new AttendanceScreenAdapter(this, attlist);
            datepic.Click += Datepic_Click;
            attlistview.ItemSelected += Attlistview_ItemSelected;

            Button totatbut = FindViewById<Button>(Resource.Id.detailviewbut);
            totatbut.Click += Totatbut_Click;
            Button delallatbut = FindViewById<Button>(Resource.Id.delattbut);
            delallatbut.Click += Delallatbut_Click;
        }

        private void Delallatbut_Click(object sender, EventArgs e)
        {
            GetAttendanceClass.deleteattendance();
        }

        private void Totatbut_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(TotalattendanceActivity));
            StartActivity(intent);
        }

        private void Attlistview_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //navigate to the other page
            var intent = new Intent(this, typeof(InattendanceActivity));
            var MySerializedObject = JsonConvert.SerializeObject(sender);
            intent.PutExtra("attendobj", MySerializedObject);
            StartActivity(intent);
        }

        private async void Datepic_Click(object sender, EventArgs e)
        {
            DatePicker datepick = FindViewById<DatePicker>(Resource.Id.datepick);
            DateTime day = datepick.DateTime;
            ///here add function call that checks with the day and brings up data from attndnce table
            List<Attendance> alist = await getatt.getattlist(day);
            ListView attlistview = FindViewById<ListView>(Resource.Id.attlistview);
            attlistview.Adapter = new AttendanceScreenAdapter(this, alist);

        }
    }
}