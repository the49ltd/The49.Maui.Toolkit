using AView = Android.Views.View;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using The49.Maui.Toolkit.Extensions;
using The49.Maui.Toolkit.Platform;

namespace The49.Maui.Toolkit;

public partial class OnClick
{
    static IDictionary<Context, int> _cachedResourceIds = new Dictionary<Context, int>();

    public static readonly BindableProperty OnClickListenerProperty =
        BindableProperty.CreateAttached("OnClickListener", typeof(ViewClickListener), typeof(VisualElement), null);

    public static readonly BindableProperty RippleDrawableProperty =
        BindableProperty.CreateAttached("RippleDrawable", typeof(RippleDrawable), typeof(VisualElement), null);
    static partial void PlatformSetupClickListener(VisualElement visualElement)
    {
        if (visualElement.Handler.PlatformView is AView view)
        {
            var oldListener = (ViewClickListener)visualElement.GetValue(OnClickListenerProperty);
            if (oldListener != null)
            {
                return;
            }
            view.Clickable = true;
            var listener = new ViewClickListener(() => TriggerClick(visualElement));
            visualElement.SetValue(OnClickListenerProperty, listener);
            view.SetOnClickListener(listener);

            view.UpdateRippleColor(visualElement);
        }
    }

    static partial void PlatformRippleColorChanged(VisualElement visualElement)
    {
        if (visualElement.Handler.PlatformView is AView view)
        {
            view.UpdateRippleColor(visualElement);
        }
    }

    internal static int GetSelectableItemBackground(Context context)
    {
        if (!_cachedResourceIds.ContainsKey(context))
        {
            using var outValue = new TypedValue();
            context.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground, outValue, true);
            _cachedResourceIds.Add(context, outValue.ResourceId);
        }
        return _cachedResourceIds[context];
    }
}
