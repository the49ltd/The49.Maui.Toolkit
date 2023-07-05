using UIKit;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;
using CollectionView = The49.Maui.Toolkit.Views.CollectionView;

namespace The49.Maui.Toolkit.Handlers;

public class CollectionViewCell : UICollectionViewCell
{
    protected View _view;
    protected DataTemplate _template;
    protected Size _calculatedSize;
    protected CollectionView _virtualView;
    private nfloat _constraint;

    public int Index { get; set; }

    public event EventHandler<View> CellSizeChanged;

    public CellSizeController CellSizeController { get; set; }

    [Export("initWithFrame:")]
    [Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
    public CollectionViewCell(CGRect frame) : base(frame)
    {
        ContentView.BackgroundColor = UIColor.Clear;
    }

    protected void InitializeContentConstraints(UIView platformView)
    {
        ContentView.TranslatesAutoresizingMaskIntoConstraints = false;
        platformView.TranslatesAutoresizingMaskIntoConstraints = false;

        ContentView.AddSubview(platformView);

        // We want the cell to be the same size as the ContentView
        ContentView.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
        ContentView.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
        ContentView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor).Active = true;
        ContentView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor).Active = true;

        // And we want the ContentView to be the same size as the root renderer for the Forms element
        ContentView.TopAnchor.ConstraintEqualTo(platformView.TopAnchor, -(float)_view.Margin.Top).Active = true;
        ContentView.BottomAnchor.ConstraintEqualTo(platformView.BottomAnchor, (float)_view.Margin.Bottom).Active = true;
        ContentView.LeadingAnchor.ConstraintEqualTo(platformView.LeadingAnchor, -(float)_view.Margin.Left).Active = true;
        ContentView.TrailingAnchor.ConstraintEqualTo(platformView.TrailingAnchor, (float)_view.Margin.Right).Active = true;
    }

    public virtual void Bind(DataTemplate template, object data, CollectionView itemsView)
    {
        _virtualView = itemsView;
        if (template is null)
        {
            return;
        }
        if (_template == template)
        {
            if (_view.BindingContext != data)
            {
                _view.MeasureInvalidated -= OnMeasureInvalidated;
                _view.BindingContext = data;
                _view.MeasureInvalidated += OnMeasureInvalidated;
                UpdateCellSize();
            }
        }
        else
        {
            if (_view is not null)
            {
                _view.MeasureInvalidated -= OnMeasureInvalidated;
                _view.BindingContext = null;
                itemsView.RemoveLogicalChild(_view);
                ContentView.ClearSubviews();
                _calculatedSize = Size.Zero;
            }
            _template = template;
            _view = (View)_template.CreateContent();
            _view.BindingContext = data;

            var platformView = _view.ToPlatform(itemsView.Handler.MauiContext);

            InitializeContentConstraints(platformView);

            itemsView.AddLogicalChild(_view);

            _view.MeasureInvalidated += OnMeasureInvalidated;
        }
    }

    protected virtual void OnMeasureInvalidated(object sender, EventArgs e)
    {
        var (needsUpdate, toSize) = NeedsContentSizeUpdate(_calculatedSize);

        if (!needsUpdate)
        {
            return;
        }

        // Cache the size for next time
        _calculatedSize = toSize;

        CellSizeChanged?.Invoke(this, _view);
    }

    protected (bool, Size) NeedsContentSizeUpdate(Size currentSize)
    {
        var size = Size.Zero;

        if (_view == null)
        {
            return (false, size);
        }

        var bounds = _view.Frame;

        if (bounds.Width < 0 || bounds.Height < 0)
        {
            return (false, currentSize);
        }

        var desiredBounds = CellSizeController.Measure(_view);

        if (CellSizeController.IsSizeTheSame(currentSize, desiredBounds))
        {
            // Nothing in the cell needs more room, so leave it as it is
            return (false, size);
        }

        return (true, desiredBounds);
    }

    public override UICollectionViewLayoutAttributes PreferredLayoutAttributesFittingAttributes(UICollectionViewLayoutAttributes layoutAttributes)
    {
        CellSizeController.SetConstraint(layoutAttributes.Frame.Size);
        if (_view is null)
        {
            layoutAttributes.Frame = new CGRect(layoutAttributes.Frame.Location, CellSizeController.Measure(_view));
            return layoutAttributes;
        }
        if (_calculatedSize.IsZero)
        {
            UpdateCellSize();
        }
        layoutAttributes.Frame = new CGRect(layoutAttributes.Frame.Location, _calculatedSize.ToCGSize());
        return layoutAttributes;
    }

    CGSize UpdateCellSize()
    {
        _calculatedSize = CellSizeController.Measure(_view);
        var platformView = (UIView)_view.Handler.PlatformView;
        platformView.Frame = new CGRect(new CGPoint(_view.Margin.Left, _view.Margin.Top), new Size(_calculatedSize.Width - _view.Margin.HorizontalThickness, _calculatedSize.Height - _view.Margin.VerticalThickness));

        // Layout the Maui element 
        var nativeBounds = platformView.Frame.ToRectangle();
        _view.Arrange(nativeBounds);

        return nativeBounds.Size;
    }
}

public abstract class CellSizeController
{
    internal nfloat ConstrainedDimension { get; set; }

    internal abstract bool IsSizeTheSame(Size original, Size candidate);

    internal abstract void SetConstraint(CGSize size);

    internal abstract Size Measure(View view);
}

public class VerticalCellSizeController : CellSizeController
{
    internal override bool IsSizeTheSame(Size original, Size candidate)
    {
        return original.Height == candidate.Height;
    }

    internal override Size Measure(View view)
    {
        if (view is null)
        {
            return new Size(ConstrainedDimension, 0);
        }
        var d = view.Handler.GetDesiredSize(ConstrainedDimension - view.Margin.VerticalThickness, double.PositiveInfinity);
        return new Size(ConstrainedDimension, d.Height);
    }

    internal override void SetConstraint(CGSize size)
    {
        ConstrainedDimension = size.Width;
    }
}

public class HorizontaCellSizeController : CellSizeController
{
    internal override bool IsSizeTheSame(Size original, Size candidate)
    {
        return original.Width == candidate.Width;
    }

    internal override Size Measure(View view)
    {
        if (view is null)
        {
            return new Size(0, ConstrainedDimension);
        }
        var d = view.Handler.GetDesiredSize(double.PositiveInfinity, ConstrainedDimension - view.Margin.HorizontalThickness);
        return new Size(d.Width, ConstrainedDimension);
    }

    internal override void SetConstraint(CGSize size)
    {
        ConstrainedDimension = size.Height;
    }
}

public class CollectionViewHeader : CollectionViewCell
{
    [Export("initWithFrame:")]
    [Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
    public CollectionViewHeader(CGRect frame) : base(frame)
    { }

    public override UICollectionViewLayoutAttributes PreferredLayoutAttributesFittingAttributes(UICollectionViewLayoutAttributes layoutAttributes)
    {
        var attrs = base.PreferredLayoutAttributesFittingAttributes(layoutAttributes);

        ((CollectionViewHandler)_virtualView.Handler).Controller.SetHeaderSize(attrs.Frame.Size);

        return attrs;
    }
}

public class CollectionViewFooter : CollectionViewCell
{
    [Export("initWithFrame:")]
    [Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
    public CollectionViewFooter(CGRect frame) : base(frame)
    {
    }

    public override UICollectionViewLayoutAttributes PreferredLayoutAttributesFittingAttributes(UICollectionViewLayoutAttributes layoutAttributes)
    {
        var attrs = base.PreferredLayoutAttributesFittingAttributes(layoutAttributes);

        ((CollectionViewHandler)_virtualView.Handler).Controller.SetFooterSize(attrs.Frame.Size);

        return attrs;
    }
}

public class CollectionViewSectionHeader : CollectionViewCell
{
    [Export("initWithFrame:")]
    [Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
    public CollectionViewSectionHeader(CGRect frame) : base(frame)
    { }

}
public class CollectionViewSectionFooter : CollectionViewCell
{
    [Export("initWithFrame:")]
    [Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
    public CollectionViewSectionFooter(CGRect frame) : base(frame)
    { }

}