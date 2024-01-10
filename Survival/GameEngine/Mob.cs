using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survival
{
    internal class Mob : LivingEntity
    {
		private Player target;

		public Player Target
		{
			get 
			{ 
				return this.target; 
			}
			set 
			{ 
				this.target = value; 
			}
		}


		private int damage;

		public int Damage
		{
			get 
			{ 
				return this.damage; 
			}
			set 
			{ 
				if(value < 0)
				{
					throw new ArgumentOutOfRangeException("Damage must be positive");
				}
				this.damage = value; 
			}
		}


		public void GoToPlayer()
		{

		}
		public void AttackPlayer()
		{

		}


	}
}
