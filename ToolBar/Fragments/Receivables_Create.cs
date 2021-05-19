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
using FragmentManager_ = Android.App.FragmentManager;
using ToolBar.Models;
using Android.Icu.Util;

namespace ToolBar.Resources.Fragments
{
    public class Receivables_Create : SupportFragment, DatePickerDialog.IOnDateSetListener
    {

        private EditText qtyEditText;
        TextView expireDateEditText;
        private Spinner productSpinner;
        private Button submitBtn, ReceivablesListBtn;
        public static string[] products;

        DateTime selected_Date;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Receivables_Create, container, false);
            productSpinner = view.FindViewById<Spinner>(Resource.Id.productSpinner);
            productSpinner.SetSelection(0);
            qtyEditText = view.FindViewById<EditText>(Resource.Id.qtyEditText);
            
            submitBtn = view.FindViewById<Button>(Resource.Id.submitBtn);
            ReceivablesListBtn = view.FindViewById<Button>(Resource.Id.ReceivablesListBtn);
            List<Product> productsList = MainActivity.db.Table<Product>()./*OrderByDescending(p => p.Id).*/ToList();
            products = new string[productsList.Count];
            for (var i = 0; i < productsList.Count; i++)
                products[i] = productsList[i].name;

            
            var productAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, products);
            productSpinner.Adapter = productAdapter;

            submitBtn.Click += SubmitBtn_Click;
            ReceivablesListBtn.Click += ReceivablesListBtn_Click;
            
            return view;
        }

        private void ImageBtn_Click(object sender, EventArgs e)
        {
            MainActivity.imageViewAction.Invoke((ImageView)sender);
        }

        private void ReceivablesListBtn_Click(object sender, EventArgs e)
        {

            MainActivity.mainActivity.replaceFragment(new Receivables_Index());
        }

        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.closeKeyboard();

            User loggedUser = MainActivity.loggedUser;
            if (loggedUser == null) {
            Toast.MakeText(this.Context, "Unauthenticated access detected!", ToastLength.Short).Show();
            return;
              }
           
            string productStr = products.Length > 0 ? productSpinner.SelectedItem.ToString():"";
            if (productStr == "" || qtyEditText.Text == "")
            {
                Toast.MakeText(this.Context, "Please fill out the required fields!", ToastLength.Short).Show();
                return;
            }
            Int64 productId = productStr != ""? MainActivity.db.Table<Product>().First(c=>c.name== productStr).Id: 0;
            Int64 qty = Int64.Parse(qtyEditText.Text);

            Receivables Receivable = new Receivables(0, productId, qty, DateTime.Now);
            if (Receivables.store(Receivable))
            {
                productSpinner.SetSelection(0);
                qtyEditText.Text = "";
                Toast.MakeText(this.Context, "Receivable added successfully", ToastLength.Short).Show();
            }
            else
            {
                //Toast.MakeText(this.Context, "Something went wrong!", ToastLength.Short).Show();
            }
        }
        
        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                           this,
                                                           currently.Year,
                                                           currently.Month - 1,
                                                           currently.Day);
            dialog.Show();
            /*
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time) {
                expireDateEditText.Text = time.ToLongDateString();
            });
            */
            //frag.Show((FragmentManager_)null, DatePickerFragment.TAG);
        }
        async void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            string default_date = e.Date.Year + "/" + e.Date.Month + "/" + e.Date.Day;
            expireDateEditText.Text = e.Date.Day + "/" + e.Date.Month + "/" + e.Date.Year;
            selected_Date = e.Date;
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            string default_date = year + "/" + month + "/" + dayOfMonth;
            expireDateEditText.Text = dayOfMonth.ToString() + "/" + month.ToString() + "/" + year.ToString();
            selected_Date = view.DateTime;
        }
    }
}
 