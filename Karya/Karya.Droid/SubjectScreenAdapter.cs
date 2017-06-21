using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Karya.Core;

namespace Karya.Droid
{
    public class SubjectScreenAdapter:BaseAdapter<Subject>
    {
        List<Subject> items;
        Activity context;
        public SubjectScreenAdapter(Activity context, List<Subject> items)
       : base()
   {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Subject this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.listview, null);
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Subjectname;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = item.Teachername;
            //view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(item.ImageResourceId);
            return view;
        }
    }
}