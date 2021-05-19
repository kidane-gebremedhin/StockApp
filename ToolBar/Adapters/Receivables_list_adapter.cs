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
    public class Receivables_list_adapter: BaseAdapter<Receivables>
    {
        List<Receivables> mReceivables;
        Context mContext;

        public override int Count
        {
            get
            {
                return mReceivables.Count;
            }
        }

        public override Receivables this[int position]
        {
            get
            {
                return mReceivables[position];
            }
        }

        public Receivables_list_adapter(Context context, List<Receivables> Receivables)
        {
            mContext = context;
            mReceivables = Receivables;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view=convertView;
            if (view == null)
                view = LayoutInflater.From(mContext).Inflate(Resource.Layout.Receivables_list_row, null, false);

            TextView productTextView = view.FindViewById<TextView>(Resource.Id.productTextView);
            TextView qtyTextView = view.FindViewById<TextView>(Resource.Id.qtyTextView);
            TextView dateTextView = view.FindViewById<TextView>(Resource.Id.dateTextView);
            
            Int64 productId = mReceivables[position].productId;
            var que = MainActivity.db.Table<Product>().Where(i => i.Id == productId);
            productTextView.Text = que.Count()>0? que.First().name:"";
            qtyTextView.Text = mReceivables[position].qty.ToString();
            dateTextView.Text = mReceivables[position].date.ToShortDateString();

            return view;
        }
    }
}