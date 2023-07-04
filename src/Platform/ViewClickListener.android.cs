using AView = Android.Views.View;

namespace The49.Maui.Toolkit.Platform;

public class ViewClickListener : Java.Lang.Object, AView.IOnClickListener
{
    Action _action;

    public ViewClickListener(Action action)
    {
        _action = action;
    }
    public void OnClick(AView v)
    {
        _action();
    }
}
