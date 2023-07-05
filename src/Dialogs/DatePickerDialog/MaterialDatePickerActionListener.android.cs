using Google.Android.Material.DatePicker;

namespace The49.Maui.Toolkit.Dialogs;

public class MaterialDatePickerActionListener : Java.Lang.Object, IMaterialPickerOnPositiveButtonClickListener
{
    Action<DateOnly> _action;
    public MaterialDatePickerActionListener(Action<DateOnly> action)
    {
        _action = action;
    }
    public void OnPositiveButtonClick(Java.Lang.Object p0)
    {
        var to = DateTimeOffset.FromUnixTimeMilliseconds((long)p0);
        _action(DateOnly.FromDateTime(to.DateTime));
    }
}
