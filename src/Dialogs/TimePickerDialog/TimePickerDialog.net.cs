namespace The49.Maui.Toolkit.Dialogs;

public partial class TimePickerDialog
{
    public Task<(int, int)> PlatformShowAsync()
    {
        return Task.FromResult((-1, -1));
    }
}
