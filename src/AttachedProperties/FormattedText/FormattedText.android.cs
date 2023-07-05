using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Platform;
using Android.Widget;

namespace The49.Maui.Toolkit;
public partial class FormattedText
{
    static partial void PlatformUpdateAttributedText(View view)
    {
        IFontManager fontManager = view.Handler.MauiContext.Services.GetService<IFontManager>();

        var input = view as ITextInput;

        var fs = GetFormattedText(view);

        if (fs == null)
        {
            return;
        }

        var pv = view.Handler.PlatformView as AppCompatEditText;

        var s = fs.ToSpannableString(fontManager);

        var cursorPosition = pv.SelectionStart;

        pv.SetText(s, TextView.BufferType.Spannable);

        pv.SetSelection(cursorPosition);
    }
}
