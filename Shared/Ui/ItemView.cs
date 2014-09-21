using System;
using UIKit;
using CoreAnimation;
using Foundation;
using System.Drawing;
using iOSHelpers;
using CoreGraphics;

namespace ToddlerAddition
{
	public class ItemView : UIView
	{
		Item item;
		UIButton button;
		UILabel label;
		UIImage image;
		public Action<Item> Tapped = (i) => {
		};
		static Random random = new Random ();
		nfloat scale;
		nfloat xScale;
		nfloat yScale;

		public ItemView (Item item)
		{
			this.item = item;
			AddSubview (button = new UIButton {
				ContentMode = UIViewContentMode.ScaleAspectFill,
				Layer = {
					ShadowColor = UIColor.Black.CGColor,
					ShadowRadius = .3f,
					ShadowOpacity = .3f,
					ShadowOffset = new CGSize (1f, 1),
				},
			});
			button.TouchUpInside += (object sender, EventArgs e) => onTap ();
			button.SetImage (image = ImageLoader.Load (item.Image).ImageWithRenderingMode (UIImageRenderingMode.AlwaysTemplate), UIControlState.Normal);

			label = new UILabel {
				AdjustsFontSizeToFitWidth = true,
				Font = UIFont.BoldSystemFontOfSize (100),
				TextAlignment = UITextAlignment.Center,
				Text = "0",
				Layer = {
					ShadowColor = UIColor.Black.CGColor,
					ShadowOpacity = .25f,
					ShadowRadius = .3f,
					ShadowOffset = new CGSize (1f, 1),
				}
			};
			xScale = (nfloat)random.NextDouble ();
			yScale = (nfloat)random.NextDouble ();
			var s = Math.Max (random.NextDouble (), .5);
			scale = (nfloat)Math.Min (s, .9);

		}

		void onTap ()
		{
			if (item.IsSelected)
				return;
			Tapped (item);
			label.TextColor = ColorSelector.Next ();
			label.Text = item.Count.ToString ();
			button.Alpha = .25f;
			AddSubview (label);

			label.Pulse (2f);
		}

		CGRect lastBounds;
		nfloat width;
		nfloat height;
		nfloat x;
		nfloat y;

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			if (lastBounds != Bounds) {
				width = Bounds.Width * scale;
				height = Bounds.Height * scale;
				var newSize = ExpandToBound (image.Size, new CGSize (width, height));
				width = newSize.Width;
				height = newSize.Height;

				x = NMath.Max (xScale * Bounds.Width, 10);
				y = NMath.Max (yScale * Bounds.Height, 10);

				if (width + x > Bounds.Width)
					x = NMath.Max (Bounds.Width - width - 10, 5);

				if (height + y > Bounds.Height)
					y = NMath.Max (Bounds.Height - height - 10, 5);
				lastBounds = Bounds;
			}

			var frame = new CGRect (x, y, width, height);
			//if (frame.Height < label.Font.xHeight)
			label.Font = UIFont.BoldSystemFontOfSize (frame.Height * 2f);
			label.Frame = button.Frame = frame;
			label.SizeToFit ();
			label.Center = button.Center = new CGPoint (Bounds.GetMidX (), Bounds.GetMidY ());
		}

		private static CGSize ExpandToBound (CGSize image, CGSize boundingBox)
		{       
			nfloat widthScale = 0, heightScale = 0;
			if (image.Width != 0)
				widthScale = boundingBox.Width / image.Width;
			if (image.Height != 0)
				heightScale = boundingBox.Height / image.Height;                

			nfloat scale = NMath.Min (widthScale, heightScale);

			var result = new CGSize ((image.Width * scale), 
				             (image.Height * scale));
			return result;
		}

		public void Pulse ()
		{
			label.Pulse (2f);
		}

		public void Clear ()
		{
			label.Alpha = 0f;
		}
	}
}

