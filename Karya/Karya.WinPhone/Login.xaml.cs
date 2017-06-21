using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using System.Windows;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Karya.WinPhone
{
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
     

        public Login()
        {
            InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        
        
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
          
        }

        
            
        private async void buttonsignin_Click(object sender, RoutedEventArgs e)
        {
            AuthClass auth = new AuthClass();
            if(await  auth.AuthenticateAsync())
               // buttonsignin.IsEnabled = false;
            buttonsignin.Visibility = Visibility.Collapsed;
            
        }


    }
}

