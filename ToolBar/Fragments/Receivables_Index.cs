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
    public class Receivables_Index : SupportFragment
    {
        private ListView ReceivablesListView;
        private Button addBtn, showBtn, editBtn, showDeleteBtn, cancelBtn, deleteBtn;
        private long mReceivableId;
        private float screenHeight = 400;
        private LinearLayout actionOptions_PupUp, deleteConfirm_PupUp;
        public static List<Receivables> receivables, ReceivablesList;
        public static Receivables Receivable;
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
            View view = inflater.Inflate(Resource.Layout.Receivables_Index, container, false);
            ReceivablesListView = view.FindViewById<ListView>(Resource.Id.ReceivablesListView);
            showBtn = view.FindViewById<Button>(Resource.Id.showBtn);
            cancelBtn = view.FindViewById<Button>(Resource.Id.cancelBtn);
            deleteBtn = view.FindViewById<Button>(Resource.Id.deleteBtn);
            addBtn = view.FindViewById<Button>(Resource.Id.addBtn);
            editBtn = view.FindViewById<Button>(Resource.Id.editBtn);
            showDeleteBtn = view.FindViewById<Button>(Resource.Id.showDeleteBtn);
            actionOptions_PupUp = view.FindViewById<LinearLayout>(Resource.Id.actionOptions_PupUp);
            deleteConfirm_PupUp = view.FindViewById<LinearLayout>(Resource.Id.deleteConfirm_PupUp);
            searchToolsArea = view.FindViewById<LinearLayout>(Resource.Id.searchToolsArea);

            ReceivablesListView.ItemClick += ReceivablesListView_ItemClick;

            ReceivablesListView.ItemLongClick += ReceivablesListView_ItemLongClick;
            showBtn.Click += ShowBtn_Click;
            addBtn.Click += AddBtn_Click;
            editBtn.Click += EditBtn_Click;
            showDeleteBtn.Click += ShowDeleteBtn_Click;
            deleteBtn.Click += DeleteBtn_Click;
            cancelBtn.Click += CancelBtn_Click;

            List<Product> productList = MainActivity.db.Table<Product>()/*.OrderByDescending(p => p.Id)*/.ToList();
            // /*
            List<Product> productsList = MainActivity.db.Table<Product>()/*.OrderByDescending(p => p.Id)*/.ToList();
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
            //Retrive Receivables
            receivables = db.Table<Receivables>().OrderByDescending(p => p.Id).ToList();

            ReceivablesList=new List<Receivables>();
            /*
            var query = db.Table<Product>();
            Product product = query != null && query.Count() > 0 ? query.First() : null;

            foreach (var i in receivables)
            {
                // 
                var queryy = MainActivity.db.Table<Product>().Where(c => c.name == selectedProduct);
                Product prod = queryy.Count() > 0 ? queryy.First() : null;
                if (prod != null && i.productId != prod.Id)
                    continue;
                //
                

                Receivables Receivable = new Receivables(i.Id, i.productId, i.qty);
                ReceivablesList.Add(Receivable);
            }
            Receivables_list_adapter adapter = new Receivables_list_adapter(this.Context, ReceivablesList);
            ReceivablesListView.Adapter = adapter;
*/
            UpdateList();
            products = new string[productList.Count];
            for (var i = 0; i < productList.Count; i++)
                products[i] = productList[i].name;
          /*   var ProductAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownReceivable, products);
           productSpinner.Adapter = ProductAdapter;
            productSpinner.SetSelection(ProductAdapter.GetPosition(selectedProduct));
            */
            return view;
        }

        private void BidsBtn_Click(object sender, EventArgs e)
        {
            //MainActivity.mainActivity.replaceFragment(new Receivable_Bid_Selection_Index());
        }

        private void ShowBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            MainActivity.mainActivity.replaceFragment(new Receivables_Show());
        }
        private void ShowDeleteBtn_Click(object sender, EventArgs e)
        {
            deleteConfirm_PupUp.TranslationY = screenHeight - 350;// screenHeight-150;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.replaceFragment(new Receivables_Create());
        }
        private void EditBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.replaceFragment(new Receivables_Edit());
            //MainActivity.Receivables_Edit.Edit_Receivable(mReceivableId);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            Toast.MakeText(this.Context, "  Receivable Delete Canceled", ToastLength.Short).Show();
        }

        
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            Receivables.delete(mReceivableId);
            
            MainActivity.mainActivity.replaceFragment(new Receivables_Index());
            Toast.MakeText(this.Context, " Receivable deleted successfully", ToastLength.Short).Show();
        }

        private void ReceivablesListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            MainActivity.mainActivity.closeKeyboard();

            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;//250;// screenHeight;
            Console.WriteLine("" + e.Id.ToString());
        }

        private void ReceivablesListView_ItemLongClick(object sender, ItemLongClickEventArgs e){
            mReceivableId = ReceivablesList[e.Position].Id;
            Receivable = ReceivablesList[e.Position];
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
            MainActivity.mainActivity.replaceFragment(new Receivables_Index());
        }

        private void ProductSpinner_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            if (!receivableProductHasBeenSelected)
            {
                receivableProductHasBeenSelected = true;
                return;
            }
            
            selectedProduct = products[e.Position];

            if (selectedProduct == ref_selectedProduct)
                return;
            ref_selectedProduct = selectedProduct;

            UpdateList();

//            MainActivity.mainActivity.replaceFragment(new Receivables_Index());

        }

        private void UpdateList()
        {
            var query = MainActivity.db.Table<Product>();
            Product product = query != null && query.Count() > 0 ? query.First() : null;
            ReceivablesList.Clear();
            foreach (var i in receivables)
            {
                // /*
                var queryy = MainActivity.db.Table<Product>().Where(c => c.name == selectedProduct);
                Product prod = queryy.Count() > 0 ? queryy.First() : null;
                if (prod != null && i.productId != prod.Id)
                    continue;
                //*/
                /*if (keyString != "" && !i.name.Contains(keyString))
                    continue;*/

                Receivables Receivable = new Receivables(i.Id, i.productId, i.qty, i.date);
                ReceivablesList.Add(Receivable);
            }
            Receivables_list_adapter adapter = new Receivables_list_adapter(this.Context, ReceivablesList);
            ReceivablesListView.Adapter = adapter;

        }

    }
}