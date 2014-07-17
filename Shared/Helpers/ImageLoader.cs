using System;
using System.Collections.Generic;
using MonoTouch.UIKit;

namespace ToddlerAddition
{
	public static class ImageLoader
	{
		static Dictionary<string,UIImage> images = new Dictionary<string, UIImage>();

		public static UIImage Load(string image)
		{
			if(images.ContainsKey(image))
				return images[image];

			var i = UIImage.FromBundle (image);
			return i;
		}
	}
}

