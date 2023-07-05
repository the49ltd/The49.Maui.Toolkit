using CoreGraphics;
using UIKit;
using The49.Maui.Toolkit.Dialogs;

namespace The49.Maui.Core.Dialog.Handlers;

internal class ContentDialogPresentationController : UIPresentationController
{
    ContentDialog _dialog;

    UIView _backdrop;

    public event EventHandler OutsideTap;

    public ContentDialogPresentationController(ContentDialog dialog, UIViewController presentedViewController, UIViewController presentingViewController) : base(presentedViewController, presentingViewController)
    {
        _dialog = dialog;
        SetupBackdrop();
    }

    void SetupBackdrop()
    {
        _backdrop = new UIView();
        _backdrop.BackgroundColor = new UIColor(0f, .5f);
        _backdrop.Alpha = 0f;
        _backdrop.TranslatesAutoresizingMaskIntoConstraints = false;

        var g = new UITapGestureRecognizer(() =>
        {
            OutsideTap?.Invoke(this, EventArgs.Empty);
        });

        _backdrop.AddGestureRecognizer(g);
    }

    public override void ContainerViewDidLayoutSubviews()
    {
        base.ContainerViewDidLayoutSubviews();
        PresentedView.Frame = FrameOfPresentedViewInContainerView;
    }

    public override CGRect FrameOfPresentedViewInContainerView
    {
        get
        {
            var r = _dialog.Measure(ContainerView.Bounds.Width, ContainerView.Bounds.Height, MeasureFlags.IncludeMargins);
            var x = (ContainerView.Bounds.Width - r.Request.Width) / 2;
            var y = (ContainerView.Bounds.Height - r.Request.Height) / 2;
            return new CGRect(x, y, r.Request.Width, r.Request.Height);
        }
    }

    public override void PresentationTransitionWillBegin()
    {
        ContainerView.InsertSubview(_backdrop, 0);

        NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[]
        {
            _backdrop.TopAnchor.ConstraintEqualTo(ContainerView.TopAnchor),
            _backdrop.BottomAnchor.ConstraintEqualTo(ContainerView.BottomAnchor),
            _backdrop.LeadingAnchor.ConstraintEqualTo(ContainerView.LeadingAnchor),
            _backdrop.TrailingAnchor.ConstraintEqualTo(ContainerView.TrailingAnchor),
        });


        var coordinator = PresentedViewController.GetTransitionCoordinator();
        if (coordinator is null)
        {
            _backdrop.Alpha = 1f;
        }
        else
        {
            coordinator.AnimateAlongsideTransition(_ =>
            {
                _backdrop.Alpha = 1f;
            }, delegate { });
        }
    }

    public override void DismissalTransitionWillBegin()
    {
        var coordinator = PresentedViewController.GetTransitionCoordinator();
        if (coordinator is null)
        {
            _backdrop.Alpha = 0f;
        }
        else
        {
            coordinator.AnimateAlongsideTransition(_ =>
            {
                _backdrop.Alpha = 0f;
            }, delegate { });
        }
    }
}

