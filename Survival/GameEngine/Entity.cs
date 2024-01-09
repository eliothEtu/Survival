using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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

		private double speed;

		public double Speed
		{
			get 
			{ 
				return this.speed; 
			}
			set 
			{ 
				if(value < 0)
				{
					throw new ArgumentOutOfRangeException("Speed must be positive");
				}
				this.speed = value; 
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







	}
}
