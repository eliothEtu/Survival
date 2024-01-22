using Survival.GameEngine;
using Survival.GameEngine.world;
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
    public class Entity
    {
		private Vector2 position;

		public Vector2 Position
		{
			get 
			{ return this.position; 
			}
			set 
			{ 
				this.position = value;
                this.rect = new Rect(this.position.X, this.position.Y, this.rectangle.Width / MapGenerator.BLOCK_SIZE, this.rectangle.Height / MapGenerator.BLOCK_SIZE);
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

        private double speed;
        public double Speed { get => speed; set => speed = value; }

        private Rect rect; public Rect Rect
		{
			get { return rect; }
			set { rect = value; }
		}


		private string name;
		public string Name { get => name; set => name = value; }

        public Entity(string name, BitmapImage texture, Vector2 position, Vector2 velocity)
		{
			this.Speed = 5;

            this.Rectangle = new Rectangle();
            this.Rectangle.Width = texture.Width;
            this.Rectangle.Height = texture.Height;
            //this.Rect = new Rect(position.X, position.Y, texture.Width, texture.Height);

            this.Position = position;
            this.Velocity = velocity;
            this.Name = name;

            Canvas.SetLeft(this.Rectangle, position.X);
			Canvas.SetTop(this.Rectangle, position.Y);

			this.Rectangle.Fill = new ImageBrush(texture);
		}

        public Entity(string name, ImageBrush texture, Vector2 position, Vector2 velocity)
        {
            this.Rectangle = new Rectangle();
            this.Rectangle.Width = MapGenerator.BLOCK_SIZE;
            this.Rectangle.Height = MapGenerator.BLOCK_SIZE;

            this.Rectangle.Fill = texture;
            //this.Rect = new Rect(position.X, position.Y, this.Rectangle.Width, this.Rectangle.Height);

            this.Position = position;
            this.Velocity = velocity;
            this.Name = name;

            

            Canvas.SetLeft(this.Rectangle, position.X);
            Canvas.SetTop(this.Rectangle, position.Y);

           
        }

        public virtual void Update(float deltaTime)
		{
			Vector2 newPos = this.Velocity != Vector2.Zero ? (this.Position + Vector2.Normalize(this.Velocity) * (float)this.Speed * deltaTime) : (this.Position + this.Velocity * (float)this.Speed * deltaTime);

            if (Engine.Instance.MapGenerator.IsInMap((int)newPos.X, (int)newPos.Y) && Engine.Instance.MapGenerator.Map[(int)newPos.X][(int)newPos.Y] == 0)
			{
				this.Position = newPos;
			}
		}

		public virtual void Collide(Entity otherEntity)
		{
			
		}

		public double GetDistanceFrom(Vector2 pos)
		{
			double distance1 = Math.Pow(this.Position.X, 2) + Math.Pow(this.Position.Y, 2);
			double distance2 = Math.Pow(pos.X, 2) + Math.Pow(pos.Y, 2);

			return Math.Sqrt(Math.Abs(distance1 - distance2));
		}
	}
}
