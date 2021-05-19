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
    public class Issuables_Edit : SupportFragment, DatePickerDialog.IOnDateSetListener
    {
        private EditText qtyEditText;
        TextView expireDateEditText;
        private Spinner productSpinner;
        private Button updateBtn;
        private Int64 mIssuableId;
        private string[] products;

        private ImageView imageView;
        Button datePickerBtn;
        DateTime selected_Date;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           View view = inflater.Inflate(Resource.Layout.Issuables_Edit, container, false);
            productSpinner = view.FindViewById<Spinner>(Resource.Id.productSpinner);
            qtyEditText = view.FindViewById<EditText>(Resource.Id.qtyEditText);
            
            updateBtn = view.FindViewById<Button>(Resource.Id.updateBtn);

            List<Product> productsList = MainActivity.db.Table<Product>()./*OrderByDescending(p => p.Id).*/ToList();
            products = new string[productsList.Count];
            for (var i = 0; i < productsList.Count; i++)
                products[i] = productsList[i].name;

            var productAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, products);
            productSpinner.Adapter = productAdapter;
            updateBtn.Click += UpdateBtn_Click;

            Issuables Issuable = Issuables_Index.Issuable;
            Product product = MainActivity.db.Table<Product>().First(c => c.Id == Issuable.productId);
            productSpinner.SetSelection(productAdapter.GetPosition(product.name));
            qtyEditText.Text = Issuable.qty.ToString();

            return view;
        }


        public void Edit_Issuable(Int64 IssuableId)
        {
            this.mIssuableId = IssuableId;
            Issuables Issuable=new Issuables();
            var query = MainActivity.db.Table<Issuables>().Where(u => u.Id == mIssuableId);
            if (query != null)
            {
                foreach (var u in query.ToList<Issuables>())
                {
                    Issuable=u;
                    break;
                }
            }

           /*
            daySpinner.SetSelection(days_adapter.GetPosition(program.day));
            qtyEditText.Text=program.name;
            startTime_HrsSpinner.SelectedIssuable = program.startTime_Hr;
            startTime_MinsSpinner.SelectedIssuable = program.startTime_Mins;
            startTime_amPmSpinner.SelectedIssuable = program.startTime_amPm;
            endTime_HrsSpinner.SelectedIssuable = program.endTime_Hr;
            endTime_MinsSpinner.SelectedIssuable = program.endTime_Mins;
            endTime_amPmSpinner.SelectedIssuable = program.endTime_amPm;
            //*/
        }


        public void UpdateBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.closeKeyboard();

            User loggedUser = MainActivity.loggedUser;
            if (loggedUser == null)
            {
                Toast.MakeText(this.Context, "Unauthenticated access detected!", ToastLength.Short).Show();
                return;
            }
            
            string productStr = productSpinner.SelectedItem.ToString();
            if (productStr == "" || qtyEditText.Text == "")
            {
                Toast.MakeText(this.Context, "Please fill out the required fields!", ToastLength.Short).Show();
                return;
            }

            Int64 productId = MainActivity.db.Table<Product>().First(c => c.name == productStr).Id;
            Int64 qty = Int64.Parse(qtyEditText.Text);

            productId = MainActivity.db.Table<Product>().First(i => i.Id == productId).Id;

            var Issuable = Issuables_Index.Issuable;
            if (Issuable == null)
                return;
            //can be edited by admin
            //Issuable.sellerId = sellerId;
            Issuable.productId = productId;
            Issuable.qty = qty;

            if (Issuables.update(Issuable))
            {
                MainActivity.mainActivity.replaceFragment(new Issuables_Index());
                Toast.MakeText(this.Context, "Issuable Updated", ToastLength.Short).Show();
            }
            else
            {
                //Toast.MakeText(this.Context, "Something went wrong!", ToastLength.Short).Show();
            }

        }


        private void ImageBtn_Click(object sender, EventArgs e)
        {
            MainActivity.imageViewAction.Invoke((ImageView)sender);
        }

        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            /*
            DatePickerDialog ddtime = new DatePickerDialog(this.Context, OnDateSet, DateTime.Today.Year, DateTime.Today.Month - 1,
                                                      DateTime.Today.Day
                                                              );

            ddtime.DatePicker.DateTime = selected_Date;
            ddtime.SetTitle("Select Bid Expiry Date");
            ddtime.Show();
            */

            DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                           this,
                                                           currently.Year,
                                                           currently.Month-1,
                                                           currently.Day);
            dialog.Show();
           // dialog.DatePicker.MaxDateTime.DayOfYear.Equals(currently.Year - 21);

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