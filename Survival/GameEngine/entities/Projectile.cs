﻿using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Survival
{
    internal class Projectile : Entity
    {
		private TimeSpan lifeSpan;

		public TimeSpan LifeSpan
		{
			get 
			{ 
				return this.lifeSpan; 
			}
			set 
			{ 
				this.lifeSpan = value; 
			}
		}

		private DateTime spawnTime;

        private LivingEntity owner;
        internal LivingEntity Owner { get => owner; set => owner = value; }

        public Projectile(string name, LivingEntity owner, TimeSpan lifeSpan, ImageBrush texture, Vector2 position, Vector2 velocity) : base(name, texture, position, velocity)
        {
			this.Owner = owner;			
			this.LifeSpan = lifeSpan;
			spawnTime = DateTime.Now;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

			if (DateTime.Now - spawnTime > lifeSpan)
			{
				Engine.Instance.EntityToRemove.Add(this);
			}
        }

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
