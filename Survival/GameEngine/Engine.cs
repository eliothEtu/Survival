using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Survival.GameEngine
{
    internal class Engine
    {
        private static Engine instance;
        public static Engine Instance { get => instance; }

        private List<Entity> entities = new List<Entity>();
        public List<Entity> Entities { get => entities; }

        private DispatcherTimer timer = new DispatcherTimer();

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

            Player = new Player(1, new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\face (1).png")), new Vector2(0f, 0f), new Vector2(0f, 0f));
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
            camera.Draw(Player.Rectangle, new Vector2(100, 100));

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
        }
    }
}
