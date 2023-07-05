namespace The49.Maui.Toolkit.Dialogs;

public partial class DatePickerDialog
{
    public Task<DateOnly?> PlatformShowAsync()
    {
        return Task.FromResult(null as DateOnly?);
    }
}
