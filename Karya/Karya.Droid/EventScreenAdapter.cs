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
    public class EventScreenAdapter:BaseAdapter<Event>
    {
        List<Event> items;
        Activity context;
        public EventScreenAdapter(Activity context, List<Event> items)
       : base()
   {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Event this[int position]
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
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = item.Date+ " "+item.Time;
            //view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(item.ImageResourceId);
            return view;
        }
    }
}