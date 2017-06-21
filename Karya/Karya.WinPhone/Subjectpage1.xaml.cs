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
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Subjectpage : Page
    {
        public Subjectpage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void buttonaddsub_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msgd;
            if (textsub1.Text == "")
            {
                msgd = new MessageDialog("Add first subjeect before you enter another");
                await msgd.ShowAsync();
            }
            else {
                StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync
(textsub1.Text, CreationCollisionOption.ReplaceExisting);
                TextBlock t1 = new TextBlock();
                t1.Name = "block 2";
                t1 = textblocksub1;
                t1.Text = "new block";
                
                TextBox t2 = textsub1;
                t2.Text = "new box";
            }
            


        }
    }
}
