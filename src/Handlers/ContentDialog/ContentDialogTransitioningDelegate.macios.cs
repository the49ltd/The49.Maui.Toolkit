using UIKit;
using The49.Maui.Toolkit.Dialogs;

namespace The49.Maui.Core.Dialog.Handlers;

internal class ContentDialogTransitioningDelegate : UIViewControllerTransitioningDelegate
{
    ContentDialog _dialog;

    public event EventHandler OutsideTap;

    public ContentDialogTransitioningDelegate(ContentDialog page) : base()
    {
        _dialog = page;
    }
    public override UIPresentationController GetPresentationControllerForPresentedViewController(UIViewController presentedViewController, UIViewController presentingViewController, UIViewController sourceViewController)
    {
        var ctrl = new ContentDialogPresentationController(_dialog, presentedViewController, presentingViewController);
        ctrl.OutsideTap += OnOutsideTap;
        return ctrl;
    }

    void OnOutsideTap(object sender, EventArgs e)
    {
        OutsideTap?.Invoke(this, EventArgs.Empty);
    }
}

