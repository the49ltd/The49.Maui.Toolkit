using UIKit;
using Foundation;
using CollectionView = The49.Maui.Toolkit.Views.CollectionView;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Controls.Handlers.Items;
using System.Collections.Specialized;
using System.Collections;
using CoreGraphics;

namespace The49.Maui.Toolkit.Handlers;

public class CollectionViewController : UICollectionViewController
{
    CollectionViewLayout _layout;

    public IItemsViewSource ItemsSource { get; protected set; }
    CollectionView VirtualView { get; }

    Dictionary<DataTemplate, string> _seenTemplates = new Dictionary<DataTemplate, string>();

    DataTemplate _headerViewTemplate;
    DataTemplate _headerStringTemplate;

    // TODO: dynamic generation based on seen templates
    string headerReuseId = "header";
    string footerReuseId = "footer";

    int _counter = 0;
    View _emptyView;
    UIView _emptyUIView;
    DataTemplate _emptyViewTemplate;
    bool _emptyViewDisplayed;
    bool _disposed;
    bool _isEmpty = true;
    bool _initialized;
    CGSize _headerSize = CGSize.Empty;

    protected float PreviousHorizontalOffset, PreviousVerticalOffset;

    public CollectionViewController(CollectionView virtualView, CollectionViewLayout layout) : base(layout)
    {
        _layout = layout;
        VirtualView = virtualView;

        _headerViewTemplate = new DataTemplate(() => (View)VirtualView.Header);
        _headerStringTemplate = new DataTemplate(() => new Label { Text = (string)VirtualView.Header });
    }

    public override nint GetItemsCount(UICollectionView collectionView, nint section)
    {
        if (!_initialized)
        {
            return 0;
        }
        CheckForEmptySource();

        return ItemsSource.ItemCountInGroup(section);
    }

    public override nint NumberOfSections(UICollectionView collectionView)
    {
        return 1;
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        ItemsSource = CreateItemsViewSource();

        if (!(OperatingSystem.IsIOSVersionAtLeast(11) || OperatingSystem.IsMacCatalystVersionAtLeast(11)))
        {
            AutomaticallyAdjustsScrollViewInsets = false;
        }
        else
        {
            CollectionView.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
        }
        CollectionView.RegisterClassForSupplementaryView(typeof(CollectionViewHeader), UICollectionElementKindSection.Header, headerReuseId);
        CollectionView.RegisterClassForSupplementaryView(typeof(CollectionViewFooter), UICollectionElementKindSection.Footer, footerReuseId);
        CollectionView.CollectionViewLayout = _layout;

        _initialized = true;
        EnsureLayoutInitialized();
    }

    public void SetHeaderSize(CGSize size)
    {
        _headerSize = size;
        LayoutEmptyView();
    }

    void EnsureLayoutInitialized()
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;

        CollectionView.SetCollectionViewLayout(_layout, false);

        UpdateEmptyView();
    }

    protected virtual IItemsViewSource CreateItemsViewSource()
    {
        if (VirtualView.ItemsSource == null)
        {
            return new EmptySource();
        }

        switch (VirtualView.ItemsSource)
        {
            case IList _ when VirtualView.ItemsSource is INotifyCollectionChanged:
                return new ObservableItemsSource(VirtualView.ItemsSource as IList, this);
            case IEnumerable _ when VirtualView.ItemsSource is INotifyCollectionChanged:
                return new ObservableItemsSource(VirtualView.ItemsSource as IEnumerable, this);
            case IList list:
                return new ListSource(list);
            case IEnumerable<object> generic:
                return new ListSource(generic);
        }

        return new ListSource(VirtualView.ItemsSource);
    }

    static DataTemplate ResolveTemplate(DataTemplate template, object item, BindableObject container)
    {
        if (template is null)
        {
            return null;
        }
        if (template is DataTemplateSelector dts)
        {
            return dts.SelectTemplate(item, container);
        }
        return template;
    }

    DataTemplate GetTemplate(object item)
    {
        return ResolveTemplate(VirtualView.ItemTemplate, item, VirtualView);
    }

    DataTemplate GetHeaderTemplate()
    {
        if (VirtualView.Header is View)
        {
            return _headerViewTemplate;
        }
        else if (VirtualView.Header is string)
        {
            return _headerStringTemplate;
        }
        return ResolveTemplate(VirtualView.HeaderTemplate, VirtualView.Header, VirtualView);
    }


    DataTemplate GetFooterTemplate()
    {
        return ResolveTemplate(VirtualView.FooterTemplate, VirtualView.Footer, VirtualView);
    }

    string GetReuseId(DataTemplate template)
    {
        if (_seenTemplates.TryGetValue(template, out string id))
        {
            return id;
        }
        _counter++;
        var newId = _counter.ToString();

        _seenTemplates.Add(template, newId);
        CollectionView.RegisterClassForCell(typeof(CollectionViewCell), newId);
        return newId;
    }

    public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
    {
        var item = ItemsSource[indexPath];
        var template = GetTemplate(item);
        var templateReuseId = GetReuseId(template);
        var cell = collectionView.DequeueReusableCell(templateReuseId, indexPath) as UICollectionViewCell;

        if (cell is CollectionViewCell cvCell)
        {
            // TODO: Also detect layout changes
            // We don't use it here but changing the orientation of the layout would cause issues here
            if (cvCell.CellSizeController is null)
            {
                cvCell.CellSizeController = CreateCellSizeController();
            }
            cvCell.CellSizeChanged -= CellSizeChanged;
            cvCell.Index = indexPath.Row;
            cvCell.Bind(template, item, VirtualView);
            cvCell.CellSizeChanged += CellSizeChanged;
        }
        return cell;
    }

    CellSizeController CreateCellSizeController()
    {
        if (VirtualView.ItemsLayout is ItemsLayout itemsLayout)
        {
            return itemsLayout.Orientation == ItemsLayoutOrientation.Vertical ? new VerticalCellSizeController() : new HorizontaCellSizeController();
        }

        return new VerticalCellSizeController();
    }

    public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
    {
        if (elementKind == UICollectionElementKindSectionKey.Header)
        {
            var template = GetHeaderTemplate();
            var headerView = collectionView.DequeueReusableSupplementaryView(UICollectionElementKindSection.Header, headerReuseId, indexPath) as CollectionViewHeader;

            if (headerView.CellSizeController is null)
            {
                headerView.CellSizeController = CreateCellSizeController();
            }
            headerView.CellSizeChanged -= HeaderViewSizeChanged;

            headerView.Bind(template, VirtualView.Header, VirtualView);

            headerView.CellSizeChanged += HeaderViewSizeChanged;

            return headerView;
        }

        return null;
    }

    private void HeaderViewSizeChanged(object sender, View e)
    {
        var ctx = new UICollectionViewLayoutInvalidationContext();
        ctx.InvalidateSupplementaryElements(UICollectionElementKindSectionKey.Header, new NSIndexPath[] { NSIndexPath.FromIndex(0) });
        _layout.InvalidateLayout(ctx);
    }

    void CellSizeChanged(object sender, View e)
    {
        if (!(sender is CollectionViewCell cell))
        {
            return;
        }

        var visibleCells = CollectionView.VisibleCells;

        for (int n = 0; n < visibleCells.Length; n++)
        {
            if (cell == visibleCells[n])
            {
                Layout?.InvalidateLayout();
                return;
            }
        }
    }

    public void UpdateItemsSource()
    {
        ItemsSource?.Dispose();
        ItemsSource = CreateItemsViewSource();
        CollectionView.ReloadData();
        CollectionView.CollectionViewLayout.InvalidateLayout();
    }

    public void UpdateItemTemplate()
    {
        CollectionView.ReloadData();
    }

    public void UpdateLayout(CollectionViewLayout newLayout)
    {
        if (CollectionView.CollectionViewLayout == newLayout)
        {
            return;
        }
        _layout = newLayout;

        _initialized = false;

        EnsureLayoutInitialized();

        if (_initialized)
        {
            // Reload the data so the currently visible cells get laid out according to the new layout
            CollectionView.ReloadData();
        }
    }

    public void UpdateHeaderTemplate()
    {
        var ctx = new UICollectionViewLayoutInvalidationContext();
        ctx.InvalidateSupplementaryElements(UICollectionElementKindSectionKey.Header, new NSIndexPath[] { NSIndexPath.Create(0) });
        Layout.InvalidateLayout(ctx);
    }

    internal protected virtual void UpdateVisibility()
    {
        if (VirtualView.IsVisible)
        {
            if (CollectionView.Hidden)
            {
                CollectionView.ReloadData();
                CollectionView.Hidden = false;
                Layout.InvalidateLayout();
                CollectionView.LayoutIfNeeded();
            }
        }
        else
        {
            CollectionView.Hidden = true;
        }
    }

    void CheckForEmptySource()
    {
        var wasEmpty = _isEmpty;

        _isEmpty = ItemsSource.ItemCount == 0;

        if (wasEmpty != _isEmpty)
        {
            UpdateEmptyViewVisibility(_isEmpty);
        }
    }

    internal void UpdateEmptyView()
    {
        if (!_initialized)
        {
            return;
        }
        if (VirtualView.EmptyViewTemplate is not null)
        {
            // Resolve the template for the empty view
            var dt = ResolveTemplate(VirtualView.EmptyViewTemplate, VirtualView.EmptyView, VirtualView);

            // If it is different from the saved template it means we need to re-create the view
            if (_emptyViewTemplate != dt)
            {
                // Clear the previous view
                if (_emptyUIView is not null)
                {
                    _emptyUIView.RemoveFromSuperview();
                }
                _emptyView = dt.CreateContent() as View;
                VirtualView.AddLogicalChild(_emptyView);
                _emptyUIView = _emptyView.ToPlatform(VirtualView.Handler.MauiContext);
                _emptyView.BindingContext = VirtualView.EmptyView;
                _emptyViewTemplate = dt;
                _emptyViewDisplayed = false;
            }
            else
            {
                _emptyView.BindingContext = VirtualView.EmptyView;
            }
        }
        else if (VirtualView.EmptyView is View view)
        {
            if (_emptyView != view)
            {
                if (_emptyUIView is not null)
                {
                    _emptyUIView.RemoveFromSuperview();
                }
                _emptyView = view;
                VirtualView.AddLogicalChild(_emptyView);
                _emptyUIView = _emptyView.ToPlatform(VirtualView.Handler.MauiContext);
                _emptyViewDisplayed = false;
            }
        }
        else if (VirtualView.EmptyView is string text)
        {
            if (_emptyView is Label label)
            {
                label.Text = text;
            }
            else
            {
                if (_emptyUIView is not null)
                {
                    _emptyUIView.RemoveFromSuperview();
                }
                _emptyView = new Label { Text = text };
                VirtualView.AddLogicalChild(_emptyView);
                _emptyUIView = _emptyView.ToPlatform(VirtualView.Handler.MauiContext);
                _emptyViewDisplayed = false;
            }
        }
        UpdateEmptyViewVisibility(ItemsSource?.ItemCount == 0);
    }

    void UpdateEmptyViewVisibility(bool isEmpty)
    {
        if (!_initialized)
        {
            return;
        }
        if (isEmpty)
        {
            ShowEmptyView();
        }
        else
        {
            HideEmptyView();
        }
    }

    void ShowEmptyView()
    {
        if (_emptyViewDisplayed || _emptyUIView is null)
        {
            return;
        }
        CollectionView.AddSubview(_emptyUIView);
        _emptyViewDisplayed = true;

        LayoutEmptyView();
    }

    protected CGRect DetermineEmptyViewFrame()
    {
        nfloat headerHeight = _headerSize.Height;

        nfloat footerHeight = 0;
        
        return new CGRect(CollectionView.Frame.X, CollectionView.Frame.Y + headerHeight, CollectionView.Frame.Width,
            Math.Abs(CollectionView.Frame.Height - (headerHeight + footerHeight)));
    }

    void LayoutEmptyView()
    {
        if (_emptyUIView is null || _emptyView is null)
        {
            return;
        }

        _emptyUIView.Frame = DetermineEmptyViewFrame();

        _emptyView.Layout(_emptyUIView.Frame.ToRectangle());
    }

    void HideEmptyView()
    {
        if (!_emptyViewDisplayed || _emptyUIView is null)
        {
            return;
        }

        _emptyUIView.RemoveFromSuperview();

        VirtualView.RemoveLogicalChild(_emptyView);

        _emptyViewDisplayed = false;
    }

    protected virtual (bool VisibleItems, int First, int Center, int Last) GetVisibleItemsIndex()
    {
        var (VisibleItems, First, Center, Last) = GetVisibleItemsIndexPath();
        int firstVisibleItemIndex = -1, centerItemIndex = -1, lastVisibleItemIndex = -1;
        if (VisibleItems)
        {
            firstVisibleItemIndex = (int)First.Item;
            centerItemIndex = (int)Center.Item;
            lastVisibleItemIndex = (int)Last.Item;
        }
        return (VisibleItems, firstVisibleItemIndex, centerItemIndex, lastVisibleItemIndex);
    }


    protected virtual (bool VisibleItems, NSIndexPath First, NSIndexPath Center, NSIndexPath Last) GetVisibleItemsIndexPath()
    {
        var indexPathsForVisibleItems = CollectionView.IndexPathsForVisibleItems.OrderBy(x => x.Row).ToList();

        var visibleItems = indexPathsForVisibleItems.Count > 0;
        NSIndexPath firstVisibleItemIndex = null, centerItemIndex = null, lastVisibleItemIndex = null;

        if (visibleItems)
        {
            firstVisibleItemIndex = indexPathsForVisibleItems.First();
            centerItemIndex = GetCenteredIndexPath(CollectionView);
            lastVisibleItemIndex = indexPathsForVisibleItems.Last();
        }

        return (visibleItems, firstVisibleItemIndex, centerItemIndex, lastVisibleItemIndex);
    }

    static NSIndexPath GetCenteredIndexPath(UICollectionView collectionView)
    {
        NSIndexPath centerItemIndex = null;

        var indexPathsForVisibleItems = collectionView.IndexPathsForVisibleItems.OrderBy(x => x.Row).ToList();

        if (indexPathsForVisibleItems.Count == 0)
            return centerItemIndex;

        var firstVisibleItemIndex = indexPathsForVisibleItems.First();

        var centerPoint = new CGPoint(collectionView.Center.X + collectionView.ContentOffset.X, collectionView.Center.Y + collectionView.ContentOffset.Y);
        var centerIndexPath = collectionView.IndexPathForItemAtPoint(centerPoint);
        centerItemIndex = centerIndexPath ?? firstVisibleItemIndex;
        return centerItemIndex;
    }

    public override void Scrolled(UIScrollView scrollView)
    {
        var (visibleItems, firstVisibleItemIndex, centerItemIndex, lastVisibleItemIndex) = GetVisibleItemsIndex();

        if (!visibleItems)
            return;

        var contentInset = scrollView.ContentInset;
        var contentOffsetX = scrollView.ContentOffset.X + contentInset.Left;
        var contentOffsetY = scrollView.ContentOffset.Y + contentInset.Top;

        var itemsViewScrolledEventArgs = new ItemsViewScrolledEventArgs
        {
            HorizontalDelta = contentOffsetX - PreviousHorizontalOffset,
            VerticalDelta = contentOffsetY - PreviousVerticalOffset,
            HorizontalOffset = contentOffsetX,
            VerticalOffset = contentOffsetY,
            FirstVisibleItemIndex = firstVisibleItemIndex,
            CenterItemIndex = centerItemIndex,
            LastVisibleItemIndex = lastVisibleItemIndex
        };

        var source = ItemsSource;
        VirtualView.SendScrolled(itemsViewScrolledEventArgs);

        PreviousHorizontalOffset = (float)contentOffsetX;
        PreviousVerticalOffset = (float)contentOffsetY;

        switch (VirtualView.RemainingItemsThreshold)
        {
            case -1:
                return;
            case 0:
                if (lastVisibleItemIndex == source.ItemCount - 1)
                    VirtualView.SendRemainingItemsThresholdReached();
                break;
            default:
                if (source.ItemCount - 1 - lastVisibleItemIndex <= VirtualView.RemainingItemsThreshold)
                    VirtualView.SendRemainingItemsThresholdReached();
                break;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (disposing)
        {
            ItemsSource?.Dispose();

            CollectionView.Delegate = null;

            _emptyUIView?.Dispose();
            _emptyUIView = null;

            _emptyView = null;

            //ItemsViewLayout?.Dispose();
            CollectionView?.Dispose();
        }

        base.Dispose(disposing);
    }
}