using System;
using UIKit;

namespace ToddlerAddition
{
	public class SettingsViewController : UIViewController
	{
		SettingsView view;
		public SettingsViewController ()
		{
			Title = "Settings";
			EdgesForExtendedLayout = UIRectEdge.None;
			this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done,(s,e)=>{
				this.DismissViewControllerAsync(true);
			});
		}
		public override void LoadView ()
		{
			View = view = new SettingsView ();
		}
		class SettingsView : UIView
		{
			UILabel settings;
			UILabel language;
			UIButton languageBtn;
			//SettingsInstructionView mode1;
			SettingsCardView card1,card2,card3;
			public SettingsView()
			{
				BackgroundColor = UIColor.White;
				Add(settings = new UILabel{
					Text = "Settings"
				});
				Add(language = new UILabel{
					Text = "Language",
				});
				Add(languageBtn = new UIBorderedButton{
					Title = "English",
				});

				Add(card1 = new SettingsCardView{
					Title = "Level 1:",
					Subtitle = "Learn",
					Progress = 1,
				});

				Add(card2 = new SettingsCardView{
					Title = "Level 2:",
					Subtitle = "Practice",
					Progress = .75f,
				});

				Add(card3 = new SettingsCardView{
					Title = "Level 3:",
					Subtitle = "Quiz",
				});
			}
			public override void LayoutSubviews ()
			{
				base.LayoutSubviews ();
				var frame = Bounds;
				const float padding = 10f;
				frame.Y = 40;
				frame.Height -= frame.Y;
				frame.Width -= padding * 4;
				frame.Width /= 3;
				frame.X = padding;
				card1.Frame = frame;

				frame.X = frame.Right + padding;
				card2.Frame = frame;

				frame.X = frame.Right + padding;
				card3.Frame = frame;
			}
		}
	}
}

