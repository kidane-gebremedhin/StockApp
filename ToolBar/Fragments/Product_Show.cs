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
    public class Product_Show : SupportFragment
    {
        MainActivity mainActivity = new MainActivity();

        private TextView nameTextView;
        private Button ProductsListBtn;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Product_Show, container, false);
            nameTextView = view.FindViewById<TextView>(Resource.Id.nameTextView);
            ProductsListBtn = view.FindViewById<Button>(Resource.Id.ProductsListBtn);

            ProductsListBtn.Click += ProductsListBtn_Click;

            Product Product = Product_Index.Product;

            nameTextView.Text = Product.name;
            
            return view;
        }

        private void ProductsListBtn_Click(object sender, EventArgs e)
        {

            MainActivity.mainActivity.replaceFragment(MainActivity.Product_Index);
        }
        
    }
    
}