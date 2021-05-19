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
using ToolBar.Models;

namespace ToolBar.Adapters
{
    public class Product_list_adapter: BaseAdapter<Product>
    {
        List<Product> mProducts;
        Context mContext;

        public override int Count
        {
            get
            {
                return mProducts.Count;
            }
        }

        public override Product this[int position]
        {
            get
            {
                return mProducts[position];
            }
        }

        public Product_list_adapter(Context context, List<Product> Products)
        {
            mContext = context;
            mProducts = Products;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view=convertView;
            if (view == null)
                view = LayoutInflater.From(mContext).Inflate(Resource.Layout.Product_list_row, null, false);

            TextView nameTextView = view.FindViewById<TextView>(Resource.Id.nameTextView);
            TextView qtyTextView = view.FindViewById<TextView>(Resource.Id.qtyTextView);

            nameTextView.Text = mProducts[position].name;
            qtyTextView.Text = mProducts[position].qty.ToString();

            return view;
        }
    }
}