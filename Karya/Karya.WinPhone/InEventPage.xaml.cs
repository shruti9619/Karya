using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Karya.Core;
using System.Collections.ObjectModel;
using Windows.Phone.UI.Input;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InEventPage : Page
    {
        GetEventClass getev = new GetEventClass();
        Event ev = new Event();
        List<Subject> lsub = new List<Subject>();
        public InEventPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
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
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            //ev = e.Parameter as Event;
            if (e.Parameter.GetType()== typeof( Event))
                ev = e.Parameter as Event;
            else
                ev = JsonConvert.DeserializeObject<Event>(e.Parameter.ToString());

            titletextblock.Text = ev.Title;
            if (ev.Description != null)
                desctextblock.Text = ev.Description;
            else
                desctextblock.Text ="No Description ";
            timeblock.Text = ev.Time;
            dateblock.Text = ev.Date;
            repeattextblock.Text += ev.Repeat;
            string subname = "None";
            if (ev.Subjectid > -1)
            {
                try
                {
                    GetSubjectClass getsub = new GetSubjectClass();
                    List<Subject> ls = new List<Subject>();
                    lsub = await getsub.ReadAllSubject();
                    ls = lsub.Select(x => x).Where(x => x.Subjectid == ev.Subjectid).ToList();
                    if (ls.Any())
                        subname = ls.FirstOrDefault().Subjectname;

                }
                catch (Exception ex)
                {
                    App.addtodebug(ex.Message);
                }
            }
            subtextblock.Text += subname;
        }

        private async void delbutton_Click(object sender, RoutedEventArgs e)
        {
           bool delres= await getev.DeleteEvent(ev);
            if (delres)
                App.addtodebug("deleted event");

            Frame.Navigate(typeof(Eventpage));
        }
    }
}
