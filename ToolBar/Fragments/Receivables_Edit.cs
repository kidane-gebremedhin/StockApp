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
    public class Receivables_Edit : SupportFragment, DatePickerDialog.IOnDateSetListener
    {
        private EditText qtyEditText;
        TextView expireDateEditText;
        private Spinner productSpinner;
        private Button updateBtn;
        private Int64 mReceivableId;
        private string[] products;

        DateTime selected_Date;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           View view = inflater.Inflate(Resource.Layout.Receivables_Edit, container, false);
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

            Receivables Receivable = Receivables_Index.Receivable;
            Product product = MainActivity.db.Table<Product>().First(c => c.Id == Receivable.productId);
            productSpinner.SetSelection(productAdapter.GetPosition(product.name));
            qtyEditText.Text = Receivable.qty.ToString();

            return view;
        }


        public void Edit_Receivable(Int64 ReceivableId)
        {
            this.mReceivableId = ReceivableId;
            Receivables Receivable=new Receivables();
            var query = MainActivity.db.Table<Receivables>().Where(u => u.Id == mReceivableId);
            if (query != null)
            {
                foreach (var u in query.ToList<Receivables>())
                {
                    Receivable=u;
                    break;
                }
            }

           /*
            daySpinner.SetSelection(days_adapter.GetPosition(program.day));
            qtyEditText.Text=program.name;
            startTime_HrsSpinner.SelectedReceivable = program.startTime_Hr;
            startTime_MinsSpinner.SelectedReceivable = program.startTime_Mins;
            startTime_amPmSpinner.SelectedReceivable = program.startTime_amPm;
            endTime_HrsSpinner.SelectedReceivable = program.endTime_Hr;
            endTime_MinsSpinner.SelectedReceivable = program.endTime_Mins;
            endTime_amPmSpinner.SelectedReceivable = program.endTime_amPm;
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

            var receivable = Receivables_Index.Receivable;
            if (receivable == null)
                return;
            //can be edited by admin
            //Receivable.sellerId = sellerId;
            receivable.productId = productId;
            receivable.qty = qty;

            if (Receivables.update(receivable))
            {
                MainActivity.mainActivity.replaceFragment(new Receivables_Index());
                Toast.MakeText(this.Context, "Receivable Updated", ToastLength.Short).Show();
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