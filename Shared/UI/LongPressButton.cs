using System;
using UIKit;
using Foundation;
using System.Drawing;
using CoreAnimation;
using CoreGraphics;

namespace ToddlerAddition
{
	public class LongPressButton : UIView
	{
		NSTimer timer;
		public Action Tapped { get; set; }
		public LongPressButton ()
		{
			FireTime = 5;
			BackgroundColor = UIColor.Clear;
		}

		nfloat progress;
		public nfloat Progress {
			get {
				return progress;
			}
			set {
				if (progress == value)
					return;
				progress = value;
				SetNeedsDisplay ();
			}
		}

		public double FireTime { get; set; }
		DateTime start;
		void UpdateProgress ()
		{
			var percent = (DateTime.Now - start).TotalSeconds / FireTime;
			Console.WriteLine (percent);
			Progress = (nfloat)percent;
			if (percent >= 1)
				Complete ();
		}

		void Reset ()
		{
			Progress = 0;
			if (timer != null) {
				timer.Invalidate ();
				timer = null;
			}
		}

		void Complete ()
		{
			Reset ();
			if (Tapped != null)
				Tapped ();
		}

		public override void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			start = DateTime.Now;
			timer = NSTimer.CreateRepeatingScheduledTimer (.1, (t) => {
				UpdateProgress();
			});
		}
		public override void TouchesCancelled (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled (touches, evt);
			Reset ();
		}
		public override void TouchesEnded (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);
			Reset ();
		}
		public override void TouchesMoved (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
		}

		static readonly UIColor DefaultFillColor = UIColor.FromRGB (114, 184, 255);
		static nfloat halfpie = (NMath.PI/2);
		public override void Draw (CGRect rect)
		{
			var context = UIGraphics.GetCurrentContext ();
			context.SaveState ();
			var center = new CGPoint (Bounds.Width / 2, Bounds.Height / 2);
			var rotationShift = -halfpie;
			var start = ConvertToRadians(0) + rotationShift;
			var end = ConvertToRadians(NMath.Min(360*Progress,360)) +rotationShift;
			var iconBezierPath = UIBezierPath.FromArc(center, Bounds.Width / 2f, start, end, false);
			iconBezierPath.AddLineTo (center);
			iconBezierPath.ClosePath ();
			context.AddPath (iconBezierPath.CGPath);
			var boundingRect = context.GetClipBoundingBox ();
			context.AddRect (boundingRect);
			context.EOClip ();
			// draw the icon shape (clipped portion is removed)
			if(progress > 0)
			DrawFilledShape ();
		

			context.RestoreState ();

			drawBorder ();
		}

		static readonly CGColor GradientOverlayStartColor = UIColor.White.ColorWithAlpha (0.7f).CGColor;
		static readonly CGColor GradientOverlayEndColor = UIColor.Clear.CGColor;
		void DrawFilledShape()
		{
			var bezierPath = new UIBezierPath();
			bezierPath.MoveTo(new CGPoint(41.35f, 17.47f));
			bezierPath.AddLineTo(new CGPoint(39.61f, 17.47f));
			bezierPath.AddCurveToPoint(new CGPoint(36.99f, 14.79f), new CGPoint(38.18f, 17.47f), new CGPoint(36.99f, 16.24f));
			bezierPath.AddCurveToPoint(new CGPoint(37.85f, 12.9f), new CGPoint(36.99f, 14.06f), new CGPoint(37.3f, 13.39f));
			bezierPath.AddLineTo(new CGPoint(38.98f, 11.8f));
			bezierPath.AddCurveToPoint(new CGPoint(38.98f, 7.8f), new CGPoint(40.09f, 10.7f), new CGPoint(40.09f, 8.9f));
			bezierPath.AddLineTo(new CGPoint(36.42f, 5.27f));
			bezierPath.AddCurveToPoint(new CGPoint(34.41f, 4.47f), new CGPoint(35.91f, 4.77f), new CGPoint(35.17f, 4.47f));
			bezierPath.AddCurveToPoint(new CGPoint(32.4f, 5.27f), new CGPoint(33.65f, 4.47f), new CGPoint(32.92f, 4.77f));
			bezierPath.AddLineTo(new CGPoint(31.33f, 6.35f));
			bezierPath.AddCurveToPoint(new CGPoint(29.38f, 7.23f), new CGPoint(30.81f, 6.92f), new CGPoint(30.12f, 7.23f));
			bezierPath.AddCurveToPoint(new CGPoint(26.68f, 4.63f), new CGPoint(27.91f, 7.23f), new CGPoint(26.68f, 6.04f));
			bezierPath.AddLineTo(new CGPoint(26.68f, 2.88f));
			bezierPath.AddCurveToPoint(new CGPoint(23.86f, 0), new CGPoint(26.68f, 1.33f), new CGPoint(25.42f, 0));
			bezierPath.AddLineTo(new CGPoint(20.38f, 0));
			bezierPath.AddCurveToPoint(new CGPoint(17.58f, 2.88f), new CGPoint(18.82f, 0), new CGPoint(17.58f, 1.32f));
			bezierPath.AddLineTo(new CGPoint(17.58f, 4.62f));
			bezierPath.AddCurveToPoint(new CGPoint(14.89f, 7.22f), new CGPoint(17.58f, 6.03f), new CGPoint(16.36f, 7.22f));
			bezierPath.AddCurveToPoint(new CGPoint(12.98f, 6.37f), new CGPoint(14.15f, 7.22f), new CGPoint(13.48f, 6.91f));
			bezierPath.AddLineTo(new CGPoint(11.87f, 5.27f));
			bezierPath.AddCurveToPoint(new CGPoint(9.86f, 4.47f), new CGPoint(11.37f, 4.75f), new CGPoint(10.62f, 4.47f));
			bezierPath.AddCurveToPoint(new CGPoint(7.86f, 5.27f), new CGPoint(9.11f, 4.47f), new CGPoint(8.37f, 4.77f));
			bezierPath.AddLineTo(new CGPoint(5.28f, 7.79f));
			bezierPath.AddCurveToPoint(new CGPoint(5.28f, 11.78f), new CGPoint(4.18f, 8.89f), new CGPoint(4.18f, 10.69f));
			bezierPath.AddLineTo(new CGPoint(6.35f, 12.85f));
			bezierPath.AddCurveToPoint(new CGPoint(7.25f, 14.79f), new CGPoint(6.93f, 13.37f), new CGPoint(7.25f, 14.06f));
			bezierPath.AddCurveToPoint(new CGPoint(4.63f, 17.47f), new CGPoint(7.25f, 16.26f), new CGPoint(6.06f, 17.47f));
			bezierPath.AddLineTo(new CGPoint(2.89f, 17.47f));
			bezierPath.AddCurveToPoint(new CGPoint(0, 20.25f), new CGPoint(1.32f, 17.47f), new CGPoint(0, 18.7f));
			bezierPath.AddLineTo(new CGPoint(0, 21.99f));
			bezierPath.AddLineTo(new CGPoint(0, 23.74f));
			bezierPath.AddCurveToPoint(new CGPoint(2.89f, 26.52f), new CGPoint(0, 25.28f), new CGPoint(1.32f, 26.52f));
			bezierPath.AddLineTo(new CGPoint(4.63f, 26.52f));
			bezierPath.AddCurveToPoint(new CGPoint(7.25f, 29.2f), new CGPoint(6.06f, 26.52f), new CGPoint(7.25f, 27.74f));
			bezierPath.AddCurveToPoint(new CGPoint(6.35f, 31.14f), new CGPoint(7.25f, 29.93f), new CGPoint(6.93f, 30.62f));
			bezierPath.AddLineTo(new CGPoint(5.28f, 32.2f));
			bezierPath.AddCurveToPoint(new CGPoint(5.28f, 36.19f), new CGPoint(4.18f, 33.3f), new CGPoint(4.18f, 35.1f));
			bezierPath.AddLineTo(new CGPoint(7.83f, 38.73f));
			bezierPath.AddCurveToPoint(new CGPoint(9.84f, 39.53f), new CGPoint(8.34f, 39.25f), new CGPoint(9.08f, 39.53f));
			bezierPath.AddCurveToPoint(new CGPoint(11.85f, 38.73f), new CGPoint(10.6f, 39.53f), new CGPoint(11.33f, 39.23f));
			bezierPath.AddLineTo(new CGPoint(12.96f, 37.63f));
			bezierPath.AddCurveToPoint(new CGPoint(14.87f, 36.78f), new CGPoint(13.44f, 37.09f), new CGPoint(14.13f, 36.78f));
			bezierPath.AddCurveToPoint(new CGPoint(17.56f, 39.38f), new CGPoint(16.33f, 36.78f), new CGPoint(17.56f, 37.97f));
			bezierPath.AddLineTo(new CGPoint(17.56f, 41.12f));
			bezierPath.AddCurveToPoint(new CGPoint(20.37f, 44), new CGPoint(17.56f, 42.67f), new CGPoint(18.8f, 44));
			bezierPath.AddLineTo(new CGPoint(23.86f, 44));
			bezierPath.AddCurveToPoint(new CGPoint(26.66f, 41.12f), new CGPoint(25.42f, 44), new CGPoint(26.66f, 42.68f));
			bezierPath.AddLineTo(new CGPoint(26.66f, 39.38f));
			bezierPath.AddCurveToPoint(new CGPoint(29.35f, 36.78f), new CGPoint(26.66f, 37.97f), new CGPoint(27.88f, 36.78f));
			bezierPath.AddCurveToPoint(new CGPoint(31.3f, 37.67f), new CGPoint(30.09f, 36.78f), new CGPoint(30.77f, 37.1f));
			bezierPath.AddLineTo(new CGPoint(32.38f, 38.74f));
			bezierPath.AddCurveToPoint(new CGPoint(34.39f, 39.54f), new CGPoint(32.9f, 39.25f), new CGPoint(33.63f, 39.54f));
			bezierPath.AddCurveToPoint(new CGPoint(36.39f, 38.74f), new CGPoint(35.14f, 39.54f), new CGPoint(35.88f, 39.25f));
			bezierPath.AddLineTo(new CGPoint(38.95f, 36.2f));
			bezierPath.AddCurveToPoint(new CGPoint(38.95f, 32.2f), new CGPoint(40.05f, 35.1f), new CGPoint(40.05f, 33.3f));
			bezierPath.AddLineTo(new CGPoint(37.83f, 31.1f));
			bezierPath.AddCurveToPoint(new CGPoint(36.97f, 29.21f), new CGPoint(37.28f, 30.61f), new CGPoint(36.97f, 29.93f));
			bezierPath.AddCurveToPoint(new CGPoint(39.58f, 26.53f), new CGPoint(36.97f, 27.74f), new CGPoint(38.16f, 26.53f));
			bezierPath.AddLineTo(new CGPoint(41.33f, 26.53f));
			bezierPath.AddCurveToPoint(new CGPoint(44, 23.75f), new CGPoint(42.89f, 26.53f), new CGPoint(44, 25.3f));
			bezierPath.AddLineTo(new CGPoint(44, 21.99f));
			bezierPath.AddLineTo(new CGPoint(44, 20.25f));
			bezierPath.AddCurveToPoint(new CGPoint(41.35f, 17.47f), new CGPoint(44.02f, 18.7f), new CGPoint(42.91f, 17.47f));
			bezierPath.ClosePath();
			bezierPath.MoveTo(new CGPoint(31.29f, 21.99f));
			bezierPath.AddLineTo(new CGPoint(31.29f, 21.99f));
			bezierPath.AddCurveToPoint(new CGPoint(22.11f, 31.16f), new CGPoint(31.29f, 27.05f), new CGPoint(27.2f, 31.16f));
			bezierPath.AddCurveToPoint(new CGPoint(12.94f, 21.99f), new CGPoint(17.03f, 31.16f), new CGPoint(12.94f, 27.05f));
			bezierPath.AddLineTo(new CGPoint(12.94f, 21.99f));
			bezierPath.AddLineTo(new CGPoint(12.94f, 21.99f));
			bezierPath.AddCurveToPoint(new CGPoint(22.11f, 12.83f), new CGPoint(12.94f, 16.94f), new CGPoint(17.03f, 12.83f));
			bezierPath.AddCurveToPoint(new CGPoint(31.29f, 21.99f), new CGPoint(27.2f, 12.83f), new CGPoint(31.29f, 16.94f));
			bezierPath.AddLineTo(new CGPoint(31.29f, 21.99f));
			bezierPath.ClosePath();
			bezierPath.MiterLimit = 4;

			DefaultFillColor.SetColor ();
			bezierPath.Fill ();
		}

		void drawBorder()
		{
			UIBezierPath bezierPath = new UIBezierPath();
			bezierPath.MoveTo(new CGPoint(22.09f, 10.97f));
			bezierPath.AddCurveToPoint(new CGPoint(14.31f, 14.18f), new CGPoint(19.15f, 10.97f), new CGPoint(16.39f, 12.11f));
			bezierPath.AddCurveToPoint(new CGPoint(11.09f, 21.94f), new CGPoint(12.24f, 16.25f), new CGPoint(11.09f, 19.01f));
			bezierPath.AddCurveToPoint(new CGPoint(14.31f, 29.7f), new CGPoint(11.09f, 24.88f), new CGPoint(12.24f, 27.62f));
			bezierPath.AddCurveToPoint(new CGPoint(22.09f, 32.91f), new CGPoint(16.39f, 31.77f), new CGPoint(19.16f, 32.91f));
			bezierPath.AddCurveToPoint(new CGPoint(29.87f, 29.7f), new CGPoint(25.04f, 32.91f), new CGPoint(27.8f, 31.77f));
			bezierPath.AddCurveToPoint(new CGPoint(33.09f, 21.94f), new CGPoint(31.95f, 27.63f), new CGPoint(33.09f, 24.88f));
			bezierPath.AddCurveToPoint(new CGPoint(29.87f, 14.18f), new CGPoint(33.09f, 19.01f), new CGPoint(31.95f, 16.26f));
			bezierPath.AddCurveToPoint(new CGPoint(22.09f, 10.97f), new CGPoint(27.8f, 12.11f), new CGPoint(25.04f, 10.97f));
			bezierPath.ClosePath();
			bezierPath.MoveTo(new CGPoint(22.09f, 31.09f));
			bezierPath.AddCurveToPoint(new CGPoint(12.92f, 21.94f), new CGPoint(17.02f, 31.09f), new CGPoint(12.92f, 26.98f));
			bezierPath.AddCurveToPoint(new CGPoint(22.09f, 12.8f), new CGPoint(12.92f, 16.9f), new CGPoint(17.02f, 12.8f));
			bezierPath.AddCurveToPoint(new CGPoint(31.26f, 21.94f), new CGPoint(27.17f, 12.8f), new CGPoint(31.26f, 16.9f));
			bezierPath.AddCurveToPoint(new CGPoint(22.09f, 31.09f), new CGPoint(31.26f, 26.98f), new CGPoint(27.17f, 31.09f));
			bezierPath.ClosePath();
			bezierPath.MiterLimit = 4;

			UIColor.LightGray.ColorWithAlpha(.5f).SetFill();
			bezierPath.Fill();


			//// Bezier 2 Drawing
			UIBezierPath bezier2Path = new UIBezierPath();
			bezier2Path.MoveTo(new CGPoint(41.31f, 17.37f));
			bezier2Path.AddLineTo(new CGPoint(39.57f, 17.37f));
			bezier2Path.AddCurveToPoint(new CGPoint(36.95f, 14.72f), new CGPoint(38.14f, 17.37f), new CGPoint(36.95f, 16.18f));
			bezier2Path.AddCurveToPoint(new CGPoint(37.81f, 12.85f), new CGPoint(36.95f, 13.99f), new CGPoint(37.26f, 13.34f));
			bezier2Path.AddLineTo(new CGPoint(38.94f, 11.75f));
			bezier2Path.AddCurveToPoint(new CGPoint(38.94f, 7.76f), new CGPoint(40.05f, 10.65f), new CGPoint(40.05f, 8.87f));
			bezier2Path.AddLineTo(new CGPoint(36.38f, 5.23f));
			bezier2Path.AddCurveToPoint(new CGPoint(34.38f, 4.43f), new CGPoint(35.88f, 4.73f), new CGPoint(35.13f, 4.43f));
			bezier2Path.AddCurveToPoint(new CGPoint(32.37f, 5.23f), new CGPoint(33.62f, 4.43f), new CGPoint(32.89f, 4.73f));
			bezier2Path.AddLineTo(new CGPoint(31.29f, 6.31f));
			bezier2Path.AddCurveToPoint(new CGPoint(29.32f, 7.19f), new CGPoint(30.78f, 6.88f), new CGPoint(30.07f, 7.19f));
			bezier2Path.AddCurveToPoint(new CGPoint(26.61f, 4.59f), new CGPoint(27.86f, 7.19f), new CGPoint(26.61f, 6));
			bezier2Path.AddLineTo(new CGPoint(26.61f, 2.87f));
			bezier2Path.AddCurveToPoint(new CGPoint(23.86f, 0), new CGPoint(26.61f, 1.33f), new CGPoint(25.4f, 0));
			bezier2Path.AddLineTo(new CGPoint(20.37f, 0));
			bezier2Path.AddCurveToPoint(new CGPoint(17.53f, 2.87f), new CGPoint(18.79f, 0), new CGPoint(17.53f, 1.31f));
			bezier2Path.AddLineTo(new CGPoint(17.53f, 4.61f));
			bezier2Path.AddCurveToPoint(new CGPoint(14.85f, 7.2f), new CGPoint(17.53f, 6.01f), new CGPoint(16.32f, 7.2f));
			bezier2Path.AddCurveToPoint(new CGPoint(12.95f, 6.35f), new CGPoint(14.12f, 7.2f), new CGPoint(13.45f, 6.89f));
			bezier2Path.AddLineTo(new CGPoint(11.84f, 5.26f));
			bezier2Path.AddCurveToPoint(new CGPoint(9.83f, 4.46f), new CGPoint(11.33f, 4.74f), new CGPoint(10.59f, 4.46f));
			bezier2Path.AddCurveToPoint(new CGPoint(7.83f, 5.26f), new CGPoint(9.07f, 4.46f), new CGPoint(8.34f, 4.75f));
			bezier2Path.AddLineTo(new CGPoint(5.27f, 7.77f));
			bezier2Path.AddCurveToPoint(new CGPoint(5.27f, 11.75f), new CGPoint(4.17f, 8.87f), new CGPoint(4.17f, 10.66f));
			bezier2Path.AddLineTo(new CGPoint(6.35f, 12.82f));
			bezier2Path.AddCurveToPoint(new CGPoint(7.24f, 14.73f), new CGPoint(6.92f, 13.34f), new CGPoint(7.24f, 13.99f));
			bezier2Path.AddCurveToPoint(new CGPoint(4.63f, 17.38f), new CGPoint(7.24f, 16.19f), new CGPoint(6.05f, 17.38f));
			bezier2Path.AddLineTo(new CGPoint(2.89f, 17.38f));
			bezier2Path.AddCurveToPoint(new CGPoint(0, 20.21f), new CGPoint(1.32f, 17.37f), new CGPoint(0, 18.65f));
			bezier2Path.AddLineTo(new CGPoint(0, 21.94f));
			bezier2Path.AddLineTo(new CGPoint(0, 23.68f));
			bezier2Path.AddCurveToPoint(new CGPoint(2.89f, 26.51f), new CGPoint(0, 25.22f), new CGPoint(1.32f, 26.51f));
			bezier2Path.AddLineTo(new CGPoint(4.63f, 26.51f));
			bezier2Path.AddCurveToPoint(new CGPoint(7.24f, 29.17f), new CGPoint(6.05f, 26.51f), new CGPoint(7.24f, 27.7f));
			bezier2Path.AddCurveToPoint(new CGPoint(6.35f, 31.09f), new CGPoint(7.24f, 29.9f), new CGPoint(6.92f, 30.57f));
			bezier2Path.AddLineTo(new CGPoint(5.27f, 32.14f));
			bezier2Path.AddCurveToPoint(new CGPoint(5.27f, 36.11f), new CGPoint(4.17f, 33.23f), new CGPoint(4.17f, 35.02f));
			bezier2Path.AddLineTo(new CGPoint(7.83f, 38.65f));
			bezier2Path.AddCurveToPoint(new CGPoint(9.83f, 39.45f), new CGPoint(8.33f, 39.17f), new CGPoint(9.07f, 39.45f));
			bezier2Path.AddCurveToPoint(new CGPoint(11.84f, 38.65f), new CGPoint(10.59f, 39.45f), new CGPoint(11.32f, 39.15f));
			bezier2Path.AddLineTo(new CGPoint(12.95f, 37.55f));
			bezier2Path.AddCurveToPoint(new CGPoint(14.84f, 36.71f), new CGPoint(13.43f, 37.02f), new CGPoint(14.11f, 36.71f));
			bezier2Path.AddCurveToPoint(new CGPoint(17.52f, 39.3f), new CGPoint(16.31f, 36.71f), new CGPoint(17.52f, 37.9f));
			bezier2Path.AddLineTo(new CGPoint(17.52f, 41.04f));
			bezier2Path.AddCurveToPoint(new CGPoint(20.35f, 43.91f), new CGPoint(17.52f, 42.58f), new CGPoint(18.78f, 43.91f));
			bezier2Path.AddLineTo(new CGPoint(23.83f, 43.91f));
			bezier2Path.AddCurveToPoint(new CGPoint(26.69f, 41.04f), new CGPoint(25.39f, 43.91f), new CGPoint(26.69f, 42.59f));
			bezier2Path.AddLineTo(new CGPoint(26.69f, 39.3f));
			bezier2Path.AddCurveToPoint(new CGPoint(29.36f, 36.71f), new CGPoint(26.69f, 37.9f), new CGPoint(27.89f, 36.71f));
			bezier2Path.AddCurveToPoint(new CGPoint(31.29f, 37.59f), new CGPoint(30.09f, 36.71f), new CGPoint(30.77f, 37.03f));
			bezier2Path.AddLineTo(new CGPoint(32.37f, 38.66f));
			bezier2Path.AddCurveToPoint(new CGPoint(34.38f, 39.46f), new CGPoint(32.89f, 39.17f), new CGPoint(33.62f, 39.46f));
			bezier2Path.AddCurveToPoint(new CGPoint(36.38f, 38.66f), new CGPoint(35.13f, 39.46f), new CGPoint(35.86f, 39.17f));
			bezier2Path.AddLineTo(new CGPoint(38.94f, 36.13f));
			bezier2Path.AddCurveToPoint(new CGPoint(38.94f, 32.14f), new CGPoint(40.04f, 35.03f), new CGPoint(40.04f, 33.23f));
			bezier2Path.AddLineTo(new CGPoint(37.81f, 31.04f));
			bezier2Path.AddCurveToPoint(new CGPoint(36.95f, 29.13f), new CGPoint(37.26f, 30.55f), new CGPoint(36.95f, 29.85f));
			bezier2Path.AddCurveToPoint(new CGPoint(39.57f, 26.43f), new CGPoint(36.95f, 27.67f), new CGPoint(38.14f, 26.43f));
			bezier2Path.AddLineTo(new CGPoint(41.31f, 26.43f));
			bezier2Path.AddCurveToPoint(new CGPoint(43.98f, 23.7f), new CGPoint(42.87f, 26.43f), new CGPoint(43.98f, 25.26f));
			bezier2Path.AddLineTo(new CGPoint(43.98f, 21.94f));
			bezier2Path.AddLineTo(new CGPoint(43.98f, 20.21f));
			bezier2Path.AddCurveToPoint(new CGPoint(41.31f, 17.37f), new CGPoint(43.98f, 18.65f), new CGPoint(42.87f, 17.37f));
			bezier2Path.ClosePath();
			bezier2Path.MoveTo(new CGPoint(42.17f, 21.94f));
			bezier2Path.AddLineTo(new CGPoint(42.17f, 23.67f));
			bezier2Path.AddCurveToPoint(new CGPoint(41.33f, 24.57f), new CGPoint(42.17f, 24.15f), new CGPoint(41.9f, 24.57f));
			bezier2Path.AddLineTo(new CGPoint(39.59f, 24.57f));
			bezier2Path.AddCurveToPoint(new CGPoint(36.44f, 25.94f), new CGPoint(38.41f, 24.57f), new CGPoint(37.29f, 25.07f));
			bezier2Path.AddCurveToPoint(new CGPoint(35.14f, 29.12f), new CGPoint(35.6f, 26.8f), new CGPoint(35.14f, 27.93f));
			bezier2Path.AddCurveToPoint(new CGPoint(36.57f, 32.34f), new CGPoint(35.14f, 30.35f), new CGPoint(35.65f, 31.5f));
			bezier2Path.AddLineTo(new CGPoint(37.66f, 33.42f));
			bezier2Path.AddCurveToPoint(new CGPoint(37.66f, 34.82f), new CGPoint(38.04f, 33.81f), new CGPoint(38.04f, 34.45f));
			bezier2Path.AddLineTo(new CGPoint(35.11f, 37.36f));
			bezier2Path.AddCurveToPoint(new CGPoint(34.39f, 37.63f), new CGPoint(34.92f, 37.53f), new CGPoint(34.66f, 37.63f));
			bezier2Path.AddCurveToPoint(new CGPoint(33.66f, 37.36f), new CGPoint(34.11f, 37.63f), new CGPoint(33.84f, 37.53f));
			bezier2Path.AddLineTo(new CGPoint(32.62f, 36.32f));
			bezier2Path.AddCurveToPoint(new CGPoint(29.36f, 34.88f), new CGPoint(31.74f, 35.39f), new CGPoint(30.58f, 34.88f));
			bezier2Path.AddCurveToPoint(new CGPoint(26.21f, 36.16f), new CGPoint(28.16f, 34.88f), new CGPoint(27.06f, 35.34f));
			bezier2Path.AddCurveToPoint(new CGPoint(24.88f, 39.3f), new CGPoint(25.33f, 37.01f), new CGPoint(24.88f, 38.11f));
			bezier2Path.AddLineTo(new CGPoint(24.88f, 41.04f));
			bezier2Path.AddCurveToPoint(new CGPoint(23.86f, 42.08f), new CGPoint(24.88f, 41.6f), new CGPoint(24.38f, 42.08f));
			bezier2Path.AddLineTo(new CGPoint(20.37f, 42.08f));
			bezier2Path.AddCurveToPoint(new CGPoint(19.38f, 41.04f), new CGPoint(19.85f, 42.08f), new CGPoint(19.38f, 41.6f));
			bezier2Path.AddLineTo(new CGPoint(19.38f, 39.3f));
			bezier2Path.AddCurveToPoint(new CGPoint(18.04f, 36.16f), new CGPoint(19.38f, 38.13f), new CGPoint(18.91f, 37.01f));
			bezier2Path.AddCurveToPoint(new CGPoint(14.87f, 34.88f), new CGPoint(17.18f, 35.34f), new CGPoint(16.05f, 34.88f));
			bezier2Path.AddCurveToPoint(new CGPoint(11.65f, 36.3f), new CGPoint(13.66f, 34.88f), new CGPoint(12.49f, 35.39f));
			bezier2Path.AddLineTo(new CGPoint(10.59f, 37.36f));
			bezier2Path.AddCurveToPoint(new CGPoint(9.87f, 37.63f), new CGPoint(10.4f, 37.53f), new CGPoint(10.14f, 37.63f));
			bezier2Path.AddCurveToPoint(new CGPoint(9.17f, 37.38f), new CGPoint(9.59f, 37.63f), new CGPoint(9.32f, 37.54f));
			bezier2Path.AddLineTo(new CGPoint(9.16f, 37.37f));
			bezier2Path.AddLineTo(new CGPoint(9.14f, 37.36f));
			bezier2Path.AddLineTo(new CGPoint(6.59f, 34.82f));
			bezier2Path.AddCurveToPoint(new CGPoint(6.59f, 33.43f), new CGPoint(6.21f, 34.45f), new CGPoint(6.21f, 33.82f));
			bezier2Path.AddLineTo(new CGPoint(7.63f, 32.4f));
			bezier2Path.AddCurveToPoint(new CGPoint(9.09f, 29.14f), new CGPoint(8.57f, 31.53f), new CGPoint(9.09f, 30.38f));
			bezier2Path.AddCurveToPoint(new CGPoint(7.79f, 26.01f), new CGPoint(9.09f, 27.95f), new CGPoint(8.63f, 26.87f));
			bezier2Path.AddCurveToPoint(new CGPoint(4.64f, 24.7f), new CGPoint(6.94f, 25.14f), new CGPoint(5.82f, 24.7f));
			bezier2Path.AddLineTo(new CGPoint(2.89f, 24.7f));
			bezier2Path.AddCurveToPoint(new CGPoint(1.83f, 23.69f), new CGPoint(2.31f, 24.7f), new CGPoint(1.83f, 24.21f));
			bezier2Path.AddLineTo(new CGPoint(1.83f, 21.94f));
			bezier2Path.AddLineTo(new CGPoint(1.83f, 20.21f));
			bezier2Path.AddCurveToPoint(new CGPoint(2.89f, 19.2f), new CGPoint(1.83f, 19.69f), new CGPoint(2.31f, 19.2f));
			bezier2Path.AddLineTo(new CGPoint(4.63f, 19.2f));
			bezier2Path.AddCurveToPoint(new CGPoint(7.78f, 17.89f), new CGPoint(5.81f, 19.2f), new CGPoint(6.93f, 18.75f));
			bezier2Path.AddCurveToPoint(new CGPoint(9.08f, 14.74f), new CGPoint(8.62f, 17.03f), new CGPoint(9.08f, 15.92f));
			bezier2Path.AddCurveToPoint(new CGPoint(7.62f, 11.5f), new CGPoint(9.08f, 13.51f), new CGPoint(8.56f, 12.35f));
			bezier2Path.AddLineTo(new CGPoint(6.57f, 10.46f));
			bezier2Path.AddCurveToPoint(new CGPoint(6.28f, 9.76f), new CGPoint(6.31f, 10.21f), new CGPoint(6.28f, 9.92f));
			bezier2Path.AddCurveToPoint(new CGPoint(6.57f, 9.06f), new CGPoint(6.28f, 9.61f), new CGPoint(6.31f, 9.31f));
			bezier2Path.AddLineTo(new CGPoint(9.11f, 6.54f));
			bezier2Path.AddCurveToPoint(new CGPoint(9.83f, 6.26f), new CGPoint(9.29f, 6.37f), new CGPoint(9.56f, 6.26f));
			bezier2Path.AddCurveToPoint(new CGPoint(10.53f, 6.51f), new CGPoint(10.11f, 6.26f), new CGPoint(10.38f, 6.35f));
			bezier2Path.AddLineTo(new CGPoint(10.54f, 6.53f));
			bezier2Path.AddLineTo(new CGPoint(10.55f, 6.54f));
			bezier2Path.AddLineTo(new CGPoint(11.63f, 7.61f));
			bezier2Path.AddCurveToPoint(new CGPoint(14.85f, 9.03f), new CGPoint(12.48f, 8.53f), new CGPoint(13.62f, 9.03f));
			bezier2Path.AddCurveToPoint(new CGPoint(18.01f, 7.75f), new CGPoint(16.04f, 9.03f), new CGPoint(17.15f, 8.57f));
			bezier2Path.AddCurveToPoint(new CGPoint(19.36f, 4.61f), new CGPoint(18.88f, 6.9f), new CGPoint(19.36f, 5.79f));
			bezier2Path.AddLineTo(new CGPoint(19.36f, 2.87f));
			bezier2Path.AddCurveToPoint(new CGPoint(20.34f, 1.83f), new CGPoint(19.36f, 2.31f), new CGPoint(19.82f, 1.83f));
			bezier2Path.AddLineTo(new CGPoint(23.83f, 1.83f));
			bezier2Path.AddCurveToPoint(new CGPoint(24.75f, 2.87f), new CGPoint(24.35f, 1.83f), new CGPoint(24.75f, 2.31f));
			bezier2Path.AddLineTo(new CGPoint(24.75f, 4.61f));
			bezier2Path.AddCurveToPoint(new CGPoint(26.12f, 7.75f), new CGPoint(24.75f, 5.78f), new CGPoint(25.25f, 6.9f));
			bezier2Path.AddCurveToPoint(new CGPoint(29.31f, 9.03f), new CGPoint(26.98f, 8.57f), new CGPoint(28.12f, 9.03f));
			bezier2Path.AddCurveToPoint(new CGPoint(32.59f, 7.59f), new CGPoint(30.55f, 9.03f), new CGPoint(31.72f, 8.51f));
			bezier2Path.AddLineTo(new CGPoint(33.63f, 6.55f));
			bezier2Path.AddCurveToPoint(new CGPoint(34.35f, 6.27f), new CGPoint(33.81f, 6.38f), new CGPoint(34.08f, 6.27f));
			bezier2Path.AddCurveToPoint(new CGPoint(35.07f, 6.54f), new CGPoint(34.63f, 6.27f), new CGPoint(34.9f, 6.38f));
			bezier2Path.AddLineTo(new CGPoint(37.63f, 9.06f));
			bezier2Path.AddCurveToPoint(new CGPoint(37.93f, 9.76f), new CGPoint(37.81f, 9.25f), new CGPoint(37.93f, 9.5f));
			bezier2Path.AddCurveToPoint(new CGPoint(37.64f, 10.46f), new CGPoint(37.93f, 10.02f), new CGPoint(37.82f, 10.27f));
			bezier2Path.AddLineTo(new CGPoint(36.55f, 11.53f));
			bezier2Path.AddCurveToPoint(new CGPoint(35.12f, 14.75f), new CGPoint(35.64f, 12.38f), new CGPoint(35.12f, 13.52f));
			bezier2Path.AddCurveToPoint(new CGPoint(36.41f, 17.89f), new CGPoint(35.12f, 15.94f), new CGPoint(35.58f, 17.03f));
			bezier2Path.AddCurveToPoint(new CGPoint(39.57f, 19.2f), new CGPoint(37.26f, 18.75f), new CGPoint(38.39f, 19.2f));
			bezier2Path.AddLineTo(new CGPoint(41.31f, 19.2f));
			bezier2Path.AddCurveToPoint(new CGPoint(42.17f, 20.23f), new CGPoint(41.93f, 19.2f), new CGPoint(42.16f, 19.77f));
			bezier2Path.AddLineTo(new CGPoint(42.17f, 21.94f));
			bezier2Path.ClosePath();
			bezier2Path.MiterLimit = 4;

			UIColor.LightGray.ColorWithAlpha(.5f).SetFill();
			bezier2Path.Fill();

		}

		public nfloat ConvertToRadians(nfloat angle)
		{
			return (NMath.PI / 180) * angle;
		}	

	}
}

