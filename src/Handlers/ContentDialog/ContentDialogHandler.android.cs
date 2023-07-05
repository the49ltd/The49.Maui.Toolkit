using The49.Maui.Toolkit.Dialogs;

namespace The49.Maui.Toolkit.Handlers;

public partial class ContentDialogHandler
{
    static Task PlatformMapDismissAsync(ContentDialog dialog)
    {
        var tcs = new TaskCompletionSource();
        void OnDismiss(object sender, EventArgs e)
        {
            dialog.Controller.DismissEvent -= OnDismiss;
            tcs.SetResult();
        }
        dialog.Controller.DismissEvent += OnDismiss;
        dialog.Controller.Dismiss();
        return tcs.Task;
    }
}
