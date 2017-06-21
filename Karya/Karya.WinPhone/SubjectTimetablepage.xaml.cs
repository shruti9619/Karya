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
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Karya.Core;
using Windows.Phone.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SubjectTimetablepage : Page
    {
        static MobileServiceClient client = AuthClass.client;
        GetEventClass getev = new GetEventClass();
        GetSubjectClass getsub = new GetSubjectClass();

        public static MobileServiceSQLiteStore store = new MobileServiceSQLiteStore(App.DB_PATH);
        public SubjectTimetablepage()
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
            Subnameblock.Text = e.Parameter as string;
            int subid = 0;
            //this code has to handle how to retrieve the num of files associated with the subject, 
            //later events and attndnc as well
            //then the numbers should also redirect the user to the intended page when he touches it
            List<Event> allev = await getev.readallevent();
            List<Subject> sublist = await getsub.ReadAllSubject();
            List<Subject> sub = sublist.Where(x => x.Subjectname == Subnameblock.Text).ToList();
            if (sub.Any())
                subid = sub.FirstOrDefault().Subjectid;
            List<Event> subev = allev.Where(x => x.Subjectid == subid).ToList();
            eventnumblock.Text = subev.Count.ToString();

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
    }
}
