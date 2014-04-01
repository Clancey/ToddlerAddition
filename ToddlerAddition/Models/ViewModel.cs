using System;
using System.Collections.Generic;
using System.Linq;

namespace ToddlerAddition
{
	public class ViewModel : IDisposable
	{

		static Random Random = new Random();

		public bool FirstComplete { get; private set; }
		public bool SecondComplete {get; private set;}
		public int FirstCount {get; private set;}
		public int SecondCount {get; private set;}
		public int Total {get; private set;}
		public List<Item> FirstItems = new List<Item>();
		public List<Item> SecondItems = new List<Item> ();

		public Action<int> FirstCompleted = (i)=> {};

		public Action<int> SecondCompleted = (i)=> {};

		public Action FirstActivated  = ()=> {};

		public Action SecondActivated = ()=> {};

		public Action Finished = ()=> {};

		public Action OneCompleted = () => {};

		public ViewModel ()
		{

		}

		public void Update()
		{
			FirstCount = FirstItems.Max (x => x.Count);
			SecondCount = SecondItems.Max (x => x.Count);
			Total = FirstCount + SecondCount;
			bool oneCompleted = false;
			if (!FirstComplete) {
				FirstComplete = FirstItems.Count == FirstCount;
				if (FirstComplete) {
					oneCompleted = true;
					FirstCompleted (FirstCount);
				}
			}
			if (!SecondComplete) {
				SecondComplete = SecondItems.Count == SecondCount;
				if (SecondComplete) {
					oneCompleted = true;
					SecondCompleted (SecondCount);
				}
			}
			if (FirstComplete && SecondComplete)
				Finished ();
			else if (oneCompleted)
				OneCompleted();
		}

		#region IDisposable implementation
		public void Dispose ()
		{
			FirstItems.ForEach (x => {
				x = null;
			});

			SecondItems.ForEach (x => {
				x = null;
			});
		}
		#endregion

		public static ViewModel CreateAddition(int max)
		{
			var count = Random.Next (2, max + 1);
			var first = Random.Next (1, count);
			var second = count - first;
			var vm = new ViewModel ();
			var itemImage = ItemHelper.Next ();
			var itemTitle = System.IO.Path.GetFileNameWithoutExtension (itemImage);
			Enumerable.Range (1, first).ForEach (x => {
				vm.FirstItems.Add(new Item{
					Image = itemImage,
					Title = itemTitle,
					Selected = (i)=>	{
						SoundPlayer.Speak(i.Count);
						vm.FirstActivated();
						vm.Update();
					}
				});
			});

			Enumerable.Range (1, second).ForEach (x => {
				vm.SecondItems.Add(new Item{
					Image = itemImage,
					Title = itemTitle,
					Selected = (i)=>	{
						SoundPlayer.Speak(i.Count);
						vm.SecondActivated();
						vm.Update();
					}
				});
			});

			return vm;

		}
	}
}

