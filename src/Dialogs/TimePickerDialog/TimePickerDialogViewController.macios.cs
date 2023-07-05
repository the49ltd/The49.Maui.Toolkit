using Foundation;
using UIKit;

namespace The49.Maui.Toolkit.Dialogs;

public class TimePickerDialogViewController : UIViewController
{
    int _hour;
    int _minute;
    UIDatePicker _picker;

    public UIDatePicker Picker => _picker;

    public event EventHandler Canceled;
    public event EventHandler<(int, int)> Selected;

    public TimePickerDialogViewController(int hour, int minute) : base()
    {
        _hour = hour;
        _minute = minute;

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
            // Date gets updated to Utc internally, re-convert to local to ensure we get the value the user entered
            var d = TimeZoneInfo.ConvertTimeFromUtc((DateTime)_picker.Date, TimeZoneInfo.Local);
            Selected?.Invoke(this, (d.Hour, d.Minute));
        });
        var space = new UIBarButtonItem(systemItem: UIBarButtonSystemItem.FlexibleSpace);
        var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (s, e) =>
        {
            Canceled?.Invoke(this, EventArgs.Empty);
            DismissViewController(true, null);
        });
        if (OperatingSystem.IsIOSVersionAtLeast(13, 4))
        {
            _picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
        }

        _picker.BackgroundColor = UIColor.White;
        _picker.Mode = UIDatePickerMode.Time;
        _picker.SetDate((NSDate)(new DateTime(1970, 1, 1, _hour, _minute, 0, DateTimeKind.Local)), false);

        var toolbar = new UIToolbar();
        toolbar.UserInteractionEnabled = true;
        View.AddSubview(toolbar);

        toolbar.TranslatesAutoresizingMaskIntoConstraints = false;

        toolbar.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
        toolbar.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
        toolbar.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor).Active = true;

        toolbar.SetItems(new UIBarButtonItem[]
        {
            cancelButton,
            space,
            doneButton
        }, false);

        View.AddSubview(_picker);

        _picker.TranslatesAutoresizingMaskIntoConstraints = false;
        _picker.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
        _picker.TopAnchor.ConstraintEqualTo(toolbar.BottomAnchor).Active = true;
    }
}
