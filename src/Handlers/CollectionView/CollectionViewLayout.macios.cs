using UIKit;

namespace The49.Maui.Toolkit.Handlers;

public class CollectionViewLayout : UICollectionViewCompositionalLayout
{
    public CollectionViewLayout(NSCollectionLayoutSection section) : base(section)
    {
    }

    public static CollectionViewLayout CreateGridLayout(GridItemsLayout itemsLayout)
    {
        var itemWidth = itemsLayout.Orientation == ItemsLayoutOrientation.Vertical ? NSCollectionLayoutDimension.CreateFractionalWidth(1f / itemsLayout.Span) : NSCollectionLayoutDimension.CreateEstimated(50);
        var itemHeight = itemsLayout.Orientation == ItemsLayoutOrientation.Horizontal ? NSCollectionLayoutDimension.CreateFractionalHeight(1f / itemsLayout.Span) : NSCollectionLayoutDimension.CreateEstimated(50);


        var itemSize = NSCollectionLayoutSize.Create(itemWidth, itemHeight);
        var item = NSCollectionLayoutItem.Create(itemSize);

        var groupWidth = itemsLayout.Orientation == ItemsLayoutOrientation.Vertical ? NSCollectionLayoutDimension.CreateFractionalWidth(1) : NSCollectionLayoutDimension.CreateEstimated(50);
        var groupHeight = itemsLayout.Orientation == ItemsLayoutOrientation.Horizontal ? NSCollectionLayoutDimension.CreateFractionalHeight(1) : NSCollectionLayoutDimension.CreateEstimated(50);

        var groupSize = NSCollectionLayoutSize.Create(groupWidth, groupHeight);
        var group = NSCollectionLayoutGroup.CreateHorizontal(groupSize, item, itemsLayout.Span);

        group.InterItemSpacing = NSCollectionLayoutSpacing.CreateFixed((float)itemsLayout.HorizontalItemSpacing);

        var headerSize = NSCollectionLayoutSize.Create(NSCollectionLayoutDimension.CreateFractionalWidth(1f), NSCollectionLayoutDimension.CreateEstimated(50f));
        var header = NSCollectionLayoutBoundarySupplementaryItem.Create(headerSize, UICollectionElementKindSectionKey.Header, NSRectAlignment.Top);

        var section = NSCollectionLayoutSection.Create(group);
        section.InterGroupSpacing = (float)itemsLayout.VerticalItemSpacing;
        var config = new UICollectionViewCompositionalLayoutConfiguration();

        config.BoundarySupplementaryItems = new NSCollectionLayoutBoundarySupplementaryItem[] { header };

        config.ScrollDirection = itemsLayout.Orientation == ItemsLayoutOrientation.Vertical ? UICollectionViewScrollDirection.Vertical : UICollectionViewScrollDirection.Horizontal;

        var layout = new CollectionViewLayout(section);

        layout.Configuration = config;

        return layout;
    }
    public static CollectionViewLayout CreateLinearLayout(LinearItemsLayout itemsLayout)
    {
        var itemWidth = itemsLayout.Orientation == ItemsLayoutOrientation.Vertical ? NSCollectionLayoutDimension.CreateFractionalWidth(1f) : NSCollectionLayoutDimension.CreateEstimated(50f);
        var itemHeight = itemsLayout.Orientation == ItemsLayoutOrientation.Vertical ? NSCollectionLayoutDimension.CreateEstimated(50f) : NSCollectionLayoutDimension.CreateFractionalHeight(1f);

        var itemSize = NSCollectionLayoutSize.Create(itemWidth, itemHeight);
        var item = NSCollectionLayoutItem.Create(itemSize);

        var groupSize = NSCollectionLayoutSize.Create(itemWidth, itemHeight);
        var group = NSCollectionLayoutGroup.CreateHorizontal(groupSize, item);


        var headerSize = NSCollectionLayoutSize.Create(itemWidth, itemHeight);

        var header = NSCollectionLayoutBoundarySupplementaryItem.Create(headerSize, UICollectionElementKindSectionKey.Header, itemsLayout.Orientation == ItemsLayoutOrientation.Vertical ? NSRectAlignment.Top : NSRectAlignment.Leading);

        var section = NSCollectionLayoutSection.Create(group);

        section.InterGroupSpacing = (float)itemsLayout.ItemSpacing;

        var config = new UICollectionViewCompositionalLayoutConfiguration();

        config.ScrollDirection = itemsLayout.Orientation == ItemsLayoutOrientation.Vertical ? UICollectionViewScrollDirection.Vertical : UICollectionViewScrollDirection.Horizontal;

        config.BoundarySupplementaryItems = new NSCollectionLayoutBoundarySupplementaryItem[] { header };

        var layout = new CollectionViewLayout(section);

        layout.Configuration = config;

        return layout;
    }
}
