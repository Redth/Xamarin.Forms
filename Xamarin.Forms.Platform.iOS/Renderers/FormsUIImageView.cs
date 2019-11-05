﻿using CoreGraphics;
using UIKit;
using RectangleF = CoreGraphics.CGRect;

namespace Xamarin.Forms.Platform.iOS
{
	public class FormsUIImageView : UIImageView
	{
		const string AnimationLayerName = "FormsUIImageViewAnimation";
		FormsCAKeyFrameAnimation _animation;

		public FormsUIImageView() : base(RectangleF.Empty)
		{
		}

		public override UIImage Image
		{
			get
			{
				return base.Image;
			}
			set
			{
				base.Image = value;
			}
		}

		public override CGSize SizeThatFits(CGSize size)
		{
			if (Image == null && Animation != null)
			{
				return new CoreGraphics.CGSize(Animation.Width, Animation.Height);
			}

			return base.SizeThatFits(size);
		}

		public FormsCAKeyFrameAnimation Animation
		{
			get { return _animation; }
			set
			{
				if (_animation != null)
				{
					Layer.RemoveAnimation(AnimationLayerName);
					_animation.Dispose();
				}

				_animation = value;
				if (_animation != null)
				{
					Layer.AddAnimation(_animation, AnimationLayerName);
				}

				Layer.SetNeedsDisplay();
			}
		}

		public override bool IsAnimating
		{
			get
			{
				if (_animation != null)
					return Layer.Speed != 0.0f;
				else
					return base.IsAnimating;
			}
		}

		public override void StartAnimating()
		{
			if (_animation != null && Layer.Speed == 0.0f)
			{
				Layer.RemoveAnimation(AnimationLayerName);
				Layer.AddAnimation(_animation, AnimationLayerName);
				Layer.Speed = 1.0f;
			}
			else
			{
				base.StartAnimating();
			}
		}

		public override void StopAnimating()
		{
			if (_animation != null && Layer.Speed != 0.0f)
			{
				Layer.RemoveAnimation(AnimationLayerName);
				Layer.AddAnimation(_animation, AnimationLayerName);
				Layer.Speed = 0.0f;
			}
			else
			{
				base.StopAnimating();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && _animation != null)
			{
				Layer.RemoveAnimation(AnimationLayerName);
				_animation.Dispose();
				_animation = null;
			}

			base.Dispose(disposing);
		}
	}
}