[assembly: XmlnsDefinition("https://schemas.the49.com/dotnet/2023/maui", "The49.Maui.Toolkit")]
[assembly: XmlnsDefinition("https://schemas.the49.com/dotnet/2023/maui", "The49.Maui.Toolkit.Views")]

namespace The49.Maui.Toolkit;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseThe49Toolkit(this MauiAppBuilder builder)
    {
#if IOS
        builder.ConfigureMauiHandlers(cfg =>
        {
            cfg.AddHandler<The49.Maui.Toolkit.Views.CollectionView, The49.Maui.Toolkit.Handlers.CollectionViewHandler>();
        });
#endif
        return builder;
    }
}

