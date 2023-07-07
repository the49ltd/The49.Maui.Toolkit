using MauiTextAlignment = Microsoft.Maui.TextAlignment;

namespace The49.Maui.Toolkit;

public partial class TextAlignment
{
    public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.CreateAttached("HorizontalTextAlignment", typeof(MauiTextAlignment), typeof(VisualElement), null, propertyChanged: OnTextAlignmentChanged);
    public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.CreateAttached("VerticalTextAlignment", typeof(MauiTextAlignment), typeof(VisualElement), null, propertyChanged: OnTextAlignmentChanged);

    public static MauiTextAlignment GetHorizontalTextAlignment(BindableObject bindable) => (MauiTextAlignment)bindable.GetValue(HorizontalTextAlignmentProperty);
    public static void SetHorizontalTextAlignment(BindableObject bindable, TextAlignment value) => bindable.SetValue(HorizontalTextAlignmentProperty, value);
    public static MauiTextAlignment GetVerticalTextAlignment(BindableObject bindable) => (MauiTextAlignment)bindable.GetValue(VerticalTextAlignmentProperty);
    public static void SetVerticalTextAlignment(BindableObject bindable, TextAlignment value) => bindable.SetValue(VerticalTextAlignmentProperty, value);

    static void OnTextAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        PlatformUpdateTextAlignment(bindable);
    }

    static partial void PlatformUpdateTextAlignment(BindableObject bindable);
}
