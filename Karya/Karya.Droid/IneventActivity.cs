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
    [Activity(Label = "Event Details")]
    public class IneventActivity : Activity
    {

        GetEventClass getev = new GetEventClass();
        Event ev = new Event();
        List<Subject> lsub = new List<Subject>();

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            var MyJsonString = Intent.GetStringExtra("Evobj");
            var ev = JsonConvert.DeserializeObject<Event>(MyJsonString);
            // Create your application here

            SetContentView(Resource.Layout.Ineventlayout);
            TextView title = FindViewById<TextView>(Resource.Id.titletext);
            TextView desc = FindViewById<TextView>(Resource.Id.desctext);
            TextView time = FindViewById<TextView>(Resource.Id.timetext);
            TextView date = FindViewById<TextView>(Resource.Id.datetext);
            TextView repeat = FindViewById<TextView>(Resource.Id.repeattext);
            TextView subgrp = FindViewById<TextView>(Resource.Id.subgrouptext);
            Button delevbut = FindViewById<Button>(Resource.Id.delevbut);
            delevbut.Click += Delevbut_Click; 

            title.Text = ev.Title;
            if (ev.Description != null)
                desc.Text = ev.Description;
            else
                desc.Text = "No Description ";
            time.Text = ev.Time;
            date.Text = ev.Date;
            repeat.Text += ev.Repeat;
            string subname = "None";
            if (ev.Subjectid > -1)
            {
                try
                {
                    GetSubjectClass getsub = new GetSubjectClass();
                    List<Subject> ls = new List<Subject>();
                    lsub = await getsub.ReadAllSubject();
                    ls = lsub.Select(x => x).Where(x => x.Subjectid == ev.Subjectid).ToList();
                    if (ls.Any())
                        subname = ls.FirstOrDefault().Subjectname;

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            subgrp.Text += subname;


        }

        private async void Delevbut_Click(object sender, EventArgs e)
        {

            bool delres = await getev.DeleteEvent(ev);
            if (delres)
                System.Diagnostics.Debug.WriteLine("deleted event");

            var intent = new Intent(this, typeof(EventActivity));
            StartActivity(intent);


        }


    }
}