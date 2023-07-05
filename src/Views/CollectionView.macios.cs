using System.Collections;
using System.Windows.Input;

namespace The49.Maui.Toolkit.Views;

public class CollectionView : View
{
    public static readonly BindableProperty ItemsSourceProperty =
                BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(CollectionView), null);

    List<View> _logicalChildren = new List<View>();

    public static readonly BindableProperty EmptyViewProperty =
        BindableProperty.Create(nameof(EmptyView), typeof(object), typeof(CollectionView), null);

    public object EmptyView
    {
        get => GetValue(EmptyViewProperty);
        set => SetValue(EmptyViewProperty, value);
    }

    public static readonly BindableProperty EmptyViewTemplateProperty =
        BindableProperty.Create(nameof(EmptyViewTemplate), typeof(DataTemplate), typeof(CollectionView), null);

    public DataTemplate EmptyViewTemplate
    {
        get => (DataTemplate)GetValue(EmptyViewTemplateProperty);
        set => SetValue(EmptyViewTemplateProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(CollectionView));

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public static readonly BindableProperty RemainingItemsThresholdReachedCommandProperty =
            BindableProperty.Create(nameof(RemainingItemsThresholdReachedCommand), typeof(ICommand), typeof(ItemsView), null);

    public ICommand RemainingItemsThresholdReachedCommand
    {
        get => (ICommand)GetValue(RemainingItemsThresholdReachedCommandProperty);
        set => SetValue(RemainingItemsThresholdReachedCommandProperty, value);
    }

    public static readonly BindableProperty RemainingItemsThresholdReachedCommandParameterProperty = BindableProperty.Create(nameof(RemainingItemsThresholdReachedCommandParameter), typeof(object), typeof(ItemsView), default(object));

    public object RemainingItemsThresholdReachedCommandParameter
    {
        get => GetValue(RemainingItemsThresholdReachedCommandParameterProperty);
        set => SetValue(RemainingItemsThresholdReachedCommandParameterProperty, value);
    }

    public static readonly BindableProperty HorizontalScrollBarVisibilityProperty = BindableProperty.Create(
        nameof(HorizontalScrollBarVisibility),
        typeof(ScrollBarVisibility),
        typeof(ItemsView),
        ScrollBarVisibility.Default);

    public ScrollBarVisibility HorizontalScrollBarVisibility
    {
        get => (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
        set => SetValue(HorizontalScrollBarVisibilityProperty, value);
    }

    public static readonly BindableProperty VerticalScrollBarVisibilityProperty = BindableProperty.Create(
        nameof(VerticalScrollBarVisibility),
        typeof(ScrollBarVisibility),
        typeof(ItemsView),
        ScrollBarVisibility.Default);

    public ScrollBarVisibility VerticalScrollBarVisibility
    {
        get => (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
        set => SetValue(VerticalScrollBarVisibilityProperty, value);
    }

    public static readonly BindableProperty RemainingItemsThresholdProperty =
        BindableProperty.Create(nameof(RemainingItemsThreshold), typeof(int), typeof(ItemsView), -1, validateValue: (bindable, value) => (int)value >= -1);

    public int RemainingItemsThreshold
    {
        get => (int)GetValue(RemainingItemsThresholdProperty);
        set => SetValue(RemainingItemsThresholdProperty, value);
    }

    public static readonly BindableProperty HeaderProperty =
        BindableProperty.Create(nameof(Header), typeof(object), typeof(CollectionView), null);

    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly BindableProperty HeaderTemplateProperty =
        BindableProperty.Create(nameof(HeaderTemplate), typeof(DataTemplate), typeof(CollectionView), null);

    public DataTemplate HeaderTemplate
    {
        get => (DataTemplate)GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public static readonly BindableProperty FooterProperty =
        BindableProperty.Create(nameof(Footer), typeof(object), typeof(CollectionView), null);

    public object Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public static readonly BindableProperty FooterTemplateProperty =
        BindableProperty.Create(nameof(FooterTemplate), typeof(DataTemplate), typeof(CollectionView), null);

    public DataTemplate FooterTemplate
    {
        get => (DataTemplate)GetValue(FooterTemplateProperty);
        set => SetValue(FooterTemplateProperty, value);
    }

    public static readonly BindableProperty ItemsLayoutProperty = BindableProperty.Create(nameof(ItemsLayout), typeof(IItemsLayout), typeof(ItemsView),
                LinearItemsLayout.Vertical, propertyChanged: OnItemsLayoutPropertyChanged);

    public IItemsLayout ItemsLayout
    {
        get => (IItemsLayout)GetValue(ItemsLayoutProperty);
        set => SetValue(ItemsLayoutProperty, value);
    }

    public static readonly BindableProperty IsGroupedProperty =
        BindableProperty.Create(nameof(IsGrouped), typeof(bool), typeof(GroupableItemsView), false);

    public bool IsGrouped
    {
        get => (bool)GetValue(IsGroupedProperty);
        set => SetValue(IsGroupedProperty, value);
    }

    public static readonly BindableProperty GroupHeaderTemplateProperty =
        BindableProperty.Create(nameof(GroupHeaderTemplate), typeof(DataTemplate), typeof(GroupableItemsView), default(DataTemplate));

    public DataTemplate GroupHeaderTemplate
    {
        get => (DataTemplate)GetValue(GroupHeaderTemplateProperty);
        set => SetValue(GroupHeaderTemplateProperty, value);
    }

    public static readonly BindableProperty GroupFooterTemplateProperty =
        BindableProperty.Create(nameof(GroupFooterTemplate), typeof(DataTemplate), typeof(GroupableItemsView), default(DataTemplate));

    public DataTemplate GroupFooterTemplate
    {
        get => (DataTemplate)GetValue(GroupFooterTemplateProperty);
        set => SetValue(GroupFooterTemplateProperty, value);
    }

    static void OnItemsLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue is BindableObject boOld)
        {
            SetInheritedBindingContext(boOld, null);
        }

        if (newValue is BindableObject boNew)
            SetInheritedBindingContext(boNew, bindable.BindingContext);
    }

    public CollectionView()
    {
    }

    public void SendRemainingItemsThresholdReached()
    {
        RemainingItemsThresholdReached?.Invoke(this, EventArgs.Empty);

        if (RemainingItemsThresholdReachedCommand?.CanExecute(RemainingItemsThresholdReachedCommandParameter) == true)
            RemainingItemsThresholdReachedCommand?.Execute(RemainingItemsThresholdReachedCommandParameter);

        OnRemainingItemsThresholdReached();
    }

    public void SendScrolled(ItemsViewScrolledEventArgs e)
    {
        Scrolled?.Invoke(this, e);

        OnScrolled(e);
    }

    protected virtual void OnScrollToRequested(ScrollToRequestEventArgs e)
    {
        ScrollToRequested?.Invoke(this, e);
    }

    protected virtual void OnRemainingItemsThresholdReached()
    {

    }

    protected virtual void OnScrolled(ItemsViewScrolledEventArgs e)
    {

    }

    public event EventHandler<ScrollToRequestEventArgs> ScrollToRequested;

    public event EventHandler<ItemsViewScrolledEventArgs> Scrolled;

    public event EventHandler RemainingItemsThresholdReached;

    internal void RemoveLogicalChild(View view)
    {
        if (!_logicalChildren.Contains(view))
        {
            return;
        }

        var oldLogicalIndex = _logicalChildren.IndexOf(view);
        _logicalChildren.Remove(view);
        OnChildRemoved(view, oldLogicalIndex);
        VisualDiagnostics.OnChildRemoved(this, view, oldLogicalIndex);
    }

    internal void AddLogicalChild(View view)
    {
        _logicalChildren.Add(view);
        view.Parent = this;
        OnChildAdded(view);
        VisualDiagnostics.OnChildAdded(this, view);
    }

    protected override void OnBindingContextChanged()
    {
        if (_logicalChildren == null)
        {
            return;
        }

        var bc = BindingContext;

        foreach (var child in _logicalChildren)
        {
            var boc = child as BindableObject;
            if (boc == null)
            {
                continue;
            }

            SetInheritedBindingContext(boc, bc);
        }

        base.OnBindingContextChanged();
        if (ItemsLayout is BindableObject bo)
        {
            SetInheritedBindingContext(bo, BindingContext);
        }
    }
}
