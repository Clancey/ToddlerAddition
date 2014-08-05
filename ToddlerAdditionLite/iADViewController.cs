using System;
using MonoTouch.UIKit;
using MonoTouch.iAd;
using System.Drawing;
using GoogleAdMobAds;

namespace ToddlerAddition
{
	public partial class IADViewController : UIViewController
	{
		UIViewController vc;
		ADBannerView iAdView;
		GADBannerView googleAdView;
		UIView adView;
		const string AdmobID = "a1533a02366e8c3";

		public IADViewController (UIViewController anyVC)
		{
			vc = anyVC;
			AddChildViewController (anyVC);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.AddSubview (vc.View);

			SetupiAd ();
			//SetUpGoogle ();


		}

		void SetupiAd ()
		{
			try {
				if (iAdView != null)
					iAdView.Dispose ();
				adView = iAdView = new ADBannerView (ADAdType.Banner);
				iAdView.Hidden = true;
				iAdView.FailedToReceiveAd += HandleFailedToReceiveAd;
				iAdView.AdLoaded += HandleAdLoaded;
				View.BackgroundColor = UIColor.Clear;
				vc.View.Frame = View.Bounds;
				View.AddSubview (iAdView);
			} catch (Exception ex) {
				SetUpGoogle ();
				Console.WriteLine (ex);
				Resize ();
			}
		}

		void SetUpGoogle ()
		{
			try {
				if (iAdView != null) {
					iAdView.RemoveFromSuperview ();
				}
				if (googleAdView == null) {
					var or =  this.ForOrientation (this.InterfaceOrientation);
					googleAdView = new GADBannerView (size: or, origin: new PointF (0, 0)) {
						AdUnitID = AdmobID,
						RootViewController = this
					};

					googleAdView.DidReceiveAd += (object sender, EventArgs e) => {
						if (adView == null)
							return;
						if (adView.Superview != View)
							View.AddSubview (adView);
						adView.Hidden = false;
						Resize ();
					};
					googleAdView.DidFailToReceiveAd += (object sender, GADBannerViewErrorEventArgs e) => {
						Console.WriteLine (e.Error);
						if (adView == null)
							return;
						adView.Hidden = true;
						;
						Resize ();
						SetupiAd ();
					};
				
					View.AddSubview (googleAdView);
				}
				adView = googleAdView;
				googleAdView.LoadRequest (GADRequest.Request);
			} catch (Exception ex) {
				Console.WriteLine (ex);
				Resize ();
			}
		}

		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate (fromInterfaceOrientation);
			Resize ();
		}
		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);

			if (googleAdView != null)
				googleAdView.AdSize = ForOrientation (toInterfaceOrientation);
		}

		GADAdSize ForOrientation(UIInterfaceOrientation orientation)
		{
			switch (orientation) {
			case UIInterfaceOrientation.LandscapeLeft:
			case UIInterfaceOrientation.LandscapeRight:
				return GADAdSizeCons.SmartBannerLandscape;
			}
			return GADAdSizeCons.SmartBannerPortrait;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			Resize ();
		}
		float AdHeight
		{
			get{
				if(iAdView == null && adView == null)
					return 0;
				if (iAdView == null || iAdView.Hidden)
					return adView.Frame.Height;
				return iAdView.Frame.Height;
			}
		}
		void Resize ()
		{

			UIView.Animate (.25,
				() => {
					if (iAdView != null && !iAdView.Hidden || (adView != null && !adView.Hidden)) {
						var frame = new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height - AdHeight);
						vc.View.Frame = frame;
						if(adView != null)
						{
							frame.Y = frame.Bottom;
							frame.Height = AdHeight;
							adView.Frame = frame;
						}
					} else {
						vc.View.Frame = View.Bounds;
					}
				});
			if (iAdView != null)
				iAdView.Frame = new RectangleF (0, vc.View.Bounds.Height, this.View.Bounds.Width, iAdView.Frame.Height);
		}

		void HandleAdLoaded (object sender, EventArgs e)
		{
			Console.WriteLine ("add handled");
			if (iAdView == null)
				return;
			adView.Hidden = false;
			Resize ();
		}

		void HandleFailedToReceiveAd (object sender, AdErrorEventArgs e)
		{
			SetUpGoogle ();
			Console.WriteLine ("add failes add : {0}", e.Error);
			if (adView == null)
				return;
			adView.Hidden = true;
			Resize ();
		}
	}
}

