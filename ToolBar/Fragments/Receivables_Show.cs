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
using Android.Media;
using Android.Graphics;

namespace ToolBar.Resources.Fragments
{
    public class Receivables_Show : SupportFragment
    {
        MainActivity mainActivity = new MainActivity();

        private TextView productTextView, qtyTextView;
        private ImageView imageImageView;
        private Button ReceivablesListBtn;
        public static bool isImageFitToScreen = false;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Receivables_Show, container, false);
            productTextView = view.FindViewById<TextView>(Resource.Id.productTextView);
            qtyTextView = view.FindViewById<TextView>(Resource.Id.qtyTextView);

            ReceivablesListBtn = view.FindViewById<Button>(Resource.Id.ReceivablesListBtn);
            ReceivablesListBtn.Click += ReceivablesListBtn_Click;


            if (MainActivity.loggedUser == null)
                ReceivablesListBtn.Visibility = ViewStates.Invisible;
            else
                ReceivablesListBtn.Visibility = ViewStates.Visible;


            Receivables Receivable = Receivables_Index.Receivable;
            if (Receivable == null)
                return view;
           productTextView.Text = MainActivity.db.Table<Product>().Where(i => i.Id == Receivable.productId).First().name;
            qtyTextView.Text = Receivable.qty.ToString();

            return view;
        }

        private void ImageImageView_Click(object sender, EventArgs e)
        {
            int img_width = MainActivity.mainActivity.ConvertPixelsToDp(imageImageView.Width);
            int img_height = MainActivity.mainActivity.ConvertPixelsToDp(imageImageView.Height);
           
            if (isImageFitToScreen)
            {
                isImageFitToScreen = false;
                //imageImageView.LayoutParameters=new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, FrameLayout.LayoutParams.WrapContent);
                imageImageView.LayoutParameters = new FrameLayout.LayoutParams(img_height, img_width);
                imageImageView.SetAdjustViewBounds(true);
            }
            else
            {
                isImageFitToScreen = true;
                imageImageView.LayoutParameters=new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.MatchParent);
                //imageImageView.SetScaleType(ImageView.ScaleType.FitXy);
            }
        }

        private void ReceivablesListBtn_Click(object sender, EventArgs e)
        {
            if (Receivables_Index.Receivable != null)
            {
              MainActivity.mainActivity.replaceFragment(new Receivables_Index());
            }
        }


        //Retrieve: Fetch the Base64String and convert that to Bitmap again.
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        
       

    }
    
}