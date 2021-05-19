using Android.App;
using Android.OS;
using Android.Widget;

public class InfoBoxFragment : DialogFragment
{
    public override Dialog OnCreateDialog(Bundle savedInstanceState)
    {
        AlertDialog.Builder builder = new AlertDialog.Builder(Activity);

        builder.SetTitle("Title");
        builder.SetMessage("Allow?");
        builder.SetPositiveButton("Yes", (sent, args) =>
        {
        Toast.MakeText(this.Context, "Yes", ToastLength.Short).Show();
        })
    .SetNegativeButton("No", (sent, args) =>
    {
        Toast.MakeText(this.Context, "No", ToastLength.Short).Show();
    })
    .SetCancelable(false)
    .Show();

        return builder.Create();
    }
}