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
    public class Issuables_Index : SupportFragment
    {
        private ListView IssuablesListView;
        private Button addBtn, showBtn, editBtn, showDeleteBtn, cancelBtn, deleteBtn;
        private long mIssuableId;
        private float screenHeight = 400;
        private LinearLayout actionOptions_PupUp, deleteConfirm_PupUp;
        public static List<Issuables> issuables, IssuablesList;
        public static Issuables Issuable;
        public static string[] products;
        private Spinner productSpinner;
        private LinearLayout searchToolsArea;
        private string selectedProduct = "", ref_selectedProduct = "", keyString = "", ref_keyStr = "";
        private EditText searchEditText;
        public bool receivableProductHasBeenSelected = false, issuableProductHasBeenSelected = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Issuables_Index, container, false);
            IssuablesListView = view.FindViewById<ListView>(Resource.Id.IssuablesListView);
            showBtn = view.FindViewById<Button>(Resource.Id.showBtn);
            cancelBtn = view.FindViewById<Button>(Resource.Id.cancelBtn);
            deleteBtn = view.FindViewById<Button>(Resource.Id.deleteBtn);
            addBtn = view.FindViewById<Button>(Resource.Id.addBtn);
            editBtn = view.FindViewById<Button>(Resource.Id.editBtn);
            showDeleteBtn = view.FindViewById<Button>(Resource.Id.showDeleteBtn);
            actionOptions_PupUp = view.FindViewById<LinearLayout>(Resource.Id.actionOptions_PupUp);
            deleteConfirm_PupUp = view.FindViewById<LinearLayout>(Resource.Id.deleteConfirm_PupUp);
            searchToolsArea = view.FindViewById<LinearLayout>(Resource.Id.searchToolsArea);

            IssuablesListView.ItemClick += IssuablesListView_ItemClick;

            IssuablesListView.ItemLongClick += IssuablesListView_ItemLongClick;
            showBtn.Click += ShowBtn_Click;
            addBtn.Click += AddBtn_Click;
            editBtn.Click += EditBtn_Click;
            showDeleteBtn.Click += ShowDeleteBtn_Click;
            deleteBtn.Click += DeleteBtn_Click;
            cancelBtn.Click += CancelBtn_Click;

            List<Product> productList = MainActivity.db.Table<Product>()./*OrderByDescending(p => p.Id).*/ToList();
            // /*
            List<Product> productsList = MainActivity.db.Table<Product>()./*OrderByDescending(p => p.Id).*/ToList();
            productSpinner = view.FindViewById<Spinner>(Resource.Id.productSpinner);
            products = new string[productsList.Count];
            for (var i = 0; i < productsList.Count; i++)
                products[i] = productsList[i].name;
            var ProductAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, products);
            productSpinner.Adapter = ProductAdapter;
            productSpinner.ItemSelected += ProductSpinner_ItemSelected;
            if (selectedProduct != "")
                productSpinner.SetSelection(ProductAdapter.GetPosition(selectedProduct));
            else
                productSpinner.SetSelection(0);

            /*searchEditText = view.FindViewById<EditText>(Resource.Id.searchEditText);
            searchEditText.TextChanged += SearchEditText_TextChanged1;
            searchEditText.Text = keyString;

            searchEditText.RequestFocus();*/
            //*/
            if (MainActivity.loggedUser == null)
            {
                editBtn.Visibility = ViewStates.Invisible;
                showDeleteBtn.Visibility = ViewStates.Invisible;
                searchToolsArea.Visibility = ViewStates.Visible;
            }

            var db = new SQLiteConnection(MainActivity.dbPath);
            //Retrive Issuables
            issuables = db.Table<Issuables>().OrderByDescending(p => p.Id).ToList();

            IssuablesList=new List<Issuables>();
            /*
            var query = db.Table<Product>();
            Product product = query != null && query.Count() > 0 ? query.First() : null;

            foreach (var i in issuables)
            {
                // 
                var queryy = MainActivity.db.Table<Product>().Where(c => c.name == selectedProduct);
                Product prod = queryy.Count() > 0 ? queryy.First() : null;
                if (prod != null && i.productId != prod.Id)
                    continue;
                //
                Issuables Issuable = new Issuables(i.Id, i.productId, i.qty);
                IssuablesList.Add(Issuable);
            }
            Issuables_list_adapter adapter = new Issuables_list_adapter(this.Context, IssuablesList);
            IssuablesListView.Adapter = adapter;
            */
            UpdateList();

            products = new string[productList.Count];
            for (var i = 0; i < productList.Count; i++)
                products[i] = productList[i].name;
          /*   var ProductAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownIssuable, products);
           productSpinner.Adapter = ProductAdapter;
            productSpinner.SetSelection(ProductAdapter.GetPosition(selectedProduct));
            */
            return view;
        }

        
        private void BidsBtn_Click(object sender, EventArgs e)
        {
            //MainActivity.mainActivity.replaceFragment(new Issuable_Bid_Selection_Index());
        }

        private void ShowBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            MainActivity.mainActivity.replaceFragment(new Issuables_Show());
        }
        private void ShowDeleteBtn_Click(object sender, EventArgs e)
        {
            deleteConfirm_PupUp.TranslationY = screenHeight - 350;// screenHeight-150;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.replaceFragment(new Issuables_Create());
        }
        private void EditBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.replaceFragment(new Issuables_Edit());
            //MainActivity.Issuables_Edit.Edit_Issuable(mIssuableId);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            Toast.MakeText(this.Context, "  Issuable Delete Canceled", ToastLength.Short).Show();
        }

        
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            Issuables.delete(mIssuableId);
            
            MainActivity.mainActivity.replaceFragment(new Issuables_Index());
            Toast.MakeText(this.Context, " Issuable deleted successfully", ToastLength.Short).Show();
        }

        private void IssuablesListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            MainActivity.mainActivity.closeKeyboard();

            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;//250;// screenHeight;
            Console.WriteLine("" + e.Id.ToString());
        }

        private void IssuablesListView_ItemLongClick(object sender, ItemLongClickEventArgs e){
            mIssuableId = IssuablesList[e.Position].Id;
            Issuable = IssuablesList[e.Position];
            actionOptions_PupUp.TranslationY = screenHeight - 350;// screenHeight-150;
    }

        private void SearchEditText_TextChanged1(object sender, Android.Text.TextChangedEventArgs e)
        {
            // return;
            keyString = searchEditText.Text;
            if (keyString == ref_keyStr)
                return;
            ref_keyStr = keyString;
            //Toast.MakeText(this.Context, searchEditText.Text, ToastLength.Short).Show();
            MainActivity.mainActivity.replaceFragment(new Issuables_Index());
        }

        private void ProductSpinner_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            if (!issuableProductHasBeenSelected)
            {
                issuableProductHasBeenSelected = true;
                return;
            }

            selectedProduct = products[e.Position];
            if (selectedProduct == ref_selectedProduct)
                return;
            ref_selectedProduct = selectedProduct;
            UpdateList();
            //MainActivity.mainActivity.replaceFragment(new Issuables_Index());

        }

        private void UpdateList()
        {
            var query = MainActivity.db.Table<Product>();
            Product product = query != null && query.Count() > 0 ? query.First() : null;
            IssuablesList.Clear();
            foreach (var i in issuables)
            {
                // /*
                var queryy = MainActivity.db.Table<Product>().Where(c => c.name == selectedProduct);
                Product prod = queryy.Count() > 0 ? queryy.First() : null;
                if (prod != null && i.productId != prod.Id)
                    continue;
                //*/
                /*if (keyString != "" && !i.name.Contains(keyString))
                    continue;*/

                Issuables Issuable = new Issuables(i.Id, i.productId, i.qty, i.date);
                IssuablesList.Add(Issuable);
            }
            Issuables_list_adapter adapter = new Issuables_list_adapter(this.Context, IssuablesList);
            IssuablesListView.Adapter = adapter;

        }
    }
}