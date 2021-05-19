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

namespace ToolBar
{
    public class ListViewAdapter: BaseAdapter<string>
    {
        List<string> mItams;
        Context mContext;

        public ListViewAdapter(Context context, List<string> items)
        {
            mItams = items;
            mContext = context;
        }

        public override int Count
        {
            get
            {
                return mItams.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override string this[int position]{
            get { return mItams[position]; }
        }

       public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(mContext).Inflate(Resource.Layout.listview_row, null, false);
            TextView textView = view.FindViewById<TextView>(Resource.Id.rowTextView);
            textView.Text = mItams[position];
            return view;
        }

    }
}