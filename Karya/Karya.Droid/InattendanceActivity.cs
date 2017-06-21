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
    [Activity(Label = "Attendance Details")]
    public class InattendanceActivity : Activity
    {
        Attendance a = new Attendance();
        GetAttendanceClass getat = new GetAttendanceClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Inattendancelayout);
            TextView totalatt = FindViewById<TextView>(Resource.Id.totalattendtext);
            TextView subname = FindViewById<TextView>(Resource.Id.subnametext);
            TextView total = FindViewById<TextView>(Resource.Id.totalinputtext);
            TextView totalbunk = FindViewById<TextView>(Resource.Id.bunkedtext);
            TextView statusondate = FindViewById<TextView>(Resource.Id.statusondatetext);


            var MyJsonString = Intent.GetStringExtra("attendobj");
            a = JsonConvert.DeserializeObject<Attendance>(MyJsonString);

            subname.Text = a.Subjectname;
            if (a.Isattended)
                statusondate.Text += a.Date.Date.ToString() + ": Attended";
            if (a.Isbunked)
                statusondate.Text += a.Date.Date.ToString() + ": Bunked";
            if (a.Isnoclass)
                statusondate.Text += a.Date.Date.ToString() + ": No Class";

            total.Text = a.Totalclass.ToString();
            totalatt.Text = a.Attendedclass.ToString();
            totalbunk.Text = a.Bunkedclass.ToString();


            Button noclass = FindViewById<Button>(Resource.Id.noclassbut);
            Button attended = FindViewById<Button>(Resource.Id.attendedbut);
            Button bunked = FindViewById<Button>(Resource.Id.bunkedbut);

        }


        private void noclassbut_Click(object sender,EventArgs e)
        {
            if (a.Isattended)
            {
                a.Isattended = false;
                a.Totalclass -= 1;
                a.Attendedclass -= 1;
            }

            if (a.Isbunked)
            {
                a.Isbunked = false;
                a.Totalclass -= 1;
                a.Bunkedclass -= 1;
            }

            a.Isnoclass = true;

            getat.updateattendance(a);
        }

        private void attendedbut_Click(object sender, EventArgs e)
        {
            if (a.Isbunked)
            {
                a.Isbunked = false;
                a.Attendedclass += 1;
                a.Bunkedclass -= 1;
            }

            if (a.Isnoclass)
            {
                a.Isnoclass = false;
                a.Attendedclass += 1;
                a.Totalclass += 1;
            }

            if (!a.Isattended)
            {
                a.Isnoclass = false;
                a.Attendedclass += 1;
                a.Totalclass += 1;
            }

            a.Isattended = true;
            getat.updateattendance(a);
        }

        private void bunkedbut_Click(object sender, EventArgs e)
        {
            if (a.Isattended)
            {
                a.Isattended = false;
                a.Attendedclass -= 1;
                a.Bunkedclass += 1;
            }

            if (a.Isnoclass)
            {
                a.Isnoclass = false;
                a.Bunkedclass += 1;
                a.Totalclass += 1;
            }

            if (!a.Isbunked)
            {
                a.Isnoclass = false;
                a.Bunkedclass += 1;
                a.Totalclass += 1;
            }

            a.Isbunked = true;
            getat.updateattendance(a);
        }
    
}
}