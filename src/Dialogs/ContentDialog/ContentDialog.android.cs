using AndroidX.AppCompat.App;
using MauiPlatform = Microsoft.Maui.ApplicationModel.Platform;

namespace The49.Maui.Toolkit.Dialogs;

public partial class ContentDialog
{
    public ContentDialogFragment Controller { get; internal set; }

    public Task PlatformShowAsync()
    {
        var tcs = new TaskCompletionSource();
        var ctx = MauiPlatform.CurrentActivity;
        var fragment = new ContentDialogFragment(this);

        Controller = fragment;

        void OnShow(object sender, EventArgs e)
        {
            fragment.ShowEvent -= OnShow;
            tcs.SetResult();
        }

        void OnDismiss(object sender, EventArgs e)
        {
            fragment.DismissEvent -= OnDismiss;
            NotifyDismissed();
        }

        fragment.ShowEvent += OnShow;
        fragment.DismissEvent += OnDismiss;

        fragment.Show(((AppCompatActivity)ctx).SupportFragmentManager, nameof(ContentDialogFragment));

        return tcs.Task;
    }
}
