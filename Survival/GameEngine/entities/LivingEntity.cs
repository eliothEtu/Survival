using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Survival
{
    internal class LivingEntity : Entity
    {
		private int life;

		public int Life
		{
			get 
			{ 
				return this.life; 
			}
			set 
			{ 
				if(value < 0)
				{
					throw new ArgumentOutOfRangeException("Hp must be positive");
					
				}
				this.life = value; 
			}
		}

        private bool bCanTakeDamage;
        public bool BCanTakeDamage { get => bCanTakeDamage; set => bCanTakeDamage = value; }

        private DateTime lastDamageTaken = DateTime.Now;

        public LivingEntity(string name, int life, BitmapImage texture, Vector2 position, Vector2 velocity) : base(name, texture, position, velocity)
		{
			this.Life = life;
			BCanTakeDamage = true;
		}

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
			this.Velocity = Vector2.Zero;

			if((DateTime.Now - this.lastDamageTaken).TotalMilliseconds > 1000)
			{
				//this.Rectangle.Fill = Brushes.White;
			}
        }

        public override void Collide(Entity otherEntity)
        {
            base.Collide(otherEntity);

			Console.WriteLine($"{this.Rect} -> {otherEntity.Rect}");

            float deltaX = this.Position.X - otherEntity.Position.X;
            float deltaY = this.Position.Y - otherEntity.Position.Y;

            Vector2 delta = Vector2.Normalize(new Vector2(deltaX, deltaY));
            this.Velocity = this.Velocity + delta;
        }

        public void TakeDamage(int damage)
		{
			if (BCanTakeDamage)
			if ((DateTime.Now - this.lastDamageTaken).TotalMilliseconds <= 1000) return;
            this.Life = Math.Max(0, this.Life - damage);
            this.lastDamageTaken = DateTime.Now;
			
        }
	}


	

}
