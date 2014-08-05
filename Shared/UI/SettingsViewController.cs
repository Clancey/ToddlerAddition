using System;
using MonoTouch.UIKit;

namespace ToddlerAddition
{
	public class SettingsViewController : UIViewController
	{
		SettingsView view;
		public SettingsViewController ()
		{
			Title = "Settings";
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
			public SettingsView()
			{
				BackgroundColor = UIColor.White;
				Add(settings = new UILabel{
					Text = "Settings"
				});
				AddSubview(language = new UILabel{
					Text = "Language",
				});
				AddSubview(languageBtn = new UIBorderedButton{
					Title = "English",
				});
			}
		}
	}
}

