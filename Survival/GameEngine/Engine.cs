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

namespace Survival.GameEngine
{
    internal class Engine
    {
        private static Engine instance;
        public static Engine Instance { get => instance; }

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
        

        public Engine() 
        {
            if (instance != null)
            {
                throw new InvalidOperationException("Engine cannot be instancied twice.");
            }
            instance = this;

            this.Player = new Player(1, new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\face.png")), new Vector2(0f, 0f), new Vector2(0f, 0f));
            this.Controller = new PlayerController();
            this.Entities.Add(Player);
            this.timer.Tick += Update;
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

        private void Update(object sender, EventArgs e)
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

        public void SpawnEntity(Entity entity)
        {
            this.Entities.Add(entity);
            ((MainWindow)Application.Current.MainWindow).canv.Children.Add(entity.Rectangle);
        }

        public void RemoveEntity(Entity entity)
        {
            this.Entities.Remove(entity);
            ((MainWindow) Application.Current.MainWindow).canv.Children.Remove(entity.Rectangle);
        }
    }
}
