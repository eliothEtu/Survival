using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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

        private double baseDamage = 20;

        public double BaseDamage
        {
            get
            {
                return this.baseDamage;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Damage must be positive");
                }
                this.baseDamage = value;
            }
        }


        private bool bCanTakeDamage;
        public bool BCanTakeDamage { get => bCanTakeDamage; set => bCanTakeDamage = value; }

        protected DateTime lastDamageTaken = DateTime.Now;

        public LivingEntity(string name, int life, BitmapImage texture, Vector2 position, Vector2 velocity) : base(name, texture, position, velocity)
		{
			this.Life = life;
			this.BCanTakeDamage = true;
		}

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

			if (this.Velocity.X > 0)
			{
				this.Velocity = new Vector2((float)Math.Max(0, this.Velocity.X - 0.1), this.Velocity.Y);
			}
			else if (this.Velocity.X < 0)
			{
                this.Velocity = new Vector2((float)Math.Min(0, this.Velocity.X + 0.1), this.Velocity.Y);
            }

            if (this.Velocity.Y > 0)
            {
                this.Velocity = new Vector2(this.Velocity.X, (float)Math.Max(0, this.Velocity.Y - 0.1));
            }
            else if (this.Velocity.Y < 0)
            {
                this.Velocity = new Vector2(this.Velocity.X, (float)Math.Min(0, this.Velocity.Y + 0.1));
            }
        }

        public override void Collide(Entity otherEntity)
        {
            base.Collide(otherEntity);

			if (otherEntity is Projectile && ((Projectile) otherEntity).Owner != this)
			{
				Projectile projectile = (Projectile) otherEntity;
				LivingEntity owner = projectile.Owner;
				this.TakeDamage(owner.BaseDamage);

				Engine.Instance.EntityToRemove.Add(otherEntity);
			}
		
        }
		
        public void TakeDamage(double damage)
		{
			if (!BCanTakeDamage) return;
			if ((DateTime.Now - this.lastDamageTaken).TotalMilliseconds <= 1000) return;
            this.Life = Math.Max(0, this.Life - damage);
            this.lastDamageTaken = DateTime.Now;

			if (this.Life <= 0)
			{
				this.OnDeath();
			}
        }

		public virtual void OnDeath()
		{
			Engine.Instance.EntityToRemove.Add(this);

			if (!(this is Player))
			{
				Engine.Instance.Player.Money = Engine.Instance.Player.Money + (int)(Engine.Instance.MobSpawner.Wave * Engine.Instance.MobSpawner.WaveMultiplier);
				Engine.Instance.Player.MobKill++;
			}
		}
	}


	

}
