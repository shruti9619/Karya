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
using Java.Util;
using Newtonsoft.Json;

namespace Karya.Droid
{
    class NotificationClass
    {

        DateTime duedt;

        public void assigntoast(Context cntxt,Event ev, params int[] num)
        {

            System.Diagnostics.Debug.WriteLine("on alarm intent");

            // Pass the current button press count value to the next activity:
            // Bundle valuesForActivity = new Bundle();
            //valuesForActivity.PutInt("count", count);

            // When the user clicks the notification, SecondActivity will start up.
            Intent resultIntent = new Intent(cntxt, typeof(EventActivity));
            
            // Pass some values to SecondActivity:
            //resultIntent.PutExtras(valuesForActivity);

            // Construct a back stack for cross-task navigation:
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(cntxt);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(EventActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            PendingIntent resultPendingIntent =
                stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);
           
           

            Intent alarmIntent = new Intent(Application.Context, typeof(AlarmReceiver));
            string desc = "";
            if (ev.Description != null)
                if (ev.Description.Length > 0)
                    desc = ev.Description;
                else { }
            else
                desc = "No Description";

            // Build the notification:
            Notification.Builder builder = new Notification.Builder(Application.Context)
                .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                .SetContentIntent(resultPendingIntent)  // Start 2nd activity when the intent is clicked.
                .SetContentTitle(ev.Title)      // Set its title
                                                        //.SetNumber(count)                       // Display the count in the Content Info
                .SetSmallIcon(Resource.Drawable.Icon)  // Display this icon
                .SetContentText(Java.Lang.String.Format(
                    desc)); // The message to display.

            var MySerializedObject = JsonConvert.SerializeObject(ev);
            alarmIntent.PutExtra("obj", MySerializedObject);
            alarmIntent.PutExtra("title", ev.Title);
            alarmIntent.PutExtra("message", desc);
            alarmIntent.PutExtra("page", "ineventpage");
            
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);

            Java.Util.Calendar calendar = Java.Util.Calendar.Instance;
            calendar.Set(CalendarField.HourOfDay, ev.Datetime.Hour);
            calendar.Set(CalendarField.Minute, ev.Datetime.Minute);

            alarmManager.Set(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);


            System.Diagnostics.Debug.WriteLine("assigning");
        


            int timevalue = 0;

            if ((num.Length > 0) && (ev.Repeat == "Hourly"))
                timevalue = num[0];


            switch (ev.Repeat)
            {
                case "Hourly":
                    while (duedt < ev.Datetime)
                    {
                        
                        alarmManager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis,
               1000 * 60 * timevalue, pendingIntent);
                       
                    }

                    break;

                case "Daily":
                    while (duedt < ev.Datetime)
                    {
                       
                        alarmManager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis,
               1000 * 86400, pendingIntent);
                        
                    }

                    break;

                case "Weekly":
                    while (duedt < ev.Datetime)
                    {
                        alarmManager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis,
              1000 * 86400*7, pendingIntent);
                      
                    }
                    break;

                case "Monthly":
                    while (duedt < ev.Datetime)
                    {
              //          alarmManager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis,
              //1000 * 86400, pendingIntent);
                       
                    }

                    break;

                case "Yearly":
                    while (duedt < ev.Datetime)
                    {
                        //duedt.AddMonths(12);
                        //new ToastNotification(toastXml);
                        //toastNotifier.AddToSchedule(new ScheduledToastNotification(toastXml, duedt));
                    }
                    break;


                case "Choose days":

                    //yet to be designed for day based repetition
                    break;

                default: break;

            }


        }




        public static async void ttattendnot(Context cntxt,Timetableobject tobj, int slottimeinminutes)
        {

            System.Diagnostics.Debug.WriteLine("on alarm intent");

            // Pass the current button press count value to the next activity:
            // Bundle valuesForActivity = new Bundle();
            //valuesForActivity.PutInt("count", count);

            // When the user clicks the notification, SecondActivity will start up.
            Intent resultIntent = new Intent(cntxt, typeof(EventActivity));

            // Pass some values to SecondActivity:
            //resultIntent.PutExtras(valuesForActivity);

            // Construct a back stack for cross-task navigation:
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(cntxt);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(EventActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            PendingIntent resultPendingIntent =
                stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            Intent alarmIntent = new Intent(Application.Context, typeof(AlarmReceiver));

            // Build the notification:
            Notification.Builder builder = new Notification.Builder(Application.Context)
                .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                .SetContentIntent(resultPendingIntent)  // Start 2nd activity when the intent is clicked.
                .SetContentTitle(tobj.Subjectname)      // Set its title
                                                        //.SetNumber(count)                       // Display the count in the Content Info
                .SetSmallIcon(Resource.Drawable.Icon)  // Display this icon
                .SetContentText(Java.Lang.String.Format(
                    "Store status of class at "+tobj.timestart)); // The message to display.

            alarmIntent.PutExtra("title", tobj.Subjectname);
            alarmIntent.PutExtra("message", "Store status of class at " + tobj.timestart);
            alarmIntent.PutExtra("page", "inattendpage");
           
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
           
            Java.Util.Calendar calendar = Java.Util.Calendar.Instance;
            TimeSpan ts = getstringtotime(tobj.timestart);
            int hours = 0;
            if ((slottimeinminutes > 60))
            {
                hours = slottimeinminutes / 60;
                slottimeinminutes = slottimeinminutes % 60;
            }

            calendar.Set(CalendarField.HourOfDay,ts.Hours +hours);
            calendar.Set(CalendarField.Minute, ts.Minutes-2+slottimeinminutes);
            alarmManager.SetInexactRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis,
              1000 * 86400 * 7, pendingIntent);
            //alarmManager.Set(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);
          
            Attendance a = new Attendance();
            List<Attendance> allat = await GetAttendanceClass.GetAttend(tobj.Subjectname);
            if (allat.Any())
                a = allat.FirstOrDefault();
            var MySerializedObject = JsonConvert.SerializeObject(a);
            alarmIntent.PutExtra("obj", MySerializedObject);
            
       
           // toast.Id = "tobj" + tobj.Ttobjectid.ToString();

        }

        //public static void removenot(int id)
        //{
        //    var scheduled = toastNotifier.GetScheduledToastNotifications();
        //    var evnotif = scheduled.Where(x => x.Id == "ev" + id.ToString()).FirstOrDefault();
        //    toastNotifier.RemoveFromSchedule(evnotif);
        //}

        public static void ttnot(Context cntxt, Timetableobject tobj)
        {


            System.Diagnostics.Debug.WriteLine("on alarm intent");

          

            // When the user clicks the notification, SecondActivity will start up.
            Intent resultIntent = new Intent(cntxt, typeof(EventActivity));

            // Construct a back stack for cross-task navigation:
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(cntxt);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(EventActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            PendingIntent resultPendingIntent =
                stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);
            Intent alarmIntent = new Intent(cntxt, typeof(AlarmReceiver));



            // Build the notification:
            Notification.Builder builder = new Notification.Builder(Application.Context)
                .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                .SetContentIntent(resultPendingIntent)  // Start 2nd activity when the intent is clicked.
                .SetContentTitle(tobj.Subjectname)      // Set its title
                                                        //.SetNumber(count)                       // Display the count in the Content Info
                .SetSmallIcon(Resource.Drawable.Icon)  // Display this icon
                .SetContentText(Java.Lang.String.Format(
                    "Your class begins at " + tobj.timestart)); // The message to display.
            alarmIntent.PutExtra("title", tobj.Subjectname);
            alarmIntent.PutExtra("message", "Your class begins at " + tobj.timestart);
            alarmIntent.PutExtra("page", "none");

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
            TimeSpan ts = getstringtotime(tobj.timestart);
            Java.Util.Calendar calendar = Java.Util.Calendar.Instance;
            calendar.Set(CalendarField.HourOfDay, ts.Hours);
            calendar.Set(CalendarField.Minute, ts.Minutes-2);

            //alarmManager.Set(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);
            alarmManager.SetInexactRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis,
              1000 * 86400*7, pendingIntent);
            
            //toast.Id = "tobj" + tobj.Ttobjectid.ToString();


        }

        //private static async void NotificationClass_Activated(ToastNotification sender, object args)
        //{
        //    XmlNodeList xml = sender.Content.GetElementsByTagName("text");
        //    string subname = xml[0].ToString();
        //    List<Attendance> alist = await GetAttendanceClass.GetAttend();
        //    List<Attendance> alistsub = alist.Where(x => x.Subjectname == subname).Distinct().ToList();
        //    if (alistsub.Any())
        //    {
        //        Attendance a = alistsub.FirstOrDefault();
        //    }
        //}



        public static TimeSpan getstringtotime(string t)
        {
            string hours = t.Split(':')[0];
            string sechalf = t.Split(':')[1];
            string minutes = sechalf.Split(' ')[0];
            string ampm = sechalf.Split(' ')[1];
            int hour = int.Parse(hours);
            int minute = int.Parse(minutes);

            if (ampm == "PM")
                hour += 12;
            TimeSpan times = new TimeSpan(hour, minute, 0);

            return times;

        }
    }
}