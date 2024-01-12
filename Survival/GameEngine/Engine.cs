using Survival.MalwareStuff;
using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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

        private Camera camera;
        public Camera Camera { get => camera; set => camera = value; }

        public Engine() 
        {
            if (instance != null)
            {
                throw new InvalidOperationException("Engine cannot be instancied twice.");
            }
            instance = this;

            Player = new Player(1, new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\faceR.png")), new Vector2(200f, 200f), new Vector2(0f, 0f));
            Controller = new PlayerController();
            Entities.Add(Player);

            timer.Tick += Update;
            timer.Interval = TimeSpan.FromMilliseconds(16);
        }

        public void Start()
        {
            MapGenerator.CreateMap();
            MapGenerator.SmoothMap(5);

            camera = new Camera();

            timer.Start();
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

            camera.Update(Player.Rectangle, Player.Position);
            camera.Draw(Entities);
        }
    }
}
