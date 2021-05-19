package crc641a57454b9217800d;


public class MySupportActionBarDrawerToggle
	extends android.support.v7.app.ActionBarDrawerToggle
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDrawerOpened:(Landroid/view/View;)V:GetOnDrawerOpened_Landroid_view_View_Handler\n" +
			"n_onDrawerClosed:(Landroid/view/View;)V:GetOnDrawerClosed_Landroid_view_View_Handler\n" +
			"n_onDrawerSlide:(Landroid/view/View;F)V:GetOnDrawerSlide_Landroid_view_View_FHandler\n" +
			"";
		mono.android.Runtime.register ("ToolBar.MySupportActionBarDrawerToggle, TenderApp", MySupportActionBarDrawerToggle.class, __md_methods);
	}


	public MySupportActionBarDrawerToggle (android.app.Activity p0, android.support.v4.widget.DrawerLayout p1, android.support.v7.widget.Toolbar p2, int p3, int p4)
	{
		super (p0, p1, p2, p3, p4);
		if (getClass () == MySupportActionBarDrawerToggle.class)
			mono.android.TypeManager.Activate ("ToolBar.MySupportActionBarDrawerToggle, TenderApp", "Android.App.Activity, Mono.Android:Android.Support.V4.Widget.DrawerLayout, Xamarin.Android.Support.v4:Android.Support.V7.Widget.Toolbar, Xamarin.Android.Support.v7.AppCompat:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3, p4 });
	}


	public MySupportActionBarDrawerToggle (android.app.Activity p0, android.support.v4.widget.DrawerLayout p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == MySupportActionBarDrawerToggle.class)
			mono.android.TypeManager.Activate ("ToolBar.MySupportActionBarDrawerToggle, TenderApp", "Android.App.Activity, Mono.Android:Android.Support.V4.Widget.DrawerLayout, Xamarin.Android.Support.v4:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onDrawerOpened (android.view.View p0)
	{
		n_onDrawerOpened (p0);
	}

	private native void n_onDrawerOpened (android.view.View p0);


	public void onDrawerClosed (android.view.View p0)
	{
		n_onDrawerClosed (p0);
	}

	private native void n_onDrawerClosed (android.view.View p0);


	public void onDrawerSlide (android.view.View p0, float p1)
	{
		n_onDrawerSlide (p0, p1);
	}

	private native void n_onDrawerSlide (android.view.View p0, float p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
