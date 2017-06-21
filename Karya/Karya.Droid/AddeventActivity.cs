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
    [Activity(Label = "Add Event")]
    public class AddeventActivity : Activity
    {
        GetSubjectClass getsub = new GetSubjectClass();
        List<Subject> lsub = new List<Subject>();
        List<string> lsubname = new List<string>();
        Event ev = new Event();
        GetEventClass getev = new GetEventClass();
        public static DateTime d;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Addeventlayout);
            lsub = await getsub.ReadAllSubject();
            lsubname = lsub.Select(x => x.Subjectname).ToList();
            lsubname.Add("None");

            List<string> repeatoption = new List<string>();
            repeatoption.Add("Choose days");
            repeatoption.Add("Hourly");
            repeatoption.Add("Daily");
            repeatoption.Add("Weekly");
            repeatoption.Add("Monthly");
            repeatoption.Add("Yearly");
            Spinner repeatspin = FindViewById<Spinner>(Resource.Id.repeatspin);
            string[] reparr = repeatoption.ToArray<string>();
            ArrayAdapter repadapter = new ArrayAdapter(this, Resource.Layout.simplespinnerlayout, reparr);
            repeatspin.Adapter = repadapter;
            Spinner subspin = FindViewById<Spinner>(Resource.Id.subspin);
            string[] subarr = lsubname.ToArray<string>();
            subspin.Adapter = new ArrayAdapter(this, Resource.Layout.simplespinnerlayout, subarr);

            Button savebut = FindViewById<Button>(Resource.Id.saveevbut);
            savebut.Click += Savebut_Click;
        }

        private async void Savebut_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("save buttin clicked");
            //apply validation methods

            EditText titlebox = FindViewById<EditText>(Resource.Id.Titletext);
            EditText descbox = FindViewById<EditText>(Resource.Id.desctext);
            EditText numtextbox = FindViewById<EditText>(Resource.Id.repeattext);
            DatePicker evdatepicker = FindViewById<DatePicker>(Resource.Id.datePicker1);
            TimePicker evtimepicker = FindViewById<TimePicker>(Resource.Id.timePicker1);

            ev.Title = titlebox.Text;
            if (descbox.Text != null)
                if (descbox.Text.Length > 0)
                    ev.Description = descbox.Text;
            List<Event> lev = new List<Event>();
            try
            {
                lev = await getev.readallevent();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            int maxtabid;
            if (lev.Any())
            {
                lev.OrderBy(x => x.Timetableid);
                maxtabid = lev[0].Timetableid;
            }
            else
            {
                maxtabid = 0;
            }
            ev.Timetableid = maxtabid + 1;
            ev.id = ev.Timetableid.ToString();
            TimeSpan t = new TimeSpan(evtimepicker.Hour, evtimepicker.Minute, 0);
            ev.Time = gettexttime(t);
            ev.Date = evdatepicker.DateTime.Date.ToString();
            
            ev.Datetime = new DateTime(evdatepicker.DateTime.Year, evdatepicker.DateTime.Month, evdatepicker.DateTime.Day, t.Hours, t.Minutes, t.Seconds);
            Spinner subjectcombobox = FindViewById<Spinner>(Resource.Id.subspin);
            ev.Userid = AuthClass.commonuser.Userid;
            if (subjectcombobox.SelectedItemPosition != -1)
            {
                Subject sub = new Subject();
                List<Subject> ls = new List<Subject>();
                ls = lsub.Select(x => x).Where(x => x.Subjectname == subjectcombobox.SelectedItem.ToString()).ToList();
                if (ls.Any())
                {
                    sub = ls.FirstOrDefault();
                    ev.Subjectid = sub.Subjectid;
                }

            }
            NotificationClass nc = new NotificationClass();

            Spinner repeatcombobox = FindViewById<Spinner>(Resource.Id.subspin);
            if (repeatcombobox.SelectedItemPosition != -1)
            {

                switch (repeatcombobox.SelectedItem.ToString())
                {

                    case "Hourly": ev.Repeat = "Hourly"; nc.assigntoast(this,ev, int.Parse(numtextbox.Text)); break;
                    case "Daily": ev.Repeat = "Daily"; nc.assigntoast(this,ev); break;
                    case "Weekly": ev.Repeat = "Weekly"; nc.assigntoast(this,ev); break;
                    case "Monthly": ev.Repeat = "Monthly"; nc.assigntoast(this,ev); break;
                    case "Yearly": ev.Repeat = "Yearly"; nc.assigntoast(this,ev); break;
                    //here get the children of gday check if they are checked and return to the params to count in 0s and 1s
                    case "Choose days": ev.Repeat = "Choose days"; nc.assigntoast(this,ev); break;
                    default: ev.Repeat = ""; nc.assigntoast(this,ev); break;

                }
            }
            else
                ev.Repeat = ""; nc.assigntoast(this,ev);



            System.Diagnostics.Debug.WriteLine("assigned");
            try
            {
                getev.insertevent(ev);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
            finally
            {
                var myintent = new Intent(this, typeof(EventActivity));
                StartActivity(myintent);
            }
        }

        public string gettexttime(TimeSpan t)
        {
            string s;
            string dn;
            double hours = t.Hours;
            if (hours > 12)
            {
                hours = (int)hours % 12;
                dn = "PM";

            }
            else
                dn = "AM";

            s = hours.ToString() + ":" + t.ToString("mm") + " " + dn;
            return s;

        }
    }
}