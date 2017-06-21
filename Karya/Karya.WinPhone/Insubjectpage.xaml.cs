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
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Karya.Core;
using Windows.Storage;
using System.Collections.ObjectModel;
using Windows.Phone.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Karya.WinPhone
{
    public class subholder
    {
        public string subname { get; set; }
        public string icon { get; set; }

        public subholder(string s, string i)
        {
            subname = s;
            icon = i;
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Insubjectpage : Page
    {
        Subject passedsub;
        GetSubjectClass getsub = new GetSubjectClass();

        TextBlock tsubname = new TextBlock();

        TextBox boxsubname = new TextBox();

        TextBlock ttname = new TextBlock();

        TextBox boxtname = new TextBox();

        Button bsave = new Button();

        Button bdel = new Button();

        public Insubjectpage()
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
            passedsub = e.Parameter as Subject;
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

        private async void sublistviewobj_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFolder localsubjectFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(passedsub.Subjectname);
            //await localsubjectFolder.CreateFileAsync("lesson2.png");
            //await localsubjectFolder.CreateFileAsync("chapter1notes.png");
            IReadOnlyList<StorageFile> allsubfiles = await localsubjectFolder.GetFilesAsync();
            ObservableCollection<subholder> allsubfilenames = new ObservableCollection<subholder>();

            foreach (StorageFile file in allsubfiles)
            {
                //here you have to get specific logos for the specific extensions and store then in the source
                string fileext = file.FileType;
                string iconpath = "Assets/SmallLogo.scale-240.png";
                if (fileext == ".txt")
                { iconpath = "Assets/WideLogo.scale-240.png"; }
                allsubfilenames.Add(new subholder(file.Name, iconpath));
            }

            sublistviewobj.ItemsSource = allsubfilenames.OrderBy(i => i.subname).ToList();

            //IReadOnlyList<StorageFile> newFolder =await localFolder.GetFilesAsync();
        }

        //edit button click
        private void button_Click(object sender, RoutedEventArgs e)
        {
            grid1.Children.Remove(sublistviewobj);
            sublistviewobj.Visibility = Visibility.Collapsed;
            textBlockhead.Text = "Edit Details";
            tsubname.Text = "Subject Name:";
            var margin = tsubname.Margin;
            tsubname.FontSize = 20;
            margin.Top = 150;
            margin.Left = 20;
            margin.Right = 200;
            margin.Bottom = 20;
            tsubname.Margin = margin;
            tsubname.Visibility = Visibility.Visible;
            grid1.Children.Add(tsubname);

            boxsubname.Text = passedsub.Subjectname;
            margin = boxsubname.Margin;
            margin.Top = 150;
            boxsubname.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
            margin.Left = 210; 
            margin.Right = 20;
            margin.Bottom = 20;
            boxsubname.Margin = margin;
            boxsubname.Name = "subnametext";
            boxsubname.Visibility = Visibility.Visible;
            grid1.Children.Add(boxsubname);

            ttname.Text = "Teacher Name:";
            ttname.FontSize = 20;
            margin = ttname.Margin;
            margin.Top = 220;
            margin.Left = 20;
            margin.Right = 200;
            margin.Bottom = 20;
            ttname.Margin = margin;
            ttname.Visibility = Visibility.Visible;
            grid1.Children.Add(ttname);

            boxtname.Text = passedsub.Teachername;
            margin = boxtname.Margin;
            // boxtname.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
            margin.Top = 220;
            margin.Left = 210;
            margin.Right = 20;
            margin.Bottom = 20;
            boxtname.Margin = margin;
            boxtname.Name = "tnametext";
            boxtname.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            boxtname.Visibility = Visibility.Visible;
            grid1.Children.Add(boxtname);

            margin = bsave.Margin;
            margin.Top = 230;
            margin.Left = 20;
            // margin.Right = 20;
            margin.Bottom = 10;
            bsave.Margin = margin;
            bsave.Content = "Save";
            bsave.Click += save_click;
            bsave.Visibility = Visibility.Visible;
            grid1.Children.Add(bsave);


        }

        private async void save_click(object sender, RoutedEventArgs e)
        {
            //here  call that getsubject update function
            string oldsubname = passedsub.Subjectname;
            Subject upsub = passedsub;
            upsub.Subjectname = boxsubname.Text;
            upsub.Teachername = boxtname.Text;
            var upres = await getsub.UpdateSubject(upsub, oldsubname);
            if (!upres)
            { //code to handle what happens when name already exists and update cant take place
                MessageDialog m = new MessageDialog("Already exists. Please enter a different subject name.");
                await m.ShowAsync();
            }
            else
                Frame.Navigate(typeof(Subjectpage));
        }


        private async void delete_click(object sender, RoutedEventArgs e)
        {
            //here  call that getsubject update function
            Subject upsub = passedsub;
            upsub.Subjectname = boxsubname.Text;
            upsub.Teachername = boxtname.Text;
            var delres = await getsub.DeleteSubject(upsub);
            if (!delres)
            { //code to handle what happens when name already exists and update cant take place
                MessageDialog m = new MessageDialog("Subject doesn't exist any longer. Please restart the app.");
                await m.ShowAsync();
            }
        }

        private async void delsubbutton_Click(object sender, RoutedEventArgs e)
        {
            Subject delsub = passedsub;

            var delres = await getsub.DeleteSubject(delsub);
            if (!delres)
            { //code to handle what happens when name already exists and update cant take place
                MessageDialog m = new MessageDialog("Subject doesn't exist any longer. Please restart the app.");
                await m.ShowAsync();
               
            }
            Frame.Navigate(typeof(Subjectpage));
        }
    }
}
