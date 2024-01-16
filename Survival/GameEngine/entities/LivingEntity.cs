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

		public LivingEntity(string name, int life, BitmapImage texture, Vector2 position, Vector2 velocity) : base(name, texture, position, velocity)
		{
			this.Life = life;
		}

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
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
