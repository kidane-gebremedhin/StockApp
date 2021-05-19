using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using ToolBar.Resources.Fragments;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Android.Webkit;
using System.IO;
using SQLite;
using Android;
using Android.Runtime;
using Android.Net;
using Android.Content;
using Android.Util;
using ToolBar.Fragments;
using ToolBar.Models;

namespace ToolBar
{
    [Activity(Label = "TenderApp", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges =Android.Content.PM.ConfigChanges.ScreenSize|Android.Content.PM.ConfigChanges.Orientation)]
    public class MainActivity : ActionBarActivity
    {
        public static MainActivity mainActivity;
        public static User loggedUser;
        public static bool IS_CONNECTED=false;
        private SupportToolbar mToolBar;
        private SupportToolbar mStandAloneToolbar;

        private SupportToolbar toolbar, footer_toolbar;
        private MySupportActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ListView leftListView, rightListView;
        private LinearLayout mLeftDrawer, mRightDrawer;
        private ListViewAdapter mLeftAdapter, mRightAdapter;
        private List<string> mLeftDataset, mRightDataSet;
        private Button developersBtn;

        public static SupportFragment currentFragment, homeFragment, program_Index, program_Create, fragment3, developers, webViewFragment;
        public static SupportFragment users_Show, users_Index, users_Create;
        public static SupportFragment category_Show, category_Index, category_Create;
        public static SupportFragment items_Show, items_Index, items_Create;
        public static SupportFragment bids_Show, bids_Index, bids_Create;
        public static SupportFragment about_Fragment, contact_Fragment;
        
        public static Program_Edit program_Edit;
        public static Users_Edit users_Edit;
        public static Category_Edit category_Edit;
        public static Items_Edit items_Edit;
        public static Bids_Edit bids_Edit;
        private Stack<SupportFragment> backFragmentStack;
        private SupportFragment[] fragmentsList;
        //sqlite database insertion
        public static string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "TenderApp2.db3");
        public static SQLiteConnection db = new SQLiteConnection(dbPath);

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            mainActivity = this;
            //AskPermissions
            TryToGetPermissions();
            //Check for Connection
            //checkNetworkConnection();

            //initialize db
           // var db = new SQLiteConnection(dbPath);
            //setup a table
            db.CreateTable<User>();
            db.CreateTable<Category>();
            db.CreateTable<Item>();
            db.CreateTable<Bid>();
            //Insert new Program
            /*
              for (var i = 0; i < 4; i++)
             {
                 User u = new User("Seller", "a", "a", "2", "2", "3", "3", "4", "4", "5", "5");
                 db.Insert(u);
             }
             */

             /*
            var query = db.Table<Item>().Where(program => program.Id > 0);

            if (query != null)
            {
                foreach (var prog in query.ToList())
                {
                    Console.WriteLine("deleted: " + prog.name + " - " + prog.Id.ToString());
                    db.Delete(prog);
                    db.Delete<Item>(prog.Id);
                }
            }
            */

            /*
             mToolBar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(mToolBar);
            SupportActionBar.Title = "Hello Support V7";


            mStandAloneToolbar = FindViewById<SupportToolbar>(Resource.Id.standAloneToolbar);
            mStandAloneToolbar.InflateMenu(Resource.Menu.edit_menu);
            mStandAloneToolbar.MenuItemClick += MStandAloneToolbar_MenuItemClick;
            */

            toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            footer_toolbar = FindViewById<SupportToolbar>(Resource.Id.footer_toolbar);
            footer_toolbar.MenuItemClick += Footer_toolbar_MenuItemClick;
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<LinearLayout>(Resource.Id.left_drawer);
            mRightDrawer = FindViewById<LinearLayout>(Resource.Id.right_drawer);
            leftListView = FindViewById<ListView>(Resource.Id.leftListView);
            rightListView = FindViewById<ListView>(Resource.Id.rightListView);
            developersBtn = FindViewById<Button>(Resource.Id.developersBtn);
            backFragmentStack = new Stack<SupportFragment>();
                
            mLeftDrawer.Tag = 0;
            mRightDrawer.Tag = 1;
            SetSupportActionBar(toolbar);
            footer_toolbar.InflateMenu(Resource.Menu.edit_menu);
            homeFragment = new Home_fragment();

            ///*
            program_Index = new Program_Index();
            program_Create = new Program_Create();
            program_Edit = new Program_Edit();
            fragment3 = new Fragment3();
            webViewFragment = new Web_viewFragment();
            developers = new Developers();
            //*/

            //Users
            users_Index = new Users_Index();
            users_Create = new Users_Create();
            users_Edit = new Users_Edit();
            users_Show = new Users_Show();

            //Categories
            category_Index = new Category_Index();
            category_Create = new Category_Create();
            category_Edit = new Category_Edit();
            category_Show = new Category_Show();

            //Items
            items_Index = new Items_Index();
            items_Create = new Items_Create();
            items_Edit = new Items_Edit();
            items_Show = new Items_Show();

            //Bids
            bids_Index = new Bids_Index();
            bids_Create = new Bids_Create();
            bids_Edit = new Bids_Edit();
            bids_Show = new Bids_Show();

            //Externals
            about_Fragment = new About_fragment();
            contact_Fragment = new Contact_fragment();



            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.frameLayout, new LogIn(), "logInFragment");
            trans.Commit();
            currentFragment = null;


            mLeftDataset = new List<string>();
            if (Users_Index.user == null)
            {
                mLeftDataset.Add("Home");
                mLeftDataset.Add("About");
                mLeftDataset.Add("Contact");
                //mLeftDataset.Add("Share");
                fragmentsList = new SupportFragment[] { homeFragment, about_Fragment, contact_Fragment };
            }
            if (Users_Index.user.role == "Seller")
            {

                mLeftDataset.Add("Home");
                mLeftDataset.Add("About");
                mLeftDataset.Add("Contact");
                //mLeftDataset.Add("Share");
                mLeftDataset.Add("Items");
                mLeftDataset.Add("Add Items");
                fragmentsList = new SupportFragment[] { homeFragment, about_Fragment, contact_Fragment, items_Index, items_Create };
            }
            if (Users_Index.user.role == "Buyer")
            {

                mLeftDataset.Add("Home");
                mLeftDataset.Add("About");
                mLeftDataset.Add("Contact");
               // mLeftDataset.Add("Share");
                mLeftDataset.Add("Bids");
                mLeftDataset.Add("Add Bids");
                fragmentsList = new SupportFragment[] { homeFragment, about_Fragment, contact_Fragment, bids_Index, bids_Create };
            }
            if (Users_Index.user.role == "Admin")
            {
                mLeftDataset.Add("Users");
                mLeftDataset.Add("Add Users");
                mLeftDataset.Add("Categories");
                mLeftDataset.Add("Add Categories");
                mLeftDataset.Add("Items");
                mLeftDataset.Add("Add Items");
                mLeftDataset.Add("Bids");
                mLeftDataset.Add("Add Bids");
                fragmentsList = new SupportFragment[] { users_Index, users_Create, category_Index, category_Create, items_Index, items_Create, bids_Index, bids_Create };
            }else
            {
                mLeftDataset.Add("Home");
                mLeftDataset.Add("About");
                mLeftDataset.Add("Contact");
                //mLeftDataset.Add("Share");
                fragmentsList = new SupportFragment[] { homeFragment, about_Fragment, contact_Fragment };
            }
            mLeftAdapter = new ListViewAdapter(this, mLeftDataset);
            leftListView.Adapter = mLeftAdapter;
            leftListView.ItemClick += MLeftDrawer_ItemClick;

            mRightDataSet = new List<string>();
            mRightDataSet.Add("Right Item 1");
            mRightDataSet.Add("Right Item 2");
            mRightDataSet.Add("Right Item 3");
            mRightAdapter = new ListViewAdapter(this, mRightDataSet);
            rightListView.Adapter = mRightAdapter;

            mDrawerToggle = new MySupportActionBarDrawerToggle(this, mDrawerLayout, Resource.String.openDrawer, Resource.String.closeDrawer);

            mDrawerLayout.SetDrawerListener(mDrawerToggle);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            mDrawerToggle.SyncState();

            if (bundle != null)
            {
                if (bundle.GetString("drawerState") == "open")
                {
                    SupportActionBar.SetTitle(Resource.String.openDrawer);
                }
                else
                {
                    SupportActionBar.SetTitle(Resource.String.closeDrawer);
                }
            }
            else
            {
                //the activity is running for thte first time
                SupportActionBar.SetTitle(Resource.String.closeDrawer);
            }
        }

        private void MLeftDrawer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            replaceFragment(fragmentsList[e.Position]);
            mDrawerLayout.CloseDrawer(mLeftDrawer);
        }

        private void Footer_toolbar_MenuItemClick(object sender, SupportToolbar.MenuItemClickEventArgs e)
        {
            mDrawerLayout.CloseDrawer(mLeftDrawer);
            mDrawerLayout.CloseDrawer(mRightDrawer);

            var transaction=SupportFragmentManager.BeginTransaction();
            switch (e.Item.ItemId)
            {
                case Resource.Id.developersBtn:
                    replaceFragment(developers);
                    return;
                case Resource.Id.homeBtn:
                    replaceFragment(homeFragment);
                    return;
                
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //closes drawer after any action
            mDrawerLayout.CloseDrawer(mLeftDrawer);
            mDrawerLayout.CloseDrawer(mRightDrawer);

            switch (item.ItemId) {
                case Android.Resource.Id.Home:
                    mDrawerToggle.OnOptionsItemSelected(item);
                    return true;
                case Resource.Id.help_action:
                    if (mDrawerLayout.IsDrawerOpen(mRightDrawer))
                        mDrawerLayout.CloseDrawer(mRightDrawer);
                    else
                    {
                        mDrawerLayout.CloseDrawer(mLeftDrawer);
                        mDrawerLayout.OpenDrawer(mRightDrawer);
                    }
                    return true;
                //Fragments Handler
                case Resource.Id.page1:
            mDrawerLayout.CloseDrawer(mLeftDrawer);
                    replaceFragment(program_Index);
                    return true;
                case Resource.Id.page2:
            mDrawerLayout.CloseDrawer(mLeftDrawer);
                    replaceFragment(program_Create);
                    return true;
                case Resource.Id.page3:
            mDrawerLayout.CloseDrawer(mLeftDrawer);
                    replaceFragment(fragment3);
                    return true;
                case Resource.Id.page4:
                    //Check for Connection
                    MainActivity.mainActivity.checkNetworkConnection();
                    replaceFragment(webViewFragment);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);

            }
        }
        private void MStandAloneToolbar_MenuItemClick(object sender, SupportToolbar.MenuItemClickEventArgs e)
        {
            //closes drawer after any action
            mDrawerLayout.CloseDrawer(mLeftDrawer);
            mDrawerLayout.CloseDrawer(mRightDrawer);

            switch (e.Item.ItemId)
            {
                case 1:
                    Console.WriteLine("Edit");
                    break;
            }
        }
        /*
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        */

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.right_drawer_action_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        protected override void OnSaveInstanceState(Bundle outState)
        {
            //is colled on orientation change
            if (mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left))
                outState.PutString("drawerState", "open");
            else
                outState.PutString("drawerState", "close");

            base.OnSaveInstanceState(outState);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            mDrawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            mDrawerToggle.OnConfigurationChanged(newConfig);
        }

        public SupportFragment replaceFragment(SupportFragment fragment)
        {
            //if (fragment.IsVisible)
              //  return;
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.frameLayout, fragment);
            trans.AddToBackStack(null);
            trans.Commit();

            currentFragment = fragment;

            return fragment;
        }

        /*
        public void showFragment(SupportFragment fragment)
        {
            if (fragment.IsVisible)
                return;
            var trans = SupportFragmentManager.BeginTransaction();
            trans.SetCustomAnimations(Resource.Animation.slide_in, Resource.Animation.slide_out, Resource.Animation.slide_in, Resource.Animation.slide_out);
            trans.Hide(currentFragment);
            trans.Show(fragment);
            trans.AddToBackStack(null);
            trans.Commit();

            backFragmentStack.Push(currentFragment);
            currentFragment = fragment;
        }
        */
        
        public override void OnBackPressed()
        {
            /*
            if (backFragmentStack.Count > 0)
            {
                SupportFragmentManager.PopBackStack();
                currentFragment = backFragmentStack.Pop();
            }
            else
            {
                base.OnBackPressed();
            }
            */
            base.OnBackPressed();
        }
        
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (e.KeyCode == Keycode.Back)
            {
                if(new Web_viewFragment().webView!=null)
                    new Web_viewFragment().webView.GoBack();
            }
            return true;
        }


        //Permissions Management
        async void TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                GetPermissionsAsync();
                return;
            }


        }
        const int RequestLocationId = 0;

        readonly string[] PermissionsGroupLocation =
            {
                            //TODO add more permissions
                            Manifest.Permission.AccessCoarseLocation,
                            Manifest.Permission.AccessFineLocation,
             };
        async void GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                //TODO change the message to show the permissions name
                Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();
                return;
            }

            if (ShouldShowRequestPermissionRationale(permission))
            {
                //set alert for executing the task
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application needs special permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });

                Dialog dialog = alert.Create();
                dialog.Show();


                return;
            }

            RequestPermissions(PermissionsGroupLocation, RequestLocationId);

        }
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults.Length>0 && grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {
                            Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();

                        }
                        else
                        {
                            //Permission Denied :(
                            Toast.MakeText(this, "Special permissions denied", ToastLength.Short).Show();

                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        //Check network Connection
        public async void checkNetworkConnection()
        {
            this.RunOnUiThread(() =>
            {
                if (isInternetAvailable(this))
                    return;
                var alertDialog = new AlertDialog.Builder(this)
                    .SetTitle("Network Error")
                    .SetMessage("There is no internet connection\n Open Mobile Data and try again!")
                    .SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        IS_CONNECTED = true; //this.Finish();
                        return;
                    })
                    .Create();
                alertDialog.Show();
            });
        }

        //Connection 
        public static bool isInternetAvailable(Context context)
        {
            string TAG = "Network Stat";
            NetworkInfo info = (NetworkInfo)((ConnectivityManager)
            context.GetSystemService(Context.ConnectivityService)).ActiveNetworkInfo;

            if (info == null)
            {
                Console.WriteLine(TAG, "There is no internet connection\n Open Mobile Data and try again!");
                return false;
            }
            else
            {
                if (info.IsConnected)
                {
                    Console.WriteLine(TAG, " internet connection available...");
                    return true;
                }
                else
                {
                    Console.WriteLine(TAG, " internet connection");
                    return true;
                }
            }
        }

/*
        spinner.setOnItemSelectedListener(new OnItemSelectedListener()
        {
            @Override
        public void onItemSelected(AdapterView<?> parentView, View selectedItemView, int position, long id)
        {
            int id = sIds.get(position);//This will be the student id.
        }

        @Override
        public void onNothingSelected(AdapterView<?> parentView)
        {
            // your code here
        }

    });
    */
    }

}

