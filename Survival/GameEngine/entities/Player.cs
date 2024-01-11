using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Survival
{
    internal class Player : LivingEntity
    {
		private List<string> inventory;

		public List<string> Inventory
		{
			get 
			{ 
				return this.inventory; 
			}
			set 
			{ 
				this.inventory = value; 
			}
		}

        private string itemEquiped;

        public string ItemEquiped
        {
            get
            {
                return this.itemEquiped;
            }
            set
            {
                this.itemEquiped = value;
            }
        }

		private int money;

		public int Money
		{
			get 
			{ 
				return this.money; 
			}
			set 
			{ 
				if(value < 0)
				{
					throw new ArgumentOutOfRangeException("Money must be positive");
				}
				this.money = value; 
			}
		}

		public Player(int life, BitmapImage texture, Vector2 position, Vector2 velocity): base (life, texture, position, velocity)
		{

		}

        public override void Update()
        {
            base.Update();
        }

        public void Fire()
		{

		}

	}

	

}
