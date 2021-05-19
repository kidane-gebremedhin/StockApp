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
using Android;
using Android.Support.V4.Content;
using Android.Graphics.Drawables;

namespace ToolBar.Fragments
{
    public class RightDrawer_ListAdapter : BaseAdapter<string>
    {
        List<string> mdata;
        Context mContext;
        public static int[] rightDrawableIconIds;
        public override int Count
        {
            get
            {
                return mdata.Count;
            }
        }

        public override string this[int position]
        {
            get
            {
                return mdata[position];
            }
        }

        public RightDrawer_ListAdapter(Context context, List<string> data)
        {
            mContext = context;
            mdata = data;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(mContext).Inflate(Resource.Layout.listview_row, null, false);

            TextView rowTextView = view.FindViewById<TextView>(Resource.Id.rowTextView);
            ImageView iconImageView = view.FindViewById<ImageView>(Resource.Id.iconImageView);

            rowTextView.Text = mdata[position];

            //Drawable icon = ContextCompat.GetDrawable(mContext, Resource.Drawable.fm_img1);
            iconImageView.SetImageDrawable(ContextCompat.GetDrawable(MainActivity.mainActivity, rightDrawableIconIds[position]));

            return view;
        }
    }
}