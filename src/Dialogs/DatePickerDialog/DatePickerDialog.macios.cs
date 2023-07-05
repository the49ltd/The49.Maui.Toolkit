namespace The49.Maui.Toolkit.Dialogs;

public partial class DatePickerDialog
{
    public Task<DateOnly?> PlatformShowAsync()
    {
        var tcs = new TaskCompletionSource<DateOnly?>();

        var viewController = new DatePickerDialogViewController(CurrentDate ?? DateOnly.FromDateTime(DateTime.Now));

        viewController.Canceled += (s, e) => tcs.SetResult(null);
        viewController.Selected += (s, e) => tcs.SetResult(e);

        var rootController = WindowStateManager.Default.GetCurrentUIViewController();

        rootController.PresentViewController(viewController, true, () => { });

        return tcs.Task;
    }
}
