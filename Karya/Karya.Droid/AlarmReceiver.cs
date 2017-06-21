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
using Newtonsoft.Json;

using Karya.Core;

namespace Karya.Droid
{

    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var resultIntent = new Intent(context, typeof(LoginActivity));
            var message = intent.GetStringExtra("message");
            var title = intent.GetStringExtra("title");
            var page = intent.GetStringExtra("page");

            if (page.ToString() == "ineventpage")
            { resultIntent = new Intent(context, typeof(IneventActivity));
            }
            if (page.ToString() == "inattendpage")
            {
                resultIntent = new Intent(context, typeof(InattendanceActivity));
            }

            if (intent.HasExtra("Evobj"))
            {
                var MyJsonStringev = intent.GetStringExtra("Evobj");
                var evobj = JsonConvert.DeserializeObject<Event>(MyJsonStringev);
                if (evobj != null)
                {
                    var MySerializedObject = JsonConvert.SerializeObject(evobj);
                    resultIntent.PutExtra("attendobj", MySerializedObject);
                }
            }

            if (intent.HasExtra("attendobj"))
            {
                var MyJsonStringatt = intent.GetStringExtra("attendobj");
                var attobj = JsonConvert.DeserializeObject<Attendance>(MyJsonStringatt);

                if (attobj != null)
                {
                    var MySerializedObject = JsonConvert.SerializeObject(attobj);
                    resultIntent.PutExtra("Evobj", MySerializedObject);
                }
            }

            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            
            var pending = PendingIntent.GetActivity(context, 0,
                resultIntent,
                PendingIntentFlags.CancelCurrent);

            var builder =
                new Notification.Builder(context)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetSmallIcon(Resource.Drawable.Icon)
                    .SetDefaults(NotificationDefaults.All);

            builder.SetContentIntent(pending);

            var notification = builder.Build();

            var manager = NotificationManager.FromContext(context);
            manager.Notify(1337, notification);
        }
    }
}
