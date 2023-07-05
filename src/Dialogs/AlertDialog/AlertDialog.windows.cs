namespace The49.Maui.Toolkit.Dialogs;

public partial class AlertDialog
{
    protected virtual Task PlatformShow()
    {
        return Task.CompletedTask;
    }
}
