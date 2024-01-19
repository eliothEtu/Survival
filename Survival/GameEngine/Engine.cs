using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Survival.GameEngine.world;
using Survival.GameEngine.entities.ai;
using System.Windows.Media;

/*
    faire le systeme d'argent pour ouvrir les coffres
    faire les images
    finir les collisions
    finir le take damage
    relancer la game
    son
 */

namespace Survival.GameEngine
{
    internal class Engine
    {
        private static Engine instance;
        public static Engine Instance { get => instance; }

        public static int xValue = 1, yValue = 1;

        public static BitmapImage imageExit = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\close.png"));

        private List<Entity> entities = new List<Entity>();
        public List<Entity> Entities { get => entities; }

        public DispatcherTimer timer = new DispatcherTimer();

        private Player player;
        public Player Player { get => player; set => player = value; }

        private PlayerController controller;
        public PlayerController Controller { get => controller; set => controller = value; }


        private MapGenerator mapGenerator = new MapGenerator();
        public MapGenerator MapGenerator { get => mapGenerator; }

        private Renderer renderer;
        public Renderer Renderer { get => this.renderer; set => this.renderer = value; }

        private List<Entity> entityToRemove = new List<Entity>();
        public List<Entity> EntityToRemove { get => entityToRemove; }
      
        private double soundVolume;
        public double SoundVolume { get => soundVolume; set => soundVolume = value; }

        public MobSpawner MobSpawner { get => this.mobSpawner; }

        private DateTime lastTick = DateTime.Now;

        MediaPlayer soundButton = new MediaPlayer();
        public Engine() 
        {
            if (instance != null)
            {
                throw new InvalidOperationException("Engine cannot be instancied twice.");
            }
            instance = this;

            this.Player = new Player("Player", 10, 5, new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\face.png")), new Vector2(0, 0), new Vector2(0f, 0f));
            this.Controller = new PlayerController();
            this.Entities.Add(Player);

            lastTick = DateTime.Now;

            soundButton.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Sound\\ButtonSound.mp3"));

            this.timer.Tick += Update;
            this.timer.Interval = TimeSpan.FromMilliseconds(16);
        }

        public void Start()
        {
            Vector2 sizeMap = new Vector2(10 * xValue, 10 * yValue);
            this.MapGenerator.SizeMap = sizeMap;

            this.MapGenerator.CreateMap();
            this.MapGenerator.SmoothMap(5);

            player.Position = MapGenerator.GetPlayerSpawnPos();

            /*Mob mob = new Mob("Mob", 100, new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\face.png")), MapGenerator.GetMobSpawnPos(2, 5), new Vector2(0f, 0f));
            FollowPlayerBehavior followPlayerBehavior = new FollowPlayerBehavior();
            followPlayerBehavior.Player = this.Player;
            mob.FocusDistance = 10;
            mob.behaviors.Add(followPlayerBehavior);
            this.Entities.Add(mob);*/

            this.Renderer = new Renderer();

            this.timer.Start();
        }

        public void Pause()
        {
            timer.Stop();
        }

        private void Update(object sender, EventArgs e)
        {
            TimeSpan deltaTime = DateTime.Now - this.lastTick;
            foreach (Entity entity in Entities)
            {
                entity.Update((float)deltaTime.TotalSeconds);

                foreach (Entity otherEntity  in Entities)
                {
                    if(entity.Rect.IntersectsWith(otherEntity.Rect) && entity != otherEntity)
                    {
                        entity.Collide(otherEntity);
                        otherEntity.Collide(entity);
                    }
                }
            }

            this.Renderer.UpdateCamera(Player.Rectangle, Player.Position);
            this.Renderer.Draw(Entities);

            foreach (Entity entity in this.entityToRemove)
            {
                this.Entities.Remove(entity);
            }
            this.EntityToRemove.Clear();

            this.MobSpawner.Update();

            this.lastTick = DateTime.Now;
        }

        public void OnPlayerDie()
        {
            timer.Stop();
            Entities.Clear();
            //Appel un fenetre de mort
        }

        public void SetVolumeSound()
        {
            soundButton.Volume = soundVolume / 100;
        }

        public void PlaySoundButton()
        {
            soundButton.Position = TimeSpan.Zero;
            soundButton.Play();
        }
    }
}
