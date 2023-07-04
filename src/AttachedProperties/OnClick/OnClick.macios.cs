using CoreFoundation;
using CoreGraphics;
using UIKit;

namespace The49.Maui.Toolkit;

class HighlightedTapGestureRecognizer : UITapGestureRecognizer
{
	public HighlightedTapGestureRecognizer(Action action) : base(action)
	{

	}
	internal static void TriggerHighlight(UIView view)
	{
		SetHighlightedForAllSubviews(view, true);

		DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, TimeSpan.FromMilliseconds(200)), () =>
		{
			SetHighlightedForAllSubviews(view, false);
		});
	}
	static void SetHighlightedForAllSubviews(UIView view, bool isHighlighted)
	{
		foreach (var subview in view.Subviews)
		{
			if (subview is UIControl control)
			{
				control.Highlighted = isHighlighted;
			}
			else if (subview is UILabel label)
			{
				label.Highlighted = isHighlighted;
				label.HighlightedTextColor = label.TextColor.ColorWithAlpha(.5f);
			}
			else if (subview is UIImageView image)
			{
				image.Highlighted = isHighlighted;
				if (image.HighlightedImage is null && image.Image is not null)
				{
					image.HighlightedImage = ImageWithAlpha(image.Image, .5f);
				}
			}
			SetHighlightedForAllSubviews(subview, isHighlighted);
		}
	}
	static UIImage ImageWithAlpha(UIImage image, float alpha)
	{
		UIGraphics.BeginImageContextWithOptions(image.Size, false, image.CurrentScale);
		image.Draw(CGPoint.Empty, blendMode: CGBlendMode.Normal, alpha: alpha);
		var newImage = UIGraphics.GetImageFromCurrentImageContext();
		UIGraphics.EndImageContext();
		return newImage;
	}
}


public partial class OnClick
{
	public static readonly BindableProperty HighlightedTapGestureRecognizerProperty =
		BindableProperty.CreateAttached(nameof(HighlightedTapGestureRecognizer), typeof(UIGestureRecognizer), typeof(VisualElement), null);

	static partial void PlatformSetupClickListener(VisualElement visualElement)
	{
		if (visualElement.Handler.PlatformView is UIView view)
		{
			var oldListener = (UIGestureRecognizer)visualElement.GetValue(HighlightedTapGestureRecognizerProperty);
			if (oldListener != null)
			{
				return;
			}

			var listener = new HighlightedTapGestureRecognizer(() =>
			{
				HighlightedTapGestureRecognizer.TriggerHighlight(view);
				TriggerClick(visualElement);
			});
			visualElement.SetValue(HighlightedTapGestureRecognizerProperty, listener);
			view.AddGestureRecognizer(listener);
		}
	}
}