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
using Windows.Phone.UI.Input;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class inattendpage : Page
    {
        Attendance a = new Attendance();
        GetAttendanceClass getat = new GetAttendanceClass();
        public inattendpage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            if(e.Parameter.GetType()==typeof( Attendance))
            a = e.Parameter as Attendance;
            else
            a = JsonConvert.DeserializeObject<Attendance>(e.Parameter.ToString());
            subnameblock.Text = a.Subjectname;
            if (a.Isattended)
                statusondateblock.Text += a.Date.Date.ToString() + ": Attended";
            if (a.Isbunked)
                statusondateblock.Text += a.Date.Date.ToString() + ": Bunked";
            if (a.Isnoclass)
                statusondateblock.Text += a.Date.Date.ToString() + ": No Class";

            totnumblc.Text = a.Totalclass.ToString();
            attnmblk.Text = a.Attendedclass.ToString();
            bunknmblk.Text = a.Bunkedclass.ToString();
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

        private void noclassbut_Click(object sender, RoutedEventArgs e)
        {
            if (a.Isattended)
            {
                a.Isattended = false;
                a.Totalclass -= 1;
                a.Attendedclass -= 1;
            }

            if (a.Isbunked)
            {
                a.Isbunked = false;
                a.Totalclass -= 1;
                a.Bunkedclass -= 1;
            }

            a.Isnoclass = true;

            getat.updateattendance(a);
        }

        private void attendedbut_Click(object sender, RoutedEventArgs e)
        {
            if (a.Isbunked)
            {
                a.Isbunked = false;
                a.Attendedclass += 1;
                a.Bunkedclass -= 1;
            }

            if (a.Isnoclass)
            {
                a.Isnoclass = false;
                a.Attendedclass += 1;
                a.Totalclass += 1;
            }

            if (!a.Isattended)
            {
                a.Isnoclass = false;
                a.Attendedclass += 1;
                a.Totalclass += 1;
            }

            a.Isattended = true;
            getat.updateattendance(a);
        }

        private void bunkedbut_Click(object sender, RoutedEventArgs e)
        {
            if (a.Isattended)
            {
                a.Isattended = false;
                a.Attendedclass -= 1;
                a.Bunkedclass += 1;
            }

            if (a.Isnoclass)
            {
                a.Isnoclass = false;
                a.Bunkedclass += 1;
                a.Totalclass += 1;
            }

            if (!a.Isbunked)
            {
                a.Isnoclass = false;
                a.Bunkedclass += 1;
                a.Totalclass += 1;
            }

            a.Isbunked = true;
            getat.updateattendance(a);
        }
    }
}
