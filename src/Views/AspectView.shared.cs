using Microsoft.Maui.Layouts;

namespace The49.Maui.Toolkit.Views;

public enum ConstrainedDimension
{
    Width,
    Height
}

public class AspectLayoutManager : LayoutManager
{
    AspectView _layout;
    public AspectLayoutManager(AspectView layout) : base(layout)
    {
        _layout = layout;
    }

    public override Size ArrangeChildren(Rect bounds)
    {
        foreach (var child in Layout)
        {
            child.Arrange(bounds);
        }
        return bounds.Size;
    }

    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        var ratio = _layout.WidthRatio / _layout.HeightRatio;

        var s = new Size(widthConstraint, heightConstraint);

        if (_layout.ConstrainedDimension == ConstrainedDimension.Width)
        {
            s.Height = s.Width / ratio;
        }
        else
        {
            s.Width = s.Height * ratio;
        }
        return s;
    }
}

[ContentProperty(nameof(Content))]
public class AspectView : Layout
{
    public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(AspectView), propertyChanged: OnContentChanged);
    public static readonly BindableProperty WidthRatioProperty = BindableProperty.Create(nameof(WidthRatio), typeof(double), typeof(AspectView), defaultValueCreator: (_) => 1d, propertyChanged: OnRatioPropertyChanged);
    public static readonly BindableProperty HeightRatioProperty = BindableProperty.Create(nameof(HeightRatio), typeof(double), typeof(AspectView), defaultValueCreator: (_) => 1d, propertyChanged: OnRatioPropertyChanged);
    public static readonly BindableProperty ConstrainedDimensionProperty = BindableProperty.Create(nameof(ConstrainedDimension), typeof(ConstrainedDimension), typeof(AspectView), defaultValueCreator: (_) => ConstrainedDimension.Width, propertyChanged: OnRatioPropertyChanged);

    public View Content
    {
        get => (View)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public double WidthRatio
    {
        get => (double)GetValue(WidthRatioProperty);
        set => SetValue(WidthRatioProperty, value);
    }

    public double HeightRatio
    {
        get => (double)GetValue(HeightRatioProperty);
        set => SetValue(HeightRatioProperty, value);
    }

    public ConstrainedDimension ConstrainedDimension
    {
        get => (ConstrainedDimension)GetValue(ConstrainedDimensionProperty);
        set => SetValue(ConstrainedDimensionProperty, value);
    }

    protected override ILayoutManager CreateLayoutManager()
    {
        return new AspectLayoutManager(this);
    }

    static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var aspect = bindable as AspectView;

        aspect.UpdateContent();
    }

    static void OnRatioPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var aspect = bindable as AspectView;

        aspect.UpdateRatio();
    }

    void UpdateRatio()
    {
        InvalidateMeasure();
    }

    void UpdateContent()
    {
        Clear();
        if (Content is not null)
        {
            Add(Content);
        }
    }
}

