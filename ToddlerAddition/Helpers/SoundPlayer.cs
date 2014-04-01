using System;
using MonoTouch.AVFoundation;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonoTouch.Foundation;
using iOSHelpers;

namespace ToddlerAddition
{
	public static class SoundPlayer
	{
		static string[] Intro = new string[0];
		static Dictionary<int,string[]> Numbers = new Dictionary<int, string[]>();
		static Dictionary<int,string[]> NumbersExcited = new Dictionary<int, string[]>();
		static string[] EqualsSounds = new string[0];
		static string[] Plus = new string[0];
		static string[] Congrats = new string[0];
		const string root = "Audio";
		static Random random = new Random ();
		static SoundPlayer()
		{
			Intro = Directory.EnumerateFiles (Path.Combine (root, "Intro")).ToArray ();
			EqualsSounds = Directory.EnumerateFiles (Path.Combine (root, "Equals")).ToArray ();
			Plus = Directory.EnumerateFiles (Path.Combine (root, "Plus")).ToArray ();
			Congrats = Directory.EnumerateFiles (Path.Combine (root, "Congrats")).ToArray ();
			foreach (var directory in Directory.EnumerateDirectories(Path.Combine(root,"Numbers"))) {
				var number = int.Parse (Path.GetFileName (directory));
				Numbers.Add(number, Directory.EnumerateFiles (directory).ToArray ());
			}
			foreach (var directory in Directory.EnumerateDirectories(Path.Combine(root,"NumbersExcited"))) {
				var number = int.Parse (Path.GetFileName (directory));
				NumbersExcited.Add(number, Directory.EnumerateFiles (directory).ToArray ());
			}
		}
		public static void Init()
		{

		}

		public static async Task Speak (int count) {

			var numbers = Numbers [count];
			await SpeakRandom(numbers);
			//return Speak (count.ToString ());
		}
		public static async Task SpeakExcited (int count) {

			var numbers = NumbersExcited [count];
			await SpeakRandom(numbers);
			//return Speak (count.ToString ());
		}

		public static async Task SpeakIntro()
		{
			await SpeakRandom (Intro);
		}
			

		public static async Task SpeakPlus()
		{
			await SpeakRandom (Plus);
		}

		public static async Task SpeakEquals()
		{
			await SpeakRandom (EqualsSounds);
		}

		public static async Task SpeakCongrats()
		{
			await SpeakRandom (Congrats);
		}

		static async Task SpeakRandom(string[] strings)
		{
			if (strings.Length == 1) {
				await PlayAudioFile (strings [0]);
				return;
			}
			var i = random.Next (strings.Length);
			var file = strings [i];
			await PlayAudioFile (file);
		}
		static List<AVAudioPlayer> players = new List<AVAudioPlayer>();
		static async Task<bool> PlayAudioFile(string path)
		{
			var tcs = new TaskCompletionSource<bool> ();
			var player = AVAudioPlayer.FromUrl (NSUrl.FromFilename (path));
			players.Add (player);
			player.FinishedPlaying += (object sender, AVStatusEventArgs e) => {
				Console.WriteLine ("done");
				tcs.TrySetResult(true);
			};
			Console.WriteLine ("play");
			player.Play ();
			var result = await tcs.Task;

			Console.WriteLine ("dispose");
			players.Remove (player);
			player.Dispose();
			return result;


		}
		public static Task Speak (string text) {
			var speechSynthesizer = new AVSpeechSynthesizer ();
			var tcs = new TaskCompletionSource<bool> ();
			var speechUtterance = new AVSpeechUtterance (text) {
				Rate = AVSpeechUtterance.MaximumSpeechRate/4,
				//Voice = AVSpeechSynthesisVoice.FromLanguage ("en-AU"),
				//Volume = 0.5f,
				//PitchMultiplier = 1.5f
			};
			EventHandler<AVSpeechSynthesizerUteranceEventArgs> del = null;
			del = (object sender, AVSpeechSynthesizerUteranceEventArgs e) => {
				if(e.Utterance == speechUtterance){
					tcs.TrySetResult(true);
					speechSynthesizer.DidFinishSpeechUtterance -= del;
				}
			};
			speechSynthesizer.DidFinishSpeechUtterance += del;

			speechSynthesizer.SpeakUtterance (speechUtterance);
			return tcs.Task;
		}
	}
}

