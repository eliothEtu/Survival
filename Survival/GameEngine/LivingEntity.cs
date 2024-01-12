using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Survival
{
    internal class LivingEntity : Entity
    {
		private double life;

		public double Life
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

		public LivingEntity(double life, BitmapImage texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity)
		{
			this.Life = life;
		}

		public void TakeDamage(int damage)
		{
			if(this.Life - damage < 0)
			{
				this.Life = 0;
			}
			else
			{
				this.Life -= damage;
			}
		}
	}
}
