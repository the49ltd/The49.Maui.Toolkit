using AndroidX.Fragment.App;
using Google.Android.Material.DatePicker;
using The49.Maui.Toolkit.Platform;
using MauiPlatform = Microsoft.Maui.ApplicationModel.Platform;

namespace The49.Maui.Toolkit.Dialogs;

public partial class DatePickerDialog
{
    FragmentManager SupportFragmentManager => ((MauiAppCompatActivity)MauiPlatform.CurrentActivity).SupportFragmentManager;
    public Task<DateOnly?> PlatformShowAsync()
    {
        var tcs = new TaskCompletionSource<DateOnly?>();
        var builder = MaterialDatePicker.Builder.DatePicker();
        if (CurrentDate.HasValue)
        {
            builder.SetSelection(new DateTimeOffset(CurrentDate.Value.ToDateTime(TimeOnly.MinValue)).ToUnixTimeMilliseconds());
        }
        var dialog = builder.Build();
        dialog.AddOnPositiveButtonClickListener(new MaterialDatePickerActionListener(date => tcs.SetResult(date)));
        dialog.AddOnNegativeButtonClickListener(new ViewClickListener(() => tcs.SetResult(null)));
        dialog.Show(SupportFragmentManager, dialog.ToString());

        return tcs.Task;
    }
}
