using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karya.Core;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

namespace Karya.WinPhone
{
    public class GetAttendanceClass
    {
        OverallAttendance oaaobj = new OverallAttendance();

        static MobileServiceClient client;

        public GetAttendanceClass()
        {
            client = AuthClass.client;
        }


        public static async Task<List<Attendance>> GetAttend()
        {
            List<Attendance> AttendList;
            try
            {
                var localtable = client.GetSyncTable<Attendance>();
                List<Attendance> myCollection = await localtable.Select(x => x).ToListAsync();
                AttendList = new List<Attendance>(myCollection);
                return AttendList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static async Task<List<Attendance>> GetAttend(string subname)
        {
            List<Attendance> AttendList;
            try
            {
                var localtable = client.GetSyncTable<Attendance>();
                List<Attendance> myCollection = await localtable.Select(x => x).ToListAsync();
                AttendList = myCollection.Where(x => x.Subjectname == subname).ToList();
                return AttendList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }


        //create a function that fetches and returns as per day to the date changed function take day enum as param

        public async Task<List<Timetableobject>> Getdayschedule(DayOfWeek d)
        {
            List<Timetableobject> daytimetable = new List<Timetableobject>();

            List<Timetableobject> alldaysList;

            try
            {
                var localtable = client.GetSyncTable<Timetableobject>();

                List<Timetableobject> myCollection = await localtable.Select(x => x).ToListAsync();
                alldaysList = new List<Timetableobject>(myCollection);

                switch (d)
                {
                    case DayOfWeek.Monday:
                        daytimetable = alldaysList.Where(x => x.row == 1).ToList();
                        break;

                    case DayOfWeek.Tuesday:
                        daytimetable = alldaysList.Where(x => x.row == 2).ToList();
                        break;

                    case DayOfWeek.Wednesday:
                        daytimetable = alldaysList.Where(x => x.row == 3).ToList();
                        break;

                    case DayOfWeek.Thursday:
                        daytimetable = alldaysList.Where(x => x.row == 4).ToList();
                        break;

                    case DayOfWeek.Friday:
                        daytimetable = alldaysList.Where(x => x.row == 5).ToList();
                        break;

                    case DayOfWeek.Saturday:
                        daytimetable = alldaysList.Where(x => x.row == 6).ToList();
                        break;

                    case DayOfWeek.Sunday:
                        daytimetable = alldaysList.Where(x => x.row == 7).ToList();
                        break;
                }

                if (!daytimetable.Any())
                    App.addtodebug("obj list empty");
                return daytimetable;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        ///this function will take the list in timetable object returned from above function and convert 
        ///to attndnc list obj and return to the view
        ///note that these functions are to create first time views. so the total class etc will be set to zero always
        
       
        public async Task<List<Attendance>> Getattschedule(List<Timetableobject> tobjlist)
        {
          
            List<Attendance> attlist = new List<Attendance>();
      
            try
            {
                var localtable = client.GetSyncTable<Attendance>();

                List<Attendance> myCollection = await localtable.Select(x => x).ToListAsync();
                
                List<int> max = new List<int>();

                int maxid = 0;

                Attendance a = new Attendance();

                foreach (Timetableobject t in tobjlist)
                {

                    a.Subjectname = t.Subjectname;
                    //a.Totalclass = 0;
                    //a.Attendedclass = 0;
                    //a.Bunkedclass = 0;
                    a.OAAttendanceid = AuthClass.commonuser.Userid;
                    a.timestart= getstringtotime(t.timestart);
                    a.Date = DateTime.Today;
                    if (attlist.Any())
                    {
                        max = attlist.OrderByDescending(x => x.id).Select(x => x.Attendid).ToList();
                        if (max.Any())
                        {
                            maxid = max.FirstOrDefault();
                        }
                    }
                    
                    a.Attendid = maxid + 1;
                    a.id = a.Attendid.ToString();
                    attlist.Add(a);
                    await localtable.InsertAsync(a);
                 
                }

                attlist.OrderBy(x=>x.timestart);
                if (!attlist.Any())
                    App.addtodebug("no att list in if");

                return attlist;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                App.addtodebug("no att list");
                return null;
            }

        }


        public TimeSpan getstringtotime(string t)
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


        ////function to retrieve the view if attendance already exists

        public async Task<List<Attendance>> getattlist(DateTime d)
        {

            try
            {
                var localtable = client.GetSyncTable<Attendance>();

                List<Attendance> myCollection = await localtable.Select(x => x).ToListAsync();

                if (myCollection.Any())
                {
                    List<Attendance> final = new List<Attendance>();
                    final = myCollection.Where(x => x.Date.Date == d.Date).ToList();
                    final.OrderBy(x=>x.timestart);
                    return final;
                }
                else
                { return null; }
            }
            catch (Exception e)
            {
                App.addtodebug(e.Message);
                return null;
            }
        }


        public void updateattendance(Attendance a)
        {
            try
            {
                var localtable = client.GetSyncTable<Attendance>();
                localtable.UpdateAsync(a);
            }
            catch (Exception e)
            {
                App.addtodebug(e.Message);
            }
        }


        public void deleteattendance(Attendance a)
        {
            try
            {
                var localtable = client.GetSyncTable<Attendance>();
                localtable.DeleteAsync(a);
            }
            catch (Exception e)
            {
                App.addtodebug(e.Message);
            }
        }

        public static async void deleteattendance()
        {
            try
            {
                var localtable = client.GetSyncTable<Attendance>();
                List<Attendance> alist =await localtable.ToListAsync();
                foreach (Attendance att in alist)
                {
                    await localtable.DeleteAsync(att);
                }
            }
            catch (Exception e)
            {
                App.addtodebug(e.Message);
            }
        }


    }
}
