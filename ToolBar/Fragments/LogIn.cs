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
using Android.Media;
using System.IO;
using SQLite;
using ToolBar.Models;
using ToolBar.Fragments;

namespace ToolBar.Resources.Fragments
{
    public class LogIn : SupportFragment
    {
        MainActivity mainActivity = new MainActivity();
        private EditText userNameEditText, passwordEditText;
        private TextView messageTextView;
        private Button logInBtn;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (MainActivity.loggedUser != null) { 
                MainActivity.mainActivity.replaceFragment(new Home_fragment());
                return null;
            }
            View view = inflater.Inflate(Resource.Layout.LogIn, container, false);
            userNameEditText = view.FindViewById<EditText>(Resource.Id.userNameEditText);
            passwordEditText = view.FindViewById<EditText>(Resource.Id.passwordEditText);
            messageTextView = view.FindViewById<TextView>(Resource.Id.messageTextView);
            logInBtn = view.FindViewById<Button>(Resource.Id.logInBtn);
            //signUpBtn = view.FindViewById<Button>(Resource.Id.signUpBtn);
            
            logInBtn.Click += LogInBtn_Click;
            //signUpBtn.Click += SignUpBtn_Click;
            return view;
        }

        private void SignUpBtn_Click(object sender, EventArgs e)
        {
            if (MainActivity.loggedUser != null)
            {
                Toast.MakeText(this.Context, "Already logged in", ToastLength.Short).Show();
                return;
            }
            MainActivity.mainActivity.replaceFragment(new Users_Create());
        }

        private void LogInBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.closeKeyboard();

            if (MainActivity.loggedUser != null)
            {
                //Toast.MakeText(this.Context, "Already logged in", ToastLength.Short).Show();
                MainActivity.mainActivity.replaceFragment(MainActivity.homeFragment);
                return;
            }
            string userName = userNameEditText.Text;
            string password = passwordEditText.Text;
            userNameEditText.Text="";
            passwordEditText.Text="";
            if (userName=="" || password == "")
            {
                messageTextView.Text = "Invalid Credentials";
                return;
            }
            /*
            if (!Users_Create.ValidatePassword(password))
            {
                messageTextView.Text = "Invalid Credentials";
                return;
            }
            */
            List<User> users = MainActivity.db.Table<User>().Where(u => (u.userName == userName && u.password==password) /*&& u.status=="active"*/).ToList();
           // /*
            if (users.Count == 0 || users.Count>1)
            {
                messageTextView.Text = "Invalid Credentials";
                return;
            }
            //*/
            MainActivity.loggedUser = users[0];
            MainActivity.mainActivity.replaceFragment(MainActivity.homeFragment);

            //Authorization
            initDrawer();

        }


        public static void initDrawer()
        {
            if (MainActivity.loggedUser == null)
            {
                MainActivity.mLeftDataset.Remove("Receivables");
                MainActivity.mLeftDataset.Remove("Issuables/Orders");
                MainActivity.mLeftDataset.Remove("Products/Items");
                MainActivity.mLeftDataset.Remove("Users");


                if (!MainActivity.mLeftDataset.Contains("Sign In"))
                    MainActivity.mLeftDataset.Add("Sign In");
                
                MainActivity.fragmentsList = new SupportFragment[] { new LogIn(), MainActivity.signUp };
            LeftDrawer_ListAdapter.leftDrawableIconIds = new int[]{ Resource.Drawable.about,  Resource.Drawable.contact};
            }
            else {
                if (MainActivity.loggedUser.role == "Admin")
                {

                    MainActivity.mLeftDataset.Remove("Sign In");
                    //MainActivity.mLeftDataset.Remove("Sign Up");

                    MainActivity.mLeftDataset.Add("Receivables");
                    MainActivity.mLeftDataset.Add("Issuables/Orders");
                    MainActivity.mLeftDataset.Add("Products/Items");
                    MainActivity.mLeftDataset.Add("Users");
                
                    MainActivity.fragmentsList = new SupportFragment[] { MainActivity.Receivables_Create, MainActivity.Issuables_Create, MainActivity.Product_Index, MainActivity.users_Index };
                    LeftDrawer_ListAdapter.leftDrawableIconIds = new int[]{ Resource.Drawable.list2, Resource.Drawable.about, Resource.Drawable._list2, Resource.Drawable.users };
                }
            }

            MainActivity.mLeftAdapter = new LeftDrawer_ListAdapter(MainActivity.mainActivity, MainActivity.mLeftDataset);
            MainActivity.leftListView.Adapter = MainActivity.mLeftAdapter;
        }

    }
}