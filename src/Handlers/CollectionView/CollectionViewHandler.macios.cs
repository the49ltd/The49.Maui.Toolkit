using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;
using CollectionView = The49.Maui.Toolkit.Views.CollectionView;

namespace The49.Maui.Toolkit.Handlers;

public partial class CollectionViewHandler : ViewHandler<CollectionView, UIView>
{
    Lazy<CollectionViewController> _ctrl;
    CollectionViewLayout _layout;

    public CollectionViewController Controller => _ctrl.Value;
    public CollectionViewHandler(PropertyMapper mapper = null) : base(mapper ?? ItemsViewMapper)
    {

    }

    public CollectionViewHandler() : base(ItemsViewMapper)
    {
        _ctrl = new Lazy<CollectionViewController>(() =>
        {
            _layout = SelectLayout();
            return new CollectionViewController(VirtualView, _layout);
        });
    }

    public static PropertyMapper<CollectionView, CollectionViewHandler> ItemsViewMapper = new PropertyMapper<CollectionView, CollectionViewHandler>(ViewHandler.ViewMapper)
    {
        [CollectionView.ItemsSourceProperty.PropertyName] = MapItemsSource,
        [CollectionView.HorizontalScrollBarVisibilityProperty.PropertyName] = MapHorizontalScrollBarVisibility,
        [CollectionView.VerticalScrollBarVisibilityProperty.PropertyName] = MapVerticalScrollBarVisibility,
        [CollectionView.ItemTemplateProperty.PropertyName] = MapItemTemplate,
        [CollectionView.EmptyViewProperty.PropertyName] = MapEmptyView,
        [CollectionView.EmptyViewTemplateProperty.PropertyName] = MapEmptyViewTemplate,
        [CollectionView.FlowDirectionProperty.PropertyName] = MapFlowDirection,
        [CollectionView.IsVisibleProperty.PropertyName] = MapIsVisible,
        //[CollectionView.ItemsUpdatingScrollModeProperty.PropertyName] = MapItemsUpdatingScrollMode,
        [CollectionView.HeaderTemplateProperty.PropertyName] = MapHeaderTemplate,
        [CollectionView.FooterTemplateProperty.PropertyName] = MapFooterTemplate,
        [CollectionView.ItemsLayoutProperty.PropertyName] = MapItemsLayout,
        //[CollectionView.ItemSizingStrategyProperty.PropertyName] = MapItemSizingStrategy
    };

    static void MapFooterTemplate(CollectionViewHandler handler, CollectionView view)
    {
        // TODO: cofigure the footer
    }

    static void MapFooter(CollectionViewHandler handler, CollectionView view)
    {
        // TODO: Update the data in header
    }

    static void MapHeaderTemplate(CollectionViewHandler handler, CollectionView view)
    {
        handler.Controller?.UpdateHeaderTemplate();
    }

    static void MapHeader(CollectionViewHandler handler, CollectionView view)
    {
        // TODO: Update the data in header
    }

    protected override UIView CreatePlatformView() => Controller?.View;

    protected virtual void UpdateLayout()
    {
        _layout = SelectLayout();
        Controller?.UpdateLayout(_layout);
    }

    CollectionViewLayout SelectLayout()
    {
        var itemsLayout = VirtualView.ItemsLayout;

        if (itemsLayout is GridItemsLayout gridItemsLayout)
        {
            return CollectionViewLayout.CreateGridLayout(gridItemsLayout);
        }

        if (itemsLayout is LinearItemsLayout listItemsLayout)
        {
            return CollectionViewLayout.CreateLinearLayout(listItemsLayout);
        }

        // Fall back to vertical list
        return CollectionViewLayout.CreateLinearLayout(new LinearItemsLayout(ItemsLayoutOrientation.Vertical));
    }

    static void MapItemsSource(CollectionViewHandler handler, CollectionView itemsView) => handler.Controller?.UpdateItemsSource();
    static void MapItemTemplate(CollectionViewHandler handler, CollectionView itemsView) => handler.Controller?.UpdateItemTemplate();
    static void MapIsVisible(CollectionViewHandler handler, CollectionView itemsView) => handler.Controller?.UpdateVisibility();
    static void MapEmptyViewTemplate(CollectionViewHandler handler, CollectionView view) => handler.Controller?.UpdateEmptyView();
    static void MapEmptyView(CollectionViewHandler handler, CollectionView view) => handler.Controller?.UpdateEmptyView();
    static void MapItemsLayout(CollectionViewHandler handler, CollectionView view) => handler.UpdateLayout();
    static void MapHorizontalScrollBarVisibility(CollectionViewHandler handler, CollectionView itemsView) => handler.Controller?.CollectionView?.UpdateHorizontalScrollBarVisibility(itemsView.HorizontalScrollBarVisibility);
    static void MapVerticalScrollBarVisibility(CollectionViewHandler handler, CollectionView itemsView) => handler.Controller?.CollectionView?.UpdateVerticalScrollBarVisibility(itemsView.VerticalScrollBarVisibility);

    protected override void ConnectHandler(UIView platformView)
    {
        base.ConnectHandler(platformView);
        Controller.CollectionView.BackgroundColor = UIColor.Clear;
    }
}
