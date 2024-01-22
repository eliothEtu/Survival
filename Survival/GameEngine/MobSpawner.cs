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
        private readonly List<BitmapImage> image = new List<BitmapImage> { new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\monster\\flameMonster.png")),
                                                                           new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\monster\\skeleton.png")),
                                                                           new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\monster\\zombie.png")),
                                                                           new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\monster\\zombie2.png")) };

        private int wave = 0;
        private double waveMultiplier = 3.5;

        private ushort maxDistanceSpawn;
        public ushort MaxDistanceSpawn { get => maxDistanceSpawn; set => maxDistanceSpawn = value; }

        public int Wave { get => this.wave; set => this.wave = value; }
        public double WaveMultiplier
        {
            get => this.waveMultiplier;
            set => this.waveMultiplier = value;
        }

        private Random rand = new Random();

        public void SpawnMobs()
        {
            for (int i = 0; i < (int)(wave * waveMultiplier); i++)
            {
                Mob mob = new Mob("Mob" + i.ToString(), 3 * wave, image[rand.Next(image.Count)], Engine.Instance.MapGenerator.GetMobSpawnPos(2, this.MaxDistanceSpawn), new Vector2(0f, 0f));
                FollowPlayerBehavior followPlayerBehavior = new FollowPlayerBehavior();
                followPlayerBehavior.Player = Engine.Instance.Player;
                mob.FocusDistance = 100;
                mob.behaviors.Add(followPlayerBehavior);
                mob.BaseDamage = wave;
                Engine.Instance.Entities.Add(mob);
            }
        }

        public void StartNewWave()
        {
            this.wave++;
            this.SpawnMobs();
        }

        public void Update()
        {
            if (Engine.Instance.Entities.Where(e => e is Mob).Count() == 0)
            {
                this.StartNewWave();
            }
        }
    }
}
