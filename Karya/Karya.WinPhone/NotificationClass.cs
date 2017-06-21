using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Karya.Core;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;

namespace Karya.WinPhone
{
    public class NotificationClass
    {
        DateTime duedt;
        /// <summary>
        /// this functtion will generate the toast and schedule it 
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>


        static ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();

        public void assigntoast(Event ev, params int[] num)
        {
            App.addtodebug("assigning");
            string desc="";
            ToastTemplateType ttype = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ttype);
            XmlNodeList toastTextElement = toastXml.GetElementsByTagName("text");
            toastTextElement[0].AppendChild(toastXml.CreateTextNode(ev.Title));
            if (ev.Description != null)
                if (ev.Description.Length > 0)
                    desc = ev.Description;
                else { }
            else
                desc = "No Description";
            toastTextElement[1].AppendChild(toastXml.CreateTextNode(desc));
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");
            duedt = ev.Datetime;

            var launchAttribute = toastXml.CreateAttribute("launch");

            var MySerializedObject = JsonConvert.SerializeObject(ev);

            var toastNavigationUriString = "#/InEventPage.xaml?param1=" + MySerializedObject;
            var toastElement = ((XmlElement)toastXml.SelectSingleNode("/toast"));
            toastElement.SetAttribute("launch", toastNavigationUriString);
            new ToastNotification(toastXml);
            var toast = new Windows.UI.Notifications.ScheduledToastNotification(toastXml, duedt);
            toast.Id = "ev"+ev.Eventid.ToString();
           
            toastNotifier.AddToSchedule(new ScheduledToastNotification(toastXml, duedt));
            

            

            int timevalue = 0;

            if ((num.Length > 0) && (ev.Repeat == "Hourly"))
                timevalue = num[0];


            switch (ev.Repeat)
            {
                case "Hourly":
                    while (duedt < ev.Datetime)
                    {
                        duedt.AddHours(timevalue);
                        new ToastNotification(toastXml);
                        toastNotifier.AddToSchedule(new ScheduledToastNotification(toastXml, duedt));
                    }

                    break;

                case "Daily":
                    while (duedt < ev.Datetime)
                    {
                        duedt.AddDays(1);
                        new ToastNotification(toastXml);
                        toastNotifier.AddToSchedule(new ScheduledToastNotification(toastXml, duedt));
                    }

                    break;

                case "Weekly":
                    while (duedt < ev.Datetime)
                    {
                        duedt.AddDays(7);
                        new ToastNotification(toastXml);
                        toastNotifier.AddToSchedule(new ScheduledToastNotification(toastXml, duedt));
                    }
                    break;

                case "Monthly":
                    while (duedt < ev.Datetime)
                    {
                        duedt.AddMonths(1);
                        new ToastNotification(toastXml);
                        toastNotifier.AddToSchedule(new ScheduledToastNotification(toastXml, duedt));
                    }

                    break;

                case "Yearly":
                    while (duedt < ev.Datetime)
                    {
                        duedt.AddMonths(12);
                        new ToastNotification(toastXml);
                        toastNotifier.AddToSchedule(new ScheduledToastNotification(toastXml, duedt));
                    }
                    break;


                case "Choose days":

                   //yet to be designed for day based repetition
                    break;

                default:break;

            }


        }


        public static void removenot(int id)
        {
            var scheduled = toastNotifier.GetScheduledToastNotifications();
            var evnotif=scheduled.Where(x => x.Id == "ev"+id.ToString()).FirstOrDefault();
            toastNotifier.RemoveFromSchedule(evnotif);
        }

        public static void ttnot(Timetableobject tobj)
        {
            ToastTemplateType ttype = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ttype);
            XmlNodeList toastTextElement = toastXml.GetElementsByTagName("text");
            toastTextElement[0].AppendChild(toastXml.CreateTextNode(tobj.Subjectname));
            string msg= "Your class begins at "+tobj.timestart;
            toastTextElement[1].AppendChild(toastXml.CreateTextNode(msg));
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");
            TimeSpan ts = getstringtotime(tobj.timestart);
            DateTime duedt = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day, ts.Hours, ts.Minutes-2, ts.Seconds);

            TimeSpan t = new TimeSpan(7,0, 0, 0);
     
            new ToastNotification(toastXml);
            toastNotifier.AddToSchedule(new ScheduledToastNotification(toastXml, duedt, t, int.MaxValue));
            var toast = new Windows.UI.Notifications.ScheduledToastNotification(toastXml, duedt, t,int.MaxValue);
            toast.Id = "tobj" + tobj.Ttobjectid.ToString();
            
           
        }


        public static async void ttattendnot(Timetableobject tobj,int slottimeinminutes)
        {
            ToastTemplateType ttype = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ttype);
            XmlNodeList toastTextElement = toastXml.GetElementsByTagName("text");
            toastTextElement[0].AppendChild(toastXml.CreateTextNode(tobj.Subjectname));
            string msg = "Store status of class at " + tobj.timestart;
            toastTextElement[1].AppendChild(toastXml.CreateTextNode(msg));
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");

            var launchAttribute = toastXml.CreateAttribute("launch");

            Attendance a = new Attendance();
            List<Attendance> allat = await GetAttendanceClass.GetAttend(tobj.Subjectname);
            if (allat.Any())
                a = allat.FirstOrDefault();
            var MySerializedObject = JsonConvert.SerializeObject(a);
            
            var toastNavigationUriString = "#/Inattendpage.xaml?param1="+MySerializedObject;
            var toastElement = ((XmlElement)toastXml.SelectSingleNode("/toast"));
            toastElement.SetAttribute("launch", toastNavigationUriString);
            
            //var toastNavigationUriString = "/Inattendpage.xaml?param1=12345";
            //var toastElement = ((XmlElement)toastXml.SelectSingleNode("/toast"));
            //toastElement.SetAttribute("launch", toastNavigationUriString);


            TimeSpan ts = getstringtotime(tobj.timestart);
            int hours = 0;
            if ((slottimeinminutes > 60))
            {
                hours = slottimeinminutes/ 60;
                slottimeinminutes = slottimeinminutes % 60;
            }
            
            DateTime duedt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, ts.Hours+hours, ts.Minutes-2+slottimeinminutes, ts.Seconds);

            TimeSpan t = new TimeSpan(7, 0, 0, 0);

            new ToastNotification(toastXml).Activated += NotificationClass_Activated;
            toastNotifier.AddToSchedule(new ScheduledToastNotification(toastXml, duedt, t, int.MaxValue));
            var toast = new Windows.UI.Notifications.ScheduledToastNotification(toastXml, duedt, t, int.MaxValue);
            
            toast.Id = "tobj" + tobj.Ttobjectid.ToString();

        }

        private static async void NotificationClass_Activated(ToastNotification sender, object args)
        {
            XmlNodeList xml = sender.Content.GetElementsByTagName("text");
            string subname = xml[0].ToString();
            List<Attendance> alist = await GetAttendanceClass.GetAttend();
            List<Attendance> alistsub = alist.Where(x => x.Subjectname == subname).Distinct().ToList();
            if (alistsub.Any())
            {
                Attendance a = alistsub.FirstOrDefault();
            }
        }

        

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
