using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;

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

        var pv = view.Handler.PlatformView as MauiTextField;

        var s = fs.ToNSAttributedString(fontManager);

        pv.AttributedText = s;
    }
}
