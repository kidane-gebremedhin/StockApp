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
using System.Text.RegularExpressions;

namespace ToolBar.Resources.Fragments
{
    public class Users_Create : SupportFragment
    {
        MainActivity mainActivity = new MainActivity();

        private EditText userNameEditText, passwordEditText, confirmPasswordEditText, firstNameEditText, lastNameEditText, phoneNumberEditText, emailEditText, addressEditText;
        private Spinner rolesSpinner;
        private Button submitBtn, usersListBtn;
        public static int passwordLength=8, phoneNumberLength=13, tinNumberLength=10, vatNumberLength=10;
        public static string[] gest_roles = new string[] { "Seller", "Buyer"/*, "Admin"*/};
        public static string[] roles= gest_roles;
        public static string[] admin_roles = new string[] { /*"Seller", "Buyer", */"Admin"};
        public static string[] types = new string[] { "Individual", "Organization"};
        public ArrayAdapter rolesAdapter, typesAdapter;
        public static string userType = types[0];

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Users_Create, container, false);
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
            submitBtn = view.FindViewById<Button>(Resource.Id.submitBtn);
            usersListBtn = view.FindViewById<Button>(Resource.Id.usersListBtn);

            if (MainActivity.loggedUser != null && MainActivity.loggedUser.role == "Admin")
                roles = admin_roles;
            else
                roles = gest_roles;
              rolesAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, roles);
             typesAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, types);
            rolesSpinner.Adapter = rolesAdapter;

            submitBtn.Click += SubmitBtn_Click;
            usersListBtn.Click += UsersListBtn_Click;
            
            if(MainActivity.loggedUser==null || MainActivity.loggedUser.role != "Admin")
            {
                usersListBtn.Visibility = ViewStates.Invisible;
            }else
            {
                usersListBtn.Visibility = ViewStates.Visible;
            }
            return view;
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

        private void UsersListBtn_Click(object sender, EventArgs e)
        {

            MainActivity.mainActivity.replaceFragment(MainActivity.users_Index);
        }
        
        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.closeKeyboard();

            string role = admin_roles[0];// rolesSpinner.SelectedItem.ToString();
            string userName = userNameEditText.Text;
            string password = passwordEditText.Text;
            string confirmPassword = confirmPasswordEditText.Text;
            string phoneNumber = phoneNumberEditText.Text;
            string email = emailEditText.Text;
            string address = addressEditText.Text;
            string firstName = "";
            string lastName = "";

            if (phoneNumber.Length != phoneNumberLength || phoneNumber.Substring(0, 4) != "+251")
            {
                Toast.MakeText(this.Context, "Phone Number Length must be " + (phoneNumberLength - 3).ToString() + " characters long\nIt must start with +251", ToastLength.Short).Show();
                return;
            }
            if (email.Length<7 || email.IndexOf("@") == -1 || email.IndexOf(".com") == -1 || email.Substring(email.Length-4, 4) != ".com")
            {
                Toast.MakeText(this.Context, "Email must include tha '@' character and end with '.com' ", ToastLength.Short).Show();
                return;
            }
            
                firstName = firstNameEditText.Text;
                lastName = lastNameEditText.Text;
                if (role == "" || userName == "" || password == "" || confirmPassword == "" || firstName == "" || lastName == "" || phoneNumber == "" || address == "")
                {
                    Toast.MakeText(this.Context, "Please fill out the required fields!", ToastLength.Short).Show();
                    return;
                }

            
            if (password != confirmPassword)
            {
                Toast.MakeText(this.Context, "Passwords do not match!", ToastLength.Short).Show();
                return;
            }
            /*//Strong password validation
            if (!ValidatePassword(password) || password.Length < Users_Create.passwordLength)
            {
                Toast.MakeText(this.Context, "Password must be atleast " + Users_Create.passwordLength + " characters long\nIt must contain atleast one of each lowercase, uppercase, special character and number", ToastLength.Long).Show();
                return;
            }
            */

            var oldUsers = MainActivity.db.Table<User>().Where(u => u.userName == userName);;
            if (oldUsers != null && oldUsers.Count()>0)
            {
                Toast.MakeText(this.Context, "Username already taken!", ToastLength.Short).Show();
                return;
            }
            User user = new User(0, role, userName, password, firstName, lastName, phoneNumber, email, address);
            if (User.store(user))
            {
                rolesSpinner.SetSelection(0);
                userNameEditText.Text = "";
                passwordEditText.Text = "";
                confirmPasswordEditText.Text = "";
                firstNameEditText.Text = "";
                lastNameEditText.Text = "";
                phoneNumberEditText.Text = "";
                emailEditText.Text = "";
                addressEditText.Text = "";
                Toast.MakeText(this.Context, "Registeration successfull", ToastLength.Short).Show();
                //MainActivity.loggedUser = user;
                MainActivity.mainActivity.replaceFragment(new Users_Index());
                //LogIn.initDrawer();
            }
        }
/*        
regStr="(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[#$@!%&*?]).{8,}"
Explanation:

(?=.*\d)           => there is at least one digit
(?=.*[a-z]) => there is at least one lowercase character
(?=.*[A-Z]) => there is at least one uppercase character
(?=.*[#$@!%&*?])   => there is at least one special character
.{ 8,}
        => length is 8 or more

var regex = new Regex(@"(?<=%download%#)\d+");
return regex.Matches(strInput);
*/
        public static bool ValidatePassword(string password)
        {
            string patternPassword = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[#$@!%&*?]).{"+passwordLength.ToString()+",}";
            if (!string.IsNullOrEmpty(password))
            {
                if (!Regex.IsMatch(password, patternPassword))
                {
                    return false;
                }

            }
            return true;
        }


    }

}