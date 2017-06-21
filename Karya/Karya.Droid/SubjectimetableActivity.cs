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
    [Activity(Label = "Subjects Details")]
    public class SubjectimetableActivity : Activity
    {
        GetEventClass getev = new GetEventClass();
        GetSubjectClass getsub = new GetSubjectClass();
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Subjecttimetablelayout);
            TextView subnametext = FindViewById<TextView>(Resource.Id.subnametext);
            TextView eventnumtext = FindViewById<TextView>(Resource.Id.eventnumtext);

            //subnametext.Text = e.Parameter as string;
            var MyJsonString = Intent.GetStringExtra("subname");
            var subname = JsonConvert.DeserializeObject<string>(MyJsonString);
            subnametext.Text = subname;
            int subid = 0;
            //this code has to handle how to retrieve the num of files associated with the subject, 
            //later events and attndnc as well
            //then the numbers should also redirect the user to the intended page when he touches it
            List<Event> allev = await getev.readallevent();
            List<Subject> sublist = await getsub.ReadAllSubject();
            List<Subject> sub = sublist.Where(x => x.Subjectname == subnametext.Text).ToList();
            if (sub.Any())
                subid = sub.FirstOrDefault().Subjectid;
            List<Event> subev = allev.Where(x => x.Subjectid == subid).ToList();
            eventnumtext.Text = subev.Count.ToString();
        }
    }
}