using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using SupportFragment = Android.Support.V4.App.Fragment;
using ToolBar.Models;

namespace ToolBar.Resources.Fragments
{
    public class Users_Show : SupportFragment
    {
        MainActivity mainActivity = new MainActivity();

        private TextView rolesTextView, userNameTextView, firstNameTextView, lastNameTextView, phoneNumberTextView, emailTextView, addressTextView;
        private Button usersListBtn, editUserBtn;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Users_Show, container, false);
            rolesTextView = view.FindViewById<TextView>(Resource.Id.rolesTextView);
            userNameTextView = view.FindViewById<TextView>(Resource.Id.userNameTextView);
            firstNameTextView = view.FindViewById<TextView>(Resource.Id.firstNameTextView);
            lastNameTextView = view.FindViewById<TextView>(Resource.Id.lastNameTextView);
            phoneNumberTextView = view.FindViewById<TextView>(Resource.Id.phoneNumberTextView);
            emailTextView = view.FindViewById<TextView>(Resource.Id.emailTextView);
            addressTextView = view.FindViewById<TextView>(Resource.Id.addressTextView);
            
            usersListBtn = view.FindViewById<Button>(Resource.Id.usersListBtn);
            editUserBtn = view.FindViewById<Button>(Resource.Id.editUserBtn);
            
            usersListBtn.Click += UsersListBtn_Click;
            editUserBtn.Click += EditUserBtn_Click;

            User user = Users_Index.user;
            if (user == null)
                user = MainActivity.loggedUser;


            
            rolesTextView.Text =user.role;
            userNameTextView.Text = user.userName;
            phoneNumberTextView.Text = user.phoneNumber;
            emailTextView.Text = user.email;
            addressTextView.Text = user.address;

            
            lastNameTextView.Text = user.lastName;
            firstNameTextView.Text = user.firstName;
            

            return view;
        }

        private void EditUserBtn_Click(object sender, EventArgs e)
        {
            Users_Index.user = MainActivity.loggedUser;
            MainActivity.mainActivity.replaceFragment(MainActivity.users_Edit);
        }

        private void UsersListBtn_Click(object sender, EventArgs e)
        {

            MainActivity.mainActivity.replaceFragment(MainActivity.users_Index);
        }
        
    }
    
}