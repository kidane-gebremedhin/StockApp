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
    public class Users_list_adapter: BaseAdapter<User>
    {
        List<User> mUsers;
        Context mContext;

        public override int Count
        {
            get
            {
                return mUsers.Count;
            }
        }

        public override User this[int position]
        {
            get
            {
                return mUsers[position];
            }
        }

        public Users_list_adapter(Context context, List<User> users)
        {
            mContext = context;
            mUsers = users;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view=convertView;
            if (view == null)
                view = LayoutInflater.From(mContext).Inflate(Resource.Layout.users_list_row, null, false);

            TextView rolesTextView = view.FindViewById<TextView>(Resource.Id.rolesTextView);
            TextView userNameTextView = view.FindViewById<TextView>(Resource.Id.userNameTextView);
            TextView phoneNumberTextView = view.FindViewById<TextView>(Resource.Id.phoneNumberTextView);

            rolesTextView.Text = mUsers[position].role;
            userNameTextView.Text = mUsers[position].userName;
            phoneNumberTextView.Text = mUsers[position].phoneNumber;
           
            return view;
        }
    }
}