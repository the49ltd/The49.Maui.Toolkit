using Android.Text.Style;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using Microsoft.Maui.Platform;
using The49.Maui.Toolkit.Sample.Pages;

namespace The49.Maui.Toolkit.Sample;

public class AttributedText
{
    public static readonly BindableProperty TextProperty = BindableProperty.CreateAttached("Text", typeof(string), typeof(ITextInput), null, propertyChanged: OnTextChanged);

    static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
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
        var pv = v.Handler.PlatformView as AppCompatEditText;

        pv.Text = GetText(v);

        ForegroundColorSpan highlightSpan = new ForegroundColorSpan(Colors.Salmon.ToPlatform());
        pv.EditableText.SetSpan(highlightSpan, 1, 4, Android.Text.SpanTypes.InclusiveInclusive);
    }

    public static string GetText(BindableObject bindable) => (string)bindable.GetValue(TextProperty);
    public static void SetText(BindableObject bindable, string value) => bindable.SetValue(TextProperty, value);
}

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(OnClickPage));
    }
}

