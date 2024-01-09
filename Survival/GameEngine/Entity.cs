using Survival.GameEngine;
using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Survival.GameEngine
{
    internal class Entity
    {
		private Vector2 position;

		public Vector2 Position
		{
			get 
			{ return this.position; 
			}
			set 
			{ 
				if(value.X < 0 || value.Y > 0)
				{
					throw new ArgumentOutOfRangeException("Y must be negative and X positive");
				}
				this.position = value; 
			}
		}

		private Vector2 velocity;

		public Vector2 Velocity
		{
			get 
			{ 
				return this.velocity; 
			}
			set 
			{ 
				this.velocity = value; 
			}
		}

		private Rectangle rectangle;

		public Rectangle Rectangle
		{
			get 
			{ 
				return this.rectangle; 
			}
			set 
			{ 
				this.rectangle = value; 
			}
		}

		private Rect rect;

        public Rect Rect
		{
			get { return rect; }
			set { rect = value; }
		}

		public Entity(BitmapImage texture, Vector2 position, Vector2 velocity)
		{
			this.Rectangle = new Rectangle();
			this.Rectangle.Width = texture.Width;
			this.Rectangle.Height = texture.Height;
			this.Rectangle.Fill = new ImageBrush(texture);
			this.Position = position;
			this.Velocity = velocity;
			this.Rect = new Rect();
		}

		public void Update()
		{

		}


	}
}
