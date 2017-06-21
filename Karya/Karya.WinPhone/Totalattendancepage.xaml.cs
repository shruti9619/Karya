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
using Microsoft.WindowsAzure.MobileServices;
using Windows.Phone.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Totalattendancepage : Page
    {
        public MobileServiceClient client = AuthClass.client;
        public Totalattendancepage()
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
            List<Subject> slist = new List<Subject>();
          
            GetSubjectClass getsub = new GetSubjectClass();
                slist = await getsub.ReadAllSubject();
            List<string> subname = slist.Select(x => x.Subjectname).ToList();
            subcomboBox.ItemsSource = subname;

           
        }

        private async void subcomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox c = sender as ComboBox;
            List<Attendance> alist = await GetAttendanceClass.GetAttend();
            List<Attendance> alistsub = alist.Where(x => x.Subjectname == c.SelectedItem.ToString()).Distinct().ToList();
            if (alistsub.Any())
            {
                totnmblk.Text = alistsub.FirstOrDefault().Totalclass.ToString();
                atnmblk.Text = alistsub.FirstOrDefault().Attendedclass.ToString();
                bnknmblk.Text = alistsub.FirstOrDefault().Bunkedclass.ToString();
            }
           
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (subcomboBox.SelectedIndex != -1)
            {
                List<Attendance> alist = await GetAttendanceClass.GetAttend();
                List<Attendance> alistsub = alist.Where(x => x.Subjectname == subcomboBox.SelectedItem.ToString()).Distinct().ToList();
                if (alistsub.Any())
                {
                    Attendance a = alistsub.FirstOrDefault();
                    a.Attendedclass = 0;
                    a.Bunkedclass = 0;
                    a.Totalclass = 0;
                    GetAttendanceClass g = new GetAttendanceClass();
                    g.updateattendance(a);
                }
            }
        }
    }
}
