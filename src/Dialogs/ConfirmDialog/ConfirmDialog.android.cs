namespace The49.Maui.Toolkit.Dialogs;

public partial class ConfirmDialog
{
    async protected override Task<bool> PlatformShow()
    {
        var tcs = new TaskCompletionSource<bool>();
        var b = await CreateAlert(tcs);

        b.SetNegativeButton(CancelText, delegate
        {
            tcs.SetResult(false);
        });

        var dialog = b.Create();
        dialog.CancelEvent += (_, _) => tcs.SetResult(false);
        dialog.Show();

        return await tcs.Task;
    }
}
