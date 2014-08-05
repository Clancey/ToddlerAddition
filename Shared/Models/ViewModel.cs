using System;
using System.Collections.Generic;
using System.Linq;

namespace ToddlerAddition
{
	public class ViewModel : IDisposable
	{
		public int Level { get; set; }

		static Random Random = new Random();

		public bool FirstComplete { get; private set; }
		public bool SecondComplete {get; private set;}

		bool equalsTapped;
		public bool EqualsTapped {
			get {
				return equalsTapped;
			}
			private set {
				if (equalsTapped == value)
					return;
				equalsTapped = value;
				Update ();
			}
		}
		int guessedValue;
		public int GuessedValue {
			get {
				return guessedValue;
			}
			set {
				if(guessedValue == value)
					return;
				guessedValue = value;
				if (guessedValue == Total)
					FlipTotal ();
				Update ();
			}
		}
		public int FirstCount {get; private set;}
		public int SecondCount {get; private set;}
		public int Total {get; private set;}
		public List<Item> FirstItems = new List<Item>();
		public List<Item> SecondItems = new List<Item> ();

		public Action FlipTotal = () => {};

		public Action<int> FirstCompleted = (i)=> {};

		public Action<int> SecondCompleted = (i)=> {};

		public Action FirstActivated  = ()=> {};

		public Action SecondActivated = ()=> {};

		public Action Finished = ()=> {};

		public Action WrongValue = () => {};

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
			if (IsFinished())
				Finished ();
			else if (oneCompleted)
				OneCompleted();
		}

		public void TappedTotal ()
		{
			if (AreBothFinished ())
				EqualsTapped = true;
			FlipTotal ();
		}

		public void TappedEquals()
		{
			if (Level != 1)
				return;
			if (!FirstComplete || !SecondComplete)
				return;
			EqualsTapped = true;
		}
		public bool AreBothFinished()
		{
			return FirstComplete && SecondComplete;
		}

		bool IsFinished()
		{
			if (!AreBothFinished ())
				return false;
			if (Level == 0)
				return true;
			if (Level == 1)
				return EqualsTapped;
			if (Level == 2) {
				if (guessedValue > 0 && Total != guessedValue) {
					guessedValue = 0;
					WrongValue ();
					return false;
				}
				return guessedValue == Total;
			}
			return false;
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
			var vm = new ViewModel {
				Level = Settings.CurrentLevel,
			};
			var itemImage = ItemHelper.Next ();
			var itemTitle = System.IO.Path.GetFileNameWithoutExtension (itemImage);
			Enumerable.Range (1, first).ForEach (x => {
				vm.FirstItems.Add(new Item{
					Image = itemImage,
					Title = itemTitle,
					Selected = async (i)=>	{
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

