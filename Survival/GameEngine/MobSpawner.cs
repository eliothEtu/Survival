using Survival.GameEngine.entities.ai;
using Survival.GameEngine.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Survival.GameEngine
{
    public class MobSpawner
    {
        private int wave = 1;
        private double waveMultiplier = 3.5;

        public void SpawnMobs()
        {
            for (int i = 0; i < (int)(wave * waveMultiplier); i++)
            {
                Mob mob = new Mob("Mob" + i.ToString(), 100, new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\face.png")), Engine.Instance.MapGenerator.GetMobSpawnPos(2, 15), new Vector2(0f, 0f));
                FollowPlayerBehavior followPlayerBehavior = new FollowPlayerBehavior();
                followPlayerBehavior.Player = Engine.Instance.Player;
                mob.FocusDistance = 100;
                mob.behaviors.Add(followPlayerBehavior);
                Engine.Instance.Entities.Add(mob);
            }
        }

        public void EndWave()
        {
            this.wave++;
            this.SpawnMobs();
        }
    }
}
