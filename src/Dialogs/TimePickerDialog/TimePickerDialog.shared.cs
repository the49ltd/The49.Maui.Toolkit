namespace The49.Maui.Toolkit.Dialogs;

public partial class TimePickerDialog
{
    public int CurrentHours { get; set; }
    public int CurrentMinutes { get; set; }
    public Task<(int, int)> ShowAsync()
    {
        return PlatformShowAsync();
    }
    public class Builder
    {
        TimePickerDialog _dialog;

        public Builder()
        {
            _dialog = new TimePickerDialog();
        }

        public static Builder Create()
        {
            return new Builder();
        }

        public TimePickerDialog Build()
        {
            return _dialog;
        }

        public Builder SetTime(int hours, int minutes)
        {
            _dialog.CurrentHours = hours;
            _dialog.CurrentMinutes = minutes;
            return this;
        }
    }
}
