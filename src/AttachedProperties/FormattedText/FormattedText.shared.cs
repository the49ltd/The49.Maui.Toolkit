namespace The49.Maui.Toolkit;

public partial class FormattedText
{
    public static readonly BindableProperty FormattedTextProperty = BindableProperty.CreateAttached("FormattedText", typeof(FormattedString), typeof(ITextInput), null, propertyChanged: OnFormattedTextChanged);

    static void OnFormattedTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is View v)
        {
            if (v.Handler is null)
            {
                void OnHandlerChanged(object sender, EventArgs e)
                {
                    v.HandlerChanged -= OnHandlerChanged;
                    UpdateAttributedText(v);
                }
                v.HandlerChanged += OnHandlerChanged;
                return;
            }
            UpdateAttributedText(v);
        }
    }

    static void UpdateAttributedText(View v)
    {
        PlatformUpdateAttributedText(v);   
    }

    static partial void PlatformUpdateAttributedText(View view);

    public static FormattedString GetFormattedText(BindableObject bindable) => (FormattedString)bindable.GetValue(FormattedTextProperty);
    public static void SetFormattedText(BindableObject bindable, FormattedString value) => bindable.SetValue(FormattedTextProperty, value);
}
