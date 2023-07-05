namespace The49.Maui.Toolkit.Dialogs;

public partial class TimePickerDialog
{
    public Task<(int, int)> PlatformShowAsync()
    {
        var tcs = new TaskCompletionSource<(int, int)>();

        var now = DateTime.Now;

        var viewController = new TimePickerDialogViewController(
            CurrentHours == -1 ? now.Hour : CurrentHours,
            CurrentMinutes == -1 ? now.Minute : CurrentMinutes
        );

        viewController.Canceled += (s, e) => tcs.SetResult((-1, -1));
        viewController.Selected += (s, e) => tcs.SetResult(e);

        var rootController = WindowStateManager.Default.GetCurrentUIViewController();

        rootController.PresentViewController(viewController, true, () => { });

        return tcs.Task;
    }
}
