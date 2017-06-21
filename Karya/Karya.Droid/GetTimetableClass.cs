using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.IO;

using Karya.Core;


//this class will be used to collect the timetable objects of a single user into a single string as proposed earlier 
//so that there's no need to increase the limit of returning data from cloud

namespace Karya.Droid
{
    public class GetTimetableClass
    {
        List<Timetableobject> ltobj = new List<Timetableobject>();
        int numofinput;
        Timetableobject tobj;
        List<string> timelist=new List<string>();

        public List<Timetableobject> getsplitinput(string schedulestring)
        {
            string[] eachinput = schedulestring.Split(';');
            tobj = new Timetableobject();
            foreach (string eachdaystring in eachinput)
            {
                if (eachdaystring.Length > 4)
                {
                    string[] objecttodivide = eachdaystring.Split('@');
               
                    tobj.timestart = objecttodivide[0];
                    tobj.row = int.Parse(objecttodivide[1]);
                    tobj.col = int.Parse(objecttodivide[2]);
                    tobj.Ttobjectid = tobj.row.ToString() + tobj.col.ToString();
                    tobj.Subjectname = objecttodivide[3];
                    //App.addtodebug(objecttodivide[1] + "~" + objecttodivide[2] + "~");
                    ltobj.Add(tobj);
                  
                    timelist.Add(tobj.timestart);
                }

            }
            numofinput = eachinput.Length;
            return ltobj.Distinct().ToList();
        }

        // a function to return total num of fields that have been input

        public int getnuminput(string schedulestring)
        {
            getsplitinput(schedulestring);

            return numofinput;
        }
    

        //function to return all the times in ascending order in a list

       public List<string> gettimeorder(string schedulestring)
        {
            getsplitinput(schedulestring);
            return timelist.Distinct().ToList();
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
