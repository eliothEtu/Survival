using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Survival
{
    internal class Weapons : Entity
    {
		private string name;

		public string Name
		{
			get 
			{ 
				return this.name; 
			}
			set 
			{ 
				if(value.Length == 0)
				{
					throw new ArgumentException("Weapon must have a name");
				}
				this.name = value; 
			}
		}

		private Rectangle rectange;

		public Rectangle Rectangle
		{
			get 
			{ 
				return this.rectange; 
			}
			set { this.rectange = value; }
		}

		private string description;

		public string Description
		{
			get 
			{ 
				return this.description; 
			}
			set 
			{ 
				this.description = value; 
			}
		}

		private string malus;

		public string Malus
		{
			get 
			{ 
				return this.malus; 
			}
			set 
			{ 
				this.malus = value; 
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
				if(value < 0 )
				{
					throw new ArgumentOutOfRangeException("Damage must be positive");
				}
				this.damage = value; 
			}
		}

		private int timeBeforeNextShot;

		public  int TimebeforeNextShot
		{
			get 
			{ 
				return this.timeBeforeNextShot; 
			}
			set 
			{ 
				if(value < 0)
				{
					throw new ArgumentOutOfRangeException("Time must be positive");
				}
				this.timeBeforeNextShot = value; 
			}
		}



	}
}
