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
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Karya.Core;
using System.Collections.ObjectModel;
using Windows.Phone.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Subjectpage : Page
    {
        
        public GetSubjectClass getsub = new GetSubjectClass();
        public Subjectpage()
        {
            this.InitializeComponent();
            //GetSubjectClass.Initsubjecttable();
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
        }


        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }
            if (rootFrame == null)
                return;

        }

        private void Sublistbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // to delete or update when something is selected in listboxobj 
            if (Sublistbox.SelectedIndex != -1)
            {
                Subject touchedsub = Sublistbox.SelectedItem as Subject;
                Frame.Navigate(typeof(Insubjectpage), touchedsub);
                //navigate to page where u  can see contents
                //to edit delete etc 
            }
            
        }

        private async void Sublistbox_Loaded(object sender, RoutedEventArgs e)
        {
            List<Subject> allsub = new List<Subject>();
            allsub = await getsub.ReadAllSubject();
            if (allsub.Any())
                Sublistbox.ItemsSource = allsub.OrderByDescending(i => i.Subjectid).ToList();//Binding DB data to LISTBOX and Latest subject ID can Display first. 
            else
                Sublistbox.ItemsSource = allsub.ToList();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Addsubjectpage));
        }

        private async void delallsubbutton_Click(object sender, RoutedEventArgs e)
        {
            await getsub.DeleteAllSubjects();   
        }
    }
}
