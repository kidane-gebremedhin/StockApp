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
    public class Product_Create : SupportFragment
    {
        MainActivity mainActivity = new MainActivity();

        private EditText nameEditText;
        private Button submitBtn, ProductListBtn;

        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Product_Create, container, false);
            nameEditText = view.FindViewById<EditText>(Resource.Id.nameEditText);
            submitBtn = view.FindViewById<Button>(Resource.Id.submitBtn);
            ProductListBtn = view.FindViewById<Button>(Resource.Id.ProductListBtn);

            submitBtn.Click += SubmitBtn_Click;
            ProductListBtn.Click += ProductListBtn_Click;
            
            return view;
        }
        
        private void ProductListBtn_Click(object sender, EventArgs e)
        {

            MainActivity.mainActivity.replaceFragment(MainActivity.Product_Index);
        }
        
        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.closeKeyboard();

            string name = nameEditText.Text;
            if (name=="") {
                Toast.MakeText(this.Context, "Please fill out the required fields!", ToastLength.Short).Show();
                return;
               } 
            Product Product = new Product(0, name);
            if (Product.store(Product))
            {
                nameEditText.Text = "";
                Toast.MakeText(this.Context, "Product added successfully", ToastLength.Short).Show();
            }
            else
            {
                //Toast.MakeText(this.Context, "Something went wrong!", ToastLength.Short).Show();
            }
        }
        
    }
    
}