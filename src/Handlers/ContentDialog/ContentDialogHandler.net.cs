using The49.Maui.Toolkit.Dialogs;

namespace The49.Maui.Toolkit.Handlers;

public partial class ContentDialogHandler
{
    static Task PlatformMapDismissAsync(ContentDialog dialog)
    {
        return Task.CompletedTask;
    }
}
