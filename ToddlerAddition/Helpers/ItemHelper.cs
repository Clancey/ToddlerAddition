using System;
using System.Collections.Generic;
using System.IO;

namespace ToddlerAddition
{
	public static class ItemHelper
	{
		public static List<string> Items = new List<string>();

		static Random Random = new Random();
		static ItemHelper()
		{
			foreach (var file in Directory.GetFiles("Images","*.png")) {
				Items.Add (file);
			}
			//var item = 
		}

		public static string Next()
		{
			var i = Random.Next (0, Items.Count);
			return Items [i];
		}


	}
}

