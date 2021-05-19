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
using System.IO;
using SQLite;
using Android;
using Android.Runtime;
using Android.Net;
using Android.Content;
using Android.Util;
using ToolBar.Fragments;
using ToolBar.Models;
using Android.Graphics;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Android.Views.InputMethods;
using System.Threading.Tasks;
using Android.Support.V4.App;
using Android.Content.PM;

namespace ToolBar
{
    [Activity(MainLauncher = true, Theme = "@style/MyTheme", ConfigurationChanges =Android.Content.PM.ConfigChanges.ScreenSize|Android.Content.PM.ConfigChanges.Orientation)]
    public class MainActivity : ActionBarActivity
    {
        public static MainActivity mainActivity;
        public static User loggedUser;
        public static bool IS_CONNECTED=false;
        private SupportToolbar toolbar, footer_toolbar;
        private MySupportActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        public static ListView leftListView, rightListView;
        private LinearLayout mLeftDrawer, mRightDrawer;
        public static LeftDrawer_ListAdapter mLeftAdapter;
        public static RightDrawer_ListAdapter mRightAdapter;
        public static List<string> mLeftDataset, mRightDataSet;
        private Button developersBtn;

        public static SupportFragment currentFragment, homeFragment, program_Index, program_Create, fragment3, developers, webViewFragment;
        public static SupportFragment users_Show, users_Index, users_Create;
        public static SupportFragment Product_Show, Product_Index, Product_Create;
        public static SupportFragment Receivables_Show, Receivables_Index, archive_Receivables_Index, Receivables_Create;
        public static SupportFragment Issuables_Show, Issuables_Index, Issuables_Create;
        public static SupportFragment bids_Show, bids_Index, archive_bids_Index, bids_Create;
        public static SupportFragment about_Fragment, contact_Fragment, logIn, signUp;
        
        public static Users_Edit users_Edit;
        public static Product_Edit Product_Edit;
        public static Receivables_Edit Receivables_Edit;
        public static Issuables_Edit Issuables_Edit;
        private Stack<SupportFragment> backFragmentStack;
        public static SupportFragment[] fragmentsList;
        //sqlite database insertion
        public static string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "TenderApp4.db3");
        public static SQLiteConnection db = new SQLiteConnection(dbPath);
        public static Action<ImageView> imageViewAction;
        public static ImageView mSelectedPic;
        public static string imageByteString = "";
        public static bool appCloseChoice = false;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            mainActivity = this;
            //AskPermissions

            //TryToGetPermissions();
            
            //Check for Connection
            //checkNetworkConnection();

            //initialize db
           // var db = new SQLiteConnection(dbPath);
            //setup a table
            db.CreateTable<User>();
            db.CreateTable<Product>();
            db.CreateTable<Receivables>();
            db.CreateTable<Issuables>();
            //Insert Initial data
            if(db.Table<User>().Where(u=>u.role=="Admin").Count()==0){
                /*
                 /for (var i = 0; i < 4; i++)
                 {
                     User seller = new User("Seller", "seller 1", "a", "firstName", "lastName", "0919054098", "adming@gmail.com", "Mekelle", "PILASA", "11111111", "11111111");
                     db.Insert(seller);
                 }
                  for (var i = 0; i < 4; i++)
                 {
                     User buyer = new User("Buyer", "Buyer 1", "a", "firstName", "lastName", "0919054098", "adming@gmail.com", "Mekelle", "PILASA", "11111111", "11111111");
                     db.Insert(buyer);
                 }
                  */
                //for (var i = 0; i < 4; i++)
                //{
                User admin = new User(0, "Admin", "gg", "gg123", "Gebretsadik", "Gebrehiwot", "+251914761171", "admin@gmail.com", "Mekelle");
                 db.Insert(admin);
                //}

                List<Product> RegisteredProducts = db.Table<Product>().ToList();
                if (RegisteredProducts.Count == 0)
                {
                    Product[] productsArr = new Product[] {
                    new Product(0, "R 10x10x0.3", 0),
                    new Product(0, "R 12x12x0.3", 0),
                    new Product(0, "R 10x20x0.3", 0),
                    new Product(0, "R 10x20x0.5", 0),
                    new Product(0, "R 15x15x0.5", 0),
                    new Product(0, "R 20x20x0.5", 0),
                    new Product(0, "R 20x20x0.6", 0),
                    new Product(0, "R 20x20x0.8", 0),
                    new Product(0, "R 20x20x0.9", 0),
                    new Product(0, "R 20x20x1.2", 0),
                    new Product(0, "R 25x25x0.5", 0),
                    new Product(0, "R 25x25x0.7", 0),
                    new Product(0, "R 25x25x0.8", 0),
                    new Product(0, "R 25x25x1.2", 0),
                    new Product(0, "R 20x30x0.5", 0),
                    new Product(0, "R 20x30x2", 0),
                    new Product(0, "R 30x30x0.6", 0),
                    new Product(0, "R 30x30x0.8", 0),
                    new Product(0, "R 30x30x1.2", 0),
                    new Product(0, "R 30x30x1.5", 0),
                    new Product(0, "R 20x40x0.5", 0),
                    new Product(0, "R 20x40x0.6", 0),
                    new Product(0, "R 20x40x1.2", 0),
                    new Product(0, "R 20x40x1.5", 0),
                    new Product(0, "R 40x40x0.6", 0),
                    new Product(0, "R 40x40x0.8", 0),
                    new Product(0, "R 40x40x1.2", 0),
                    new Product(0, "R 40x40x1.5", 0),
                    new Product(0, "R 40x40x2.5", 0),
                    new Product(0, "R 60x60x3", 0),
                    new Product(0, "R 60x40x1.2", 0),
                    new Product(0, "R 60x40x1.5", 0),
                    new Product(0, "R 60x40x2.5", 0),
                    new Product(0, "R 80x80x3", 0),
                    new Product(0, "L 28x0.6", 0),
                    new Product(0, "T 28x0.6", 0),
                    new Product(0, "L 28x0.8", 0),
                    new Product(0, "T 28x0.8", 0),
                    new Product(0, "Z 28x0.8", 0),
                    new Product(0, "L 28x1.2/0.9", 0),
                    new Product(0, "L 38x0.6", 0),
                    new Product(0, "T 38x0.6", 0),
                    new Product(0, "Z 38x0.6", 0),
                    new Product(0, "L 38x1.2", 0),
                    new Product(0, "T 38x1.2", 0),
                    new Product(0, "Z 38x1.2", 0),
                    new Product(0, "D 20x30", 0),
                    new Product(0, "An 25", 0),
                    new Product(0, "An 20", 0),
                    new Product(0, "An 30x2", 0),
                    new Product(0, "An 30x3", 0),
                    new Product(0, "An 40x2", 0),
                    new Product(0, "An 40x3", 0),
                    new Product(0, "An 50x3", 0),
                    new Product(0, "An 50x4", 0),
                    new Product(0, "An 60x5", 0),
                    new Product(0, "Tr 25", 0),
                    new Product(0, "Tr 32", 0),
                    new Product(0, "Rt 16x0.8", 0),
                    new Product(0, "Rt 22x0.6", 0),
                    new Product(0, "Rt 22x0.8", 0),
                    new Product(0, "Rt 22x1.2", 0),
                    new Product(0, "Rt 25x0.6", 0),
                    new Product(0, "Rt 25x1.2", 0),
                    new Product(0, "Rt 32x0.6", 0),
                    new Product(0, "Rt 32x1.2", 0),
                    new Product(0, "Rt 38x", 0),
                    new Product(0, "Rt 40x2", 0),
                    new Product(0, "Rt 50x1.2", 0),
                    new Product(0, "Rt 50x1.5", 0),
                    new Product(0, "Rt 50x3", 0),
                    new Product(0, "WWT", 0),
                    new Product(0, "WT 3/4", 0),
                    new Product(0, "F 10", 0),
                    new Product(0, "F 12x2", 0),
                    new Product(0, "F 15x2", 0),
                    new Product(0, "F 15x3", 0),
                    new Product(0, "F 20x2", 0),
                    new Product(0, "F 20x3", 0),
                    new Product(0, "F 30x3", 0),
                    new Product(0, "F 40x2", 0),
                    new Product(0, "F 40x3", 0),
                    new Product(0, "F 40x4", 0),
                    new Product(0, "F 50x5", 0),
                    new Product(0, "LTZ 38China", 0),
                    new Product(0, "LTZ 48China", 0),
                };
                    for (var i = 0; i < productsArr.Length; i++)
                    {
                        Product.store(productsArr[i]);
                    }
                }
             }

            // */

            /*
           var query = db.Table<Receivable>().Where(program => program.Id > 0);

           if (query != null)
           {
               foreach (var prog in query.ToList())
               {
                   Console.WriteLine("deleted: " + prog.name + " - " + prog.Id.ToString());
                   db.Delete(prog);
                   db.Delete<Receivable>(prog.Id);
               }
           }
           */
            /*
            var query = db.Table<Receivables>().Where(i => i.isActive==true); 
             if (query != null)
             {
                 foreach (var Receivable in query.ToList())
                 {
                     if (Receivables.isActive == false)
                         continue;
                     var expireDate = Receivable.expireDate;
                     if (Date_class.first_isAfter(expireDate, Date_class.getToday()))
                         continue;
                     var bids = db.Table<Bid>().Where(b => b.ReceivableId == Receivable.Id).OrderBy(b_p=>b_p.price);
                     if(bids==null || bids.Count()==0)
                         continue;
                     Bid topBid = bids.First();
                     Receivable.winnerId = topBid.buyerId;
                     Receivable.isActive = false;

                     db.Update(Receivable);
             Console.WriteLine("Receivable Bid Closed and Winner Selected\nReceivable Name: " + Receivable.name + "  Price: " + Receivable.price);
                 }
             }
             //*/
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

            imageViewAction = SelectPic;
            //accountTitle = FindViewById<Menu>(Resource.Id.accountTitle);
            mLeftDrawer.Tag = 0;
            mRightDrawer.Tag = 1;
            SetSupportActionBar(toolbar);
            footer_toolbar.InflateMenu(Resource.Menu.footer_menu);
            homeFragment = new Home_fragment();

            ///*
            developers = new Developers();
            //*/

            //Users
            users_Index = new Users_Index();
            users_Create = new Users_Create();
            users_Edit = new Users_Edit();
            users_Show = new Users_Show();

            //Products
            Product_Index = new Product_Index();
            Product_Create = new Product_Create();
            Product_Edit = new Product_Edit();
            Product_Show = new Product_Show();

            //Receivables
            Receivables_Index = new Receivables_Index();
            Receivables_Create = new Receivables_Create();
            Receivables_Edit = new Receivables_Edit();
            Receivables_Show = new Receivables_Show();

            //Issuables
            Issuables_Index = new Issuables_Index();
            Issuables_Create = new Issuables_Create();
            Issuables_Edit = new Issuables_Edit();
            Issuables_Show = new Issuables_Show();

            //Bids
            /*
            bids_Index = new Bids_Index();
            bids_Create = new Bids_Create();
            bids_Edit = new Bids_Edit();
            bids_Show = new Bids_Show();
            */

            //Externals
            logIn = new LogIn();
            signUp = new Users_Create();

            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.frameLayout, homeFragment, "homeFragment");
            trans.Commit();
            currentFragment = null;


            mLeftDataset = new List<string>();
           
                //mLeftDataset.Add("Home");
                //mLeftDataset.Add("All Active Receivables");
                //mLeftDataset.Add("All Archived Receivables");
                //mLeftDataset.Add("About Developer");
                mLeftDataset.Add("Sign In");
                //mLeftDataset.Add("Sign Up");
                //mLeftDataset.Add("Share");
                fragmentsList = new SupportFragment[] { /*homeFragment, bids_Create, archive_Receivables_Index, developers, */logIn, /*signUp*/ };
            LeftDrawer_ListAdapter.leftDrawableIconIds = new int[] { Resource.Drawable.about, Resource.Drawable.contact };
           RightDrawer_ListAdapter.rightDrawableIconIds = new int[] { Resource.Drawable.facebook, Resource.Drawable.youtube, Resource.Drawable.linkedin };

            mLeftAdapter = new LeftDrawer_ListAdapter(this, mLeftDataset);
            leftListView.Adapter = mLeftAdapter;
            leftListView.ItemClick += MLeftDrawer_ItemClick;
            rightListView.ItemClick += RightListView_ItemClick;
            mRightDataSet = new List<string>();
            mRightDataSet.Add("Facebook");
            mRightDataSet.Add("YouTube");
            mRightDataSet.Add("LinkedIn");
            mRightAdapter = new RightDrawer_ListAdapter(this, mRightDataSet);
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


            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (!(CheckPermissionGranted(Manifest.Permission.ReadExternalStorage) && CheckPermissionGranted(Manifest.Permission.WriteExternalStorage)))
                {
                    RequestPermission();
                }
            }
        }

        private void RightListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            openUrlInBrowser(mRightAdapter[e.Position]);
        }

        private void MLeftDrawer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            replaceFragment(fragmentsList[e.Position]);
            
            mDrawerLayout.CloseDrawer(mLeftDrawer);
            mDrawerLayout.CloseDrawer(mRightDrawer);
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

        public override bool OnOptionsItemSelected(IMenuItem Item)
        {
            //closes drawer after any action
            mDrawerLayout.CloseDrawer(mLeftDrawer);
            mDrawerLayout.CloseDrawer(mRightDrawer);

            switch (Item.ItemId) {
                case Android.Resource.Id.Home:
                    mDrawerToggle.OnOptionsItemSelected(Item);
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
            
                case Resource.Id.page3:
                    mDrawerLayout.CloseDrawer(mLeftDrawer);
                    mDrawerLayout.CloseDrawer(mRightDrawer);
                    Intent sendIntent = new Intent();
                    sendIntent.SetAction(Intent.ActionSend);
                    sendIntent.PutExtra(Intent.ExtraText, "Share To");
                    sendIntent.SetType("text/plain");
                    Intent.CreateChooser(sendIntent, "Share via");
                    StartActivity(sendIntent);  
                    //replaceFragment(fragment3);
                    return true;
                case Resource.Id.logOutOption:
                    //if (MainActivity.loggedUser == null)
                    //    return true;
                    MainActivity.loggedUser = null;
                    //reset sensitive entities
                    MainActivity.users_Create = new Users_Create();
                    MainActivity.users_Edit = new Users_Edit();
                    /*
                    SupportFragmentManager.Fragments.Clear();
                     */
                    LogIn.initDrawer();
                    replaceFragment(new LogIn());
                    
                    return true;
                default:
                    return base.OnOptionsItemSelected(Item);

            }
        }
        
        public void openUrlInBrowser(string domain)
        {
            var uri = Android.Net.Uri.Parse("http://www."+ domain + ".com");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
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
            trans.SetCustomAnimations(Resource.Animation.slide_in, Resource.Animation.slide_out, Resource.Animation.slide_in, Resource.Animation.slide_out);
            trans.AddToBackStack(null);
            trans.Commit();
            
            backFragmentStack.Push(currentFragment);
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

        public /*override*/ void _OnBackPressed()
        {
            //base.OnBackPressed();
            ///*
            if (backFragmentStack.Count > 0)
            {
                SupportFragmentManager.PopBackStack();
                currentFragment = backFragmentStack.Pop();
                //replaceFragment(currentFragment);
            }
            else
            {
                base.OnBackPressed();
            }
            //*/
           // base.OnBackPressed();
        }
        
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            /*
            Toast.MakeText(this, "Back clicked", ToastLength.Short).Show();
            if (e.KeyCode == Keycode.Back)
            {
                if(new Web_viewFragment().webView!=null)
                    new Web_viewFragment().webView.GoBack();
            }*/
            if (e.KeyCode == Keycode.Back)
            {

                if (MainActivity.loggedUser == null)
                {
                    MainActivity.mainActivity.replaceFragment(new LogIn());
                    return true;
                }

                if (backFragmentStack.Count == 0)
                    {
                        string message = "Are you Sure to close this Application? ";
                        showAlertDialog(message);
                        if (!appCloseChoice)
                            return true;
                        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                        Application.Dispose();

                    }
                    /*MainActivity.mainActivity.replaceFragment(new Home_fragment());
                    backFragmentStack.Clear();
                    return true;*/
                if (backFragmentStack.Count > 0)
                {
                    SupportFragmentManager.PopBackStack();
                    currentFragment = backFragmentStack.Pop();
                    //replaceFragment(currentFragment);
                }
                else 
                {
                    //base.OnBackPressed();
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    Application.Dispose();

                }
            }
            return true;
        }

        public void showAlertDialog(string message)
        {
            //set alert for executing the task
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Message");
            alert.SetMessage(message);
            alert.SetPositiveButton("OK", (senderAlert, args) =>
            {
                //closeApp();
            });
            /*
            alert.SetNegativeButton("No", (senderAlert, args) =>
            {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                
            });
            */

            Dialog dialog = alert.Create();
            dialog.Show();

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

         string[] PermissionsGroupLocation =
            {
                            //TODO add more permissions
                            Manifest.Permission.AccessCoarseLocation,
                            Manifest.Permission.AccessFineLocation,
                            Manifest.Permission.WriteExternalStorage,
                            Manifest.Permission.ReadExternalStorage,
             };
        async void GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                //TODO change the message to show the permissions name
                //Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();
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
        public void closeApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            Application.Dispose();
        }
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults.Length>0 && grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {
                            //Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();

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

    public void SelectPic(ImageView selectedPic)
        {
            mSelectedPic = selectedPic;
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            this.StartActivityForResult(Intent.CreateChooser(intent, "Select Receivable image"), 0);
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                Stream stream = ContentResolver.OpenInputStream(data.Data);
                mSelectedPic.SetImageBitmap(null);
                mSelectedPic.SetImageBitmap(DecodeBitmapFromStream(data.Data, 100, 100));
                GC.Collect();
                ///*
                MemoryStream memoryStream = new MemoryStream();
                BitmapFactory.DecodeStream(stream).Compress(Bitmap.CompressFormat.Jpeg, 50, memoryStream);
                //bitmap.Compress(Bitmap.CompressFormat.Webp, 100, memoryStream);
               
                byte[] picByteData = memoryStream.ToArray();
                imageByteString = Convert.ToBase64String(picByteData);
                GC.Collect();

                /*
               System.Net.WebClient webClient = new System.Net.WebClient();
               System.Uri uri = new System.Uri("http://127.0.0.1/insertImage.php");

               NameValueCollection parameters = new NameValueCollection();
               parameters.Add("Image", Convert.ToBase64String(picByteData));

               webClient.UploadValuesAsync(uri, parameters);
               webClient.UploadValuesCompleted += WebClient_UploadValuesCompleted;
               */
            }
        }

        private void WebClient_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            RunOnUiThread(()=>{
                Console.WriteLine(Encoding.UTF8.GetString(e.Result));
            });
        }

        private Bitmap DecodeBitmapFromStream(Android.Net.Uri data, int requestedWidth, int requestedHeight)
        {
            Stream stream = ContentResolver.OpenInputStream(data);
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeStream(stream);

            options.InSampleSize = CalculateInSampleSize(options, requestedHeight, requestedWidth);

            stream = ContentResolver.OpenInputStream(data);
            options.InJustDecodeBounds = false;
            Bitmap bitMap = BitmapFactory.DecodeStream(stream, null, options);

            return bitMap;
        }
        private int CalculateInSampleSize(BitmapFactory.Options options, int requestedHeight, int requestedWidth)
        {
            int height = options.OutHeight;
            int width = options.OutWidth;
            int insumpleSize = 1;

            if(height>requestedHeight || width > requestedWidth)
            {
               int halfHeight= height/2;
               int halfWidth=width/2;

                while((halfHeight/insumpleSize)>requestedHeight && (halfWidth / insumpleSize) > requestedWidth)
                {
                    insumpleSize ^= 2;
                }
            }
            return insumpleSize;
        }

        //---------toget Screen Width and Height
        public int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }


        public int[] getScreen_Width_and_Height()
        {
            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            return new int[] { widthInDp, heightInDp };
        }

        /* 
        //Store: Convert the Image bitmap into a Base64String and store it to SQLite.
       public static string Base64Encode(string plainText) {
         var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
         return System.Convert.ToBase64String(plainTextBytes);
       }

      // You can store your bitmap image as a byte[] to SQlite. For this operation you should convert bitmap to byte array. This piece of code, convert bitmap to byte[]
       var memoryStream = new MemoryStream();
       bitmap.Compress(Bitmap.CompressFormat.Png, 0, memoryStream);
       imageByteArray = memoryStream.ToArray();

       //

       //Retrieve: Fetch the Base64String and convert that to Bitmap again.
       public static string Base64Decode(string base64EncodedData) {
         var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
         return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
       }

       imageView.BuildDrawingCache (true);
       Bitmap bitmap = imageView.GetDrawingCache (true);
       BitmapDrawable drawable = (BitmapDrawable)imageView.GetDrawable();
       Bitmap bitmap = drawable.GetBitmap();
        */
        
        public void closeKeyboard()
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);

            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);

        }


        public override bool OnTouchEvent(MotionEvent e)
        {
            closeKeyboard();
            return true;
        }

        public void clearFragmentBackStack()
        {
            for (int i = 0; i < SupportFragmentManager.BackStackEntryCount - 1; i++)
            {
                SupportFragmentManager.PopBackStack();
            }
        }

        private void RequestPermission()
        {
            ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage,  }, 0);
        }

        public bool CheckPermissionGranted(string Permissions)
        {
            // Check if the permission is already available.
            if (ActivityCompat.CheckSelfPermission(this, Permissions) != Permission.Granted)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }


}

