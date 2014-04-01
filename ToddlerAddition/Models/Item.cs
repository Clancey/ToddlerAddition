using System;

namespace ToddlerAddition
{
	public class Item
	{
		public Action<Item> Selected  = (i)=>{};

		public bool IsSelected { get; set; }

		public int Count { get; set; }

		public string Image {get;set;}
		public string Title {get;set;}
		public Item ()
		{

		}

		public void Select(int count)
		{
			IsSelected = true;
			Count = count + 1;
			Selected (this);
		}
	}
}

