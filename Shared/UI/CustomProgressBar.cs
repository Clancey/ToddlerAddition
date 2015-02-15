using System;
using UIKit;
using CoreGraphics;
using System.Linq;

namespace ToddlerAddition
{
	public class CustomProgressBar : UIView
	{
		public CustomProgressBar () : base(new CGRect(0,0,100,25))
		{
			BackgroundColor = UIColor.Clear;
			Layer.BorderColor = UIColor.LightGray.CGColor;
			Layer.BorderWidth = 1;
			Layer.CornerRadius = 5;
			this.ClipsToBounds = true;
		}
		public float Progress { get; set; }
		public override void Draw (CoreGraphics.CGRect rect)
		{
			//// General Declarations
			var colorSpace = CGColorSpace.CreateDeviceRGB();
			var context = UIGraphics.GetCurrentContext();
			context.ClearRect (rect);
			//// Color Declarations
			UIColor color2 = UIColor.FromRGBA(0.013f, 0.298f, 0.866f, 1.000f);
			UIColor color3 = UIColor.FromRGBA(0.000f, 0.590f, 0.886f, 1.000f);

			//// Gradient Declarations
			var gradientColors = new CGColor [] {color2.CGColor, color3.CGColor};
			var gradientLocations = new nfloat [] {0, 1};
			var gradient = new CGGradient(colorSpace, gradientColors, gradientLocations);

			var progressFrame = rect;
			progressFrame.Width *= Progress;
			var rectangle2Path = UIBezierPath.FromRect(progressFrame);
			context.SaveState();
			rectangle2Path.AddClip();
			context.DrawLinearGradient(gradient, new CGPoint(rect.Height,0), new CGPoint(rect.Height, rect.Height), 0);
			context.RestoreState();

			var width = rect.Width / 10f;
			UIColor.LightGray.ColorWithAlpha(.5f).SetFill();
			Enumerable.Range (1, 9).ForEach(i=> {
				var x = width * i;
				var rectanglePath = UIBezierPath.FromRect(new CGRect(x, 0, 1, rect.Height));
				rectanglePath.Fill();
			});

		}
	}
}

