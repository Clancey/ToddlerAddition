using System;
using MonoTouch.UIKit;
using iOSHelpers;
using System.Drawing;

namespace ToddlerAddition
{
	public class NumberBar : UIView
	{
		public Action<int> Tapped {get;set;}
		public NumberBar ()
		{
			BackgroundColor = UIColor.White;
		}

		public void SetRange(int start, int end)
		{
			foreach (var v in Subviews)
				v.RemoveFromSuperview ();
			for (var i = start; i <= end; i++)
				AddSubview(new UIBorderedButton{
					Tag = i,
					Title = i.ToString(),
					Tapped = (b) => Tapped (b.Tag),
				});
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			const float padding = 5f;
			var width = (Bounds.Width / Subviews.Length) - padding;
			var h = Bounds.Height - (padding * 2);
			var frame = new RectangleF (padding, padding, width, h);
			foreach (var v in Subviews) {
				v.Frame = frame;
				frame.X = frame.Right + padding;
			}
		}
	}
}

