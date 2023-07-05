namespace The49.Maui.Toolkit.Dialogs;

public partial class AlertDialog
{
    public ImageSource Icon { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string ActionText { get; set; } = "Ok";

    public virtual Task ShowAsync()
    {
        return PlatformShow();
    }

    public class Builder
    {
        AlertDialog _dialog;

        public Builder()
        {
            _dialog = new AlertDialog();
        }

        public static Builder Create(string title, string message)
        {
            var builder = new Builder();
            builder._dialog.Title = title;
            builder._dialog.Message = message;

            return builder;
        }

        public AlertDialog Build()
        {
            return _dialog;
        }

        public Builder SetActionText(string actionText)
        {
            _dialog.ActionText = actionText;
            return this;
        }

        public Builder SetIcon(ImageSource icon)
        {
            _dialog.Icon = icon;
            return this;
        }
    }
}
