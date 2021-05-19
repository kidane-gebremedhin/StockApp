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
    public class Product_Edit : SupportFragment
    {
        MainActivity mainActivity = new MainActivity();
        private EditText nameEditText;
        private Button updateBtn;
        private Int64 mProductId;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Product_Edit, container, false);
            nameEditText = view.FindViewById<EditText>(Resource.Id.nameEditText);
            updateBtn = view.FindViewById<Button>(Resource.Id.updateBtn);

            updateBtn.Click += UpdateBtn_Click;

            Product Product = Product_Index.Product;
            nameEditText.Text = Product.name;

            return view;
        }


        public void Edit_Product(Int64 ProductId)
        {
            this.mProductId = ProductId;

        }


        public void UpdateBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.closeKeyboard();

            string name = nameEditText.Text;
            if (name == "")
            {
                Toast.MakeText(this.Context, "Please fill out the required fields!", ToastLength.Short).Show();
                return;
            }

            Product Product = Product_Index.Product; //MainActivity.db.Table<Product>().First(u => u.Id == mProductId);
            if (Product == null)
                return;
            Product.name = name;
            if (Product.update(Product))
            {
                MainActivity.mainActivity.replaceFragment(MainActivity.Product_Index);
                Toast.MakeText(this.Context, "Product Updated.", ToastLength.Short).Show();
            }
            else
            {
                //Toast.MakeText(this.Context, "Something went wrong!", ToastLength.Short).Show();
            }

        }

    }

}