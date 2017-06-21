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
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Karya.Droid
{
    [Activity(Label = "Timetable") ]
    public class TimetableActivity : Activity
    {

        //variable to check with the settings.
        int numofslots = 3;//in the end it will be taken fom the settings page
        int slotlengthinmins = 50;
        string subjectname;
        int numinput = 0;

        Timetable timetable;
        GetTimetableClass gettimetableclass = new GetTimetableClass();
        static MobileServiceClient client = AuthClass.client;
        private int hour;
        private int minute;

        private async Task<Timetable> gettimetable()
        {
            List<Timetable> t = new List<Timetable>();
            try
            {
                var localtable = AuthClass.client.GetSyncTable<Timetable>();
                var localtobjtable = AuthClass.client.GetSyncTable<Timetableobject>();
                List<Timetableobject> timesqltable1 = await localtobjtable.Select(x => x).ToListAsync();
                t = await localtable.Select(x => x).ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            //this line will be uncommented when i test the complete app from login part since userid wont be set
            // List<Timetable> t = timesqltable.Where(x => x.Userid == AuthClass.commonuser.Userid).ToList();
            if (t.Any())
                return t.FirstOrDefault();
            else
                return null;
        }



        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Timetablelayout);

            GridLayout slotholdergrid = FindViewById<GridLayout>(Resource.Id.slotholdergrid);

            slotholdergrid.ColumnCount = numofslots;
            slotholdergrid.RowCount = 8;
            await AuthClass.InitTimetableLocalStoreAsync();
            timetable = await gettimetable();

            if (timetable == null)
            {
                timetable = new Timetable();


                timetable.Schedulestring = "";
                timetable.Timetableactive = false;
                timetable.Lengthoftimeslot = slotlengthinmins;
                timetable.Numofslot = numofslots;

            }
            else
            {
                timetable.Timetableactive = true;
                //function to retrieve the timetable and feed it as per row n col
            }
            timetable.Userid = 1;

            //slottimetable.Background ;
            //slotholdergrid.Background = new SolidColorBrush(Colors.Tomato);

            //the generation of days will always remain in this order only 

            for (int i = 0; i < 8; i++)
            {

                TextView s1 = new TextView(this);
                string idstring = i.ToString() + 0.ToString();
                s1.Id = int.Parse(idstring);
                //TextBlock s1 = new TextBlock();

                switch (i)
                {
                    case 0: s1.Text = "Day/Time"; break;
                    case 1: s1.Text = "Monday"; break;
                    case 2: s1.Text = "Tuesday"; break;
                    case 3: s1.Text = "Wednesday"; break;
                    case 4: s1.Text = "Thursday"; break;
                    case 5: s1.Text = "Friday"; break;
                    case 6: s1.Text = "Saturday"; break;
                    case 7: s1.Text = "Sunday"; break;
                }

                
                //s1.SetValue(Grid.ColumnProperty, 0);
                //s1.SetValue(Grid.RowProperty, i);
                //slotholdergrid.Children.Add(s1);
                GridLayout.Spec s = GridLayout.InvokeSpec(0);
                slotholdergrid.AddView(s1, new GridLayout.LayoutParams(GridLayout.InvokeSpec(i), GridLayout.InvokeSpec(0)));
       
            }



            for (int j = 1; j < numofslots; j++)
            {
                TextView s1 = new TextView(this);
                TimePicker t = new TimePicker(this);
                Button timebut = new Button(this);
                EditText timeview = new EditText(this);
                timeview.Hint = "Add Time";

               
               
              

                timeview.TextChanged += Timeview_TextChanged;
                timebut.Id = j;
                string idstring = 0.ToString() + j.ToString() ;

                timeview.Id = int.Parse(idstring);
               
                //important a code is yet to be designed to use the time in actual sense so that in future it can really be used as true time
                // to give reminders of classes etc.
                if (timetable.Timetableactive == false)
                {
                    
                    slotholdergrid.AddView(timeview, new GridLayout.LayoutParams(GridLayout.InvokeSpec(0), GridLayout.InvokeSpec(j)));

                    //the one time changed function 
                    //t.TimeChanged += T_TimeChanged;
                }
                else
                {
                    List<string> tlist = gettimetableclass.gettimeorder(timetable.Schedulestring);
                    if (tlist.Any())
                    {
                        
                        //create a function in gettimetble to return the times from the is sorted order in a list
                        //case 0: s1.Text = "Day/Time"; break;
                        s1.Text = tlist[j - 1];
                        string idstringa = 0.ToString() + j.ToString();
                        s1.Id = int.Parse(idstringa);
                        slotholdergrid.AddView(s1, new GridLayout.LayoutParams(GridLayout.InvokeSpec(0), GridLayout.InvokeSpec(j)));

                    }
                    else
                    {
                       // MessageDialog msgd = new MessageDialog("Issues in fetching timetable. Restart the app");
                        //await msgd.ShowAsync();
                    }

                }

            }



            List<Subject> myCollection = new List<Subject>();
            List<string> subnames = new List<string>();
            try
            {
                var localtable = client.GetSyncTable<Subject>();
                myCollection = await localtable.Select(x => x).ToListAsync();
                subnames = myCollection.Select(x => x.Subjectname).ToList();
                subnames.Add("Add New Subject");
                subnames.Add("None");
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }



            for (int i = 1; i < 8; i++)
            {


                for (int j = 1; j < numofslots; j++)
                {

                    TextView subjectslabel = new TextView(this);
                    Spinner subjectbox = new Spinner(this);

                    if (timetable.Timetableactive == false)
                    {
                        //subjectbox.PlaceholderText = "Subject";
                        ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.simplespinnerlayout, subnames);
                        subjectbox.Adapter = adapter;
                        subjectbox.ItemSelected += showfirstsubontap;
                        //subjectbox.Tapped += showfirstsubontap;

                        //use get value function to get the properties like row and column
                        //then store in string

                        //£££temporary
                        subjectslabel.Text = "kuchbhi";
                        string idstring = i.ToString() + j.ToString();
                        subjectbox.Id = int.Parse(idstring);
                        subjectname = subjectslabel.Text;
                        slotholdergrid.AddView(subjectbox, new GridLayout.LayoutParams(GridLayout.InvokeSpec(i), GridLayout.InvokeSpec(j)));


                    }
                    else
                    {  //get from the schedulestring and also
                       //add code to search subid from table subject and store in the string
                       //retrieve subjects from sdb in list and match name to get id
                       //code to add timeslot search

                        List<Timetableobject> tlist = gettimetableclass.getsplitinput(timetable.Schedulestring);
                        if (tlist.Any())
                        {
                            foreach (Timetableobject tobj in tlist)
                            {
                                if ((tobj.row == i) && (tobj.col == j))
                                {
                                    subjectslabel.Text = tobj.Subjectname;
                                    System.Diagnostics.Debug.WriteLine("matched row n col" + tobj.row + tobj.col);
                                }
                                else
                                {
                                    subjectslabel.Text = "None";
                                    System.Diagnostics.Debug.WriteLine(tobj.row.ToString() + i + tobj.col.ToString() + j);
                                }
                            }
                            //subjectslabel.Name = i.ToString() + j.ToString();
                            //subjectslabel.Foreground = new SolidColorBrush(Colors.Black);
                            subjectslabel.Touch += showsubontap;
                            subjectslabel.LongClick += showoptiononhold;
                            subjectname = subjectslabel.Text;
                            //slotholdergrid.Children.Add(subjectslabel);
                            slotholdergrid.AddView(subjectslabel, new GridLayout.LayoutParams(GridLayout.InvokeSpec(i), GridLayout.InvokeSpec(j)));
                            


                        }
                        else
                        {
                            //MessageDialog msgd = new MessageDialog("Issues in fetching timetable. Restart the app");
                            //await msgd.ShowAsync();
                        }

                    }
                }
            }

        }


        private void Timeview_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            EditText ts = sender as EditText;
            string time = ts.Text;
            
            GridLayout slotholdergrid = FindViewById<GridLayout>(Resource.Id.slotholdergrid);
            slotholdergrid.RemoveView(ts);
            int id = ts.Id;
            int row; int col;

                row = (int)id / 10;
                col = (int)id % 10;
            TextView tv = new TextView(this);
            tv.Text = ts.Text;

            slotholdergrid.AddView(tv, new GridLayout.LayoutParams(GridLayout.InvokeSpec(row), GridLayout.InvokeSpec(col)));



        }

        //this will be needed when storing the timetble time in timespan formt for sql table
        private TimeSpan strtotime(string timestring)
        {
            string[] splitstring = timestring.Split(':');
            string hours = splitstring[0];
           
            string minutes = splitstring[1];
            
            
            TimeSpan t = new TimeSpan(int.Parse(hours), int.Parse(minutes),00);
            return t;
        }


        //###################################################################################################################################
        //function to handle the case when the time is changed in the timepicker
        //###################################################################################################################################


      
        


       

        //private void T_TimeChanged(object sender, EventArgs e)
        //{
        //    GridLayout slotholdergrid = FindViewById<GridLayout>(Resource.Id.slotholdergrid);
        //    TimePicker tsend = sender as TimePicker;
        //    //slotholdergrid.Children.Remove(tsend);
        //    slotholdergrid.RemoveView(tsend);
        //    //check if it removes on timepicker or both of them 
        //    var margin = tsend.Margin;
        //    TextView tblok = new TextView(this);

        //    tblok.Text = gettimetableclass.gettexttime(tsend.Time);
        //    tblok.FontSize = 20;
        //    tblok.Margin = margin;
        //    tblok.SetValue(Grid.RowProperty, tsend.GetValue(Grid.RowProperty));
        //    tblok.SetValue(Grid.ColumnProperty, tsend.GetValue(Grid.ColumnProperty));
        //    slotholdergrid.Children.Add(tblok);
        //    slotholdergrid.AddView(subjectslabel, new GridLayout.LayoutParams(GridLayout.InvokeSpec(i), GridLayout.InvokeSpec(j)));



        //}

        //    //###################################################################################################################################
        //    //function that handles the event when the first tap is made to store the initial subject
        //    //###################################################################################################################################



        private void showfirstsubontap(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("showsubonfirsttap triggered");

            Spinner t = sender as Spinner;
            // a drp down list option is given on hold to change the name of the subject in a particular slot
            //t.SetValue(Grid.ColumnSpanProperty, numofslots - 1);
            //t.BorderBrush = new SolidColorBrush(Colors.BlueViolet);
            //t.Background = new SolidColorBrush(Colors.BurlyWood);
            //t.SetValue(Grid.RowSpanProperty, 3);
            //t.LostFocus += onsubchanged;
            t.FocusChange += onsubchanged;
            System.Diagnostics.Debug.WriteLine(timetable.Schedulestring);
        }


        //    //###################################################################################################################################
        //    //design way to handle timetable and its edit view with various events
        //    //###################################################################################################################################

        private void showoptiononhold(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(SubjectimetableActivity));
            var MySerializedObject = JsonConvert.SerializeObject(sender);
            intent.PutExtra("subname", MySerializedObject);
            StartActivity(intent);

        }



        //    //###################################################################################################################################
        //    //function to handle the case when a set subject is held to change the seubject content in it
        //    //###################################################################################################################################


        private async void showsubontap(object sender, EventArgs e)
        {
            TextView t = sender as TextView;
            // a drp down list option is given on hold to change the name of the subject in a particular slot
            try
            {
                List<Subject> myCollection = new List<Subject>();
                List<string> subnames = new List<string>();
                try
                {
                    var localtable = client.GetSyncTable<Subject>();
                    myCollection = await localtable.Select(x => x).ToListAsync();
                    subnames = myCollection.Select(x => x.Subjectname).ToList();
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(exc.Message);
                }
                subnames.Add("Add New Subject");
                subnames.Add("eng");
                subnames.Add("None");
                //var margin = t.Margin;
                Spinner c = new Spinner(this);

                //margin.Left= 50;

                //c.Margin = margin;

                //c.BorderBrush = new SolidColorBrush(Colors.BlueViolet);
                //c.Background = new SolidColorBrush(Colors.BurlyWood);
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.simplespinnerlayout, subnames);
                c.Adapter = adapter;
                //c.ItemsSource = subnames;
                //add to the ryt place and take care of the limitation of space
                //c.SetValue(Grid.ColumnSpanProperty, numofslots - 2);
                //c.SetValue(Grid.RowProperty, t.GetValue(Grid.RowProperty));
                //c.SetValue(Grid.ColumnProperty, t.GetValue(Grid.ColumnProperty));
                //c.SetValue(Grid.RowSpanProperty, 3);
                int id = t.Id;
                int row; int col;

                row = (int)id / 10;
                col = (int)id % 10;
                GridLayout slotholdergrid = FindViewById<GridLayout>(Resource.Id.slotholdergrid);
                slotholdergrid.AddView(c,new GridLayout.LayoutParams(GridLayout.InvokeSpec(row), GridLayout.InvokeSpec(col)));


              
                //if (subnames.Contains(t.Text))
                //{ c.SelectedItem = t.Text; }

                c.Touch += onsubchanged;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }


        }






        //    //###################################################################################################################################
        //    //the function that handles the event when the combobox is fed with a choice
        //    //###################################################################################################################################
        private async void onsubchanged(object sender, EventArgs args)
        {
            GridLayout slotholdergrid = FindViewById<GridLayout>(Resource.Id.slotholdergrid);
            System.Diagnostics.Debug.WriteLine("onsubchange triggered");

            Spinner csend = sender as Spinner;
            TextView newtext = new TextView(this);

            if (csend.SelectedItem != null)
            {
                //int rowtofeed = (int)csend.GetValue(Grid.RowProperty);
                //int coltofeed = (int)csend.GetValue(Grid.ColumnProperty);
                int id = csend.Id;
                //int row; int col;

                int rowtofeed = (int)id / 10;
                int coltofeed = (int)id % 10;
                string changedsub = csend.SelectedItem.ToString();

                if (changedsub == "Add New Subject")
                {
                    //Frame.Navigate(typeof(Addsubjectpage), "timetablepage");
                    var myintent = new Intent(this, typeof(AddSubjectActivity));
                    StartActivity(myintent);
                }

                else
                {
                    Timetableobject tobj = new Timetableobject();
                    tobj.Ttobjectid = tobj.id = rowtofeed.ToString() + coltofeed.ToString();
                    tobj.id = tobj.Ttobjectid;
                    tobj.Subjectname = csend.SelectedItem.ToString();
                    tobj.row = rowtofeed;
                    System.Diagnostics.Debug.WriteLine(rowtofeed.ToString());
                    tobj.col = coltofeed;
                    System.Diagnostics.Debug.WriteLine(coltofeed.ToString());
                    //this case is to feed the day of the week the choice of text belongs to
                    switch (rowtofeed)
                    {
                        case 1: tobj.day = "Mon"; break;
                        case 2: tobj.day = "Tue"; break;
                        case 3: tobj.day = "Wed"; break;
                        case 4: tobj.day = "Thu"; break;
                        case 5: tobj.day = "Fri"; break;
                        case 6: tobj.day = "Sat"; break;
                        case 7: tobj.day = "Sun"; break;
                    }

                    TextView t = new TextView(this);

                    // then make the mechanism to fetch the time from the correct column top keeping the row value 0 by getting the child of the grid

                    System.Diagnostics.Debug.WriteLine("col no :" + coltofeed.ToString());

                    t = GetChildren(slotholdergrid, 0, coltofeed);
                    if (t != null)
                        tobj.timestart = t.Text;
                    else
                        tobj.timestart = "NULL";



                    tobj.Userid = AuthClass.commonuser.Userid;

                    System.Diagnostics.Debug.WriteLine(tobj.Subjectname + " " + tobj.day + " " + tobj.timestart + " " + tobj.Ttobjectid);

                    //this portion of the code accepts the selection and stores it in timetableobject
                    //note that till now the objects are yet to be stored in the schedule string of the sake of cloud

                    List<Subject> myCollection = new List<Subject>();
                    List<string> subnames = new List<string>();


                    try
                    {
                        var localtable = client.GetSyncTable<Subject>();
                        myCollection = await localtable.Select(x => x).ToListAsync();
                        subnames = myCollection.Select(x => x.Subjectname).ToList();
                    }
                    catch (Exception exc)
                    {
                        System.Diagnostics.Debug.WriteLine(exc.Message);
                    }

                    //finally code to get the teachers name
                    if (myCollection.Any())
                    {
                        List<Subject> selectedsub = myCollection.Select(x => x).Where(x => x.Subjectname == tobj.Subjectname).ToList();
                        if (selectedsub.Any())
                        {
                            Subject selectedsubdetail = selectedsub.FirstOrDefault();
                            tobj.Teachername = selectedsubdetail.Teachername;
                            try
                            {
                                var localtobjtable = client.GetSyncTable<Timetableobject>();
                                await localtobjtable.InsertAsync(tobj);
                                //function to set a notification whenever a new class is fed in the timetable
                                NotificationClass.ttnot(this,tobj);

                                //a function that knows the length of the slot from the timetable object then it
                                // calls a notification just a few minutes before the class is going to end asking if the 
                                // user attended or bunked or whatever
                                //if this is dismissed then it is taken as no class which user can change later
                                //this function will also come from the notifcation class
                                NotificationClass.ttattendnot(this,tobj, slotlengthinmins);
                            }
                            catch (Exception tobjex)
                            {
                                System.Diagnostics.Debug.WriteLine(tobjex.Message);
                            }


                            //here you have to add to the schedule string,
                            //when the user completes all comboboxes only then the page must move to change schedulestring false to true
                            //how to check whether all comboboxes have been filled check whether the schedule string has specific number of
                            // ids or not. if not fill prompt user to fill all the boxes

                            timetable.Schedulestring += tobj.timestart + "@" + tobj.row + "@" + tobj.col + "@" + tobj.Subjectname + ";";

                            numinput = gettimetableclass.getnuminput(timetable.Schedulestring);

                            System.Diagnostics.Debug.WriteLine(numinput.ToString());

                            //finally to replace the combobox with textblock

                            newtext.Text = changedsub;
                            //newtext.SetValue(Grid.RowProperty, rowtofeed);
                            //newtext.SetValue(Grid.ColumnProperty, coltofeed);
                            slotholdergrid.RemoveView(csend);
                            slotholdergrid.AddView(newtext, new GridLayout.LayoutParams(GridLayout.InvokeSpec(rowtofeed), GridLayout.InvokeSpec(coltofeed)));//adding specs row n col


                            //if back is tried and the time table slots are complete then the tt is saved
                            if (numinput == ((numofslots - 1) * 7))
                            {
                                timetable.Timetableactive = true;

                                try
                                {
                                    var localtime = client.GetSyncTable<Timetable>();
                                    await localtime.InsertAsync(timetable);
                                }
                                catch (Exception timex)
                                {
                                    System.Diagnostics.Debug.WriteLine(timex.Message);
                                }

                                var intent = new Intent(this, typeof(TimetableActivity));
                                //this.Frame.Navigate(typeof(Timetablepage));
                            }

                        }
                        else
                        {
                            Toast.MakeText(this, "Subject not found issue detected. Restart the app", ToastLength.Long).Show();
                            //MessageDialog m2 = new MessageDialog("Subject not found issue detected. Restart the app");
                            //await m2.ShowAsync();
                        }

                    }
                    else
                    {
                        Toast.MakeText(this, "Issues detected with local storage. Restart the app", ToastLength.Long).Show();
                        //MessageDialog m1 = new MessageDialog("Issues detected with local storage. Restart the app");
                        //await m1.ShowAsync();
                    }



                }
            }
        }





        //    //###################################################################################################################################
        //    //function to get the child at the position row and column //using to get time of the selected column object
        //    //###################################################################################################################################


        private TextView GetChildren(GridLayout grid, int row, int column)
        {
            GridLayout slotholdergrid = FindViewById<GridLayout>(Resource.Id.slotholdergrid);

            string idstring = row.ToString() + column.ToString();
            int count = slotholdergrid.ChildCount;
            for (int i = 0; i < count; i++)
            {
                View child = slotholdergrid.GetChildAt(i);
                if (child.Id == int.Parse(idstring))
                    return (TextView)child;
            }

            return null;
        }







        
    }
}