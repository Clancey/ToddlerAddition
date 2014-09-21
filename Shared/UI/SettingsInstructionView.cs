using System;
using UIKit;
using Foundation;
using CoreText;

namespace ToddlerAddition
{
	public class SettingsInstructionView : UIView
	{
		public UILabel topLabel;
		public SettingsInstructionView ()
		{
			Layer.BorderColor = UIColor.LightGray.CGColor;
			Layer.CornerRadius = 10f;
			Layer.BorderWidth = 1f;
			BackgroundColor = UIColor.LightGray.ColorWithAlpha (.1f);

			Add(topLabel = new UILabel ());

			var firstAttributes = new UIStringAttributes {
				ForegroundColor = UIColor.Blue,
				BackgroundColor = UIColor.Yellow,
				Font = UIFont.FromName("Courier", 18f)
			};

			var secondAttributes = new UIStringAttributes {
				ForegroundColor = UIColor.Red,
				BackgroundColor = UIColor.Gray,
				StrikethroughStyle = NSUnderlineStyle.Single
			};

			var thirdAttributes = new UIStringAttributes {
				ForegroundColor = UIColor.Green,
				BackgroundColor = UIColor.Black
			};

			var prettyString = new NSMutableAttributedString ("UITextField is not pretty!");
			prettyString.SetAttributes (firstAttributes.Dictionary, new NSRange (0, 11));
			prettyString.SetAttributes (secondAttributes.Dictionary, new NSRange (15, 3));
			prettyString.SetAttributes (thirdAttributes.Dictionary, new NSRange (19, 6));

			// assign the styled text
			topLabel.AttributedText = prettyString;
		}
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			topLabel.Frame = Bounds;
		}
	}
}

