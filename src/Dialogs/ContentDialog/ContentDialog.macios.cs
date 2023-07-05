using The49.Maui.Core.Dialog.Handlers;
using UIKit;
using MauiPlatform = Microsoft.Maui.ApplicationModel.Platform;

namespace The49.Maui.Toolkit.Dialogs;

public partial class ContentDialog
{
    public ContentDialogViewController Controller { get; internal set; }

    async public Task PlatformShowAsync()
    {
        Parent = Application.Current.MainPage;

        var ctrl = new ContentDialogViewController(this);

        Controller = ctrl;

        var d = new ContentDialogTransitioningDelegate(this);

        d.OutsideTap += OnOutsideTap;

        ctrl.TransitioningDelegate = d;

        ctrl.ModalPresentationStyle = UIModalPresentationStyle.Custom;
        ctrl.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;

        var vc = MauiPlatform.GetCurrentUIViewController();

        await vc.PresentViewControllerAsync(ctrl, true);

        void OnOutsideTap(object sender, EventArgs e)
        {
            Controller.DismissAsync(true).ConfigureAwait(false);
        }
    }
}
