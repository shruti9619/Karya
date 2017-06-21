using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Karya.Core;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Timetablepage : Page
    {
        //variable to check with the settings.
        int numofslots = 3;//in the end it will be taken fom the settings page
        int slotlengthinmins = 50;
        string subjectname;
        int numinput = 0;

        TextBlock s2 = new TextBlock();
        TextBlock s3 = new TextBlock();

        static MobileServiceClient client = AuthClass.client;

        GetTimetableClass gettimetableclass = new GetTimetableClass();

        Timetable timetable;





        public Timetablepage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            //another page is to be created to handle the timetable string which comes from the local database
        }

        async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            //this checking so that the msg is shown only the first time the timetable is created
            numinput = gettimetableclass.getnuminput(timetable.Schedulestring);
            if ((numinput != 0) && (numinput < numofslots * 7) && (timetable.Timetableactive == false))
            {
                //generate msg to fill all time and slots
                MessageDialog m = new MessageDialog("You must fill all subject and time slots to save your personalised schedule");
                await m.ShowAsync();
            }
            else
            {
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame != null && rootFrame.CanGoBack)
                {
                    rootFrame.GoBack();
                    e.Handled = true;
                }
            }

        }

        private async Task<Timetable> gettimetable()
        {
            List<Timetable> t = new List<Timetable>();
            try
            {
                var localtable = client.GetSyncTable<Timetable>();
                var localtobjtable = client.GetSyncTable<Timetableobject>();
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



        //////////function to fetch timetable if it is already there in the database
        private async Task<Timetableobject> getsavedtimetable()
        {
            List<Timetableobject> t = new List<Timetableobject>();
            try
            {

                var localtobjtable = client.GetSyncTable<Timetableobject>();
                t = await localtobjtable.Select(x => x).ToListAsync();
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



        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

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

            slotholdergrid.Background = new SolidColorBrush(Colors.Green);

            //in column definition added full namespace as it was ambiguous with the one in sqlite

            for (int i = 0; i < numofslots; i++)
            {
                slotholdergrid.ColumnDefinitions.Add(new Windows.UI.Xaml.Controls.ColumnDefinition());
                List<Windows.UI.Xaml.Controls.ColumnDefinition> rlist = slotholdergrid.ColumnDefinitions.ToList();
                rlist[i].Width = new GridLength(1, GridUnitType.Star);
            }


            for (int i = 0; i < 8; i++)
            {
                slotholdergrid.RowDefinitions.Add(new RowDefinition());
                List<RowDefinition> rlist = slotholdergrid.RowDefinitions.ToList();
                rlist[i].Height = new GridLength(1, GridUnitType.Star);
            }


            //the generation of days will always remain in this order only 

            for (int i = 0; i < 8; i++)
            {

                TextBlock s1 = new TextBlock();

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

                s1.SetValue(Grid.ColumnProperty, 0);
                s1.SetValue(Grid.RowProperty, i);
                slotholdergrid.Children.Add(s1);

            }



            for (int j = 1; j < numofslots; j++)
            {
                TextBlock s1 = new TextBlock();
                TimePicker t = new TimePicker();
                //important a code is yet to be designed to use the time in actual sense so that in future it can really be used as true time
                // to give reminders of classes etc.
                if (timetable.Timetableactive == false)
                {
                    t.SetValue(Grid.ColumnProperty, j);
                    t.SetValue(Grid.RowProperty, 0);
                    slotholdergrid.Children.Add(t);
                    //the one time changed function 
                    t.TimeChanged += T_TimeChanged;
                }
                else
                {
                    List<string> tlist = gettimetableclass.gettimeorder(timetable.Schedulestring);
                    if (tlist.Any())
                    {

                        //create a function in gettimetble to return the times from the is sorted order in a list
                        //case 0: s1.Text = "Day/Time"; break;
                        s1.Text = tlist[j - 1];
                        s1.SetValue(Grid.ColumnProperty, j);
                        s1.SetValue(Grid.RowProperty, 0);
                        slotholdergrid.Children.Add(s1);
                    }
                    else
                    {
                        MessageDialog msgd = new MessageDialog("Issues in fetching timetable. Restart the app");
                        await msgd.ShowAsync();
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

                    TextBlock subjectslabel = new TextBlock();
                    ComboBox subjectbox = new ComboBox();

                    if (timetable.Timetableactive == false)
                    {
                        subjectbox.PlaceholderText = "Subject";
                        subjectbox.SelectedIndex = -1;
                        subjectbox.ManipulationMode = ManipulationModes.System;
                        subjectbox.ItemsSource = subnames;
                        subjectbox.Foreground = new SolidColorBrush(Colors.Black);
                        subjectbox.SetValue(Grid.ColumnProperty, j);
                        subjectbox.SetValue(Grid.RowProperty, i);
                        subjectbox.Tapped += showfirstsubontap;

                        //use get value function to get the properties like row and column
                        //then store in string
                        subjectname = subjectslabel.Text;
                        slotholdergrid.Children.Add(subjectbox);

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
                                    App.addtodebug("matched row n col" + tobj.row + tobj.col);                                  
                                }
                                else
                                {
                                    subjectslabel.Text = "None";
                                    // App.addtodebug(tobj.row.ToString() + i + tobj.col.ToString() + j);
                                }
                            }
                            subjectslabel.Name = i.ToString() + j.ToString();
                            subjectslabel.Foreground = new SolidColorBrush(Colors.Black);
                            subjectslabel.SetValue(Grid.ColumnProperty, j);
                            subjectslabel.SetValue(Grid.RowProperty, i);
                            subjectslabel.Tapped += showsubontap;
                            subjectslabel.Holding += showoptiononhold;
                            subjectname = subjectslabel.Text;
                            slotholdergrid.Children.Add(subjectslabel);


                        }
                        else
                        {
                            MessageDialog msgd = new MessageDialog("Issues in fetching timetable. Restart the app");
                            await msgd.ShowAsync();
                        }

                    }
                }
            }

        }


        //###################################################################################################################################
        //function to handle the case when the time is changed in the timepicker
        //###################################################################################################################################


        private void T_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            TimePicker tsend = sender as TimePicker;
            slotholdergrid.Children.Remove(tsend);
            var margin = tsend.Margin;
            TextBlock tblok = new TextBlock();
            tblok.Text = gettimetableclass.gettexttime(tsend.Time);
            tblok.FontSize = 20;
            tblok.Margin = margin;
            tblok.SetValue(Grid.RowProperty, tsend.GetValue(Grid.RowProperty));
            tblok.SetValue(Grid.ColumnProperty, tsend.GetValue(Grid.ColumnProperty));
            slotholdergrid.Children.Add(tblok);


        }

        //###################################################################################################################################
        //function that handles the event when the first tap is made to store the initial subject
        //###################################################################################################################################



        private void showfirstsubontap(object sender, RoutedEventArgs e)
        {
            App.addtodebug("showsubonfirsttap triggered");

            ComboBox t = sender as ComboBox;
            // a drp down list option is given on hold to change the name of the subject in a particular slot
            //t.SetValue(Grid.ColumnSpanProperty, numofslots - 1);
            t.BorderBrush = new SolidColorBrush(Colors.BlueViolet);
            t.Background = new SolidColorBrush(Colors.BurlyWood);
            //t.SetValue(Grid.RowSpanProperty, 3);
            t.LostFocus += onsubchanged;
            System.Diagnostics.Debug.WriteLine(timetable.Schedulestring);
        }


        //###################################################################################################################################
        //design way to handle timetable and its edit view with various events
        //###################################################################################################################################

        private void showoptiononhold(object sender, RoutedEventArgs e)
        {
            //how to find which row column from the grid was tapped because the label name is same
            TextBlock t = sender as TextBlock;
            Frame.Navigate(typeof(SubjectTimetablepage), t.Text);

        }



        //###################################################################################################################################
        //function to handle the case when a set subject is held to change the seubject content in it
        //###################################################################################################################################


        private async void showsubontap(object sender, RoutedEventArgs e)
        {
            TextBlock t = sender as TextBlock;
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
                ComboBox c = new ComboBox();

                //margin.Left= 50;

                //c.Margin = margin;

                c.BorderBrush = new SolidColorBrush(Colors.BlueViolet);
                c.Background = new SolidColorBrush(Colors.BurlyWood);
                c.ItemsSource = subnames;
                //add to the ryt place and take care of the limitation of space
                c.SetValue(Grid.ColumnSpanProperty, numofslots - 2);
                c.SetValue(Grid.RowProperty, t.GetValue(Grid.RowProperty));
                c.SetValue(Grid.ColumnProperty, t.GetValue(Grid.ColumnProperty));
                //c.SetValue(Grid.RowSpanProperty, 3);
                slotholdergrid.Children.Add(c);

                if (subnames.Contains(t.Text))
                { c.SelectedItem = t.Text; }

                c.Tapped += onsubchanged;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }


        }






        //###################################################################################################################################
        //the function that handles the event when the combobox is fed with a choice
        //###################################################################################################################################
        private async void onsubchanged(object sender, RoutedEventArgs args)
        {
            App.addtodebug("onsubchange triggered");

            ComboBox csend = sender as ComboBox;
            TextBlock newtext = new TextBlock();

            if (csend.SelectedItem != null)
            {
                int rowtofeed = (int)csend.GetValue(Grid.RowProperty);
                int coltofeed = (int)csend.GetValue(Grid.ColumnProperty);
                string changedsub = csend.SelectedItem as string;

                if (changedsub == "Add New Subject")
                {
                    Frame.Navigate(typeof(Addsubjectpage), "timetablepage");
                }

                else
                {
                    Timetableobject tobj = new Timetableobject();
                    tobj.Ttobjectid = tobj.id = rowtofeed.ToString() + coltofeed.ToString();
                    tobj.id = tobj.Ttobjectid;
                    tobj.Subjectname = csend.SelectedItem.ToString();
                    tobj.row = rowtofeed;
                    App.addtodebug(rowtofeed.ToString());
                    tobj.col = coltofeed;
                    App.addtodebug(coltofeed.ToString());
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

                    TextBlock t = new TextBlock();

                    // then make the mechanism to fetch the time from the correct column top keeping the row value 0 by getting the child of the grid

                    App.addtodebug("col no :" + coltofeed.ToString());

                    t = GetChildren(slotholdergrid, 0, coltofeed);
                    if (t != null)
                        tobj.timestart = t.Text;
                    else
                        tobj.timestart = "NULL";



                    tobj.Userid = AuthClass.commonuser.Userid;

                    App.addtodebug(tobj.Subjectname + " " + tobj.day + " " + tobj.timestart + " " + tobj.Ttobjectid);

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
                                NotificationClass.ttnot(tobj);

                                //a function that knows the length of the slot from the timetable object then it
                                // calls a notification just a few minutes before the class is going to end asking if the 
                                // user attended or bunked or whatever
                                //if this is dismissed then it is taken as no class which user can change later
                                //this function will also come from the notifcation class
                                NotificationClass.ttattendnot(tobj, slotlengthinmins);
                            }
                            catch (Exception tobjex)
                            {
                                App.addtodebug(tobjex.Message);
                            }


                            //here you have to add to the schedule string,
                            //when the user completes all comboboxes only then the page must move to change schedulestring false to true
                            //how to check whether all comboboxes have been filled check whether the schedule string has specific number of
                            // ids or not. if not fill prompt user to fill all the boxes

                            timetable.Schedulestring += tobj.timestart + "@" + tobj.row + "@" + tobj.col + "@" + tobj.Subjectname + ";";

                            numinput = gettimetableclass.getnuminput(timetable.Schedulestring);

                            App.addtodebug(numinput.ToString());

                            //finally to replace the combobox with textblock

                            newtext.Text = changedsub;
                            newtext.SetValue(Grid.RowProperty, rowtofeed);
                            newtext.SetValue(Grid.ColumnProperty, coltofeed);
                            slotholdergrid.Children.Remove(csend);
                            slotholdergrid.Children.Add(newtext);


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
                                    App.addtodebug(timex.Message);
                                }


                                this.Frame.Navigate(typeof(Timetablepage));
                            }

                        }
                        else
                        {
                            MessageDialog m2 = new MessageDialog("Subject not found issue detected. Restart the app");
                            await m2.ShowAsync();
                        }

                    }
                    else
                    {
                        MessageDialog m1 = new MessageDialog("Issues detected with local storage. Restart the app");
                        await m1.ShowAsync();
                    }



                }
            }
        }





        //###################################################################################################################################
        //function to get the child at the position row and column //using to get time of the selected column object
        //###################################################################################################################################


        private static TextBlock GetChildren(Grid grid, int row, int column)
        {
            try
            {
                List<UIElement> children = grid.Children.ToList();
                foreach (UIElement child in children)//grid.Children
                {
                    if (child.GetType() == typeof(TextBlock))
                        if ((Grid.GetRow((FrameworkElement)child) == row)
                              &&
                           (Grid.GetColumn((FrameworkElement)child) == column))
                        {

                            return (TextBlock)child;
                        }
                }
            }
            catch (Exception ex)
            {
                App.addtodebug(ex.Message);
            }

            return null;
        }






    }
}
