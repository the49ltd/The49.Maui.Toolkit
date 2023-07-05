namespace The49.Maui.Toolkit.Dialogs;

public partial class ConfirmDialog
{
    protected override Task<bool> PlatformShow()
    {
        return Task.FromResult(true);
    }
}
