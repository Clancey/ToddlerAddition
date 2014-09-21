using System;
using UIKit;
using System.Collections.Generic;
using iOSHelpers;
using System.Linq;
using System.Drawing;
using CoreGraphics;

namespace ToddlerAddition
{
	public class CardView : UIControl
	{
		UILabel label;
		UIView lockView;
		public Action Tapped { get; set; }
		public CardView ()
		{
			Layer.BorderColor = UIColor.LightGray.CGColor;
			Layer.CornerRadius = 10f;
			Layer.BorderWidth = 1f;
			BackgroundColor = UIColor.LightGray.ColorWithAlpha (.1f);
			this.TouchUpInside += (object sender, EventArgs e) => {
				if(Tapped != null)
					Tapped();
			};

		}
		public void Clear()
		{
			completed = false;
			Subviews.ForEach(x=> {
				x.RemoveFromSuperview();
				x.Dispose();
			});
		}

		List<ItemView> items;
		public List<ItemView> Items {
			get {
				return items;
			}
			set {
				items = value;
				setupItems ();
			}
		}

		int columns;
		int rows;
		void setupItems()
		{
			if (items.Count > 0) {
				items.ForEach (x => AddSubview (x));
				columns = (int)Math.Sqrt (items.Count);
				rows = (int)Math.Ceiling (items.Count / (nfloat)columns);
			}
			label = new UILabel {
				AdjustsFontSizeToFitWidth = true,
				Font = UIFont.BoldSystemFontOfSize (300),
				TextAlignment = UITextAlignment.Center,
				Text = "",
				Layer = {
					ShadowColor = UIColor.Black.CGColor,
					ShadowOpacity = .25f,
					ShadowRadius = .3f,
					ShadowOffset = new CGSize (1f, 1),
				}
			};

			lockView = new UIView () {
				ExclusiveTouch = true,
			};


			TintColor = ColorSelector.Next ();
		}

		public override void LayoutSubviews ()
		{
			if (columns > 0) {
				var lastRow = items.Count / columns;
				var columnsLastRow = items.Count % columns;
				int lastRowOffset = (columns - columnsLastRow) / 2;
				if (rows == 0)
					rows = lastRow + (columnsLastRow > 0 ? 1 : 0);
				var width = Bounds.Width / columns;
				var height = Bounds.Height / rows;
				base.LayoutSubviews ();
				for (int i = 0; i < items.Count; i++) {
					var column = (i % columns);
					int row = i / columns;
					var tile = this.Subviews [i];
					nfloat x = column * width;
					if (row == lastRow) {
						x += (width * lastRowOffset);
					}
					var frame = new CGRect (x, row * height, width, height);
					tile.Frame = frame;
				}
			}
			var f = Bounds;
			lockView.Frame = label.Frame = f;
			if (label.Font == null || f.Height < label.Font.LineHeight)
				label.Font = UIFont.BoldSystemFontOfSize (f.Height * .9f);
		}

		public void Activated ()
		{
			if (completed)
				return;
			Layer.BorderColor = UIColor.Blue.CGColor;
			Layer.BorderWidth = 3f;
			if (lockView.Superview != null)
				lockView.RemoveFromSuperview ();
		}

		bool completed;
		public async void Completed (int total)
		{
			completed = true;
			Layer.BorderWidth = 1f;
			Layer.BorderColor = UIColor.LightGray.CGColor;
			label.Text = total.ToString ();
			await UIView.AnimateAsync (.3, () => {
				items.ForEach(x=> x.Clear());
			});
			label.TextColor = ColorSelector.Next ();
			AddSubview(label);
			label.Pulse (1.2f);

		}

		public void Pulse()
		{
			label.Pulse (1.7f);
		}
		public void Lock()
		{
			AddSubview (lockView);
		}
	}
}

