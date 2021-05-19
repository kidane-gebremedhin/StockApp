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
//using Plugin.MediaManager;
using System.IO;
using SQLite;
using ToolBar.Models;

namespace ToolBar.Resources.Fragments
{
    public class Users_Edit : SupportFragment
    {
        MainActivity mainActivity = new MainActivity();
        private EditText userNameEditText, passwordEditText, confirmPasswordEditText, firstNameEditText, lastNameEditText, phoneNumberEditText, emailEditText, addressEditText;
        private Spinner rolesSpinner;
        private Button updateBtn;
        private Int64 mUserId;
        public ArrayAdapter rolesAdapter, typesAdapter;
        public static string[] types = Users_Create.types;
        public static string userType = Users_Create.userType;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Users_Edit, container, false);
            rolesSpinner = view.FindViewById<Spinner>(Resource.Id.rolesSpinner);
            rolesSpinner.Visibility = ViewStates.Gone;
            userNameEditText = view.FindViewById<EditText>(Resource.Id.userNameEditText);
            passwordEditText = view.FindViewById<EditText>(Resource.Id.passwordEditText);
            confirmPasswordEditText = view.FindViewById<EditText>(Resource.Id.confirmPasswordEditText);
            firstNameEditText = view.FindViewById<EditText>(Resource.Id.firstNameEditText);
            lastNameEditText = view.FindViewById<EditText>(Resource.Id.lastNameEditText);
            phoneNumberEditText = view.FindViewById<EditText>(Resource.Id.phoneNumberEditText);
            emailEditText = view.FindViewById<EditText>(Resource.Id.emailEditText);
            addressEditText = view.FindViewById<EditText>(Resource.Id.addressEditText);
            updateBtn = view.FindViewById<Button>(Resource.Id.updateBtn);

            if (MainActivity.loggedUser != null && MainActivity.loggedUser.role == "Admin")
                Users_Create.roles = Users_Create.admin_roles;
            else
                Users_Create.roles = Users_Create.gest_roles;

            rolesAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, Users_Create.roles);
             typesAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, types);
            rolesSpinner.Adapter = rolesAdapter;
            
            userNameEditText.Text = "";
            updateBtn.Click += UpdateBtn_Click;

            User user = Users_Index.user;
            if ((user == null))
                return view;
            rolesSpinner.SetSelection(rolesAdapter.GetPosition(user.role));
            userNameEditText.Text = user.userName;
            firstNameEditText.Text = user.firstName;
            lastNameEditText.Text = user.lastName;
            string phoneNumber = user.phoneNumber.Length > 1 ? user.phoneNumber.Substring(1):"";
            phoneNumberEditText.Text = "+"+phoneNumber;
            emailEditText.Text = user.email;
            addressEditText.Text = user.address;
            
            return view;
        }


        public void Edit_User(Int64 userId)
        {
            this.mUserId = userId;
            User user=new User();
            var query = MainActivity.db.Table<User>().Where(u => u.Id == mUserId);
            if (query != null)
            {
                foreach (var u in query.ToList<User>())
                {
                    user=u;
                    break;
                }
            }

           /*
            daySpinner.SetSelection(days_adapter.GetPosition(program.day));
            nameEditText.Text=program.name;
            startTime_HrsSpinner.SelectedItem = program.startTime_Hr;
            startTime_MinsSpinner.SelectedItem = program.startTime_Mins;
            startTime_amPmSpinner.SelectedItem = program.startTime_amPm;
            endTime_HrsSpinner.SelectedItem = program.endTime_Hr;
            endTime_MinsSpinner.SelectedItem = program.endTime_Mins;
            endTime_amPmSpinner.SelectedItem = program.endTime_amPm;
            //*/
        }


        public void UpdateBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.closeKeyboard();

            string role = rolesSpinner.SelectedItem.ToString();
            string userName = userNameEditText.Text;
            string password = passwordEditText.Text;
            string confirmPassword = confirmPasswordEditText.Text;
            string firstName = firstNameEditText.Text;
            string lastName = lastNameEditText.Text;
            string phoneNumber = phoneNumberEditText.Text;
            string email = emailEditText.Text;
            string address = addressEditText.Text;

            if (phoneNumber.Length != Users_Create.phoneNumberLength || phoneNumber.Substring(0, 4) != "+251")
            {
                Toast.MakeText(this.Context, "Phone Number Length must be " + (Users_Create.phoneNumberLength - 3).ToString() + " characters long\nIt must start with +251", ToastLength.Short).Show();
                return;
            }
            if (email.Length < 7 || email.IndexOf("@") == -1 || email.IndexOf(".com") == -1 || email.Substring(email.Length - 4, 4) != ".com")
            {
                Toast.MakeText(this.Context, "Email must include tha '@' character and end with '.com' ", ToastLength.Short).Show();
                return;
            }
            
                firstName = firstNameEditText.Text;
                lastName = lastNameEditText.Text;
                if (role == "" || userName == "" || firstName == "" || lastName == "" || phoneNumber == "" || address == "")
                {
                    Toast.MakeText(this.Context, "Please fill out the required fields!", ToastLength.Short).Show();
                    return;
                }

                
            

            if (password != "" || confirmPassword != "")
            {
                if (password != confirmPassword)
                {
                    Toast.MakeText(this.Context, "Passwords do not match!", ToastLength.Short).Show();
                    return;
                }
                /*//Strong password validation
                if (!Users_Create.ValidatePassword(password) || password.Length < Users_Create.passwordLength)
                {
                    Toast.MakeText(this.Context, "Password must be atleast " + Users_Create.passwordLength + " characters long\nIt must contain atleast one of each lowercase, uppercase, special character and number", ToastLength.Short).Show();
                    return;
                }
                */
            }
            var user = Users_Index.user;//MainActivity.db.Table<User>().First(u => u.Id == mUserId);
            if (user == null)
                return;
            var usersQuery = MainActivity.db.Table<User>().Where(u => u.userName == userName);
            User oldUser = usersQuery.Count()>0? usersQuery.First():null;
            if (oldUser != null && oldUser.Id!=user.Id)
            {
                Toast.MakeText(this.Context, "Username already taken!", ToastLength.Long).Show();
                return;
            }

         
            user.role = role;
            user.userName = userName;
            if(password!="")
                user.password = password;
            user.firstName = firstName;
            user.lastName = lastName;
            user.phoneNumber = phoneNumber;
            user.email = email;
            user.address = address;
            if (User.update(user))
            {

                if(MainActivity.loggedUser.role=="Admin")
                    MainActivity.mainActivity.replaceFragment(MainActivity.users_Index);
                else
                    MainActivity.mainActivity.replaceFragment(MainActivity.homeFragment);

                Toast.MakeText(this.Context, "User Profile Updated", ToastLength.Short).Show();
            }
            
        }
        
        private void TypeSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            userType = types[e.Position];
            if (userType == types[0])
            {
                //make visible type specific fields
                firstNameEditText.Visibility = ViewStates.Visible;
                lastNameEditText.Visibility = ViewStates.Visible;
            }
            else if (userType == types[1])
            {
                firstNameEditText.Visibility = ViewStates.Gone;
                lastNameEditText.Visibility = ViewStates.Gone;

            }
        }

    }

}