using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Linq;
using iOSHelpers;
using System.Threading.Tasks;

namespace ToddlerAddition
{
	public class MainViewController : UIViewController
	{
		ViewModel model;
		MainView view;
		public MainViewController ()
		{

		}
		public override void LoadView ()
		{
			View = view = new MainView ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (model == null)
				newModel ();


		}

		void newModel()
		{
			if (model != null)
				model.Dispose ();

			model = ViewModel.CreateAddition (10);
			model.Finished = finished;
			view.SetupModel (model);
			SoundPlayer.SpeakIntro ();
			
		}
		async void finished()
		{
			await Task.Delay(1500);
			view.PulseFirst ();
			await SoundPlayer.Speak (model.FirstCount);

			//await Task.Delay(500);
			view.PulseOperator ();
			await SoundPlayer.SpeakPlus();

			//await Task.Delay(500);
			view.PulseSecond ();
			await SoundPlayer.Speak (model.SecondCount);

			//await Task.Delay(500);
			view.PulseEquals ();
			await SoundPlayer.SpeakEquals ();
			view.UpdateTotal ();

			await Task.Delay(300);

			view.PulseTotal ();
			await SoundPlayer.SpeakExcited(model.Total);

			await Task.WhenAll (Task.Delay (2000), SoundPlayer.SpeakCongrats ());
			newModel ();
		}


		class MainView : UIView
		{
			CardView firstCard;
			CardView secondCard;
			TotalView totalView;
			UILabel operatorLabel;
			UILabel equalLabel;
			public MainView()
			{
				BackgroundColor = UIColor.White;
				AddSubview(firstCard = new CardView());
				AddSubview(operatorLabel = new UILabel{
					AdjustsFontSizeToFitWidth = true,
					Font = UIFont.BoldSystemFontOfSize(100),
					TextAlignment = UITextAlignment.Center,
					Text = "+",
				});
				AddSubview(secondCard = new CardView());
				AddSubview(equalLabel = new UILabel{
					AdjustsFontSizeToFitWidth = true,
					Font = UIFont.BoldSystemFontOfSize(100),
					TextAlignment = UITextAlignment.Center,
					Text = "=",
				});
				AddSubview(totalView = new TotalView());
			}

			ViewModel model;
			public void SetupModel(ViewModel model)
			{
				this.model = model;
				firstCard.Clear ();
				secondCard.Clear ();
				totalView.Clear ();

				firstCard.Items = model.FirstItems.Select (x => new ItemView (x) {
					Tapped = (i) => {
						i.Select (model.FirstCount);
					},
				}).ToList ();
				secondCard.Items = model.SecondItems.Select (x => new ItemView (x) {
					Tapped = (i)=>{
						i.Select(model.SecondCount);
					},
				}).ToList();
				model.FirstActivated = () => {
					secondCard.Lock();
					firstCard.Activated ();
				};
				model.FirstCompleted = (i) => {
					firstCard.Completed(i);
					secondCard.Activated();
				};
				model.SecondActivated = () => {
					secondCard.Activated();
					firstCard.Lock();
				};
				model.SecondCompleted = (i) => {
					secondCard.Completed(i);
					firstCard.Activated();
				};
				model.OneCompleted = async () => {
					await Task.Delay(1000);
					PulseOperator();
					SoundPlayer.SpeakPlus();
				};
			}
			public void UpdateTotal()
			{
				totalView.Total = model.Total;
			}
			public void PulseOperator()
			{
				operatorLabel.Pulse(2f);
			}
			public void PulseEquals()
			{
				equalLabel.Pulse(2f);
			}
			public void PulseFirst()
			{
				firstCard.Pulse ();
			}
			public void PulseSecond()
			{
				secondCard.Pulse ();
			}
			public void PulseTotal()
			{
				totalView.Pulse ();
			}

			public override void LayoutSubviews ()
			{
				base.LayoutSubviews ();

				const float padding = 50;

				var width = ((Bounds.Width - padding) / 3) - padding;
				var height = Bounds.Height - (padding * 2);

				var frame = new RectangleF (padding, padding, width, height);
				firstCard.Frame = frame;

				operatorLabel.Frame = new RectangleF (frame.Right, padding, padding, height);

				frame.X = frame.Right + padding;
				secondCard.Frame = frame;

				equalLabel.Frame = new RectangleF (frame.Right, padding, padding, height);

				frame.X = frame.Right + padding;
				totalView.Frame = frame;

			}
		}
	}
}

