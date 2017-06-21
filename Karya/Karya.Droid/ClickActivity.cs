using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Karya.Core;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Android.Provider;
using Android.Graphics;

namespace Karya.Droid
{
    [Activity(Label = "Click")]
    public class ClickActivity : Activity
    {

        public static File _file;
        public static File _dir;
        public Bitmap bitmap;
        public static readonly int PickImageId = 1000;
        GetSubjectClass getsub = new GetSubjectClass();
        
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Clicklayout);
            Button clickbut = FindViewById<Button>(Resource.Id.imageclickbut);
            clickbut.Click += onclickbutclick;
            // Create your application here
            Spinner subspin = FindViewById<Spinner>(Resource.Id.subspin);
            List<Subject> slist = await getsub.ReadAllSubject();
            string[] subname = slist.Select(x => x.Subjectname).ToArray<string>();
            ArrayAdapter adapter = new ArrayAdapter(this,Resource.Layout.simplespinnerlayout, subname);
            subspin.Adapter = adapter;
            subspin.ItemSelected += Subspin_ItemSelected;
            
        }

        private void Subspin_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //add code to save the image in the selected folder
            Spinner sp = sender as Spinner;
            string selectedsubjectname = sp.SelectedItem.ToString();
            _dir = new File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/" + selectedsubjectname);

        }

       
        protected override void OnPause()
        {
            base.OnPause();
            AppClass.addtodebug("on pause here");
        }



        protected override void OnResume()
        {
            base.OnResume();
            AppClass.addtodebug("on resume here");
            Spinner subspin = FindViewById<Spinner>(Resource.Id.subspin);
            subspin.Visibility = ViewStates.Visible;
        }


        private void onclickbutclick(object sender, EventArgs e)
        {
            CreateAndShowDialog();
            
        }


        private void CreateAndShowDialog()
        {
          
            AlertDialog dialog = (new AlertDialog.Builder(this)).Create();
            dialog.SetButton("Take a Picture", ontakepic);
            dialog.SetButton2("Choose From Gallery", onchoosepic);
            dialog.SetCancelable(true);
            dialog.SetCanceledOnTouchOutside(true);
            dialog.Show();
        }


        private void onchoosepic(object sender, DialogClickEventArgs e)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            _file = new File(_dir, string.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }


        private void ontakepic(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            _file = new File(_dir, string.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
            StartActivityForResult(intent, 0);
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            AppClass.addtodebug("Back From camera");
            base.OnActivityResult(requestCode, resultCode, data);
           
            // Make it available in the gallery


            if (resultCode == Result.Canceled)
                return;
            else
            {
                if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
                {
                    Android.Net.Uri uri = data.Data;
                    //_imageView.SetImageURI(uri);
                    int ht = Resources.DisplayMetrics.HeightPixels;
                    int wid= Resources.DisplayMetrics.WidthPixels;

                    Intent mediaScanIntent1 = new Intent(Intent.ActionMediaScannerScanFile);
                    Android.Net.Uri contentUri1 = Android.Net.Uri.FromFile(_file);
                    mediaScanIntent1.SetData(contentUri1);
                    SendBroadcast(mediaScanIntent1);

                    bitmap = _file.Path.LoadAndResizeBitmap(wid, ht);
                    
                    // Dispose of the Java side bitmap.
                    GC.Collect();
                }

                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Android.Net.Uri contentUri = Android.Net.Uri.FromFile(_file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);

                // Display in ImageView. We will resize the bitmap to fit the display.
                // Loading the full sized image will consume to much memory
                // and cause the application to crash.

                int height = Resources.DisplayMetrics.HeightPixels;
                int width = Resources.DisplayMetrics.WidthPixels;

                bitmap = _file.Path.LoadAndResizeBitmap(width, height);
               
                // Dispose of the Java side bitmap.
                GC.Collect();
            }
        }
    }



    public static class BitmapHelpers
    {
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }
    }


}

