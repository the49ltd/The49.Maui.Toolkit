using Google.Android.Material.TimePicker;

namespace The49.Maui.Toolkit.Dialogs;

public class MaterialTimePickerActionListener : Java.Lang.Object, Android.Views.View.IOnClickListener
{
    MaterialTimePicker _picker;
    Action<int, int> _action;
    public MaterialTimePickerActionListener(MaterialTimePicker picker, Action<int, int> action)
    {
        _picker = picker;
        _action = action;
    }

    public void OnClick(Android.Views.View v)
    {
        _action(_picker.Hour, _picker.Minute);
    }
}