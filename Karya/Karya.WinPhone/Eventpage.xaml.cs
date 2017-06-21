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
    public sealed partial class Eventpage : Page
    {

        GetEventClass getev = new GetEventClass();
        List<Event> lev = new List<Event>();
        public Eventpage()
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
            lev = await getev.readallevent();
            eventlistview.ItemsSource =lev ;
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



        private void addevebutton_Click(object sender, RoutedEventArgs e)
        {
           
            Frame.Navigate(typeof(AddEventpage));
        }




        private async void delallbutton_Click(object sender, RoutedEventArgs e)
        {
            await getev.DeleteAllEvents();
        }

        private void eventlistview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // a page where full event can be viewed and deleted as well
            Event touchedev = eventlistview.SelectedItem as Event;
            Frame.Navigate(typeof(InEventPage),touchedev);
        }
    }
}
