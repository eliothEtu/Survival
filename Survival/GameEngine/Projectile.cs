using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Survival
{
    internal class Projectile : Entity
    {
		private Vector2 spawnPosition;

		public Vector2 SpawnPosition
		{
			get 
			{ 
				return this.spawnPosition; 
			}
			set 
			{ 
				if(value.X < 0 || value.Y > 0)
				{
					throw new ArgumentOutOfRangeException("X must be positive and Y negative");
				}
				this.spawnPosition = value; 
			}
		}

		private Vector2 direction;

		public Vector2 Direction
		{
			get 
			{ 
				return this.direction; 
			}
			set 
			{ 
				this.direction = value; 
			}
		}

		private double lifeSpan;

		public double LifeSpan
		{
			get 
			{ 
				return this.lifeSpan; 
			}
			set 
			{ 
				if(value < 0)
				{
					throw new ArgumentOutOfRangeException("Life Span must be positive");
				}
				this.lifeSpan = value; 
			}
		}


        private LivingEntity owner;
        internal LivingEntity Owner { get => owner; set => owner = value; }

        public bool IsCollidingWith(Rect r)
		{
			if (this.Rect.IntersectsWith(r))
			{
				return true;
			}
			return false;
		}


	}
}
