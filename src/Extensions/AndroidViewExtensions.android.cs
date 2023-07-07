using Android.Content.Res;
using Android.Graphics.Drawables;
using AndroidX.Core.Graphics;
using AView = Android.Views.View;
using Microsoft.Maui.Platform;

namespace The49.Maui.Toolkit.Extensions;

internal static class AndroidViewExtensions
{
    internal static void UpdateRippleColor(this AView view, VisualElement visualElement)
    {
        var d = visualElement.GetValue(OnClick.RippleDrawableProperty) as RippleDrawable;

        if (d is null)
        {
            view.UpdateRippleDrawable(visualElement);
        }
        d = visualElement.GetValue(OnClick.RippleDrawableProperty) as RippleDrawable;
        var color = OnClick.GetRippleColor(visualElement);
        if (color is not null)
        {
            var c = color.ToPlatform();
            d.SetColor(GetPressedColorSelector(ColorUtils.SetAlphaComponent(c, (int)(OnClick.GetRippleOpacity(visualElement) * 255))));
        }
    }

    internal static void UpdateRippleDrawable(this AView view, VisualElement visualElement)
    {
        var d = view.Context.GetDrawable(OnClick.GetSelectableItemBackground(view.Context));
        if (d is RippleDrawable rd)
        {
            visualElement.SetValue(OnClick.RippleDrawableProperty, rd);
            if (OperatingSystem.IsAndroidVersionAtLeast(23))
            {
                view.Foreground = rd;
            }
        }
    }

    internal static ColorStateList GetPressedColorSelector(int pressedColor)
    {
        return new ColorStateList(new int[][]
        {
            new int[]{}
        },
        new int[]
        {
            pressedColor
        });
    }
}
