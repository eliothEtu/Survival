using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survival
{
    internal class LivingEntity : Entity
    {
		private int hp;

		public int Hp
		{
			get 
			{ 
				return this.hp; 
			}
			set 
			{ 
				if(value < 0)
				{
					throw new ArgumentOutOfRangeException("Hp must be positive");
					
				}
				this.hp = value; 
			}
		}
		public void TakeDamage(int damage)
		{
			if(this.Hp - damage < 0)
			{
				this.Hp = 0;
			}
			else
			{
				this.Hp -= damage;
			}
		}
	}


	

}
