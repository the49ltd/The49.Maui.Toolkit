using UIKit;

namespace The49.Maui.Toolkit.Dialogs;

public partial class AlertDialog
{
    protected virtual Task PlatformShow()
    {
        var tcs = new TaskCompletionSource();
        var alert = UIAlertController.Create(Title, Message, UIAlertControllerStyle.Alert);
        var okAction = UIAlertAction.Create(
            ActionText,
            UIAlertActionStyle.Default,
            (_) => tcs.SetResult()
        );

        alert.AddAction(okAction);

        WindowStateManager.Default.GetCurrentUIViewController()?.PresentViewController(alert, true, null);

        return tcs.Task;
    }
}
