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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Loginpage : Page
    {
        public Loginpage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        public string username { get; set; }
        public string email { get; set; }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void signinbutton_Click(object sender, RoutedEventArgs e)
        {
            new AuthClass();
            var authresult = await AuthClass.AuthenticateAsync();
           
            if (authresult)
            {
                signin_label.Visibility = Visibility.Collapsed;
                signinbutton.Visibility = Visibility.Collapsed;
                signupbutton.Visibility = Visibility.Collapsed;
                forgotpasswordlink.Visibility = Visibility.Collapsed;
                textBlock.Visibility = Visibility.Visible;
                textBlock1.Visibility = Visibility.Visible;
                usernametext.Visibility = Visibility.Visible;
                //remove line after testing
                usernametext.Text = "Komal";
                username = usernametext.Text;
                email = emailtext.Text;
                emailtext.Visibility = Visibility.Visible;
                gobutton.Visibility = Visibility.Visible;
                
            }
        }

        private async void gobutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await AuthClass.insertuser(usernametext.Text, emailtext.Text);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.Message); }
            Frame.Navigate(typeof(Homepage));
        }
    }
}
