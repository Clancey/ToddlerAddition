﻿using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using GoogleAnalytics.iOS;

namespace ToddlerAddition
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		public IGAITracker Tracker;

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			GAI.SharedInstance.DispatchInterval = 20;

			// Optional: automatically send uncaught exceptions to Google Analytics.
			GAI.SharedInstance.TrackUncaughtExceptions = true;

			// Initialize tracker.
			#if LITE
			Tracker = GAI.SharedInstance.GetTracker ("UA-49576326-1");
			window.RootViewController = new IADViewController(new MainViewController ());
			#else
			Tracker = GAI.SharedInstance.GetTracker ("UA-49576326-2");
			window.RootViewController = new MainViewController ();
			#endif
			// make the window visible
			window.MakeKeyAndVisible ();
			SoundPlayer.Init ();
			return true;
		}
	}
}

