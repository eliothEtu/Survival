using Survival.GameEngine;
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
		public List<Behavior> behaviors = new List<Behavior> ();

		public int FocusDistance { get; set; }

		public Mob(string name, int life, BitmapImage texture, Vector2 position, Vector2 velocity) : base(name, life, texture, position, velocity)
        {

        }

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			foreach (Behavior behavior in this.behaviors)
			{
				behavior.Update(this);
			}
		}

        public override void Collide(Entity otherEntity)
        {
            base.Collide(otherEntity);

            if (otherEntity is Mob)
            {
                float deltaX = this.Position.X - otherEntity.Position.X;
                float deltaY = this.Position.Y - otherEntity.Position.Y;

                Vector2 delta = Vector2.Normalize(new Vector2(deltaX, deltaY)) / 2;
                this.Velocity = this.Velocity + delta;
            }
        }


    }
}
