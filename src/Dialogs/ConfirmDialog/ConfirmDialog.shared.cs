namespace The49.Maui.Toolkit.Dialogs;

public partial class ConfirmDialog: AlertDialog
{
    public string CancelText { get; set; } = "Cancel";
    public bool IsDestructive { get; set; }

    public override Task<bool> ShowAsync()
    {
        return PlatformShow();
    }

    public new class Builder
    {
        ConfirmDialog _dialog;

        public Builder()
        {
            _dialog = new ConfirmDialog();
        }

        public static Builder Create(string title, string message)
        {
            var builder = new Builder();
            builder._dialog.Title = title;
            builder._dialog.Message = message;

            return builder;
        }

        public ConfirmDialog Build()
        {
            return _dialog;
        }

        public Builder SetActionText(string actionText, bool isDestructive = false)
        {
            _dialog.ActionText = actionText;
            _dialog.IsDestructive = isDestructive;
            return this;
        }

        public Builder SetCancelText(string cancelText)
        {
            _dialog.CancelText = cancelText;
            return this;
        }

        public Builder SetIcon(ImageSource icon)
        {
            _dialog.Icon = icon;
            return this;
        }
    }
}
