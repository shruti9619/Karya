using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Karya.Core;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class AddEventpage : Page
    {

        GetSubjectClass getsub = new GetSubjectClass();
        List<Subject> lsub = new List<Subject>();
        List<string> lsubname = new List<string>();
        Event ev = new Event();
        GetEventClass getev = new GetEventClass();
        public static DateTime d;

        public AddEventpage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            lsub = await getsub.ReadAllSubject();
            lsubname = lsub.Select(x => x.Subjectname).ToList();
            lsubname.Add("None");

            List<string> repeatoption = new List<string>();
            repeatoption.Add("Choose days");
            repeatoption.Add("Hourly");
            repeatoption.Add("Daily");
            repeatoption.Add("Weekly");
            repeatoption.Add("Monthly");
            repeatoption.Add("Yearly");
            repeatcombobox.ItemsSource = repeatoption;
            subjectcombobox.ItemsSource = lsubname;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }

        }

        private async void savebutton_Click(object sender, RoutedEventArgs e)
        {

            App.addtodebug("save buttin clicked");
            //apply validation methods

            ev.Title = titlebox.Text;
            if (descbox.Text != null)
                if (descbox.Text.Length > 0)
                    ev.Description = descbox.Text;
            List<Event> lev = new List<Event>();
            try
            {
                lev = await getev.readallevent();
            }
            catch (Exception ex)
            {
                App.addtodebug(ex.Message);
            }

            int maxtabid;
            if (lev.Any())
            {
                lev.OrderBy(x => x.Timetableid);
                maxtabid = lev[0].Timetableid;
            }
            else
            {
                maxtabid = 0;
            }
            ev.Timetableid = maxtabid + 1;
            ev.id = ev.Timetableid.ToString();
            ev.Time = gettexttime(evtimepicker.Time);
            ev.Date = evdatepicker.Date.ToString();

            ev.Datetime = new DateTime(evdatepicker.Date.Year, evdatepicker.Date.Month, evdatepicker.Date.Day, evtimepicker.Time.Hours, evtimepicker.Time.Minutes, evtimepicker.Time.Seconds);

            ev.Userid = AuthClass.commonuser.Userid;
            if (subjectcombobox.SelectedIndex != -1)
            {
                Subject sub = new Subject();
                List<Subject> ls = new List<Subject>();
                ls = lsub.Select(x => x).Where(x => x.Subjectname == subjectcombobox.SelectedItem.ToString()).ToList();
                if (ls.Any())
                {
                    sub = ls.FirstOrDefault();
                    ev.Subjectid = sub.Subjectid;
                }

            }
            NotificationClass nc = new NotificationClass();

            if (repeatcombobox.SelectedIndex != -1)
            {

                switch (repeatcombobox.SelectedItem.ToString())
                {

                    case "Hourly": ev.Repeat = "Hourly"; nc.assigntoast(ev, int.Parse(numtextbox.Text)); break;
                    case "Daily": ev.Repeat = "Daily"; nc.assigntoast(ev); break;
                    case "Weekly": ev.Repeat = "Weekly"; nc.assigntoast(ev); break;
                    case "Monthly": ev.Repeat = "Monthly"; nc.assigntoast(ev); break;
                    case "Yearly": ev.Repeat = "Yearly"; nc.assigntoast(ev); break;
                    //here get the children of gday check if they are checked and return to the params to count in 0s and 1s
                    case "Choose days": ev.Repeat = "Choose days"; nc.assigntoast(ev); break;
                    default: ev.Repeat = ""; nc.assigntoast(ev);break;

                }
            }
            else
                ev.Repeat = ""; nc.assigntoast(ev);



            App.addtodebug("assigned");
            try
            {
                getev.insertevent(ev);

            }
            catch (Exception ex)
            {
                App.addtodebug(ex.Message);

            }
            finally
            {
                Frame.Navigate(typeof(Eventpage));
            }


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

        private void repeatcombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (repeatcombobox.SelectedItem.ToString() == "Hourly")
                numtextbox.Visibility = Visibility.Visible;
            else
                numtextbox.Visibility = Visibility.Collapsed;
            // add case if choose days and then add grid and the check boxes on that grid
            if (repeatcombobox.SelectedItem.ToString() == "Choose days")
            {

                gdays.Visibility = Visibility.Visible;
                

                for (int i = 0; i < 7; i++)
                {
                    gdays.RowDefinitions.Add(new RowDefinition());
                    List<RowDefinition> rlist = gdays.RowDefinitions.ToList();
                    rlist[i].Height = new GridLength(1, GridUnitType.Star);
                }



                for (int i = 0; i < 7; i++)
                {
                    switch (i)
                    {
                        case 0:
                            CheckBox monday = new CheckBox();
                            monday.SetValue(Grid.RowProperty, i);
                            monday.Content = "Monday";
                            gdays.Children.Add(monday); break;

                        case 1:
                            CheckBox tuesday = new CheckBox();
                            tuesday.SetValue(Grid.RowProperty, i);
                            tuesday.Content = "Tuesday";
                            gdays.Children.Add(tuesday); break;

                        case 2:
                            CheckBox wednes = new CheckBox();
                            wednes.SetValue(Grid.RowProperty, i);
                            wednes.Content = "Wednesday";
                            gdays.Children.Add(wednes); break;

                        case 3:
                            CheckBox thurs = new CheckBox();
                            thurs.SetValue(Grid.RowProperty, i);
                            thurs.Content = "Thursday";
                            gdays.Children.Add(thurs); break;

                        case 4:
                            CheckBox fri = new CheckBox();
                            fri.SetValue(Grid.RowProperty, i);
                            fri.Content = "Friday";
                            gdays.Children.Add(fri); break;

                        case 5:
                            CheckBox sat = new CheckBox();
                            sat.SetValue(Grid.RowProperty, i);
                            sat.Content = "Saturday";
                            gdays.Children.Add(sat); break;

                        case 6:
                            CheckBox sun = new CheckBox();
                            sun.SetValue(Grid.RowProperty, i);
                            sun.Content = "Sunday";
                            gdays.Children.Add(sun); break;

                    }

                }
            }
        }
    }
}
