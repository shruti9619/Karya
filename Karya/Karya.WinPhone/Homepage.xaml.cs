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
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Karya.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Homepage : Page
    {

       


        public Homepage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>

  
        private void subbutton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Subjectpage));
        }

        private void timetablebutton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Timetablepage));
        }

        private void clickbutton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Clickpage));
        }

        private void settingbutton_Click(object sender, RoutedEventArgs e)
        {
            //setting page 
        }

        private void eventbutton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Eventpage));
        }

        private void vocabbutton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Vocabpage));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Attendancepage));
        }
    }
}
