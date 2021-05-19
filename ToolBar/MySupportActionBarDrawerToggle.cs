using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SupportActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using Android.Support.V7.App;
using Android.Support.V4.Widget;

namespace ToolBar
{
    class MySupportActionBarDrawerToggle: SupportActionBarDrawerToggle
    {
        private ActionBarActivity mHostActivity;
        private int mOpenedResource;
        private int mClosedResource;

        public MySupportActionBarDrawerToggle(ActionBarActivity hostedActivity, DrawerLayout drawerLayout, int openedResource, int closedResource) : base(hostedActivity, drawerLayout, openedResource, closedResource)
        {
            mHostActivity=hostedActivity;
            mOpenedResource = openedResource;
            mClosedResource = closedResource;
        }

        public override void OnDrawerOpened(View drawerView)
        {
            int drawerTag = (int)drawerView.Tag;
            if (drawerTag==0)
            {//only left drawer to animate the humberger
                mHostActivity.SupportActionBar.SetTitle(mOpenedResource);
                base.OnDrawerOpened(drawerView);
            }
        }

        public override void OnDrawerClosed(View drawerView)
        {
            int drawerTag = (int)drawerView.Tag;
            if (drawerTag == 0)
            {//only left drawer to animate the humberger
                mHostActivity.SupportActionBar.SetTitle(mClosedResource);
                base.OnDrawerClosed(drawerView);
            }
        }
        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {

            int drawerTag = (int)drawerView.Tag;
            if (drawerTag == 0)
            {//only left drawer to animate the humberger
                base.OnDrawerSlide(drawerView, slideOffset);
            }
        }
    }
}