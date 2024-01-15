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

namespace Survival.GameEngine
{
    internal class Engine
    {
        private static Engine instance;
        public static Engine Instance { get => instance; }

        private List<Entity> entities = new List<Entity>();
        public List<Entity> Entities { get => entities; }

        public AsyncTimer timer = new AsyncTimer();

        private Player player;
        public Player Player { get => player; set => player = value; }

        private PlayerController controller;
        public PlayerController Controller { get => controller; set => controller = value; }


        private MapGenerator mapGenerator = new MapGenerator();
        public MapGenerator MapGenerator { get => mapGenerator; }

        private Renderer renderer;
        public Renderer Renderer { get => this.renderer; set => this.renderer = value; }

        public Engine() 
        {
            if (instance != null)
            {
                throw new InvalidOperationException("Engine cannot be instancied twice.");
            }
            instance = this;

            this.Player = new Player("Player", 1, new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\face.png")), new Vector2(0f, 0f), new Vector2(0f, 0f));
            this.Controller = new PlayerController();
            this.Entities.Add(Player);


            Mob mob = new Mob("Mob", 100, new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\face.png")), new Vector2(100f, 100f), new Vector2(0f, 0f));
            FollowPlayerBehavior followPlayerBehavior = new FollowPlayerBehavior();
            followPlayerBehavior.Player = this.Player;
            mob.FocusDistance = 10;
            mob.behaviors.Add(followPlayerBehavior);
            Entities.Add(mob);
            //this.Entities.Add(mob);

            this.timer.OnTick += Update;
            this.timer.Interval = TimeSpan.FromMilliseconds(16);
        }

        public void Start()
        {
            this.MapGenerator.CreateMap();
            this.MapGenerator.SmoothMap(5);

            this.Renderer = new Renderer();

            this.timer.Start();
        }

        public void Pause()
        {
            timer.Stop();
        }

        private void Update()
        {            
            foreach (Entity entity in Entities)
            {
                
                entity.Update();

                foreach(Entity otherEntity  in Entities)
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
        }
    }
}
