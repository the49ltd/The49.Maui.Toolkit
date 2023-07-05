using AView = Android.Views.View;
using Android.Views;
using Android.OS;
using Microsoft.Maui.Platform;
using MauiApplication = Microsoft.Maui.Controls.Application;
using Android.Graphics.Drawables;
using AndroidX.Fragment.App;

namespace The49.Maui.Toolkit.Dialogs;

public class ContentDialogFragment : DialogFragment
{
    ContentDialog _page;

    public event EventHandler ShowEvent;
    public event EventHandler DismissEvent;

    public ContentDialogFragment(ContentDialog page) : base()
    {
        _page = page;
    }
    public override AView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var v = _page.ToContainerView(MauiApplication.Current.Windows[0].Handler.MauiContext);
        v.LayoutChange += OnLayoutChange;
        return v;
    }

    void OnLayoutChange(object sender, AView.LayoutChangeEventArgs e)
    {
        Layout();
    }

    public override Android.App.Dialog OnCreateDialog(Bundle savedInstanceState)
    {
        var dialog = base.OnCreateDialog(savedInstanceState);
        dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
        dialog.Window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));

        dialog.ShowEvent += OnDialogShown;
        dialog.DismissEvent += OnDialogDismiss;

        return dialog;
    }

    void OnDialogDismiss(object sender, EventArgs e)
    {
        DismissEvent?.Invoke(this, EventArgs.Empty);
    }

    void OnDialogShown(object sender, EventArgs e)
    {
        ShowEvent?.Invoke(this, EventArgs.Empty);
    }

    public override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetStyle(DialogFragment.StyleNoFrame, Resource.Style.ThemeOverlay_Material3_Dialog);
    }

    public override void OnResume()
    {
        base.OnResume();
        Layout();
    }

    void Layout()
    {
        if (Dialog is null)
        {
            return;
        }
        var density = DeviceDisplay.MainDisplayInfo.Density;
        var widthConstraint = DeviceDisplay.MainDisplayInfo.Width / density;
        var heightConstraint = DeviceDisplay.MainDisplayInfo.Height / density;

        var r = _page.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
        Dialog.Window.SetLayout((int)(r.Request.Width * density), (int)(r.Request.Height * density));
    }
}