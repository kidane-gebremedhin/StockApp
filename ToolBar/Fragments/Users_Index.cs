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
using System.IO;
using SQLite;
using ToolBar.Adapters;
using Android.Support.V7.App;
using static Android.Widget.AdapterView;
using ToolBar.Models;

namespace ToolBar.Resources.Fragments
{
    public class Users_Index : SupportFragment
    {
        private ListView usersListView;
        private Button addBtn, showBtn, editBtn, showDeleteBtn, cancelBtn, deleteBtn;
        private long mUserId;
        private float screenHeight = 400;
        private LinearLayout actionOptions_PupUp, deleteConfirm_PupUp;
        public static List<User> users;
        public static User user;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Users_Index, container, false);
            usersListView = view.FindViewById<ListView>(Resource.Id.usersListView);
            showBtn = view.FindViewById<Button>(Resource.Id.showBtn);
            addBtn = view.FindViewById<Button>(Resource.Id.addBtn);
            cancelBtn = view.FindViewById<Button>(Resource.Id.cancelBtn);
            deleteBtn = view.FindViewById<Button>(Resource.Id.deleteBtn);
            editBtn = view.FindViewById<Button>(Resource.Id.editBtn);
            showDeleteBtn = view.FindViewById<Button>(Resource.Id.showDeleteBtn);
            actionOptions_PupUp = view.FindViewById<LinearLayout>(Resource.Id.actionOptions_PupUp);
            deleteConfirm_PupUp = view.FindViewById<LinearLayout>(Resource.Id.deleteConfirm_PupUp);
            
            usersListView.ItemClick += UsersListView_ItemClick;
            usersListView.ItemLongClick += UsersListView_ItemLongClick;

            addBtn.Click += AddBtn_Click;
            showBtn.Click += ShowBtn_Click;
            editBtn.Click += EditBtn_Click;
            showDeleteBtn.Click += ShowDeleteBtn_Click;
            deleteBtn.Click += DeleteBtn_Click;
            cancelBtn.Click += CancelBtn_Click;

            //Retrive Programs
            users = MainActivity.db.Table<User>().ToList();
            List<User> usersList=new List<User>();

            foreach (var u in users)
            {
                User user = new User(u.Id, u.role, u.userName, u.password, u.firstName, u.lastName, u.phoneNumber, u.email, u.address);
                usersList.Add(user);
            }
            Users_list_adapter adapter = new Users_list_adapter(this.Context, usersList);
            usersListView.Adapter = adapter;
            return view;
        }

        private void ShowBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            MainActivity.mainActivity.replaceFragment(MainActivity.users_Show);
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.replaceFragment(new Users_Create());
        }

        private void ShowDeleteBtn_Click(object sender, EventArgs e)
        {
            deleteConfirm_PupUp.TranslationY = screenHeight - 350;// screenHeight-150;
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.replaceFragment(new Users_Edit());
           // MainActivity.users_Edit.Edit_User(mUserId);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            Toast.MakeText(this.Context, "  User Delete Canceled", ToastLength.Short).Show();
        }

        
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            User.delete(mUserId);
            MainActivity.mainActivity.replaceFragment(new Users_Index());
            Toast.MakeText(this.Context, " User deleted successfully", ToastLength.Short).Show();
        }

        private void UsersListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;//250;// screenHeight;
            Console.WriteLine("" + e.Id.ToString());
        }

        private void UsersListView_ItemLongClick(object sender, ItemLongClickEventArgs e){
            mUserId = users[e.Position].Id;
            user = users[e.Position];
            actionOptions_PupUp.TranslationY = screenHeight - 350;// screenHeight-150;
    }

}
}