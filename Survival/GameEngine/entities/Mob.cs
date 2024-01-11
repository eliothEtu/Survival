using Survival.GameEngine.entities.ai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Survival
{
    internal class Mob : LivingEntity
    {
		public List<Behavior> Behaviors { get; set; }


		private int baseDamage;

		public int BaseDamage
		{
			get 
			{ 
				return this.baseDamage; 
			}
			set 
			{ 
				if(value < 0)
				{
					throw new ArgumentOutOfRangeException("Damage must be positive");
				}
				this.baseDamage = value; 
			}
		}

        public Mob(int life, BitmapImage texture, Vector2 position, Vector2 velocity) : base(life, texture, position, velocity)
        {

        }

		public override void Update()
		{
			base.Update();

			foreach (Behavior behavior in Behaviors)
			{
				behavior.Update(this);
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
