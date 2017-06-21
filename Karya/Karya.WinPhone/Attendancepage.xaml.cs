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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Attendancepage : Page
    {

        GetAttendanceClass getatt = new GetAttendanceClass();
        public Attendancepage()
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
          
            datewiselistbox.ItemsSource= await getatt.Getattschedule(await getatt.Getdayschedule(DateTime.Today.DayOfWeek));
            
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

        private async void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            DateTime day = datedatpick.Date.DateTime;
            ///here add function call that checks with the day and brings up data from attndnce table
            List<Attendance> alist = await getatt.getattlist(day);
            datewiselistbox.ItemsSource = alist;
           
        }

        private void datewiselistbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Attendance a = datewiselistbox.SelectedItem as Attendance;
            Frame.Navigate(typeof(inattendpage),a);
        }

        private void totalattbut_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Totalattendancepage));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            GetAttendanceClass.deleteattendance();
        }
    }
}
