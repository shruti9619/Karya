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
    [Activity(Label = "Events")]
    public class EventActivity : Activity
    {
        GetEventClass getev = new GetEventClass();
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Eventlayout);
            // Create your application here
            Button evaddbut = FindViewById<Button>(Resource.Id.butev);
            evaddbut.Click += onevsavebutclick;

            Button evsdelbut = FindViewById<Button>(Resource.Id.delevsbut);
            evsdelbut.Click += onevdelbutclick;
            List<Event> evlist = await getev.readallevent();
            ListView evlv = FindViewById<ListView>(Resource.Id.eventlist);


            evlv.Adapter = new EventScreenAdapter(this, evlist);
            evlv.ItemClick += onevlistitemclick;
            
        }

        private async void onevdelbutclick(object sender, EventArgs e)
        {
            await getev.DeleteAllEvents();
        }

        private void onevsavebutclick(object sender, EventArgs e)
        {
            //navigate to the other page
            System.Diagnostics.Debug.WriteLine("ev but clicked");
            var intentss = new Intent(this, typeof(AddeventActivity));
            StartActivity(intentss);
        }

        private void onevlistitemclick(object sender, EventArgs e)
        {
            //navigate to the other page
            var intent = new Intent(this, typeof(IneventActivity));
            var MySerializedObject = JsonConvert.SerializeObject(sender);
            intent.PutExtra("Evobj", MySerializedObject);
            StartActivity(intent);
            
        }

    }
    }
