using System;
using UIKit;
using CoreGraphics;

namespace ToddlerAddition
{
	public class SettingsCardView : UIView
	{
		public string Title {
			get { return topLabel.Text; }
			set { topLabel.Text = value; }
		}

		public string Subtitle {
			get { return subTitleLabel.Text; }
			set { subTitleLabel.Text = value; }
		}

		public bool Selected { get; set; }

		public string[] Descriptions { get; set; }

		public float Progress {
			get {
				return progress.Progress;
			}
			set {
				progress.Progress = value;
			}
		}

		OvalView topHeader;
		UIView bottomView;
		UILabel topLabel;
		UILabel subTitleLabel;
		CustomProgressBar progress;

		public SettingsCardView ()
		{
			BackgroundColor = UIColor.Clear;
			Descriptions = new string[0];
			Add (topHeader = new OvalView ());
			Add (topLabel = new UILabel {
				Font = UIFont.BoldSystemFontOfSize (20),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Center,
			});
			Add (subTitleLabel = new UILabel {
				Font = UIFont.BoldSystemFontOfSize (18),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Center,
			});
			Add (bottomView = new UIView {
				BackgroundColor = UIColor.White,
				Layer = {
					BorderColor = UIColor.LightGray.CGColor,
					CornerRadius = 5f,
					BorderWidth = 1f,
				}
			});

			Add (progress = new CustomProgressBar ());
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			const float topLabelPadding = 20;
			var bounds = Bounds;
			var x = bounds.Width / 6;
			var frame = new CGRect (x, 0, x*4, 70);
			topHeader.Frame = frame;
			topLabel.SizeToFit ();
			frame = topLabel.Frame = new CGRect (topLabelPadding, 10, bounds.Width - (topLabelPadding * 2), topLabel.Frame.Height); 
			frame.Y = frame.Bottom;
			subTitleLabel.Frame = frame;

			var bframe = bottomView.Frame = new CGRect (0, 60, bounds.Width, bounds.Height - 70);

			progress.SizeToFit ();
			frame = progress.Frame;
			frame.X = bframe.X;
			frame.Y = bframe.Bottom - frame.Height;
			frame.Width = bframe.Width;
			progress.Frame = frame;

		}

		class OvalView : UIView
		{

			public OvalView()
			{
				BackgroundColor = UIColor.Yellow.ColorWithAlpha(.6f);
			}
			public override CGRect Frame {
				get {
					return base.Frame;
				}
				set {
					Layer.CornerRadius = Frame.Height / 2f;
					base.Frame = value;
				}
			}
//			public override void Draw (CoreGraphics.CGRect rect)
//			{
//				//// General Declarations
//				var colorSpace = CGColorSpace.CreateDeviceRGB ();
//				var context = UIGraphics.GetCurrentContext ();
//
//				//// Color Declarations
//				UIColor color = UIColor.FromRGBA (1.000f, 1.000f, 0.571f, 1.000f);
//
//				//// Gradient Declarations
//				var gradientColors = new CGColor [] { color.CGColor, UIColor.White.CGColor };
//				var gradientLocations = new nfloat [] { 0, 1 };
//				var gradient = new CGGradient (colorSpace, gradientColors, gradientLocations);
//
//				//// Oval Drawing
//				var ovalPath = UIBezierPath.FromOval (rect);
//				context.SaveState ();
//				ovalPath.AddClip ();
//				const float startPercent = .1464f;
//				const float endPercent = 1f - startPercent;
//				context.DrawLinearGradient (gradient, new CGPoint (rect.Width * startPercent,  rect.Height * startPercent), new CGPoint (rect.Width * endPercent, rect.Height * endPercent), 0);
//				context.RestoreState ();
//
//
//
//			}
		}
	}
}

