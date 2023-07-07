using Android.Views;
using AndroidX.AppCompat.Widget;
using The49.Maui.Toolkit.Extensions;
using MauiTextAlignment = Microsoft.Maui.TextAlignment;

namespace The49.Maui.Toolkit;

public partial class TextAlignment
{
    async static partial void PlatformUpdateTextAlignment(BindableObject bindable)
    {
        if (bindable is Button button)
        {
            await button.EnsureHandlerAsync();
            var platformView = (AppCompatButton)button.Handler.PlatformView;
            GravityFlags horizontalFlag = GetHorizontalTextAlignment(button) switch
            {
                MauiTextAlignment.Start => GravityFlags.Left,
                MauiTextAlignment.Center => GravityFlags.CenterHorizontal,
                MauiTextAlignment.End => GravityFlags.Right,
                _ => GravityFlags.Center
            };
            GravityFlags verticalFlag = GetHorizontalTextAlignment(button) switch
            {
                MauiTextAlignment.Start => GravityFlags.Top,
                MauiTextAlignment.Center => GravityFlags.CenterVertical,
                MauiTextAlignment.End => GravityFlags.Bottom,
                _ => GravityFlags.Center
            };
            platformView.Gravity = horizontalFlag | verticalFlag;
            platformView.TextAlignment = Android.Views.TextAlignment.Center;
        }
    }
}
