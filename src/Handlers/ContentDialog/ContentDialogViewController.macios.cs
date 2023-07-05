using Microsoft.Maui.Platform;
using The49.Maui.Toolkit.Dialogs;
using UIKit;

namespace The49.Maui.Core.Dialog.Handlers;

public class ContentDialogViewController : UIViewController
{
    ContentDialog _dialog;

    public ContentDialogViewController(ContentDialog dialog)
    {
        _dialog = dialog;
    }

    public ContentDialogViewController(IntPtr handle) : base(handle)
    {
    }
    public override void LoadView()
    {
        base.LoadView();

        var platformView = _dialog.ToPlatform(Application.Current.Windows[0].Handler.MauiContext);

        View.AddSubview(platformView);

        platformView.TranslatesAutoresizingMaskIntoConstraints = false;
        platformView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
        platformView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
        platformView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
        platformView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
    }
    async internal Task DismissAsync(bool animate)
    {
        await _dialog.Controller?.PresentingViewController.DismissViewControllerAsync(animate);
        _dialog.NotifyDismissed();
    }
}

