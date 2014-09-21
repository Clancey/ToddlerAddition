using System;
using UIKit;
using System.Collections.Generic;

namespace ToddlerAddition
{
	public static class ColorSelector
	{

		static Random Random = new Random();

		public static UIColor Next()
		{
			var i = Random.Next (0, colors.Count);
			return colors [i];
//			var r = Random.Next (0, 255);
//			var b = Random.Next (0, 255);
//			var g = Random.Next (0, 255);
//			return UIColor.FromRGB (r, g, b);
		}

		static List<UIColor> colors = new List<UIColor>{
			UIColor.FromRGB(150,193,54), //light green
			UIColor.Black,
			UIColor.FromRGB(203,31,70), //red
			UIColor.FromRGB(117,174,206), //light blue
			UIColor.FromRGB(220,56,10), //Orange
			UIColor.FromRGB(126,57,146), //Mauve
			UIColor.FromRGB(248,231,55), //yellow
			UIColor.FromRGB(98,48,141), //purple
			UIColor.FromRGB(214,69,110), //pink
			UIColor.Green,
			UIColor.Yellow,
		};
	}
}

