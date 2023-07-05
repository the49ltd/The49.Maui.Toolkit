namespace The49.Maui.Toolkit.Dialogs;

public partial class ContentDialog : ContentPage
{
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(nameof(Margin), typeof(Thickness), typeof(View), default(Thickness),
                            propertyChanged: MarginPropertyChanged);

    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }
    public event EventHandler Dismiss;
    public override SizeRequest Measure(double widthConstraint, double heightConstraint, MeasureFlags flags = MeasureFlags.None)
    {
        double widthRequest = WidthRequest;
        double heightRequest = HeightRequest;

        bool includeMargins = (flags & MeasureFlags.IncludeMargins) != 0;

        if (includeMargins)
        {
            widthConstraint = Math.Max(0, widthConstraint - Margin.HorizontalThickness);
            heightConstraint = Math.Max(0, heightConstraint - Margin.VerticalThickness);
        }

        var childRequest = new SizeRequest();

        if ((widthRequest == -1 || heightRequest == -1) && Content is not null)
        {
            childRequest = Content.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
        }

        return new SizeRequest
        {
            Request = new Size { Width = widthRequest != -1 ? widthRequest : childRequest.Request.Width, Height = heightRequest != -1 ? heightRequest : childRequest.Request.Height },
            Minimum = childRequest.Minimum
        };
    }

    static void MarginPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((ContentDialog)bindable).InvalidateMeasureNonVirtual(Microsoft.Maui.Controls.Internals.InvalidationTrigger.MarginChanged);
    }

    internal void NotifyDismissed()
    {
        Dismiss?.Invoke(this, EventArgs.Empty);
    }

    public Task DismissAsync()
    {
        var tcs = new TaskCompletionSource();
        Handler.Invoke(nameof(DismissAsync), tcs);
        return tcs.Task;
    }

    public Task ShowAsync()
    {
        return PlatformShowAsync();
    }
}
