using Android.Graphics.Drawables;
using Android.Util;
using Google.Android.Material.Dialog;
using MauiPlatform = Microsoft.Maui.ApplicationModel.Platform;

namespace The49.Maui.Toolkit.Dialogs;

public partial class AlertDialog
{
    static Task<Drawable> GetDrawable(ImageSource img)
    {
        var tcs = new TaskCompletionSource<Drawable>();

        var app = MauiPlatform.CurrentActivity.Application as IPlatformApplication;

        // TODO: Find a better way to get the context
        img.LoadImage(Application.Current.Handler.MauiContext, result =>
        {
            var drawable = result.Value;
            TypedValue typedValue = new TypedValue();
            var theme = MauiPlatform.CurrentActivity.Theme;
            theme.ResolveAttribute(Android.Resource.Attribute.ColorSecondary, typedValue, true);
            int color = typedValue.Data;
            drawable.SetTint(color);
            tcs.SetResult(drawable);
        });

        return tcs.Task;
    }

    protected async Task<MaterialAlertDialogBuilder> CreateAlert(TaskCompletionSource<bool> tcs)
    {
        var b = new MaterialAlertDialogBuilder(MauiPlatform.CurrentActivity, Resource.Style.ThemeOverlay_Material3_MaterialAlertDialog_Centered);

        b
            .SetTitle(Title)
            .SetMessage(Message)
            .SetPositiveButton(ActionText, delegate
            {
                tcs.SetResult(true);
            });

        if (Icon != null)
        {
            b.SetIcon(await GetDrawable(Icon));
        }

        return b;
    }
    async protected virtual Task PlatformShow()
    {
        var tcs  = new TaskCompletionSource<bool>();
        var b = await CreateAlert(tcs);

        var dialog = b.Create();
        dialog.CancelEvent += (_, _) => tcs.SetResult(false);
        dialog.Show();

        await tcs.Task;
    }
}
