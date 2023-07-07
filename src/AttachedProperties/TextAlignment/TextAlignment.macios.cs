using The49.Maui.Toolkit.Extensions;
using UIKit;
using MauiTextAlignment = Microsoft.Maui.TextAlignment;

namespace The49.Maui.Toolkit;

public partial class TextAlignment
{
    async static partial void PlatformUpdateTextAlignment(BindableObject bindable)
    {
        if (bindable is Button button)
        {
            await button.EnsureHandlerAsync();
            var platformView = (UIButton)button.Handler.PlatformView;
            platformView.HorizontalAlignment = GetHorizontalTextAlignment(button) switch
            {
                MauiTextAlignment.Start => UIControlContentHorizontalAlignment.Left,
                MauiTextAlignment.Center => UIControlContentHorizontalAlignment.Center,
                MauiTextAlignment.End => UIControlContentHorizontalAlignment.Right,
                _ => UIControlContentHorizontalAlignment.Center,
            };
            platformView.VerticalAlignment = GetVerticalTextAlignment(button) switch
            {
                MauiTextAlignment.Start => UIControlContentVerticalAlignment.Top,
                MauiTextAlignment.Center => UIControlContentVerticalAlignment.Center,
                MauiTextAlignment.End => UIControlContentVerticalAlignment.Bottom,
                _ => UIControlContentVerticalAlignment.Center,
            };
        }
    }
}
