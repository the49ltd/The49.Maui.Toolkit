using System.Windows.Input;

namespace The49.Maui.Toolkit;

public partial class OnClick
{
    // TODO: Allow light and dark and bindings
    internal const string OnClickRippleColor = nameof(OnClickRippleColor);
    internal const string OnClickRippleOpacity = nameof(OnClickRippleOpacity);

    public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached("Command", typeof(ICommand), typeof(VisualElement), null, propertyChanged: OnClickPropertyChanged);

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(VisualElement), null, propertyChanged: OnClickPropertyChanged);

    public static readonly BindableProperty RippleColorProperty = BindableProperty.CreateAttached("RippleColor", typeof(Color), typeof(VisualElement), null, defaultValueCreator: _ => GetDefaultRippleColor(), propertyChanged: OnRippleColorChanged);

    public static readonly BindableProperty RippleOpacityProperty = BindableProperty.CreateAttached("RippleOpacity", typeof(float), typeof(VisualElement), 1f, defaultValueCreator: _ => GetDefaultRippleOpacity(), propertyChanged: OnRippleColorChanged);

    public static ICommand GetCommand(BindableObject bindable) => (ICommand)bindable.GetValue(CommandProperty);
    public static void SetCommand(BindableObject bindable, ICommand value) => bindable.SetValue(CommandProperty, value);

    public static object GetCommandParameter(BindableObject bindable) => bindable.GetValue(CommandParameterProperty);
    public static void SetCommandParameter(BindableObject bindable, object value) => bindable.SetValue(CommandParameterProperty, value);

    public static Color GetRippleColor(BindableObject bindable) => (Color)bindable.GetValue(RippleColorProperty);
    public static void SetRippleColor(BindableObject bindable, Color value) => bindable.SetValue(RippleColorProperty, value);

    public static float GetRippleOpacity(BindableObject bindable) => (float)bindable.GetValue(RippleOpacityProperty);
    public static void SetRippleOpacity(BindableObject bindable, float value) => bindable.SetValue(RippleOpacityProperty, value);

    static partial void PlatformSetupClickListener(VisualElement visualElement);
    static partial void PlatformRippleColorChanged(VisualElement visualElement);

    static Color GetDefaultRippleColor()
    {
        if (Application.Current.Resources.TryGetValue(OnClickRippleColor, out object val) && val is Color color)
        {
            return color;
        }
        return null;
    }

    static float GetDefaultRippleOpacity()
    {
        if (Application.Current.Resources.TryGetValue(OnClickRippleOpacity, out object val) && val is float op)
        {
            return op;
        }
        return 1f;
    }

    private static void OnClickPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is VisualElement visualElement)
        {
            if (visualElement.Handler is not null)
            {
                SetupClickListener(visualElement);
            }
            visualElement.HandlerChanged += TargetHandlerChanged;
        }
    }

    static void SetupClickListener(VisualElement visualElement)
    {
        if (visualElement.Handler is not null)
        {
            PlatformSetupClickListener(visualElement);
        }
    }

    static void OnRippleColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (((VisualElement)bindable).Handler is not null)
        {
            PlatformRippleColorChanged((VisualElement)bindable);
        }
    }

    static void TargetHandlerChanged(object sender, EventArgs e)
    {
        if (sender is VisualElement visualElement)
        {
            SetupClickListener(visualElement);
        }
    }

    static void TriggerClick(VisualElement visualElement)
    {
        var command = GetCommand(visualElement);
        var commandParameter = GetCommandParameter(visualElement);

        command?.Execute(commandParameter);
    }
}
