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
    Faire une fenêtre pour donner les items et leur bonus
    finir les collisions
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

        private MobSpawner mobSpawner = new MobSpawner();
        public MobSpawner MobSpawner { get => this.mobSpawner; }

        private DateTime timeStartGame, lastTick = DateTime.Now;

        private int moneyStartgame;
        public int MoneyStartgame { get => moneyStartgame; set => moneyStartgame = value; }
        public DateTime TimeStartGame { get => timeStartGame; set => timeStartGame = value; }

        MediaPlayer soundButton = new MediaPlayer();
        public Engine() 
        {
            if (instance != null)
            {
                throw new InvalidOperationException("Engine cannot be instancied twice.");
            }
            instance = this;

            this.Player = new Player("Player", 10, 2, new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\face.png")), new Vector2(0, 0), new Vector2(0f, 0f));
            this.Controller = new PlayerController();

            lastTick = DateTime.Now;

            soundButton.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Sound\\ButtonSound.mp3"));

            this.timer.Tick += Update;
            this.timer.Interval = TimeSpan.FromMilliseconds(16);
        }

        public void Start()
        {
            Vector2 sizeMap = new Vector2(20 * xValue, 20 * yValue);
            this.MapGenerator.SizeMap = sizeMap;

            this.MobSpawner.MaxDistanceSpawn = (ushort)Math.Clamp(this.MapGenerator.SizeMap.X / 2, 4, 15);

            this.MapGenerator.CreateMap();
            this.MapGenerator.SmoothMap(5);

            this.Entities.Add(player);
            player.Life = 10;
            player.Position = MapGenerator.GetPlayerSpawnPos();

            MobSpawner.Wave = 0;

            this.MoneyStartgame = Player.Money;
            this.TimeStartGame = DateTime.Now;

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
                if (entity is Player)
                {
                    OnPlayerDeath();
                    break;
                }
                this.Entities.Remove(entity);
            }
            this.EntityToRemove.Clear();

            this.MobSpawner.Update();

            this.lastTick = DateTime.Now;
        }

        public void OnPlayerDeath()
        {
            Entities.Clear();
            ((MainWindow)Application.Current.MainWindow).OpenDeathWindow();
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
