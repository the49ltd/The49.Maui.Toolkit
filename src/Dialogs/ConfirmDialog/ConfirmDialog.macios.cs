using UIKit;

namespace The49.Maui.Toolkit.Dialogs;

public partial class ConfirmDialog
{
    protected override Task<bool> PlatformShow()
    {
        var tcs = new TaskCompletionSource<bool>();
        var alert = UIAlertController.Create(Title, Message, UIAlertControllerStyle.Alert);
        var okAction = UIAlertAction.Create(
            ActionText,
            IsDestructive ? UIAlertActionStyle.Destructive : UIAlertActionStyle.Default,
            (_) => tcs.SetResult(true)
        );
        var cancelAction = UIAlertAction.Create(CancelText, UIAlertActionStyle.Cancel, (_) => tcs.SetResult(false));

        alert.AddAction(okAction);
        alert.AddAction(cancelAction);

        WindowStateManager.Default.GetCurrentUIViewController()?.PresentViewController(alert, true, null);

        return tcs.Task;
    }
}
