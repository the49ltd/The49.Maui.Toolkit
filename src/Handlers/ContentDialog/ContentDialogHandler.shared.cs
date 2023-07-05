using Microsoft.Maui.Handlers;
using The49.Maui.Toolkit.Dialogs;

namespace The49.Maui.Toolkit.Handlers;

public partial class ContentDialogHandler : PageHandler
{
    public static new IPropertyMapper<ContentDialog, ContentDialogHandler> Mapper =
            new PropertyMapper<ContentDialog, ContentDialogHandler>(ContentViewHandler.Mapper);

    public static new CommandMapper<ContentDialog, ContentDialogHandler> CommandMapper =
        new(ContentViewHandler.CommandMapper)
        {
            [nameof(ContentDialog.DismissAsync)] = MapDismissAsync,
        };

    static void MapDismissAsync(ContentDialogHandler handler, ContentDialog page, object arg)
    {
        PlatformMapDismissAsync(page);
        ((TaskCompletionSource)arg).SetResult();
    }

    public ContentDialogHandler() : base(Mapper, CommandMapper)
    {
    }

    public ContentDialogHandler(IPropertyMapper? mapper)
        : base(mapper ?? Mapper, CommandMapper)
    {
    }

    public ContentDialogHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }
}
