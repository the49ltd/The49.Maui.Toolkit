using Foundation;
using UIKit;

namespace The49.Maui.Toolkit.Dialogs;

public class DatePickerDialogViewController : UIViewController
{
    DateOnly _defaultValue;
    UIDatePicker _picker;

    public UIDatePicker Picker => _picker;

    public event EventHandler Canceled;
    public event EventHandler<DateOnly> Selected;

    public DatePickerDialogViewController(DateOnly defaultValue) : base()
    {
        _defaultValue = defaultValue;

        _picker = new UIDatePicker();
        if (OperatingSystem.IsIOSVersionAtLeast(13))
        {
            ModalInPresentation = true;
        }
        if (OperatingSystem.IsIOSVersionAtLeast(15))
        {
            SheetPresentationController.Detents = new UISheetPresentationControllerDetent[] {
                UISheetPresentationControllerDetent.CreateMediumDetent()
            };
        }
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        View.BackgroundColor = UIColor.White;

        var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) =>
        {
            DismissViewController(true, null);
            Selected?.Invoke(this, DateOnly.FromDateTime((DateTime)_picker.Date));
        });
        var spaceButton = new UIBarButtonItem(systemItem: UIBarButtonSystemItem.FlexibleSpace);
        var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (s, e) =>
        {
            Canceled?.Invoke(this, EventArgs.Empty);
            DismissViewController(true, null);
        });

        if (OperatingSystem.IsIOSVersionAtLeast(13, 4))
        {
            _picker.PreferredDatePickerStyle = UIDatePickerStyle.Inline;
        }

        _picker.BackgroundColor = UIColor.White;
        _picker.Mode = UIDatePickerMode.Date;
        _picker.SetDate((NSDate)_defaultValue.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc), false);

        var toolbar = new UIToolbar();
        toolbar.UserInteractionEnabled = true;
        View.AddSubview(toolbar);

        toolbar.TranslatesAutoresizingMaskIntoConstraints = false;

        toolbar.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
        toolbar.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
        toolbar.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor).Active = true;

        toolbar.SetItems(new UIBarButtonItem[] { cancelButton, spaceButton, doneButton }, false);

        View.AddSubview(_picker);

        _picker.TranslatesAutoresizingMaskIntoConstraints = false;
        _picker.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
        _picker.TopAnchor.ConstraintEqualTo(toolbar.BottomAnchor).Active = true;
    }
}
