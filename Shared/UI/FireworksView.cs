using System;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using System.Drawing;

namespace ToddlerAddition
{
	public class FireworksView : UIView
	{
		public FireworksView ()
		{
			BackgroundColor = UIColor.Clear;
		}
		CAEmitterLayer mortor;
		public void Start()
		{

			string fileName = NSBundle.MainBundle.PathForResource ("tspark", "png");
			var dataProvider = CGDataProvider.FromFile (fileName);
			var img = CGImage.FromPNG (dataProvider, null, false, CGColorRenderingIntent.Default);
			mortor = new CAEmitterLayer {
				Position =new PointF (Bounds.Width / 2, 0),
				RenderMode = CAEmitterLayer.RenderAdditive,
			};
			var pi = (float)Math.PI;
			var rocket =new  CAEmitterCell();
			rocket.EmissionLongitude = pi / 3;
			rocket.EmissionLatitude = 0;
			rocket.LifeTime = 1.6f;
			rocket.BirthRate = 1.5f;
			rocket.Velocity = 40;
			rocket.VelocityRange = 600;
			rocket.AccelerationY = -250;
			rocket.EmissionRange = pi / 4;
			var color = UIColor.FromRGBA (.5f, .5f, .5f, .5f).CGColor;
			rocket.Color = color;
			rocket.RedRange = 0.5f;
			rocket.GreenRange = 0.5f;
			rocket.BlueRange = 0.5f;
			rocket.Name = "rocket";

			CAEmitterCell flare = CAEmitterCell.EmitterCell();
			flare.Contents = img;
			flare.EmissionLongitude = (4 * pi) / 2;
			flare.Scale = 0.4f;
			flare.Velocity = 100;
			flare.BirthRate = 45;
			flare.LifeTime = 1.5f;
			flare.AccelerationY = -350;
			flare.EmissionRange = pi / 7;
			flare.AlphaSpeed = -0.7f;
			flare.ScaleSpeed = -0.1f;
			flare.ScaleRange = 0.1f;
			flare.BeginTime = 0.01;
			flare.Duration = 0.7;

			CAEmitterCell firework = CAEmitterCell.EmitterCell();
			firework.Contents = img;
			firework.BirthRate = 9999;
			firework.Scale = 0.6f;
			firework.Velocity = 130;
			firework.LifeTime = 2;
			firework.AlphaSpeed = -0.2f;
			firework.AccelerationY = -80;
			firework.BeginTime = 1.5;
			firework.Duration = 0.1;
			firework.EmissionRange = 2 * pi;
			firework.ScaleSpeed = -0.1f;
			firework.Spin = 2;
			firework.Name = ("firework");

			CAEmitterCell preSpark = CAEmitterCell.EmitterCell();
			preSpark.BirthRate = 80;
			preSpark.Velocity = firework.Velocity * 0.7f;
			preSpark.LifeTime = 1.7f;
			preSpark.AccelerationY = firework.AccelerationY * 0.85f;
			preSpark.BeginTime = firework.BeginTime - 0.2;
			preSpark.EmissionRange = firework.EmissionRange;
			preSpark.GreenSpeed = 100;
			preSpark.BlueSpeed = 100;
			preSpark.RedSpeed = 100;
			preSpark.Name = ("preSpark");
			CAEmitterCell spark = CAEmitterCell.EmitterCell();
			spark.Contents = img;
			spark.LifeTime = 0.05f;

			spark.AccelerationY = -250;
			spark.BeginTime = 0.8;
			spark.Scale = 0.4f;
			spark.BirthRate = 10;
			preSpark.Cells = new []{spark};
			rocket.Cells = new []{flare, firework, preSpark};
			mortor.Cells = new []{rocket};
			this.Layer.AddSublayer(mortor);
			SetNeedsLayout ();
			this.Layer.SetNeedsDisplay ();
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			if(mortor != null)
				mortor.Position = new PointF (Bounds.Width / 2, Bounds.Height);


		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			mortor.Dispose ();
		}
	}
}

