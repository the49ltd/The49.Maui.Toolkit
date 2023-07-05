namespace The49.Maui.Toolkit.Dialogs;

public partial class DatePickerDialog
{
    public DateOnly? CurrentDate { get; set; }
    public Task<DateOnly?> ShowAsync()
    {
        return PlatformShowAsync();
    }
    public class Builder
    {
        DatePickerDialog _dialog;

        public Builder()
        {
            _dialog = new DatePickerDialog();
        }

        public static Builder Create()
        {
            return new Builder();
        }

        public DatePickerDialog Build()
        {
            return _dialog;
        }

        public Builder SetDate(DateOnly date)
        {
            _dialog.CurrentDate = date;
            return this;
        }
    }
}
