using System;
using MonoTouch.UIKit;
using iOSHelpers;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using System.Threading.Tasks;

namespace ToddlerAddition
{
	public class TotalView : UIView
	{
		UILabel label;
		UIView emptyView;
		UIView nestedView;
		public TotalView ()
		{	
			AddSubview(emptyView = new UIView () {
				Layer = {
					BorderColor = UIColor.LightGray.CGColor,
					CornerRadius = 10f,
					BorderWidth = 1f,
				},
				BackgroundColor = UIColor.LightGray.ColorWithAlpha (.1f),
			});

			nestedView = new UIView () {
				Layer = {
					BorderColor = UIColor.LightGray.CGColor,
					CornerRadius = 10f,
					BorderWidth = 1f,
				},
				BackgroundColor = UIColor.LightGray.ColorWithAlpha (.1f),
			};

			nestedView.AddSubview (label = new UILabel {
				AdjustsFontSizeToFitWidth = true,
				Font = UIFont.BoldSystemFontOfSize (300),
				TextAlignment = UITextAlignment.Center,
				Text = "",
				Layer = {
					ShadowColor = UIColor.Black.CGColor,
					ShadowOpacity = .25f,
					ShadowRadius = .3f,
				}
			});
		}
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var frame = Bounds;
			emptyView.Frame = nestedView.Frame = label.Frame = frame;
			if (frame.Height < label.Font.LineHeight)
				label.Font = UIFont.BoldSystemFontOfSize (frame.Height * .9f);
			label.Center = new System.Drawing.PointF (Bounds.GetMidX (), Bounds.GetMidY ());
		}

		public async void Clear ()
		{
			await UIView.TransitionNotifyAsync (nestedView, emptyView, .3, UIViewAnimationOptions.TransitionFlipFromRight);
			Total = 0;
		}

		int total;
		public int Total {
			get {
				return total;
			}
			set {

				if (total == 0 && value > 0)
					Animate ();
				total = value;

				label.TextColor = ColorSelector.Next ();
				label.Text = total > 0 ? total.ToString(): "";
				label.SizeToFit ();

			}
		}

		async Task Animate()
		{
			await UIView.TransitionNotifyAsync (emptyView, nestedView, .3, UIViewAnimationOptions.TransitionFlipFromLeft);
		}
		public void Pulse()
		{
			label.Pulse (2f);
		}
	}
}

