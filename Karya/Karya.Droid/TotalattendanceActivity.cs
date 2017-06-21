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

namespace Karya.Droid
{
    [Activity(Label = "Total Attendance")]
    public class TotalattendanceActivity : Activity
    {
        GetSubjectClass getsub = new GetSubjectClass();
        protected  async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Totalattendancelayout);
            Spinner subspin = FindViewById<Spinner>(Resource.Id.subspinner);
            List<Subject> slist = await getsub.ReadAllSubject();
            string[] subname = slist.Select(x => x.Subjectname).ToArray<string>();
            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.simplespinnerlayout, subname);
            subspin.Adapter = adapter;
            subspin.ItemSelected += Subspin_ItemSelected;
            Button resetsubbut = FindViewById<Button>(Resource.Id.resetsubatbut);
            resetsubbut.Click += Resetsubbut_Click;
        }

        private async void Resetsubbut_Click(object sender, EventArgs e)
        {
            Spinner subspin = FindViewById<Spinner>(Resource.Id.subspinner);
            if (subspin.SelectedItemPosition != -1)
            {
                List<Attendance> alist = await GetAttendanceClass.GetAttend();
                List<Attendance> alistsub = alist.Where(x => x.Subjectname == subspin.SelectedItem.ToString()).Distinct().ToList();
                if (alistsub.Any())
                {
                    Attendance a = alistsub.FirstOrDefault();
                    a.Attendedclass = 0;
                    a.Bunkedclass = 0;
                    a.Totalclass = 0;
                    GetAttendanceClass g = new GetAttendanceClass();
                    g.updateattendance(a);
                }
            }
        }

        private async void Subspin_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner c = sender as Spinner;
            TextView totnmblk = FindViewById<TextView>(Resource.Id.totalinputtext1);
            TextView atnmblk = FindViewById<TextView>(Resource.Id.totalattendtext1);
            TextView bnknmblk = FindViewById<TextView>(Resource.Id.bunkedtext1);
            List<Attendance> alist = await GetAttendanceClass.GetAttend();
            List<Attendance> alistsub = alist.Where(x => x.Subjectname == c.SelectedItem.ToString()).Distinct().ToList();
            if (alistsub.Any())
            {
                totnmblk.Text = alistsub.FirstOrDefault().Totalclass.ToString();
                atnmblk.Text = alistsub.FirstOrDefault().Attendedclass.ToString();
                bnknmblk.Text = alistsub.FirstOrDefault().Bunkedclass.ToString();
            }
        }
    }
}