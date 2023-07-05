using AndroidX.Fragment.App;
using Google.Android.Material.TimePicker;
using The49.Maui.Toolkit.Platform;
using MauiPlatform = Microsoft.Maui.ApplicationModel.Platform;

namespace The49.Maui.Toolkit.Dialogs;

public partial class TimePickerDialog
{
    FragmentManager SupportFragmentManager => ((MauiAppCompatActivity)MauiPlatform.CurrentActivity).SupportFragmentManager;
    public Task<(int, int)> PlatformShowAsync()
    {
        var tcs = new TaskCompletionSource<(int, int)>();
        var builder = new MaterialTimePicker.Builder();
        if (CurrentHours != -1)
        {
            builder.SetHour(CurrentHours);
        }
        if (CurrentMinutes != -1)
        {
            builder.SetMinute(CurrentMinutes);
        }
        var dialog = builder.Build();
        dialog.AddOnPositiveButtonClickListener(new MaterialTimePickerActionListener(dialog, (hours, minutes) => tcs.SetResult((hours, minutes))));
        dialog.AddOnNegativeButtonClickListener(new ViewClickListener(() => tcs.SetResult((-1, -1))));
        dialog.Show(SupportFragmentManager, dialog.ToString());

        return tcs.Task;
    }
}
