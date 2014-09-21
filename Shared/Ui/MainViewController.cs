using System;
using UIKit;
using System.Linq;
using iOSHelpers;
using System.Threading.Tasks;
using CoreGraphics;

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
			view.Parent = this;
			//GAI.SharedInstance.DefaultTracker.Set (GAIConstants.ScreenName, "Main View");

			//GAI.SharedInstance.DefaultTracker.Send (GAIDictionaryBuilder.CreateAppView ().Build ());
		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			view.Parent = null;
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

			if(model.Level == 0)
				await Task.Delay(300);

			view.PulseTotal ();
			if(model.HasFireworks)
				view.ShowFireworks ();
			await SoundPlayer.SpeakExcited(model.Total);
			await Task.WhenAll (Task.Delay (4000), SoundPlayer.SpeakCongrats ());
			view.HideFireworks ();
			newModel ();
		}


		class MainView : UIView
		{
			public MainViewController Parent;
			CardView firstCard;
			CardView secondCard;
			TotalView totalView;
			UILabel operatorLabel;
			UIButton equalLabel;
			LongPressButton settingsButton;
			NumberBar topNumbers;
			NumberBar bottomNumbers;

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
				AddSubview(equalLabel = new SimpleButton{
					//AdjustsFontSizeToFitWidth = true,
					Font = UIFont.BoldSystemFontOfSize(100),
					//TextAlignment = UITextAlignment.Center,
					TitleColor = UIColor.Black,
					Title = "=",
					Tapped = s => model.TappedEquals ()
				});
				equalLabel.TitleLabel.AdjustsFontSizeToFitWidth = true;
				AddSubview(totalView = new TotalView{
					Tapped = () => model.TappedTotal ()
				});
				AddSubview(settingsButton = new LongPressButton{
					Tapped = () => Parent.PresentViewControllerAsync (new UINavigationController (new SettingsViewController ()), true),
				});

				topNumbers = new NumberBar{
					Tapped = (i) =>{model.GuessedValue = i;}
				};
				topNumbers.SetRange(1,5);
				bottomNumbers = new NumberBar{
					Tapped = (i) =>{model.GuessedValue = i;}
				};;
				bottomNumbers.SetRange(6,10);
				fireworks = new FireworksView();
				fireworks.Start();
			}

			public void ShowFireworks()
			{
				AddSubview (fireworks);
			}
			public void HideFireworks()
			{
				fireworks.RemoveFromSuperview ();
			}
			FireworksView fireworks;
			ViewModel model;
			public void SetupModel(ViewModel model)
			{
				this.model = model;
				firstCard.Clear ();
				secondCard.Clear ();
				totalView.Clear ();
				HideNumbers ();
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
					if(!model.AreBothFinished()){
						PulseOperator();
						SoundPlayer.SpeakPlus();
					}
					else if(model.Level == 1)
					{
						await SoundPlayer.SpeakQuestion();
						totalView.Activated();
					}
					else
					{
						await SoundPlayer.SpeakQuestion();
						ShowNumbers();
					}
				};
				model.FlipTotal = () => {
					UpdateTotal();
				};
				model.WrongValue = () => {
					//TODO: play error
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
			public void ShowNumbers()
			{
				if (topNumbers.Superview != this) {
					var frame = new CGRect (0, -padding, Bounds.Width, padding);
					topNumbers.Frame = frame;
					frame.Y = Bounds.Bottom;
					bottomNumbers.Frame = frame;
					AddSubview (topNumbers);
					AddSubview (bottomNumbers);
					AnimateAsync (.3, () => {
						frame.Y -= frame.Height;
						bottomNumbers.Frame = frame;

						frame.Y = 0;
						topNumbers.Frame = frame;
					});
				}
			}

			public async void HideNumbers()
			{
				if (topNumbers.Superview != this)
					return;
				var frame = topNumbers.Frame;
				await AnimateAsync (.3, () => {
					frame.Y = -frame.Height;
					topNumbers.Frame = frame;

					frame.Y = Bounds.Bottom;
					bottomNumbers.Frame = frame;
				});

				topNumbers.RemoveFromSuperview ();
				bottomNumbers.RemoveFromSuperview ();
			}

			static nfloat padding = 50;
			public override void LayoutSubviews ()
			{
				base.LayoutSubviews ();

				fireworks.Frame = Bounds;
				var width = ((Bounds.Width - padding) / 3) - padding;
				var height = Bounds.Height - (padding * 2);

				var frame = new CGRect (padding, padding, width, height);
				firstCard.Frame = frame;

				operatorLabel.Frame = new CGRect (frame.Right, padding, padding, height);

				frame.X = frame.Right + padding;
				secondCard.Frame = frame;

				equalLabel.Frame = new CGRect (frame.Right, padding, padding, height);

				frame.X = frame.Right + padding;
				totalView.Frame = frame;

				frame = Bounds;
				frame.Y = frame.Bottom - 50;
				frame.Height = 44;
				frame.X = 10;
				frame.Width = 44;
				settingsButton.Frame = frame;

			}
		}
	}
}

